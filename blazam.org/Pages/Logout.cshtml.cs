
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace blazam.org.Pages
{
    public class LogOutModel : PageModel
    {
        public LogOutModel(AppAuthenticationStateProvider auth, NavigationManager _nav)
        {
            Auth = auth;
            Nav = _nav;
        }

        public bool _authenticating { get; set; }
        public bool _directoryAvailable { get; set; }
        public string _username { get; set; }
        public string _password { get; set; }
        public string RedirectUri { get; private set; }
        public AppAuthenticationStateProvider Auth { get; }
        public NavigationManager Nav { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            var user = this.User;

            var result = Auth.Logout(User);
            if (result != null)
            {
                await HttpContext.SignOutAsync();

            }

            return Redirect("/");

        }

    }
}
