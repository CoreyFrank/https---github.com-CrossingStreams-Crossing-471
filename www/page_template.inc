<?php
/*
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Standard page template to generate the pages. Changing this chnages all pages
 * using this template without having to do anything to those pages.
 * 
 * Versions:
 *
*/

// note: flags should be bits (1,2,4,8,16...)
// *** FLAGS for HEAD section scripts to include in page ***
define ('HEADFLAG_NONE', 0);

// *** FLAGS for Header section for whatever options (if any) come up ***
define ('HEADERFLAG_NONE', 0);
define ('HEADERFLAG_HIDE_LOGOUT', 1);

// *** FLAGS for which menu items to deactivate ***
define ('MENUFLAG_NONE', 0);
define ('MENUFLAG_NO_HOME', 1);
define ('MENUFLAG_NO_PROFILE', 2);

// *** FLAGS for footer - footer based scripts that need to be included ***
define ('FOOTERFLAG_NONE', 0);

/* Simple utility that uses depth to prepend ../'s to path to
form the proper name for items */
function buildPathAtDepth($name, $depth) {
	$result = "";
	for ($cntr = 0; $cntr < $depth; ++$cntr)
		$result .= "../";
	$result .= $name;
	return $result;
}

/* write any scripts that the page requests (via flags)
*/
function writeHeadScripts($script_flags, $depth = 0) {
	// Currently no head based scripts being used
	echo "<!-- Crossing Streams Scripts go here... -->\n";
}

/* write any scripts that the page requests (via flags)
*/
function writeHeader($header_flags, $depth = 0) {
	// Currently no head based scripts being used
	echo "<!-- Crossing Streams header -->\n";
	echo "<header>\n";
	$path = buildPathAtDepth("images/banner.png", $depth);
	echo "<img src=\"$path\" alt=\"Crossing Streams Logo\" />\n";
	
	// Process additional header flags here...
	
	// finish header section
	echo "</header>\n";
}

/* write any scripts that the page requests (via flags)
*/
function writeMenu($menu_flags, $depth = 0) {
	// Currently no head based scripts being used
	echo "<!-- Crossing Streams menu go here... -->\n";
	echo "<nav>\n";
	
	// When we have a menu, write it here using flags to determine which item to disable

	// HOME (index and login)
	$temp = buildPathAtDepth("index.php", $depth);
	if (($menu_flags & MENUFLAG_NO_HOME) > 0)
		echo "<span class='menu_disabled'> Home </span>\n";
	else
		echo "<span class='menu_btn'><a href='$temp'> Home </a></span>\n";

	// Profile
	$temp = buildPathAtDepth("profile.php", $depth);
	if (($menu_flags & MENUFLAG_NO_PROFILE) > 0)
		echo "<span class='menu_disabled'> Profile </span>\n";
	else
		echo "<span class='menu_btn'><a href='$temp'> Profile </a></span>\n";

	// Note : Additional menu items should be done like profile was
	
	// finish navigation section
	echo "</nav>\n";
}

/* write footer (copyright, minor links, and any bottom of
page scripts (via flags)
*/
function writeFooter($script_flags, $depth = 0) {
	// Currently no head based scripts being used
	echo "<!-- Crossing Streams footer and end of page scripts go here... -->";
	echo "<footer>\n";
	echo "<p>Copyright 2016 &copy; COSC 470 Team B (2016). All Rights Reserved.</p>\n";
	// finish footer section
	echo "</footer>\n";	
	
	// write end of page scripts here when they exist
}

?>