using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.DTOs;
using backend.Middleware;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 5 * 1024 * 1024;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DnDCompanion API",
        Version = "v1",
        Description = "REST API навчального MVP-проєкту DnDCompanion (D&D companion app)."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть JWT-токен, отриманий з ендпоінта /auth/login."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DnDCompanion API v1");
    options.DocumentTitle = "DnDCompanion API";
});

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

var welcome = app.Configuration["AppSettings:WelcomeMessage"];
var version = app.Configuration["AppSettings:Version"];
app.Logger.LogInformation("Застосунок запущено. Середовище: {Env}", app.Environment.EnvironmentName);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.GetMigrations().Any())
    {
        db.Database.Migrate();
    }
    else
    {
        db.Database.EnsureCreated();
    }
}

app.MapGet("/", () =>
{
    app.Logger.LogInformation("Опрацювання запиту до головного ендпоінта");
    return $"{welcome} (версія {version}). API готове до роботи.";
});

var api = app.MapGroup("/api");

api.MapGet("/boom", () =>
{
    throw new Exception("Тестова помилка для перевірки Middleware");
});

api.MapPost("/auth/register", async (RegisterDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        return Results.BadRequest("Email та пароль обов'язкові.");

    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
        return Results.Conflict("Користувач з таким email вже існує.");

    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = "user",
        AvatarSeed = string.IsNullOrWhiteSpace(dto.Name)
            ? Guid.NewGuid().ToString("N")[..8]
            : dto.Name.ToLowerInvariant().Replace(" ", "-")
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}",
        new { user.Id, user.Name, user.Email, user.Role, user.AvatarSeed });
})
.WithTags("Auth");

api.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, IConfiguration config) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = CreateToken(user, config);
    return Results.Ok(new { access_token = token, token_type = "Bearer" });
})
.WithTags("Auth");

api.MapGet("/auth/me", async (ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var user = await db.Users.FindAsync(userId);
    if (user is null) return Results.NotFound();

    return Results.Ok(new
    {
        user.Id,
        user.Name,
        user.Email,
        user.Role,
        user.AvatarSeed
    });
})
.RequireAuthorization()
.WithTags("Auth");

api.MapPut("/auth/profile", async (UpdateProfileDto dto, ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var user = await db.Users.FindAsync(userId);
    if (user is null) return Results.NotFound();

    if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Ім'я не може бути порожнім.");

    user.Name = dto.Name.Trim();
    if (!string.IsNullOrWhiteSpace(dto.AvatarSeed))
        user.AvatarSeed = dto.AvatarSeed.Trim();

    await db.SaveChangesAsync();
    return Results.Ok(new { user.Id, user.Name, user.Email, user.Role, user.AvatarSeed });
})
.RequireAuthorization()
.WithTags("Auth");

