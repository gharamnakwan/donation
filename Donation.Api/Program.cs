using Donation.Application.Abstractions.Services;
using Donation.Application.Services;
using Donation.Domain.Entities;
using Donation.Infrastructure;
using Donation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. تسجيل الـ DbContext الخاص بقاعدة البيانات
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. تسجيل الـ Identity Core مع دعم الأدوار (Roles) وربطه مع الـ ApplicationDbContext
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>() // <-- تم إضافة هذا السطر لحل مشكلة الـ Role Store
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 3. إضافة وتكوين إعدادات الـ JWT Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey1234567890123456"))
    };
});
builder.Services.AddAuthorization();

// 4. Register Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. Register Infrastructure Services (لتسجيل الـ UserService وباقي خدمات الطبقة تلقائياً)
builder.Services.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// تفعيل ممر التوثيق والصلاحيات (الترتيب مهم جداً: Authentication أولاً ثم Authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();