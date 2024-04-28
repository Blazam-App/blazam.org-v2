
using blazam.org;
using blazam.org.Data;
using blazam.org.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazamNews.Pages
{
    [IgnoreAntiforgeryToken]
    public class SignInModel : PageModel
    {
        public SignInModel(AppAuthenticationStateProvider auth, NavigationManager _nav)
        {
            Auth = auth;
            Nav = _nav;
        }


        public string RedirectUri { get; set; }
        public AppAuthenticationStateProvider Auth { get; }
        public NavigationManager Nav { get; private set; }

        public void OnGet(string returnUrl="")
        {
            ViewData["Layout"] = "_Layout";
            if (returnUrl.IsUrlLocalToHost())
            {
                RedirectUri = returnUrl;
            }
        }

      
        /// <summary>
        /// The authentication endpoint for web clients
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost([FromFormAttribute]LoginRequest req)
        {
            try
            {
                req.IPAddress = HttpContext.Connection.RemoteIpAddress;
            }catch(Exception ex)
            {
            }
            try
            {
                
                var result = await Auth.Login(req);
                if (result != null && result.Status == LoginResultStatus.OK)
                {
                    await HttpContext.SignInAsync(result.AuthenticationState.User);
                }
                return new ObjectResult(result.Status);

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
           
            
        }


    }
}
