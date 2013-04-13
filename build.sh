#!/usr/bin/env bash
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Release" CubicleWarsLibrary.csproj
nunit-console ./bin/Release/CubicleWarsLibrary.dll
cp bin/Release/CubicleWarsLibrary.dll ../CubicleWarsPrototype/Assets
cp bin/Release/Stateless.dll ../CubicleWarsPrototype/Assets
