@echo off

echo PostBuild.bat START ===========

set DIR_SRC=%1
set DIR_OUTPUT=%2
set DIR_DEST=%3

rmdir /S /Q %DIR_DEST% 2>nul
mkdir %DIR_DEST%

echo Creating portable package:
echo ^ ^ Copy files from %DIR_OUTPUT%
FOR %%G IN (CodePortingTracker.exe) DO copy %DIR_OUTPUT%\%%G %DIR_DEST%

echo PostBuild.bat END ===========
