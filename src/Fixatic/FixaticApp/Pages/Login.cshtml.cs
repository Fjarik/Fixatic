using System.Reflection;
using Fixatic.BO;
using Fixatic.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FixaticApp.Pages
{
	[ValidateAntiForgeryToken]
	public class LoginModel : PageModel
	{
		private readonly ILogger _logger;
		private readonly ApplicationSettings _applicationSettings;

		public LoginModel(ILogger<LoginModel> logger, IOptions<ApplicationSettings> applicationSettings)
		{
			_logger = logger;
			_applicationSettings = applicationSettings.Value;
		}

		[BindProperty]
		public string Email { get; set; } = string.Empty;
		[BindProperty]
		public string Password { get; set; } = string.Empty;

		public string Message { get; set; }


		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();


			var props = new AuthenticationProperties
			{
				RedirectUri = "./",
				IsPersistent = true,
				ExpiresUtc = DateTime.UtcNow.AddDays(14),
				AllowRefresh = true,
			};

			var manager = new CurrentUserManager(_logger, _applicationSettings);
			var res = await manager.LoginAsync(Email, Password);
			if (res == null)
				return Unauthorized();

			return SignIn(res, props, CookieAuthenticationDefaults.AuthenticationScheme);
		}
	}
}
