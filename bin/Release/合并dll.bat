move JJStart.exe temp.exe
..\..\packages\ILMerge.2.14.1208\tools\ilmerge /targetplatform:v2 /ndebug /target:winexe /out:JJStart.exe temp.exe /log DevComponents.DotNetBar2.dll
del temp.exe