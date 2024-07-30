using Microsoft.AspNetCore.Authentication.Certificate;
using YARPDemo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ForwarderTelemetry>();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", builder =>
    {
        builder.AllowAnyOrigin();
    });
});
var app = builder.Build();
app.UseAuthentication();
app.UseHttpLogging();
app.MapReverseProxy(x =>
{
    x.Use(Utils.MyCustomProxyStep);
    x.UseSessionAffinity();
    x.UseLoadBalancing();
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
