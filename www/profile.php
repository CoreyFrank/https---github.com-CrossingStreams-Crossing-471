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
 * BT-201 Site Stats (Billy)
*/
	require_once ('page_template.inc');
	require_once ('userDatabase.inc');
	require_once ('userStatsDatabase.inc');

	// get user info or send user to login screen
	$user = validateLoggedInOrGotoLogin();
	
?>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Crossing Streams - Profile</title>
<link rel="stylesheet" type="text/css" href="css/styles.css" />
<?php
	writeHeadScripts(HEADFLAG_NONE);
?>
</head>
<body>
<?php
	writeHeader(HEADERFLAG_NONE);
	$user->writeLogoutLine();	
	writeMenu(MENUFLAG_NO_PROFILE);
?>
<h1>Profile</h1>
<p>Additional profile information goes here...</p>

<h2>Game statistics</h2>
<table border='1'>
<?php

	$stats = getStatsForAllGames($user);
	$numRows = $stats->num_saves + 1;

	// table heading
	echo "<tr>\n<th>Statistics</th>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->save_name[$cntr];
		echo "<th> $temp </th>\n";
	}
	echo "</tr>\n";
	
	// display monster killed stats row
	echo "<tr>\n<td>Monsters killed</td>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->monsters_killed[$cntr];
		echo "<td> $temp </td>\n";
	}
	echo "</tr>\n";

	// display coins collected stats row
	echo "<tr>\n<td>Coins collected</td>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->coins_collected[$cntr];
		echo "<td> $temp </td>\n";
	}
	echo "</tr>\n";

	// display power ups stats row
	echo "<tr>\n<td>Power-ups collected</td>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->power_ups_collected[$cntr];
		echo "<td> $temp </td>\n";
	}
	echo "</tr>\n";

	// display hits taken stats row
	echo "<tr>\n<td>Hits taken</td>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->hits_taken[$cntr];
		echo "<td> $temp </td>\n";
	}
	echo "</tr>\n";

	// display time stats row
	echo "<tr>\n<td>Time played</td>\n";
	for ($cntr = 0; $cntr < $numRows; ++$cntr) {
		$temp = $stats->parseTimePlayed($cntr);
		echo "<td> $temp </td>\n";
	}
	echo "</tr>\n";

?>
</table>
<p>Any additional info goes here...</p>
<p>
  <?php
	writeFooter(FOOTERFLAG_NONE);
?>
</p>
<p>&nbsp;</p>
</body>
</html>