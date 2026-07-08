using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectMannager.API.Data;
using ProjectMannager.API.Repositories.Implementations;
using ProjectMannager.API.Repositories.Interfaces;
using ProjectMannager.API.Services;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authorization; // Importante para mapear o [Authorize]
using ProjectMannager.API.Infrastructure; // Adicione isso no topo do Program.cs


var builder = WebApplication.CreateBuilder(args);

// 1. Connection String & DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();

// 3. Serviços
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IBoardService, BoardService>();
// 4. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

// 5. JWT Authentication Configuration
var jwtSecret = builder.Configuration["Jwt:Secret"];
var key = Encoding.UTF8.GetBytes(jwtSecret!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();

// 6. OpenAPI Configurado para mapear [Authorize] nativamente no .NET 10
//builder.Services.AddOpenApi(options =>
//{
//    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
//});

// 6. OpenAPI Configurado nativamente para o .NET 10
builder.Services.AddOpenApi(options =>
{
    // Registra a definição do Token nos componentes
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

    // Registra o filtro inteligente por endpoint
    options.AddOperationTransformer<AuthOperationTransformer>();
});


var app = builder.Build();

// 7. Request Pipeline Middlewares
app.UseCors("AllowVite");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("TaskFlow API Documentation")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

        // O .NET 10 gera o JSON na rota padrão automaticamente
        options.OpenApiRoutePattern = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
