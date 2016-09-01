Framework "4.0"

#
# properties
#

properties {
    $buildConfiguration = 'debug'
    $buildDirectory = 'build'
    $buildReportsDirectory =  Join-Path $buildDirectory 'reports'
    $buildPackageDirectory =  Join-Path $buildDirectory 'packages'
    $buildDistDirectory =  Join-Path $buildDirectory 'dist'

    $buildContants = ''

    $srcDirectory = 'src'
    $srcSolution = Join-Path $srcDirectory 'MainSolutionTemplate.sln'
    $srcWebFolder = Join-Path $srcDirectory 'MainSolutionTemplate.Website'

    $codeCoverRequired = 70

    $versionMajor = 0
    $versionMinor = 0
    $versionBuild = 1
    $versionRevision = 0

    $vsVersion = "12.0"

    $msdeploy = 'C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe';
    $deployServiceDest = "computerName='xxxx',userName='xxx',password='xxxx',includeAcls='False',tempAgent='false',dirPath='d:\server\temp'"
    $deployApiDest = 'auto,includeAcls="False",tempAgent="false"'
}

#
# task
#

task default -depends build  -Description "By default it just builds"
task clean -depends build.clean,build.cleanbin -Description "Removes build folder"
task build -depends build.cleanbin,version,build.compile,build.publish,build.copy,build.website -Description "Cleans bin/object and builds the project placing binaries in build directory"
task test -depends build,test.run  -Description "Builds and runs part cover tests"
task full -depends test,deploy.zip -Description "Versions builds and creates distributions"
task package -depends version,build,deploy.package -Description "Creates packages that could be user for deployments"
task deploy -depends version,build,deploy.api,deploy.service -Description "Deploy the files to webserver using msdeploy"
task appveyor -depends build, test.run, deploy.zip -Description "Deploy the files to webserver using msdeploy"

#
# task depends
#

task build.clean {
    remove-item -force -recurse $buildDirectory -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem ($srcDirectory + '\*\*') | where { $_.name -eq 'bin' -or $_.name -eq 'obj'} | Foreach-Object {$_.fullname}
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue
    }
}

task build.cleanbin {
    remove-item -force -recurse $buildReportsDirectory -ErrorAction SilentlyContinue
    remove-item -force -recurse (buildConfigDirectory) -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem $srcDirectory -include bin,obj  | Foreach-Object {$_.fullname}
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue
    }
}

task build.compile {
    'Compile '+$buildConfiguration+' version '+(srcBinFolder)
    msbuild  $srcSolution /t:rebuild /p:Configuration=$buildConfiguration /p:VisualStudioVersion=$vsVersion /v:q
    if (!$?) {
        throw 'Failed to compile'
    }
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
    $fromFolder =  Join-Path $srcDirectory (Join-Path 'MainSolutionTemplate.Console' (srcBinFolder) )
    $toFolder =  Join-Path (buildConfigDirectory) 'MainSolutionTemplate.Console'
    copy-files $fromFolder $toFolder
}


task build.website -depend gulp.build {
    $fromFolder =  Join-Path (srcWebFolder) 'dist'
    $toFolder =  (Join-Path (buildConfigDirectory) 'MainSolutionTemplate.Api/static')
    copy-files $fromFolder $toFolder
}

task gulp.build {
    'Npm install'
    pushd (srcWebFolder)
    npm install --silent
    'Bower install'
    bower install --silent
    gulp build
    popd
}

task gulp.watch {
    'Guld watch'
    pushd (srcWebFolder)
    gulp serve
    popd
}

task build.publish {
    $toFolder = Join-Path ( Join-Path (resolve-path .)(buildConfigDirectory)) 'MainSolutionTemplate.Api'
    $project = Join-Path $srcDirectory 'MainSolutionTemplate.Api\MainSolutionTemplate.Api.csproj'
    $publishProfile = "Publish - $buildConfiguration.pubxml";
    msbuild  $project /p:DeployOnBuild=true /p:publishurl=$toFolder /p:VisualStudioVersion=$vsVersion /p:DefineConstants=$buildContants /p:Configuration=$buildConfiguration /p:PublishProfile=$publishProfile /p:VisualStudioVersion=$vsVersion /v:q
    if (!$?) {
        throw 'Failed to compile'
    }
}

task build.nugetPackages -depend build {
    $packagesFolder =  $buildDistDirectory
    mkdir $packagesFolder -ErrorAction SilentlyContinue
    ./src/.nuget/NuGet.exe pack src\MainSolutionTemplate.Sdk\MainSolutionTemplate.Sdk.csproj -Prop Configuration=$buildConfiguration  -includereferencedprojects
    Move-Item -force *.nupkg $packagesFolder
 }

task publish.nuget {
    $packagesFolder =  $buildDistDirectory
    mkdir $packagesFolder -ErrorAction SilentlyContinue
   ./src/.nuget/NuGet.exe push  ( Join-Path $packagesFolder ('MainSolutionTemplate.Sdk.'+(fullversionrev)+'.nupkg'))
}

task nuget.restore {
    ./src/.nuget/NuGet.exe install src\.nuget\packages.config -OutputDirectory lib
}

