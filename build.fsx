#r "packages/build/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.ProcessHelper
open Fake.AssemblyInfoFile

let version =
    match buildServer with
    | LocalBuild -> null
    | _ -> buildVersion

let solutionFile = "Bootstrapper.sln"

let srcDir = __SOURCE_DIRECTORY__ </> "src"
let distDir = __SOURCE_DIRECTORY__ </> "dist"

let projectDir = srcDir </> "Hakudu.Bootstrapper"
let binDir configuration = projectDir </> "bin" </> configuration
let exeFile binDir = binDir </> "hakudu.exe"

let BuildTarget name configuration target =
    TargetTemplate (fun _ ->
        !! solutionFile
        |> MSBuild null target [ "Configuration", configuration ]
        |> ignore
    ) name ()

let BuildDebugTarget name = BuildTarget name "Debug"
let BuildReleaseTarget name = BuildTarget name "Release"

BuildDebugTarget "clean-debug" "Clean"
BuildReleaseTarget "clean" "Clean"

BuildDebugTarget "build-debug" "Build"
BuildReleaseTarget "build" "Build"

BuildDebugTarget "rebuild-debug" "Rebuild"
BuildReleaseTarget "rebuild" "Rebuild"

Target "run" (fun _ ->
    let debugBinDir = binDir "Debug"

    execProcess (fun p ->
        p.FileName <- debugBinDir |> exeFile
        p.WorkingDirectory <- debugBinDir
        p |> platformInfoAction
    ) TimeSpan.MaxValue |> ignore
)

"build-debug" ==> "run"

Target "version" (fun _ ->
    let assemblyInfoFile = projectDir </> "Properties/AssemblyInfo.cs"

    let makeFxVersion (version: string) =
        let semver = version |> SemVerHelper.parse
        sprintf "%d.%d.%d.0" semver.Major semver.Minor semver.Patch

    if (version = null)
        then invalidOp "Build version is not provided."

    let netversion = version |> makeFxVersion

    [
        Attribute.InformationalVersion version
        Attribute.Version netversion
        Attribute.FileVersion netversion
    ]
    |> UpdateAttributes assemblyInfoFile
)

Target "package" (fun _ ->
    let releaseBinDir = binDir "Release"

    distDir |> CleanDir

    !! (releaseBinDir |> exeFile)
    |> Copy distDir
)

"build" ==> "package"
"version" =?> ("package", not isLocalBuild)
"version" ?=> "build"

RunTargetOrDefault "build"
