#!/usr/bin/env bash
/Applications/Unity/MonoDevelop.app/Contents/MacOS/mdtool -v build "--configuration:Release" CubicleWarsLibrary.csproj
nunit-console ./bin/Release/CubicleWarsLibrary.dll
cp ./bin/Release/CubicleWarsLibrary.dll ../CubicleWarsForPresentation/Assets/Source
cp ./bin/Release/Stateless.dll ../CubicleWarsForPresentation/Assets/Source
