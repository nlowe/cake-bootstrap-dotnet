#addin "nuget:?package=Cake.NPM&version=0.13.0"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./MyApp.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean::Dist")
    .Does(() => 
{
    if(DirectoryExists("./_dist"))
    {
        DeleteDirectory("./_dist", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean::Test")
    .Does(() => 
{
    if(DirectoryExists("_tests"))
    {
        DeleteDirectory("_tests", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Clean::Test")
    .Does(() =>
{
    DotNetCoreClean(SOLUTION);
});

Task("Restore::NPM")
    .Does(() =>
{
    NpmInstall(new NpmInstallSettings
    {
        WorkingDirectory = "./src/MyApp"
    });
});

Task("Restore::Nuget")
    .Does(() =>
{
    DotNetCoreRestore(SOLUTION);
});

Task("Restore")
    .IsDependentOn("Restore::NPM")
    .IsDependentOn("Restore::Nuget");

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild(SOLUTION, new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoRestore = true
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    foreach(var project in GetFiles("./tests/**/*.csproj"))
    {
        DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true
        });
    }
});

Task("Dist")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Test")
    .Does(() => 
{
    EnsureDirectoryExists("./_dist");

    DotNetCorePublish("./src/MyApp/MyApp.csproj", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        NoRestore = true,
        OutputDirectory = MakeAbsolute(Directory("./_dist/MyApp")).FullPath
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
