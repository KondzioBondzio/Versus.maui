using MudBlazor.Services;
using Serilog;
using Versus.Core.Services;
using Versus.Core.Services.Countries;
using Versus.Core.Services.Session;
using Versus.Web.Components;
using Versus.Web.Services.Session;
using CoreImports = Versus.Core.Components._Imports;
using ISession = Versus.Core.Services.Session.ISession;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddMudServices();
builder.Services.AddSingleton<CountryService>();
builder.Services.AddLocalization();
builder.Services.AddLogging(options =>
{
    options.ClearProviders();
    options.AddSerilog();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

HttpClient httpClient = new()
{
    BaseAddress = new Uri(builder.Configuration["ApiAddress"]!)
};
builder.Services.AddSingleton(new ApiClient(httpClient));
builder.Services.AddSingleton<ISession, HttpStorageSession>();
builder.Services.AddSingleton<SessionManager>();

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
app.UseSession();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(CoreImports).Assembly)
    .AddInteractiveServerRenderMode();

app.Run();