public enum EBuildType
{
    none,
    design,
    track,
    game,
}

public static class BuildTypeExtension
{
    public static string GetFolderPopupName(this EBuildType buildType)
    {
        return buildType switch
        {
            EBuildType.design => "PopupDesgins",
            EBuildType.track => "PopupTrack",
            EBuildType.game => "Popup",
            _ => "",
        };
    }
}
