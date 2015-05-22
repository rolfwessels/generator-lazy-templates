Framework "4.0"

# 
# properties
# 

properties {
    $buildConfiguration = 'debug'
    $buildDirectory = 'build'
    $buildConfigDirectory = Join-Path $buildDirectory $buildConfiguration
    $buildReportsDirectory =  Join-Path $buildDirectory 'reports'
    $buildPackageDirectory =  Join-Path $buildDirectory 'packages'
    $buildDistDirectory =  Join-Path $buildDirectory 'dist'
    
    $buildContants = ''

    $srcDirectory = 'src'
    $srcSolution = Join-Path $srcDirectory 'MainSolutionTemplate.sln'    
    $srcBinFolder = Join-Path bin $buildConfiguration
    $codeCoverRequired = 70

    $versionMajor = 0
    $versionMinor = 0
    $versionBuild = 1
    $versionRevision = 0

    $msdeploy = 'C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe';
    $deployServiceDest = "computerName='xxxx',userName='xxx',password='xxxx',includeAcls='False',tempAgent='false',dirPath='d:\server\temp'"
    $deployApiDest = 'auto,includeAcls="False",tempAgent="false"'
}

# 
# task
# 

task default -depends build  -Description "By default it just builds"
task clean -depends build.clean -Description "Removes bin/object and build folder"
task build -depends clean,build.compile,build.publish,build.copy -Description "Cleans and builds the project placing binaries in build directory"
task test -depends build,test.run  -Description "Builds and runs part cover tests"
task full -depends version,test,deploy.zip -Description "Versions builds and creates distributions"
task package -depends version,build,deploy.package -Description "Creates packages that could be user for deployments"
task deploy -depends version,build,deploy.api,deploy.service -Description "Deploy the files to webserver using msdeploy"

# /*
#     <target name="build" depends="clean compile" description="Compile and Run Tests" />
#     <target name="test" depends="build, run.tests" description="Compile and Run Tests" />
#   <target name="cover" depends="build, run.codecover.tests" description="Compile and Run Tests" />
#     <target name="full" depends="version, cover, dist , version.update" description="Compiles, tests, and produces distributions" />
#     <target name="ci.tests" depends="compile, run.codecover.tests" description="Compile and Run Tests" />
#     <target name="ci.deploy" depends="version build deploy.msdeploy" description="Compile and Run Tests" />
#     <target name="ci.deploy.qa" depends="config.qa ci.deploy" description="Compile and Run Tests" />
#     <target name="ci.deploy.staging" depends="config.staging ci.deploy" description="Compile and Run Tests" />
#     <target name="ci.deploy.release" depends="config.release ci.deploy" description="Compile and Run Tests" />

# */

# 
# task depends
# 

task build.clean { 
    remove-item -force -recurse $buildDirectory -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem $srcDirectory -include bin,obj -Recurse | Foreach-Object {$_.fullname}  
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue   
    }
}

task build.compile { 
    msbuild  $srcSolution /t:rebuild /p:Configuration=$buildConfiguration /v:q
}

task version {
    $commonAssemblyInfo = Join-Path $srcDirectory 'MainSolutionTemplate.Core/Properties/CommonAssemblyInfo.cs'
    $regEx = 'AssemblyVersion\(.*\)'
    $replace = 'AssemblyVersion("' + (fullversionrev) + '")'
    $replace
    'Set the version in ' +$commonAssemblyInfo
    (gc  $commonAssemblyInfo )  -replace $regEx, $replace |sc $commonAssemblyInfo 
}

task build.copy { 
    'Copy the console'
    $fromFolder =  Join-Path $srcDirectory (Join-Path 'MainSolutionTemplate.Console' $srcBinFolder )
    $toFolder =  Join-Path $buildConfigDirectory 'MainSolutionTemplate.Console'
    copy-files $fromFolder $toFolder
    'Copy the static files'
    $fromFolder =  Join-Path $srcDirectory 'MainSolutionTemplate.Website' 
    $toFolder =  Join-Path $buildConfigDirectory 'MainSolutionTemplate.Api\static'
    copy-files $fromFolder $toFolder index.html,avicon.ico,robots.txt
    copy-files (Join-Path $fromFolder 'views') (Join-Path $toFolder 'views')
    copy-files (Join-Path $fromFolder 'assets') (Join-Path $toFolder 'assets')
    copy-files (Join-Path $fromFolder 'scripts\dist') (Join-Path $toFolder 'scripts\dist')
}