api.MapPost("/auth/change-password", async (ChangePasswordDto dto, ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var user = await db.Users.FindAsync(userId);
    if (user is null) return Results.NotFound();

    if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 6)
        return Results.BadRequest("Новий пароль має бути не менше 6 символів.");

    if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
        return Results.BadRequest("Старий пароль невірний.");

    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Auth");

api.MapGet("/characters", async (ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var list = await db.Characters
        .Where(c => c.UserId == userId)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();
    return Results.Ok(list);
})
.RequireAuthorization()
.WithTags("Characters");

api.MapGet("/characters/{id}", async (int id, ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var character = await db.Characters.FindAsync(id);
    if (character is null || character.UserId != userId)
        return Results.NotFound();

    return Results.Ok(character);
})
.RequireAuthorization()
.WithTags("Characters");

api.MapPost("/characters", async (CharacterDto dto, ClaimsPrincipal principal, AppDbContext db) =>
{
    if (!CharacterService.IsValidNewCharacter(dto))
        return Results.BadRequest("Некоректні дані персонажа.");

    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var character = new Character
    {
        Name = dto.Name,
        Race = dto.Race,
        Class = dto.Class,
        Level = dto.Level,
        HitPoints = dto.HitPoints,
        ArmorClass = dto.ArmorClass,
        Strength = dto.Strength,
        Dexterity = dto.Dexterity,
        Constitution = dto.Constitution,
        Intelligence = dto.Intelligence,
        Wisdom = dto.Wisdom,
        Charisma = dto.Charisma,
        Background = dto.Background,
        Alignment = dto.Alignment,
        PersonalityTraits = dto.PersonalityTraits,
        Ideals = dto.Ideals,
        Bonds = dto.Bonds,
        Flaws = dto.Flaws,
        ProficientSkills = dto.ProficientSkills,
        Description = dto.Description,
        UserId = userId
    };

    db.Characters.Add(character);
    await db.SaveChangesAsync();
    return Results.Created($"/characters/{character.Id}", character);
})
.RequireAuthorization()
.WithTags("Characters");

api.MapPut("/characters/{id}", async (int id, CharacterDto dto, ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var character = await db.Characters.FindAsync(id);
    if (character is null || character.UserId != userId)
        return Results.NotFound();

    if (!CharacterService.IsValidNewCharacter(dto))
        return Results.BadRequest("Некоректні дані персонажа.");

    character.Name = dto.Name;
    character.Race = dto.Race;
    character.Class = dto.Class;
    character.Level = dto.Level;
    character.HitPoints = dto.HitPoints;
    character.ArmorClass = dto.ArmorClass;
    character.Strength = dto.Strength;
    character.Dexterity = dto.Dexterity;
    character.Constitution = dto.Constitution;
    character.Intelligence = dto.Intelligence;
    character.Wisdom = dto.Wisdom;
    character.Charisma = dto.Charisma;
    character.Background = dto.Background;
    character.Alignment = dto.Alignment;
    character.PersonalityTraits = dto.PersonalityTraits;
    character.Ideals = dto.Ideals;
    character.Bonds = dto.Bonds;
    character.Flaws = dto.Flaws;
    character.ProficientSkills = dto.ProficientSkills;
    character.Description = dto.Description;

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Characters");

api.MapDelete("/characters/{id}", async (int id, ClaimsPrincipal principal, AppDbContext db) =>
{
    var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var character = await db.Characters.FindAsync(id);
    if (character is null || character.UserId != userId)
        return Results.NotFound();

    db.Characters.Remove(character);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Characters");

api.MapGet("/races", async (AppDbContext db) =>
    await db.Races.OrderBy(r => r.Name).ToListAsync())
    .WithTags("Reference");

api.MapGet("/races/{id}", async (int id, AppDbContext db) =>
    await db.Races.FindAsync(id) is DndRace race
        ? Results.Ok(race)
        : Results.NotFound())
    .WithTags("Reference");

api.MapGet("/classes", async (AppDbContext db) =>
    await db.Classes.OrderBy(c => c.Name).ToListAsync())
    .WithTags("Reference");

api.MapGet("/classes/{id}", async (int id, AppDbContext db) =>
    await db.Classes.FindAsync(id) is DndClass cls
        ? Results.Ok(cls)
        : Results.NotFound())
    .WithTags("Reference");

api.MapGet("/spells", async (AppDbContext db, int? level) =>
{
    var query = db.Spells.AsQueryable();
    if (level.HasValue) query = query.Where(s => s.Level == level.Value);
    return await query.OrderBy(s => s.Level).ThenBy(s => s.Name).ToListAsync();
})
.WithTags("Reference");

api.MapGet("/spells/{id}", async (int id, AppDbContext db) =>
    await db.Spells.FindAsync(id) is DndSpell spell
        ? Results.Ok(spell)
        : Results.NotFound())
    .WithTags("Reference");

app.Run();

static string CreateToken(User user, IConfiguration config)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(4),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

public partial class Program { }
