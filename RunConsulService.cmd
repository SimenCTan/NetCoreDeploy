@echo off
cd ConsulService 
call dotnet run -c Release  --urls "http://127.0.0.1:5300"