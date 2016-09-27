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

		This tool does not currently support encryption, decryption, compression,
		decompression or delta, you can however still edit the files.

		So each xex you run through this must have been passed through to xextool,
		Which you can grab at (http://xorloser.com/)
		Run with the command : xextool -c u -e u xex.xex
		
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


				             [ Changelog ]

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
