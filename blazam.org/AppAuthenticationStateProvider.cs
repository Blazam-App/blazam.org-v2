using ApplicationNews;
using blazam.org.Data;
using blazam.org.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace blazam.org
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        public static int SessionTimeout { get; set; } = 15;
        public AppAuthenticationStateProvider(IHttpContextAccessor ca, IDbContextFactory<NewsDbContext> con)
        {

            this.CurrentUser = this.GetAnonymous();
            this._httpContextAccessor = ca;
            this._factory = con;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContextFactory<NewsDbContext> _factory;

        public static Action<CookieAuthenticationOptions> ApplyAuthenticationCookieOptions()
        {
            return options =>
            {

                options.Events.OnSigningIn = async (context) =>
                {

                    var currentUtc = DateTimeOffset.UtcNow;
                    context.Properties.IssuedUtc = currentUtc;
                    context.Properties.ExpiresUtc = currentUtc.AddMinutes(SessionTimeout);

                };

                options.Events.OnValidatePrincipal = async (context) =>
                {

                    var currentUtc = DateTimeOffset.UtcNow;
                    context.Properties.IssuedUtc = currentUtc;
                    context.Properties.ExpiresUtc = currentUtc.AddMinutes(SessionTimeout);

                };
                options.LoginPath = new PathString("/login");
                options.LogoutPath = new PathString("/logout");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeout);

                options.SlidingExpiration = true;
            };
        }

        private ClaimsPrincipal? CurrentUser;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var task = Task.FromResult(new AuthenticationState(this.CurrentUser));

            return task;
        }

        /// <summary>
        /// Creates an annonymous ClaimsPrincipal to handle authentication
        /// before login.
        /// </summary>
        /// <returns>An unauthenticated annonymous User ClaimsPrincipal</returns>
        private ClaimsPrincipal GetAnonymous()
        {

            var identity = new ClaimsIdentity(new[]
           {
                    new Claim(ClaimTypes.Sid, "0"),
                    new Claim(ClaimTypes.Name, "Anonymous"),
                    new Claim(ClaimTypes.Role, "Anonymous"),
                    new Claim(ClaimTypes.Actor,"0")
                }, null);

            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal GetLocalAdmin(string name = "admin")
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes. Sid, "1"),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Actor,"1"),
                    new Claim(ClaimTypes.Role,UserRoles.SuperAdmin)
            };
            var identity = new ClaimsIdentity(claims.ToArray(), "Local");
            return new ClaimsPrincipal(identity);
        }
        /// <summary>
        /// Processes a login request.
        /// </summary>
        /// <param name="loginReq">The authentication details and options for login</param>
        /// <returns>A fully processed AuthenticationState with all Claims and application permissions applied.</returns>
        public async Task<LoginResult> Login(LoginRequest loginReq)
        {
            LoginResult loginResult = new();

            var context = await _factory.CreateDbContextAsync();
            AuthenticationState? result = null;

            //Set the current user from the HttpContext which gets it from the user's browser cookie
            CurrentUser = _httpContextAccessor?.HttpContext?.User;

            if (loginReq == null) return loginResult.NoData();
            if (loginReq.Username.IsNullOrEmpty()) return loginResult.NoUsername();
            var settings = context.Settings.FirstOrDefault();
            if (settings == null)
            {
                context.Settings.Add(new());
                await context.SaveChangesAsync();
                settings = context.Settings.FirstOrDefault();
            }
            var adminPass = settings.AdminPassword;
            if (adminPass == null) adminPass = "";
            //Check admin credentials
            if (loginReq.Username != null
                && loginReq.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                if (loginReq.Password == adminPass)
                    result = await SetUser(this.GetLocalAdmin());


            }


            //Return the authenticationstate
            if (result != null)
            {
                //await _audit.Logon.Login(result.User);

                return loginResult.Success(result);
            }
            else
                return loginResult.BadCredentials();


        }



        /// <summary>
        /// Sets the User AuthenticationState in the AuthenticationProvider
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public Task<AuthenticationState> SetUser(ClaimsPrincipal claimsPrincipal)
        {
            this.CurrentUser = claimsPrincipal;
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return task;
        }
        /// <summary>
        /// This may not be entirely neccessary the way I am implementin authentication and authorization
        /// Though, this likey is needed to remove the cookie to actually signout?
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public Task<AuthenticationState> Logout(ClaimsPrincipal claimsPrincipal)
        {
            this.CurrentUser = this.GetAnonymous();
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return task;
        }

    }
    public static class UserRoles
    {

        public const string SuperAdmin = "SuperAdmin";
    }
}
