/*#addin nuget:https://www.myget.org/F/beta-cake-addins/api/v2/?package=Cake.MsBuild15*/
//#addin "Cake.CodeAnalysisReporting"
#tool "nuget:?package=Mono.TextTransform"

bool NewerThan(string file1, string file2)
{
    var dt1 = new System.IO.FileInfo(File(file1)).LastWriteTimeUtc;
    var dt2 = new System.IO.FileInfo(File(file2)).LastWriteTimeUtc;
    if (dt1 > dt2)
    {
        return true;
    }
    return false;
}

void CompileT4(string name)
{
    if (FileExists(string.Format("{0}.cs", name)))
    {
        if (NewerThan(string.Format("{0}.tt", name), string.Format("{0}.cs", name)))
        {
            DeleteFiles(string.Format("{0}.cs", name));
        }
    }

    if (!FileExists(string.Format("{0}.cs", name)))
        TransformTemplate(File(string.Format("{0}.tt", name)));
}

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

void CompileCsproj(string project)
{
    NuGetRestore(project, new NuGetRestoreSettings(){PackagesDirectory = "./packages"});
    var bs = new MSBuildSettings {
        ToolVersion = MSBuildToolVersion.VS2017,
        Configuration = configuration,
        //ToolPath = ResolveMSBuild2017ToolPath()
    };
    /*bs.AddFileLogger(new MSBuildFileLogger {
        LogFile = "./msbuild.log",
        MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.All,
        Verbosity = Verbosity.Diagnostic,
        AppendToLogFile = false
    });*/
    MSBuild(project, bs);
}

void DotnetCompileCsproj(string project)
{
    DotNetCoreRestore(project, new DotNetCoreRestoreSettings(){PackagesDirectory = "./packages"});
    var bs = new DotNetCoreBuildSettings {
        //ToolVersion = MSBuildToolVersion.VS2017,
        Configuration = configuration,
        //ToolPath = ResolveMSBuild2017ToolPath()
    };
    /*bs.AddFileLogger(new MSBuildFileLogger {
        LogFile = "./msbuild.log",
        MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.All,
        Verbosity = Verbosity.Diagnostic,
        AppendToLogFile = false
    });*/
    DotNetCoreBuild(project, bs);
}

/*void CreateReport(string source, string dest)
{
    CreateMsBuildCodeAnalysisReport(
        source,
        CodeAnalysisReport.MsBuildXmlFileLoggerByAssembly,
        dest);
}*/

Task("Patcher").Does(() => CompileCsproj("./Patcher/Patcher.csproj"));

Task("Patcher.Rules.Compiled").Does(() => CompileCsproj("./Patcher.Rules.Compiled/Patcher.Rules.Compiled.csproj"));

Task("Default")
    .IsDependentOn("Patcher")
    .IsDependentOn("Patcher.Rules.Compiled")
    ;

RunTarget(target);
