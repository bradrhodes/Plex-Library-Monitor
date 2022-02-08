using Microsoft.AspNetCore.ResponseCompression;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Plibmon.Domain;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Console();
    lc.MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddPlibmonDomain();
// Todo: this needs to be removed
builder.Services.AddPlibmonSampleConfig();

builder.Services.AddHangfire(x => x.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddMediatR(typeof(IPlibmonService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseHangfireDashboard();

System.Net.WebRequest.DefaultWebProxy = new System.Net.WebProxy("127.0.0.1", 8888);

app.Run();
