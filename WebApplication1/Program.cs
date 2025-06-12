using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;         // ← Ajouté
using System.Text;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Politique CORS pour autoriser le front
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 2. Swagger + configuration JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entrez 'Bearer ' suivi de votre token JWT",
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

// 3. Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// 4. Autoriser multipart/form-data jusqu’à 50 Mo
builder.Services.Configure<FormOptions>(opts =>
{
    opts.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 Mo
});

// 5. DI pour vos services et repositories
builder.Services.AddScoped<StepRepository>();
builder.Services.AddScoped<IngredientRepository>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<AutheService>();

// 6. EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 7. Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 8. Authentification JWT
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

// 9. Swagger en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 10. Servir les fichiers statiques (wwwroot/uploads)
app.UseStaticFiles();

// 11. CORS, HTTPS, Auth
app.UseCors("FrontOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 12. Map controllers
app.MapControllers();

app.Run();
