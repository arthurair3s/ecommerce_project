@echo off
setlocal

REM Define paths relative to script location
set BASE_DIR=%~dp0..
set BIN=%BASE_DIR%\Bin
set DATA=%BASE_DIR%\Data
set SHARE=%BASE_DIR%\Share

echo Initializing PostgreSQL data directory...
"%BIN%\initdb.exe" -D "%DATA%" -U postgres --encoding=UTF8 --locale=English_United_States || (
    echo ERROR: Failed to initialize database
    exit /b 1
)

echo Database initialized successfully.
pause
