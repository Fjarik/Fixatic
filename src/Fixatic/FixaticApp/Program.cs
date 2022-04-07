using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Fixatic.Types;
using Microsoft.Extensions.Options;
using Fixatic.Services;
using Fixatic.Services.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using FixaticApp.Types;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection("ApplicationSettings");
var appSettings = new ApplicationSettings();
section.Bind(appSettings);

var services = builder.Services;

// Add services to the container.
services.AddLogging();
services.AddRazorPages();
services.AddServerSideBlazor();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
services.AddAuthorization();
services.AddHttpClient();
services.AddHttpContextAccessor();

services.Configure<ApplicationSettings>(section);

services.AddMudServices(x =>
{
	x.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomLeft;
	x.SnackbarConfiguration.PreventDuplicates = true;
	x.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});

services.AddScoped<TokenProvider>();
services.AddTransient<IAttachementsService, AttachementsService>();
services.AddTransient<ICategoriesService, CategoriesService>();
services.AddTransient<ICommentsService, CommentsService>();
services.AddTransient<ICurrentUserService, CurrentUserService>();
services.AddTransient<ICustomPropertiesService, CustomPropertiesService>();
services.AddTransient<ICustomPropertyOptionsService, CustomPropertyOptionsService>();
services.AddTransient<IGroupsService, GroupsService>();
services.AddTransient<IProjectsService, ProjectsService>();
services.AddTransient<IPublicTicketsService, PublicTicketsService>();
services.AddTransient<ITicketsService, TicketsService>();
services.AddTransient<IUsersService, UsersService>();

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

app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapBlazorHub(); //.RequireAuthorization(new AuthorizeAttribute());
	endpoints.MapFallbackToPage("/_Host");
});

app.Run();
