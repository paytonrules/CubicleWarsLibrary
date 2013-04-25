#!/usr/bin/env bash
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Release" CubicleWarsLibrary.csproj
nunit-console ./bin/Release/CubicleWarsLibrary.dll
