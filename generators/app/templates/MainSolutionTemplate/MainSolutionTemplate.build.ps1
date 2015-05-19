Framework "4.0"
properties {
    $buildConfiguration = 'debug'
    $buildDirectory = 'build'
    $buildConfigDirectory = Join-Path $buildDirectory $buildConfiguration

    $srcDirectory = 'src'
    $srcSolution = Join-Path $srcDirectory 'MainSolutionTemplate.sln'    
    $srcBinFolder = Join-Path bin $buildConfiguration
}

task default -depends build 
task clean -depends build.clean
task build -depends clean,build.compile,build.copy
task test -depends build


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


task build.clean { 
    remove-item -force -recurse $buildDirectory -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem $srcDirectory -include bin,obj -Recurse | Foreach-Object {$_.fullname}  
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue   
    }
}

task build.compile { 
    msbuild  $srcSolution /t:clean /p:Configuration=$buildConfiguration /v:q
    msbuild  $srcSolution /t:rebuild /p:Configuration=$buildConfiguration /v:q
}

task build.copy { 
    'Copy the console'
    $fromFolder =  Join-Path $srcDirectory (Join-Path 'MainSolutionTemplate.Console' $srcBinFolder )
    $toFolder =  Join-Path $buildConfigDirectory 'MainSolutionTemplate.Console'
    # copy-files $fromFolder $toFolder
    'Copy the static files'
    $fromFolder =  Join-Path $srcDirectory 'MainSolutionTemplate.Website' 
    $toFolder =  Join-Path $buildConfigDirectory 'MainSolutionTemplate.Api\static'
    

    copy-files $fromFolder $toFolder index.html,avicon.ico,robots.txt
    copy-files (Join-Path $fromFolder 'views') (Join-Path $toFolder 'views')
    copy-files (Join-Path $fromFolder 'assets') (Join-Path $toFolder 'assets')
    copy-files (Join-Path $fromFolder 'scripts\dist') (Join-Path $toFolder 'scripts\dist')
}

task build.publish { 
   
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}


function global:copy-files($source,$destination,$include=@(),$exclude=@()){    
    $sourceFullName = resolve-path $source
    $relativePath = Get-Item $source | Resolve-Path -Relative
    mkdir $destination -ErrorAction SilentlyContinue
    $files = Get-ChildItem $source -include $include -Recurse -Exclude $exclude
     foreach ($file in $files) {
       $relativePathOfFile = Get-Item $file.FullName | Resolve-Path -Relative
       $tofile = Join-Path $destination $relativePathOfFile.Substring($relativePath.length)
       $relativePathOfFile 
       $tofile
       Copy-Item -Force $relativePathOfFile $tofile
     }
}