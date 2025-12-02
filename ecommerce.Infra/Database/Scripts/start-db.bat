@echo off
setlocal enabledelayedexpansion

set PG_DIR=%~dp0..\Bin
set DATA_DIR=%~dp0..\Data
set LOG_FILE=%DATA_DIR%\logfile.log
set SCRIPT_FILE=%~dp0setup.sql

echo Starting PostgreSQL...

"%PG_DIR%\pg_ctl.exe" -D "%DATA_DIR%" -l "%LOG_FILE%" start
if %errorlevel% neq 0 (
    echo Failed to start PostgreSQL.
    exit /b 1
)

echo Waiting server startup...
timeout /t 3 > nul

echo Running initial setup...
"%PG_DIR%\psql.exe" -U postgres -h 127.0.0.1 -p 5432 -f "%SCRIPT_FILE%"
if %errorlevel% neq 0 (
    echo Failed to run setup.sql.
)

echo PostgreSQL is running.
echo Log file: %LOG_FILE%
pause
