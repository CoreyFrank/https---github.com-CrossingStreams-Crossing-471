<?php
/*
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Logging out by removing cookies
 * 
 * Versions:
 * BT-74As a user: I want to be able to log-out of the website
*/
setcookie("User", "", time()-1, "/");
?>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Untitled Document</title>
<link href="css/styles.css" rel="stylesheet" type="text/css">
<meta http-equiv="refresh" content="5;URL=index.php">
</head>

<body>
<?php
	require ('page_template.inc');
	writeHeader(HEADERFLAG_NONE);
?>
<p>You have been logged out successfully. You should be returned
to the log-in page in a few seconds. If this does not happen,
<a href="index.php">click here.</a>
<?php writeFooter(FOOTERFLAG_NONE); ?>
</body>
</html>