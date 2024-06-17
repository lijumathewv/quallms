using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.Contracts;
using QualLMS.Repository;
using QualvationLibrary;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection") ??
    throw new InvalidOperationException("Unable to find the database")));

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

app.UseMiddleware<ClientIpEnricherMiddleware>();
app.UseSerilogRequestLogging(); // Add Serilog request logging

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
