@SET FrameworkDir=C:\Windows\Microsoft.NET\Framework\v4.0.30319
@SET FrameworkVersion=v4.0.30319
@SET FrameworkSDKDir=
@SET PATH=%FrameworkDir%;%FrameworkSDKDir%;%PATH%
@SET LANGDIR=EN

msbuild.exe WatchersNET.TagCloud.sln /p:Configuration=Deploy /t:Clean;Build /p:WarningLevel=0 /flp1:logfile=errors.txt;errorsonly