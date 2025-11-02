# DMX for Gamers

## Editing of Help Files

Use *HelpMaker Help Authoring Tool* to edit the 'DMXForGamersHelp.sh5' file: https://sourceforge.net/projects/helpmaker/

Compile the SH5 to a CHM file and replace what is in 'DMXForGamers' folder.

## WPF and Images

Images in WPF need to be 96 DPI. This can be changed via a scaling function in a graphics application like GIMP.

## Building

- Make sure a valid path to 'signtool.exe' is in the 'PATH' environment variable on the development machine.
	- *Windows SDK* may need to be installed for 'signtool.exe' to be present on the development machine.
- Make sure to update the certificate thumbnail in 'signfile.bat'.
- Make sure to build NSIS install using elevated privileges.  Otherwise 'signtool' will fail with "After private key filter, 0 certs were left"