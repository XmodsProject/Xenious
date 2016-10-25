 ______     .--`      .--`                            --.                                             ______
|  ____|     `--`    --.       ```                    ``                                             |____  |
| |            --- `--`     .-------`   .--.-----.    --.    .------.    .-.     .-`   `-------`          | |
| |             .---.      --.    `--.  .--`    --.   --.  `--.    .--`  .--     .-.  .--`    ``          | |
| |             .:-:-    `-::------::.  .:-     -:.   -:.  .:-      -:.  -:-     -:.  `-:-...`            | |
| |           `/+- .//.   `++`          :+/     :+-   /+-  .+/      /+-  :+:     :+-    `..-:/+-          | |
| |          -so.   .os/   /so-`   .:-  /s+     +s:   os:  `os/`  `:so`  :so.  `-os:  .-`    :so          | |
| |____     /so`     `+s+`  -+syyyys+-  /s+     +s:   os/    :shhhhs/`    /yhhhs+os:  -syhhhhs/`      ____| |
|______|                                                        ``          ``           ```         |______|
 
                                    The Xbox 360 Xenon Executable Editor
					By [ Hect0r ] / staticpi.net

				Contact : staticpi.net / sysop@staticpi.net

					   [ Basic Overview ]

		The project ahead is to create a fully working editor for the Xenon Executable
		file, That includes a pe editor, right now the tool lacks some research.

				           [ * PLEASE NOTE * ]

		This tool uses xextool by xorloser to decompress/decrypt the executable,
		just create a folder in the applications directory called "bin", then place
		xextool.exe in there.

		Which you can grab at (http://xorloser.com/)
		
		This is only for full browsing, like pe sections, extracting resources.

				       [ What can you do to help ]

		If you get this error : 
	     "This executable has option header data that is currently unsupported by this editor..."
					          OR
    				"It appears this executable is different..."
		Then please compress the file with winrar and email it to me, So I can investigate 
			it as you might have found a new header I dont know of.

			            Any SDK > 21256.3, I wont leak :)

		Please consider donating some spare crypto to cover development costs.
		
		For source code, read DEBUG..., it is easy to build and debug :)


				             [ Changelog ]
					     
				     0.0.2380.0 [Beta] (24/10/1026)
				     Added Proof of concept XEX Editor (ASM)
				     Added Launcher, Now select the tools.
			       Added Ability to edit Callcap data in metaeditor.
					     
			             0.0.2101.0 [Beta] (28/08/2016)
				Fixed 2 bugs that where left from the previous
				build, sorry for that :)

				     0.0.2100.0 [Beta] (28/08/2016)
				Added support for xextool, now you can load
				compressed / encrypted xexs and still view 
				and edit the file. NOTE the original file will
				be decrypted and decompressed, no backups.
				
				Fixed bugs in GUI, mainly Numeric editing 
				optional headers, they wouldent load.
				
				Fixed a buf in Xbox360.
			             0.0.1175.0 [Beta] (27/09/2016)
				Added the ability edit most of the optional
				headers. 
				
				Few bug fixes in the Xbox 360 libary where
				it wouldent load certain types of xex's with
				single import libary kernals.
				
			             0.0.1050.0 [Beta] (27/09/2016)
				Added update feature, will tell you whenever
				there is a new version and downloads it.
				
			             0.0.1025.0 [Beta] (27/09/2016)
				Added the ability to save encrypted/compressed
				files edits.
				
				Fixed bug where resource would still extract
				(encrypted), Now disabled when loading a encrypted
				/ compressed executable.
				
				     0.0.1015.0 [Beta] (26/09/2016)
				Moved all Xbox 360 code into a Dynamic Libary
				called Xbox360.
				
				     0.0.1010.0 [Beta] (26/09/2016)
				Added Support for Alternative Title IDs.
				Added Support for Other Certificate Info.
				Added Support for TLS Info.
				Added Support for Hash Table / XeSections.
				Added Ability to save Certificate Other,
				RegionFlags, MediaFlags, ImageFlags, SystemFlags,
				ModuleFlags, Execution info (Minus Versions),
				RatingsData, Alternative Title IDs.

				Added support for further releases.

				Fixed bug where gui would flip bytes instead of
				following the endian of the machine.

				Few cleanups of the code and more.

				      0.0.780.0 [Beta] - (25/09/2016)
				Inital Release.
