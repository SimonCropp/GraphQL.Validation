public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyHttp.Enable();
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.IgnoreMembers("Content-Length");
        });
    }
}