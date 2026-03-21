using ApiTodo.Data;
using ApiTodo.Profiles;
using ApiTodo.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// BD
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//CORS
var originesPermitidos = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Se asegura de que no sea nulo para evitar excepciones en runtime
        if (originesPermitidos != null)
        {
            policy.WithOrigins(originesPermitidos)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

//SERVICIOS
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IToDoService, ToDoService>();

//AUTOMAPPER
var mappingConfig = new AutoMapper.MapperConfiguration(mc =>
{
    mc.AddProfile(new ToDoProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key is missing in appsettings.json");

var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false; // Solo true en producción
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();
app.Run();