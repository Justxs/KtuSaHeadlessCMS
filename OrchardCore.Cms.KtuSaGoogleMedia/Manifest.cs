using OrchardCore.Modules.Manifest;

[assembly: Module(
    Id = "KtuSaGoogleMedia",
    Name = "KTU SA Google Cloud Media",
    Author = "Justas Pranauskis",
    Website = "https://github.com/Justxs",
    Version = "0.0.1",
    Description = "Provides Google Cloud Storage integration for OrchardCore media.",
    Category = "Content Management",
    Dependencies = ["OrchardCore.Media"]
)]