MSBuild /p:Configuration=Release

dotnet pack -o ..\artifacts --include-symbols -c Release .\HessianCSharp
nuget init .\artifacts\ D:\yuniansheng\Downloads\nuget\packages