task build.publish { 
    $toFolder = Join-Path ( Join-Path (resolve-path .)$buildConfigDirectory) 'MainSolutionTemplate.Api'
    $project = Join-Path $srcDirectory 'MainSolutionTemplate.Api\MainSolutionTemplate.Api.csproj'
    $publishProfile = "Publish - $buildConfiguration.pubxml";
    msbuild  $project /p:DeployOnBuild=true /p:publishurl=$toFolder /p:DefineConstants=$buildContants /p:Configuration=$buildConfiguration /p:PublishProfile=$publishProfile /p:VisualStudioVersion=11.0 /v:q
}

task nuget.restore { 
    ./src/.nuget/NuGet.exe install src\.nuget\packages.config -OutputDirectory lib
}

task test.run -depends nuget.restore { 
    mkdir $buildReportsDirectory -ErrorAction SilentlyContinue
    
    $currentPath = resolve-path '.'
    $partcoverDirectory = resolve-path 'lib\OpenCover.4.5.3723\'
    $partcoverExe = Join-Path $partcoverDirectory 'OpenCover.Console.exe'
    $nunitDirectory =  resolve-path 'tools/nunit/nunit-console.exe'

    $runTestsTimeout = '60000'
    $runTestsDirectory = '.Tests'
    $runTestsSettings = '/exclude:Unstable /timeout:' + $runTestsTimeout

    $nunit2failed = 'false'
    $hasFailure = false
    $testFolders = Get-ChildItem $srcDirectory '*.Tests' -Directory
    foreach ($testFolder in $testFolders) {
        
        $runTestsFolder = Join-Path $testFolder.FullName $srcBinFolder 
        $runTestsFolderDll = Join-Path $runTestsFolder ($testFolder.Name + '.dll')
        
        $buildReportsDirectoryResolved = '..\..\'+ $buildReportsDirectory;
        $runTestsFolderResult =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.xml')
        $runTestsFolderOut =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.txt')
        $runTestsFolderPartResult =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.part.xml')
        '----------------------------------------------'
        $testFolder.Name

        Set-Location $partcoverDirectory
        
        $target = '-targetargs:'+$runTestsFolderDll+' /nologo /noshadow /out:'+$runTestsFolderOut +' /xml:'+$runTestsFolderResult
        $runTestsFolder

        ./OpenCover.Console.exe -target:$nunitDirectory $target   -register:user -output:$runTestsFolderPartResult -log:Warn  
        [xml]$Xml = Get-Content $runTestsFolderResult
        [int]$result= $Xml.'test-results'.failures
        $hasFailure =  $hasFailure -or $result -gt 0       

    }

    if ($hasFailure)
    {
        throw "Tests have failed"
    }
    
    write-host 'Generate report' -foreground "magenta"
    Set-Location $currentPath
    Set-Location 'lib\ReportGenerator.2.1.1.0'
    $buildReportsDirectoryRelative = Join-Path '..\..\' $buildReportsDirectory
    $reports = Join-Path  $buildReportsDirectoryRelative '*.Tests.part.xml'
    $targetdir = Join-Path  $buildReportsDirectoryRelative 'CodeCoverage'
    $reporttypes = 'HTML;HTMLSummary;XMLSummary'
    $filters = '+MainSolutionTemplate*;-MainSolutionTemplate*Tests';

    ./ReportGenerator.exe -reports:$reports -targetdir:$targetdir -reporttypes:$reporttypes -filters:$filters -verbosity:Error
    Set-Location $currentPath
    write-host 'Validate code coverage' -foreground "magenta"

    $codeCoverSummary = Join-Path $buildReportsDirectory 'CodeCoverage\Summary.xml'
    [xml]$Xml = Get-Content $codeCoverSummary
    [int]$codeCover = $Xml.CoverageReport.Summary.LineCoverage -replace '%', ''
    if ($codeCover -lt $codeCoverRequired) {
        throw 'The solution currently has '+$codeCover+'% coverage, less than the required '+$codeCoverRequired+'%'
    }
}

task deploy.zip { 
    mkdir $buildDistDirectory -ErrorAction SilentlyContinue
    $folders = Get-ChildItem $buildConfigDirectory -Directory
    foreach ($folder in $folders) {
        $version = fullversion
        $zipname = Join-Path $buildDistDirectory ($folder.name + '.v.'+ $version  +'.zip' )
        write-host ('Create '+$zipname) 
        ZipFiles $zipname $folder.fullname
    }
}

