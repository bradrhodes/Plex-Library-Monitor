using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Plibmon.Domain;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ReSharper disable once UnusedParameter.Local
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Console();
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddPlibmonDomain(builder);
// Todo: this needs to be removed
// builder.Services.AddPlibmonSampleConfig();

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

// Note: Uncomment this next line to enable using Fiddler as a proxy to capture traffic from this app
// System.Net.WebRequest.DefaultWebProxy = new System.Net.WebProxy("127.0.0.1", 8888);

app.Run();
