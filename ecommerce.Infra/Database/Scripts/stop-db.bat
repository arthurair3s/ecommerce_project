@echo off
setlocal

set BASE_DIR=%~dp0..
set BIN=%BASE_DIR%\Bin
set DATA=%BASE_DIR%\Data

echo Stopping PostgreSQL...
"%BIN%\pg_ctl.exe" -D "%DATA%" stop -m fast

echo PostgreSQL stopped.
pause
