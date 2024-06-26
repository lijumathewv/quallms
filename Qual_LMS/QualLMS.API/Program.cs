using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QualLMS.Domain.Contracts;
using QualLMS.Repository;
using QualvationLibrary;
using Serilog;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection") ??
    throw new InvalidOperationException("Unable to find the database")));
//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>()
//    .AddSignInManager()
//    .AddRoles<IdentityRole>()
//    .AddApiEndpoints();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSpecificOrigin", policy =>
    {
        //policy.WithOrigins(builder.Configuration.GetSection("LMSConfig:CORSOrigins").Get<string[]>())
        policy.AllowAnyOrigin()
        .AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddScoped<IApplicationUserAccount, ApplicationUserAccountRepository>();
builder.Services.AddScoped<IAttendance, AttendanceRepository>();
builder.Services.AddScoped<IOrganization, OrganizationRepository>();

builder.Services.AddScoped<ICourse, CourseRepository>();
builder.Services.AddScoped<IFees, FeesRepository>();
builder.Services.AddScoped<IFeesReceived, FeesReceivedRepository>();
builder.Services.AddScoped<IStudentCourse, StudentCourseRepository>();

builder.Services.AddScoped<ICalendar, CalendarRepository>();

builder.Services.AddSingleton<CustomLogger>();

builder.Services.AddAuthorizationBuilder();
//Configure Logging
var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

Log.Logger = new LoggerConfiguration()
     .Enrich.FromLogContext()
.WriteTo.Console()
    .WriteTo.File(new CustomFormatter(), "logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ClientIpEnricherMiddleware>();
app.UseSerilogRequestLogging(); // Add Serilog request logging

app.MapControllers();

app.Run();
