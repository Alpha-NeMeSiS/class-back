using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Définition d’une politique CORS pour autoriser le front 
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL de votre front 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Ajout des services Swagger avec configuration de la sécurité JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entrez 'Bearer' suivi d'un espace et de votre token JWT",
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Ajout des services MVC/Controllers...
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// Ajout des services d’exploration d’API (Swagger)
builder.Services.AddEndpointsApiExplorer();

// Injection des dépendances (repositories & services)
builder.Services.AddScoped<StepRepository>();
builder.Services.AddScoped<IngredientRepository>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<AutheService>();

// Configuration du DbContext Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuration JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Activation du CORS avant l’authentification et l’autorisation
app.UseCors("FrontOrigins");

app.UseHttpsRedirection();

// Activation de l’authentification et de l’autorisation JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
