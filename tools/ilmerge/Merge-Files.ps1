
param(
    $Platform = "Debug"
)

Push-Location ('{0}\..\..\ObjectManager.Rest\bin\{1}' -f $PSScriptRoot, $Platform)
New-Item -ItemType Directory -Path ..\..\..\bin\$Patform -Force > $null
..\..\..\tools\ilmerge\ILMerge.exe /wildcards /out:..\..\..\bin\$Patform\ObjectManager.Rest.dll /target:library  /lib:.\ /targetPlatform:v4 .\ObjectManager.Rest.*.dll
Pop-Location