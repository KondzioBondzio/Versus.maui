using MudBlazor.Services;
using Serilog;
using Versus.Core.Services.Countries;
using Versus.Web.Components;
using _Imports = Versus.Core.Components._Imports;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSingleton<CountryService>();
builder.Services.AddLocalization();
builder.Services.AddLogging(options =>
{
    options.ClearProviders();
    options.AddSerilog();
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly)
    .AddInteractiveServerRenderMode();

app.Run();
