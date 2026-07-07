using CentroLuant.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CentroLuant.Filters.LoginFilter>();
});
builder.Services.AddSingleton<ConexionBD>();
builder.Services.AddScoped<CitaRepository>();
builder.Services.AddScoped<PacienteRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<HistorialRepository>();
builder.Services.AddScoped<FacturaRepository>();
builder.Services.AddScoped<EspecialistaRepository>();
builder.Services.AddHttpClient<CentroLuant.Services.DniService>();
builder.Services.AddHttpClient<CentroLuant.Services.TipoCambioService>();
builder.Services.AddScoped<CentroLuant.Services.FacturaPdfService>();
builder.Services.AddScoped<CentroLuant.Services.CorreoService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();