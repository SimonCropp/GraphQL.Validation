public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyHttp.Enable();
        VerifyDiffPlex.Initialize();
        VerifierSettings.IgnoreMembers("Content-Length");
    }
}