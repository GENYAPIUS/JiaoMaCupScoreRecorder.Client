# JiaoMaCupScoreRecorder
A score recorder for JiaoMa Cup

# Requirement
- .Net 6.0.201 SDK
  - [Windows x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.201-windows-x64-installer)
  - [Linux](https://docs.microsoft.com/zh-tw/dotnet/core/install/linux)
- .Net Worktool (if use AOT Compilation)
  - `dotnet workload install wasm-tools`
- appsettings.json
# Release Build
1. Check `AOT Compilation setting` if the following block is included in JiaoMaCupScoreRecorder.csproj.  
If not need AOT Compilation, set it `false`
```xml
<PropertyGroup>
	<RunAOTCompilation>true</RunAOTCompilation>
</PropertyGroup>
```
1. Check `.Net 6.0.201 SDK` and `.Net Worktool` installed. see [Requirement](#requirement)
2. Set Machine environment variable
   - `ASPNETCORE_ENVIRONMENT`
     - `Development`
     - `PreProduction`
     - `Production`
4. Copy `appsettings.json` to `./JiaoMaCupScoreRecorder/wwwroot`
5. Open Terminal, enter `dotnet publish -c Release -o <output_path>`
6. If it succeeds, the <output_path> should have files.
