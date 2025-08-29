using ApplicationNews;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace blazam.org.Shared.UI
{
    public class NewsComponentBase : ComponentBase
    {
        [Inject]
        protected IDbContextFactory<NewsDbContext> Factory { get; set; }
        [Inject]
        protected ISnackbar SnackbarService { get; set; }
        [Inject]
        protected IDialogService DialogService { get; set; }

        protected NewsDbContext Context { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Context = await Factory.CreateDbContextAsync();
        }
    }
}