task clean.database {
   $mongoDbLocations = 'D:\Var\mongodb\bin\mongo.exe','C:\mongodb\bin\mongo.exe','C:\bin\MongoDB\bin\mongo.exe','C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe','C:\Program Files\MongoDB\Server\3.2\mongo.exe'
   $mongo = $mongoDbLocations | Where-Object {Test-Path $_} | Select-Object -first 1
   $database = 'MainSolutionTemplate_Develop'
   'Use '+ $mongo + ' to drop the database '+$database
   exec { &($mongo) $database  --eval 'db.dropDatabase()' }
}

task test.run -depend  clean.database, nuget.restore -precondition { return $buildConfiguration -eq 'debug' } {
    mkdir $buildReportsDirectory -ErrorAction SilentlyContinue

    $currentPath = resolve-path '.'
    $partcoverDirectory = resolve-path 'lib\OpenCover.4.6.166\tools'
    $nunitDirectory =  resolve-path 'lib\NUnit.Runners.2.6.4\tools\nunit-console.exe'
    $reportGenerator = 'lib\ReportGenerator.2.3.2.0\tools'

    $partcoverExe = Join-Path $partcoverDirectory 'OpenCover.Console.exe'



    $runTestsTimeout = '60000'
    $runTestsDirectory = '.Tests'
    $runTestsSettings = '/exclude:Unstable /timeout:' + $runTestsTimeout

    $nunit2failed = 'false'
    $hasFailure = $FALSE
    $testFolders = Get-ChildItem $srcDirectory '*.Tests' -Directory
    foreach ($testFolder in $testFolders) {

        $runTestsFolder = Join-Path $testFolder.FullName (srcBinFolder)
        $runTestsFolderDll = Join-Path $runTestsFolder ($testFolder.Name + '.dll')

        $buildReportsDirectoryResolved = '..\..\..\'+ $buildReportsDirectory;
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
    Set-Location $reportGenerator
    $buildReportsDirectoryRelative = Join-Path '..\..\..\' $buildReportsDirectory
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
    $folders = Get-ChildItem (buildConfigDirectory) -Directory
    foreach ($folder in $folders) {
        $version = fullversion
        $zipname = Join-Path $buildDistDirectory ($folder.name + '.v.'+ $version+'.'+$buildConfiguration+'.zip' )
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
    msbuild /v:q  /t:restorepackages  /T:Package  /p:VisualStudioVersion=$vsVersion /p:Configuration=$configuration  /p:PackageTempRootDir=c:\temp  $project
    if (!$?) {
        throw 'Failed to deploy'
    }
}


task deploy.api -depends deploy.package {
    $deployWebsiteName = 'Default Web Site/MainSolutionTemplate.Api.'+$buildConfiguration
    $version = fullversion
    $toFolder = Join-Path ( resolve-path $buildPackageDirectory ) "$buildConfiguration.MainSolutionTemplate.Api.v.$version.zip"
    $skip = 'skipAction=Delete,objectName=filePath,absolutePath=Logs'
    $setParam = 'ApplicationPath='+$deployWebsiteName+''
    &($msdeploy) -source:package=$toFolder -dest:$deployApiDest -verb:sync -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -skip:$skip
    if (!$?) {
        throw 'Failed to deploy'
    }
}

task deploy.service {
    $source = 'dirPath='+( resolve-path (Join-Path (buildConfigDirectory) 'MainSolutionTemplate.Console'))
    &($msdeploy) -verb:sync -allowUntrusted -source:$source -dest:$deployServiceDest
    # &($msdeploy) -verb:sync -preSync:runCommand='D:\Dir\on\remote\server\stop-service.cmd',waitInterval=30000 -source:dirPath='C:\dir\of\files\to\be\copied\on\build\server ' -dest:computerName='xx.xx.xx.xx',userName='xx.xx.xx.xx',password='xxxxxxxxxxxxxxx',includeAcls='False',tempAgent='false',dirPath='D:\Dir\on\remote\server\'  -allowUntrusted -postSync:runCommand='D:\Dir\on\remote\server\start-service.cmd',waitInterval=30000
    if (!$?) {
        throw 'Failed to deploy'
    }
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

function srcWebFolder() {
    $possibleWebLocations = ($srcWebFolder + "2"),$srcWebFolder
    $webLocation = $possibleWebLocations | Where-Object {Test-Path $_} | Select-Object -first 1
    write-host 'Found web folder:' $webLocation -foreground "magenta"
    return $webLocation;
}


function srcBinFolder() {
    return  Join-Path bin $buildConfiguration
}

function buildConfigDirectory() {
    Join-Path $buildDirectory $buildConfiguration
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
        if ($_ -eq "default" -or $_ -eq "?") {
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

    $docs | where {-not [string]::IsNullOrEmpty($_.Description)} | sort 'Name' | sort 'Description' -Descending | format-table -autoSize -wrap -property Name,Description

    'Examples:'
    '----------'
    ''
    'Clean build directory before executing build:'
    'go clean,build'
    ''
    ''
    'Qa build:'
    'go build -properties @{''buildConfiguration''=''Qa''}'
    ''
    'Staging deploy to sepecified folder:'
    'go deploy -properties @{buildConfiguration=''Staging'';deployServiceDest =''computerName=''''xxxx'''',userName=''''xxx'''',password=''''xxxx'''',includeAcls=''''False'''',tempAgent=''''false'''',dirPath=''''d:\server\temp'''''' }'

}

