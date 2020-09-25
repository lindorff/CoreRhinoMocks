$assemblyPath = "Rhino.Mocks\bin\debug\net45\"
$rhinoMocksName = "Rhino.Mocks.dll"
$castleName = "Castle.Core.dll"
.\Tools\ILRepack.exe "$assemblyPath$rhinoMocksName" "$assemblyPath$castleName" `
  /out:Rhino.Mocks.dll `
  /t:library `
  "/keyfile:ayende-open-source.snk" `
  "/internalize:ilrepack.exclude"