all: help

help:
	@type <<
nmake build    Build the installer.
nmake clean    Clean all intermediate files.
nmake help     Show help for this Makefile. (default target)

<<

MAKENSIS_EXE="C:\Program Files (x86)\NSIS\makensis.exe"

build:
	$(MAKENSIS_EXE) msrdcui.nsi

clean:
	del *.exe
