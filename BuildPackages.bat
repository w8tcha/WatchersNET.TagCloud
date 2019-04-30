@SET FrameworkSDKDir=
@SET PATH=%FrameworkDir%;%FrameworkSDKDir%;%PATH%
@SET LANGDIR=EN
@SET MSBUILDPATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MsBuild.exe"

%MSBUILDPATH% WatchersNET.TagCloud.sln /p:Configuration=Deploy /t:Clean;Build /p:WarningLevel=0 /flp1:logfile=errors.txt;errorsonly