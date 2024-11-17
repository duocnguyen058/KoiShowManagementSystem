using KoiShowManagementSystem.Repositories;
using KoiShowManagementSystem.Repositories.Repositories;
using KoiShowManagementSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

}, ServiceLifetime.Transient);

builder.Services.AddAuthentication("UserAuth")
    .AddCookie("UserAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

//Repository
builder.Services.AddTransient<IEventsRepository, EventsRepository>();
builder.Services.AddTransient<IKoiRepository, KoiRepository>();
builder.Services.AddTransient<IScoresRepository, ScoresRepository>();
builder.Services.AddTransient<IReportsRepository, ReportsRepository>();
builder.Services.AddTransient<IJudgeAssignmentsRepository, JudgeAssignmentsRepository>();
builder.Services.AddTransient<IEventKoiParticipationRepository, EventKoiParticipationRepository>();

//Service
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<IEventKoiParticipationService, EventKoiParticipationService>();
builder.Services.AddTransient<IJudgeAssignmentsService, JudgeAssignmentsService>();
builder.Services.AddTransient<IKoiService, KoiService>();
builder.Services.AddTransient<IReportsService, ReportsService>();
builder.Services.AddTransient<IScoresService, ScoresService>();
builder.Services.AddTransient<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
