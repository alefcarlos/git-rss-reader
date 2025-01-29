using Microsoft.FluentUI.AspNetCore.Components;
using GitRssReader.Web.Components;
using GitRssReader.GitIntegration;
using GitRssReader.Web.GitTasks;
using GitRssReader.Web.Data;
using Microsoft.EntityFrameworkCore;
using GitRssReader.Web.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddOptions<GitOptions>()
    .BindConfiguration(GitOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart()
    ;

builder.Services.AddSingleton<GitOperations>();
builder.Services.AddHostedService<CloneRepositoryTask>();

builder.Services.AddFeedsCollectionProvider();

builder.Services.AddDbContextFactory<AppDbContext>(opt => opt.UseInMemoryDatabase("db"));

builder.Services.AddHostedService<ImportNewArticlesTask>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
