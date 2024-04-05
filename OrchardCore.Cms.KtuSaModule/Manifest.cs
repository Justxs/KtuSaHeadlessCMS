using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "KTU SA Module",
    Author = "Justas Pranauskis",
    Website = "https://github.com/Justxs",
    Version = "0.0.1",
    Description = "KTU SA Module provides all functionality to manage KTU SA Content",
    Category = "Content Management",
    Dependencies =
    [
        "OrchardCore.Admin",
        "OrchardCore.Contents",
        "OrchardCore.ContentTypes",
        "OrchardCore.ContentFields",
        "OrchardCore.Localization",
        "OrchardCore.Navigation",
        "OrchardCore.Resources",
        "OrchardCore.Roles",
        "OrchardCore.Settings",
        "OrchardCore.Google",
    ]
)]
