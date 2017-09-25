@ECHO OFF
SET APP=SwitchButton
SET APP_DIR=%APP%Demo
SET LIB_DIR=%APP%Lib

ECHO Cleaning up project
ECHO ===================

:: DELETE without prompt
RMDIR .vs /s /q
RMDIR packages /s /q

RMDIR %APP_DIR%\.vs /s /q
RMDIR %APP_DIR%\bin /s /q
RMDIR %APP_DIR%\obj /s /q
RMDIR %APP_DIR%\packages /s /q


RMDIR %LIB_DIR%\.vs /s /q
RMDIR %LIB_DIR%\bin /s /q
RMDIR %LIB_DIR%\obj /s /q
RMDIR %LIB_DIR%\packages /s /q

ECHO Done.
ECHO rofiqsetiawan@gmail.com

EXIT