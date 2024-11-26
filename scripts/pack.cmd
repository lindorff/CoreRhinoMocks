@echo off

set root=%~dp0\..
set version=%1
set packableproject=%root%\Rhino.Mocks\Rhino.Mocks.csproj
set nuggiedestination=%root%\build

if "%version%"=="" (
	echo Please specify which version to build
	goto exit_fail
)

where dotnet.exe
if not "%errorlevel%"=="0" (
	echo Could not find dotnet.exe in the current PATH!
	goto exit_fail
)

if not exist "%packableproject%" (
	echo Could not find project to pack here:
	echo %packableproject%
	goto exit_fail
)

if exist "%nuggiedestination%" (
	rd "%nuggiedestination%" /s/q
)

mkdir "%nuggiedestination%"

dotnet pack "%packableproject%" -c Release -o "%nuggiedestination%" -p:PackageVersion=%version%
if not "%errorlevel%"=="0" (
	echo dotnet pack failed
	goto exit_fail
)

git tag %version%
if not "%errorlevel%"=="0" (
	echo Could not create GIT tag %version%
	goto exit_fail
)

git push origin %version%


:exit_success
exit /b 0

:exit_fail
exit /b 1