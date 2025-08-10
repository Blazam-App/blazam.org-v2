using System.Security.Claims;
using blazam.org.Data.Plugins;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace blazam.org.Shared.UI
{
    public class PluginsComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        protected IHttpContextAccessor HttpContextAccessor { get; set; }

        protected ClaimsPrincipal? CurrentUser => HttpContextAccessor.HttpContext?.User;
        [Inject]
        protected IDbContextFactory<PluginsDbContext> Factory { get; set; }
        [Inject]
        protected ISnackbar SnackbarService { get; set; }
        [Inject]
        protected IDialogService DialogService { get; set; }
        [Inject]
        protected ISnackbar Snackbar { get; set; }
        [Inject]
        protected PluginAuthService PluginAuth { get; set; }

        protected PluginsDbContext Context { get; set; }

        public void Dispose()
        {
            Context?.Dispose();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Context = await Factory.CreateDbContextAsync();
        }


    }
}
