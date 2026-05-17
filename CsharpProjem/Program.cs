using CsharpProjem.Components;
using AuroraVault; // 1. ADIM: Senin yazdığın sınıfların (VaultManager vb.) algılanması için bu satırı ekledik.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. ADIM: Senin yazdığın backend motorunu (VaultManager) web projesine bağladık. 
// Arayüzdeki butonlar artık doğrudan senin kodlarını tetikleyecek.
builder.Services.AddScoped<VaultManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
