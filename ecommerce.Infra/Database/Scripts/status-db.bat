@echo off
setlocal

set BASE_DIR=%~dp0..
set BIN=%BASE_DIR%\Bin
set DATA=%BASE_DIR%\Data

"%BIN%\pg_ctl.exe" -D "%DATA%" status
pause
