@echo off

rem ------------ (1) Set name of project here ------------------------------------------------------
set name=Tababular
rem ------------------------------------------------------------------------------------------------

set version=%1
set currentdir=%~dp0
set root=%currentdir%\..
set toolsdir=%root%\tools
set ilmerge=%toolsdir%\Ilmerge\ilmerge.exe
set nuget=%toolsdir%\NuGet\NuGet.exe
set projectdir=%root%\%name%
set releasedir=%projectdir%\bin\Release
set mergedir=%releasedir%\merged
set deploydir=%root%\deploy
set projectfile=%projectdir%\%name%.csproj
set nuspecfile=%projectdir%\%name%.nuspec

if "%version%"=="" (
	echo Please specify which version to build as a parameter.
	echo.
	goto exit
)

echo This will build, tag, and release version %version% of %name%.
echo.
echo Please make sure that all changes have been properly committed!
pause

if exist "%deploydir%" (
	echo Cleaning up old deploy dir %deploydir%
	rd %deploydir% /s/q
)

echo Building version %version%

msbuild %projectfile% /p:Configuration=Release

rem ------------ (2) Comment in/out depending on merge demands -------------------------------------
REM goto skipmerge
rem ------------------------------------------------------------------------------------------------

if exist "%mergedir%" (
	echo Cleaning up old merge dir %mergedir%
	rd %mergedir% /s/q
)

echo Creating merge dir %mergedir%
mkdir %mergedir%

rem ------------ (3) If merging: Customize this part -----------------------------------------------

echo Merging
echo.
echo     %releasedir%\%name%.dll
echo     %releasedir%\Newtonsoft.Json.dll
echo.
echo into
echo.
echo     %mergedir%\%name%.dll
echo.

%ilmerge% /out:%mergedir%\%name%.dll %releasedir%\%name%.dll %releasedir%\Newtonsoft.Json.dll /targetplatform:"v4,%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /internalize

rem ------------------------------------------------------------------------------------------------

:skipmerge

echo Packing...

echo Creating deploy dir %deploydir%
mkdir %deploydir%

%nuget% pack %nuspecfile% -OutputDirectory %deploydir% -Version %version%

echo Tagging...

git tag %version%

echo Pushing to NuGet.org...

%nuget% push %deploydir%\*.nupkg

git push --tags

:exit