using OrchardCore.Cms.KtuSaModule.Settings;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

builder.Services.AddOrchardCms();

var googleCloudSettings = new GoogleCloudSettings();
builder.Configuration.GetSection(googleCloudSettings.SectionName).Bind(googleCloudSettings);

var fientaSettings = new FientaSettings();
builder.Configuration.GetSection(fientaSettings.SectionName).Bind(fientaSettings);

builder.Services.AddSingleton(googleCloudSettings);
builder.Services.AddSingleton(fientaSettings);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOrchardCore();

app.Run();
