using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaModule.Constants;

public static class RolesConstants
{
    public const string Administrator = "Administrator";
    public const string President = "President";
    public const string CsaEditor = "CSA Editor";
    public const string BrkEditor = "BRK Editor";
    public const string Marketing = "Marketing";
    public const string InfosaEditor = "InfoSA Editor";
    public const string VivatChemijaEditor = "VIVAT chemija Editor";
    public const string IndiEditor = "InDi Editor";
    public const string EsaEditor = "ESA Editor";
    public const string FumsaEditor = "FUMSA Editor";
    public const string StatiusEditor = "Statius Editor";
    public const string VfsaEditor = "VFSA Editor";
    public const string ShmEditor = "SHM Editor";

    public static string GetEditorRole(SaUnit unit) => unit switch
    {
        SaUnit.CSA => CsaEditor,
        SaUnit.BRK => BrkEditor,
        SaUnit.InfoSA => InfosaEditor,
        SaUnit.Vivat_Chemija => VivatChemijaEditor,
        SaUnit.InDi => IndiEditor,
        SaUnit.ESA => EsaEditor,
        SaUnit.FUMSA => FumsaEditor,
        SaUnit.STATIUS => StatiusEditor,
        SaUnit.VFSA => VfsaEditor,
        SaUnit.SHM => ShmEditor,
        _ => throw new ArgumentException("Invalid SaUnit", nameof(unit))
    };
}