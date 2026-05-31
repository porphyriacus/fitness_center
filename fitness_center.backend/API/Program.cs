using API;
using API.Services;
using Application;
using Core.Abstractions;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// register db 
var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// register layers
builder.Services
    .AddInfrastructure()
    .AddApplication();

//regidter identity 
builder.Services.AddIdentity();

// JWT token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                             // проверяем издателя (токен выпущен мои а не каким другим чудом)
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // кто выдал токен

        ValidateAudience = true,                           // проверяем получателя
        ValidAudience = builder.Configuration["Jwt:Audience"], // для кого токен     токен предназначен именно для моего апи

        ValidateLifetime = true,                           // проверяем срок действия
        ClockSkew = TimeSpan.FromSeconds(30),                         // без запаса по времени

        ValidateIssuerSigningKey = true,                   // проверяем подпись
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "FitnessCenterCookie";    // имя куки
    options.ExpireTimeSpan = TimeSpan.FromDays(7);  // кука буде жить 7 дней
    options.SlidingExpiration = true;               // продлевать жизнь при активности пользователя
    options.LoginPath = "/Account/Login";           
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;                 // защита от XSS 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Только HTTPS
});

builder.Services.AddJWTAuthentication();

builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fitness Center API",
        Version = "v1",
        Description = "API для управления фитнес-центром"
    });

    // Определяем схему безопасности JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token. Example: \"Bearer {your_token}\""
    });

    c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services);
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();  

app.Run();