task deploy.package {
    $version = fullversion
    $mkdirResult = mkdir $buildPackageDirectory  -ErrorAction SilentlyContinue
    $toFolder = Join-Path ( resolve-path $buildPackageDirectory ) "$buildConfiguration.MainSolutionTemplate.Api.v.$version.zip"
    $configuration = $buildConfiguration+';Platform=AnyCPU;AutoParameterizationWebConfigConnectionStrings=false;PackageLocation=' + $toFolder + ';EnableNuGetPackageRestore=true'
    $project = Join-Path $srcDirectory 'MainSolutionTemplate.Api\MainSolutionTemplate.Api.csproj'
    msbuild /v:q  /t:restorepackages  /T:Package  /p:Configuration=$configuration  /p:PackageTempRootDir=c:\temp  $project
}


task deploy.api {
    $deployWebsiteName = 'Default Web Site/MainSolutionTemplate.Api.'+$buildConfiguration
    $version = fullversion
    $toFolder = Join-Path ( resolve-path $buildPackageDirectory ) "$buildConfiguration.MainSolutionTemplate.Api.v.$version.zip"
    $skip = 'skipAction=Delete,objectName=filePath,absolutePath=Logs'
    $setParam = 'ApplicationPath='+$deployWebsiteName+''
    &($msdeploy) -source:package=$toFolder -dest:$deployApiDest -verb:sync -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -skip:$skip
}

task deploy.service {
    $source = 'dirPath='+( resolve-path (Join-Path $buildConfigDirectory 'MainSolutionTemplate.Console'))
    &($msdeploy) -verb:sync -allowUntrusted -source:$source -dest:$deployServiceDest
    # &($msdeploy) -verb:sync -preSync:runCommand='D:\Dir\on\remote\server\stop-service.cmd',waitInterval=30000 -source:dirPath='C:\dir\of\files\to\be\copied\on\build\server ' -dest:computerName='xx.xx.xx.xx',userName='xx.xx.xx.xx',password='xxxxxxxxxxxxxxx',includeAcls='False',tempAgent='false',dirPath='D:\Dir\on\remote\server\'  -allowUntrusted -postSync:runCommand='D:\Dir\on\remote\server\start-service.cmd',waitInterval=30000
}

task ? -Description "Helper to display task info" {
	WriteDocumentation
}

# 
# functions
# 

function fullversion() {
    $version = $versionBuild
    if ($env:BUILD_NUMBER) {
        $version = $env:BUILD_NUMBER
    }
    return "$versionMajor.$versionMajor.$version"
}

function fullversionrev() {
    return  (fullversion) + ".$versionRevision"
}

function global:copy-files($source,$destination,$include=@(),$exclude=@()){    
    $sourceFullName = resolve-path $source
    $relativePath = Get-Item $source | Resolve-Path -Relative
    $mkdirResult = mkdir $destination -ErrorAction SilentlyContinue
    $files = Get-ChildItem $source -include $include -Recurse -Exclude $exclude
     foreach ($file in $files) {
       $relativePathOfFile = Get-Item $file.FullName | Resolve-Path -Relative
       $tofile = Join-Path $destination $relativePathOfFile.Substring($relativePath.length)
       Copy-Item -Force $relativePathOfFile $tofile
     }
}

function ZipFiles( $zipfilename, $sourcedir )
{
   del $zipfilename -ErrorAction SilentlyContinue
   Add-Type -Assembly System.IO.Compression.FileSystem
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}

function WriteDocumentation() {
    $currentContext = $psake.context.Peek()

    if ($currentContext.tasks.default) {
        $defaultTaskDependencies = $currentContext.tasks.default.DependsOn
    } else {
        $defaultTaskDependencies = @()
    }

    $docs = $currentContext.tasks.Keys | foreach-object {
        if ($_ -eq "default") {
            return
        }

        if ($_ -contains '.') {
            return
        }

        $task = $currentContext.tasks.$_
        new-object PSObject -property @{
            Name = $task.Name;
            Description = $task.Description;
        }
    }
    $docs | sort 'Name' | format-table -autoSize -wrap -property Name,Description
    'Examples'
    'go build -properties @{"buildConfiguration"="Qa"}'

}