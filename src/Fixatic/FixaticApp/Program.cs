using FixaticApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Fixatic.Types;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection("ApplicationSettings");
var appSettings = new ApplicationSettings();
section.Bind(appSettings);

var services = builder.Services;

// Add services to the container.
services.AddLogging();
services.AddRazorPages();
services.AddServerSideBlazor();

services.AddAuthorization();
services.AddHttpClient();
services.AddHttpContextAccessor();

services.Configure<ApplicationSettings>(section);

services.AddSingleton<WeatherForecastService>();
services.AddMudServices(x =>
{
    x.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomLeft;
    x.SnackbarConfiguration.PreventDuplicates = true;
    x.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});

var app = builder.Build();
var settingOpt = app.Services.GetRequiredService<IOptions<ApplicationSettings>>();

var asm = typeof(Program).Assembly;
var version = asm.GetName()?.Version ?? new Version(1, 0, 0);
settingOpt.Value.AppVersion = version;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

if (!string.IsNullOrWhiteSpace(settingOpt.Value.BasePath))
{
    app.UsePathBase(settingOpt.Value.BasePath);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
