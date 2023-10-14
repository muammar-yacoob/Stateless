@echo off

rem Check if the Doxygen configuration file (DoxyConfig.ini) exists
if not exist DoxyConfig.ini (
  rem Generate the Doxygen configuration file if it doesn't exist
  doxygen -x_noenv DoxyConfig.ini
  echo Doxygen configuration file DoxyConfig.ini created
)

rem Check if the "html" directory exists
if not exist html\ (
  rem Create the "html" directory if it doesn't exist
  md html
  echo html directory created
)

rem Set the path to Graphviz's bin directory
set GRAPHVIZ_PATH=..\Graphviz\bin

rem Specify the full path to the dot executable
set DOT_EXECUTABLE=%GRAPHVIZ_PATH%\dot.exe

rem Check if the dot executable exists
if not exist "%DOT_EXECUTABLE%" (
  echo Error: Graphviz dot executable not found.
  exit /b 1
)

rem Run Doxygen with the specified configuration file and increased verbosity
echo Running Doxygen with increased verbosity...
"%DOT_EXECUTABLE%" -version
echo ---
doxygen -v=2 DoxyConfig.ini 2> DoxygenErrors.log

rem Check if there were any errors during the Doxygen run
if exist DoxygenErrors.log (
  echo Errors were found during Doxygen execution. See DoxygenErrors.log for details.
  exit /b 1
)

echo Doxygen completed successfully.
