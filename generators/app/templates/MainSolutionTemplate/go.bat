@echo off
powershell -NoProfile -ExecutionPolicy unrestricted -Command "iex tools\installtools.ps1" 
tools\nant\nant.exe -buildfile:MainSolutionTemplate.build %*