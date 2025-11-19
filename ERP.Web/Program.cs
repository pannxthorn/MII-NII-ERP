using ERP.Web.Components;
using ERP.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API calls
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";

// Register services - Use Singleton for TokenStorage to persist across page refresh
// Note: In production, consider using ProtectedSessionStorage or Database for better security
builder.Services.AddSingleton<TokenStorageService>();

// Register AuthorizationHandler as Transient (will be created per request)
builder.Services.AddTransient<AuthorizationHandler>();

// Register a single named HttpClient for all API services with AuthorizationHandler
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<AuthorizationHandler>(); // Automatically add token to all requests

// Register all API services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<UserService>();
// เพิ่ม services อื่นๆ ตรงนี้
// builder.Services.AddScoped<BranchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
