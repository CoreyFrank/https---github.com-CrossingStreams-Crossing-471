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
 * BT-200 Database table & BT-201 Stats page (Billy)
*/

	require_once ('userDatabase.inc');

	/* Object for holding the stats for the various saves */
	class CSUserStats {
		// Variables to hold the user stats table fields
		public $userID = 0;
		public $num_saves = 0;
		public $save_id = array(0);
		public $save_name = array("Global");
		public $monsters_killed = array(0);
		public $coins_collected = array(0);
		public $power_ups_collected = array(0);
		public $hits_taken = array(0);
		public $seconds_played = array(0);

		/* Take seconds played and convert to printable string */
		function parseTimePlayed($indx) {
			$temp = $this->seconds_played[$indx];

			// Find the seconds
			$secs = $temp % 60;
			$temp -= $secs;
			$temp /= 60;

			// Find the minutes
			$mins = $temp % 60;
			$temp -= $mins;
			$temp /= 60;

			// find the hours
			$hours = $temp % 24;
			$temp -= $hours;
			$temp /= 24;
			
			// find the days
			$days = $temp;
			if ($days > 0)
				$timeString = $days . ':' . $hours . ':' . $mins;
			else
				$timeString = $hours . ':' . $mins;
			if ($secs < 10)
				$timeString .= ':0';
			else
				$timeString .= ':';
			$timeString .= $secs;
				
			return $timeString;
		}
	}

	/* Return an array of CSUserStats for each save game stored 
	 * for the indicated CSUser. */
	function getStatsForAllGames($user) {
		$stats = new CSUserStats();
		
		// Open a connection to the database
		$dbconn = openConnection();
		if ($dbconn == FALSE) {
			echo "Not able to connect to database...";
			return null;
		}

		// build stored procedure for getting stats for indicated user
		$prep = pg_prepare($dbconn, "stats_for_user", 
				'SELECT save_id, save_name, monsters_killed, coins_collected, power_ups_collected, hits_taken, seconds_played FROM user_stats WHERE id = $1');

		if ($prep == null)
			echo "FAILED TO create  " . pg_last_error();
		
		// execute the query
		$result = pg_execute($dbconn, "stats_for_user", array($user->userID));
		if ($result == FALSE)
			echo "FAILED TO execute " . pg_last_error();
		else {
		
			$rowCount = pg_num_rows($result);
			if ($rowCount > 0) {
				$stats->userID = $user->userID;
				$stats->num_saves = $rowCount - 1;
				// Loop through all the saves (0 is global save) and add to stats object
				for ($cntr = 0; $cntr < $rowCount; ++$cntr) {
					$row = pg_fetch_row($result);
					// copy data from row into class
					$stats->save_id[$cntr] = $row[0];
					$stats->save_name[$cntr] = $row[1];
					$stats->monsters_killed[$cntr] = $row[2];
					$stats->coins_collected[$cntr] = $row[3];
					$stats->power_ups_collected[$cntr] = $row[4];
					$stats->hits_taken[$cntr] = $row[5];
					$stats->seconds_played[$cntr] = $row[6];
				}
			}
			
		}
		return $stats;
	}
	
?>
