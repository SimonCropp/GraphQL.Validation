public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.IgnoreMembers("Content-Length");
    }
}