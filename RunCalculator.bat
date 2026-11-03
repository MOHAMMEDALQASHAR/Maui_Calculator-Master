@echo off
title Maui Calculator Runner
cd /d "%~dp0MauiCalculator"
echo Starting Maui Calculator...
dotnet run -f net8.0-windows10.0.19041.0
pause

