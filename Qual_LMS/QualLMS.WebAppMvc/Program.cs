using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.Contracts;
using QualLMS.Repository;
using QualvationLibrary;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connection = builder.Configuration.GetConnectionString("DBConnection");

builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(connection, b => b.MigrationsAssembly("QualLMS.WebAppMvc")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddTransient<IApplicationUserAccount, ApplicationUserAccountRepository>();
builder.Services.AddTransient<IAttendance, AttendanceRepository>();
builder.Services.AddTransient<IOrganization, OrganizationRepository>();

builder.Services.AddTransient<ICourse, CourseRepository>();
builder.Services.AddTransient<IFees, FeesRepository>();
builder.Services.AddTransient<IFeesReceived, FeesReceivedRepository>();
builder.Services.AddTransient<IStudentCourse, StudentCourseRepository>();

builder.Services.AddTransient<ICalendar, CalendarRepository>();

builder.Services.AddSingleton<CustomLogger>();
builder.Services.AddSingleton<Client>();
builder.Services.AddSingleton<LoginProperties>();
builder.Services.AddHttpContextAccessor();

//Configure Logging
var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
Log.Logger = new LoggerConfiguration()
     .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Map(le => new DateTime(le.Timestamp.Year, le.Timestamp.Month, le.Timestamp.Day),
        (day, wt) => wt.File($"./Logs/{day:yyyyMMdd}/Log_.log",
                             rollingInterval: RollingInterval.Minute,
                             fileSizeLimitBytes: 10,
                             rollOnFileSizeLimit: true,
                             retainedFileCountLimit: 30,
                             outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:t4}] {Message:j}{NewLine}"),
        sinkMapCountLimit: 1)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

//app.UseMiddleware<ClientIpEnricherMiddleware>();
app.UseSerilogRequestLogging(); // Add Serilog request logging

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
