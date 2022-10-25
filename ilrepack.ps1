param([string]$assemblyPath)

$rhinoMocksName = "Rhino.Mocks.dll"
$originalRhinoMocksName = "Rhino.Mocks.dll.original"
$castleName = "Castle.Core.dll"

Move-Item "$assemblyPath$rhinoMocksName" "$assemblyPath$originalRhinoMocksName"

.\Tools\ILRepack.exe "$assemblyPath$originalRhinoMocksName" "$assemblyPath$castleName" `
  /out:"$assemblyPath$rhinoMocksName" `
  /xmldocs `
  /t:library `
  "/keyfile:ayende-open-source.snk" `
  "/internalize:ilrepack.exclude"
