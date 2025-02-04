@echo off
set projectPath=%~dp0
set projectPath=%projectPath:~0,-1%

C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File "%projectPath%\BancoDigitalAPI\start-variaveis-sistema.ps1" -projectPath "%projectPath%"
pause