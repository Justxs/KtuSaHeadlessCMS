using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.Cms.KtuSaModule.Settings;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

// Must run before Orchard opens any tenant database: a SQLite file cannot be
// replaced while it is in use, so a backup uploaded in the admin panel is only
// staged there and swapped in here.
PendingRestoreApplier.Apply(Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

builder.Host.UseNLogHost();
builder.Services.AddOrchardCms();

var fientaSettings = new FientaSettings();
builder.Configuration.GetSection(fientaSettings.SectionName)
    .Bind(fientaSettings);

builder.Services.AddSingleton(fientaSettings);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/media", StringComparison.OrdinalIgnoreCase),
    static appBuilder => appBuilder.UseStaticFiles());

app.UseOrchardCore();

app.Run();