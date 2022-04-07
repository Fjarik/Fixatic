using Fixatic.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FixaticApp.Pages
{
	public class LogOutModel : PageModel
	{
		private readonly ICurrentUserService _currentUserService;

		public LogOutModel(ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
		}

		public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
		{
			_currentUserService.InvalidateCache();
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var url = Url.IsLocalUrl(returnUrl) ? returnUrl : "~/";

			return LocalRedirect(url);
		}
	}
}
