<?php
/*
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Routines for handling the logging in and managing of the users database table.
 * 
 * Versions:
 * Sprint 1 - All website tasks
 * Sprint 2 - 
*/

	/* Class for holding the user information for logging in (Billy)*/
	class CSUser {
		// Attributes (columns) stored in user database
		public $userID = 0;
		public $name = "";
		public $password = "";
		public $email = "";

		/* routine to compare the provided password with the one stored in 
		 * the database. Hashing used to make sure passwords in database
		 * are not easy to obtain in case of breach of databse */
		function checkPassword($pass) {
			$hashedPassword = hash('sha384', $this->name . $pass);
			if (strcmp($this->password, $hashedPassword) == 0)
				return TRUE;
			else
				return FALSE;
		}
		
		function writeLogoutLine() {
			echo "<p>Welcome back " . $user->name . " at " . $user->email;
			echo "<a href=\"logout.php\">Not you, click here.</a>\n";
			echo "<span class=\"nav_btn\"><a href=\"logout.php\">Logout</a></span></p>\n";
		}
	}
	
	/* if not the home page, need to be logged in so if no cookie, go to login page
		otherwise return the CSUer (Billy) */
	function validateLoggedInOrGotoLogin() {
		$user = null;
		
		// determine if user already logged in via cookie	
		if (isset($_COOKIE["User"])) {
			$user = getUserFromName($_COOKIE["User"]);
			// Note(Billy) may want additional fields in cookie, as well as some way
			// of making sure that cookies have not been faked
		}

		// if the user doesn't exst, redirect to start screen and exit script
		if ($user == null) {
			echo "
<!doctype html>
<html>
<head>
<meta http-equiv='refresh' content='0;URL=index.php'>
</head>
<body>
<p>You are not logged in. You should be returned
to the log-in page in a few seconds. If this does not happen,
<a href='index.php'>click here.</a>
</body>
</html>
			";
			exit;
		}
		
		// if this is reached, valid user, so return it
		return $user;
	}

	/* Opens a connection to PostgreSQL database. Note that the connection 
	 * information should probably be stored outside the www directory 
	 * somewhere and read in for better security.  (Billy) */ 
	function openConnection() {
		$dbconn = pg_connect("host=localhost dbname=crossingdatabase user=web_admin password=letmein");
		return $dbconn;
	}

	/* internal routine to take a result from the database (users table) and 
	 * convert it into a CSUser object. 
	 * This method is used by the getUserFromEmail and getUserFromName 
	 * functions which select data from database in set order. 
	 * Returns CSUser from database or null if invalid user (Billy) */
	function buildUserFromResults($result) {
		$user = null;
		// if database read was successful, will be a rowcount of 1
		$rowCount = pg_num_rows($result);
		if ($rowCount > 0) {
			// get row from results and fill out user object fields
			$row = pg_fetch_row($result);
			$user = new CSUser();
			$user->userID = $row[0];
			$user->name = $row[1];
			$user->password = $row[2];
			$user->email = $row[3];
		}
		// return user object back to caller
		return $user;
	}

	/* Routine to get user object from the database using the user_name field (Billy)*/
	function getUserFromName($name) {
		$user = null;
		
		// open database, returning null if unable to connect
		$dbconn = openConnection();
		if ($dbconn == FALSE) {
			echo "Not able to connect to database...";
			return null;
		}
		
		// create stored procedure to prevent sql attacks
		$prep = pg_prepare($dbconn, "userbyname", 
				'SELECT id, user_name, password, email FROM users WHERE user_name = $1');

		// execute stored procedure using user name provided
		$result = pg_execute($dbconn, "userbyname", array($name));
		if ($result == FALSE)
			echo "FAILED TO execute " . pg_last_error();
		else
			$user = buildUserFromResults($result);

		// clean up database connection
		pg_free_result($prep);
		pg_free_result($result);
		pg_close($dbconn);

		return $user;
	}

	/* Routine to get user object from the database using the email field (Billy)*/
	function getUserFromEmail($email) {
		$user = null;
		
		// open database, returning null if unable to connect
		$dbconn = openConnection();
		if ($dbconn == FALSE) {
			echo "Not able to connect to database...";
			return null;
		}
		
		// create stored procedure to prevent sql attacks
		$prep = pg_prepare($dbconn, "userbyemail", 
				'SELECT id, user_name, password, email FROM users WHERE email = $1');

		// execute stored procedure using email provided
		$result = pg_execute($dbconn, "userbyemail", array($email));
		if ($result == FALSE)
			echo "FAILED TO execute " . pg_last_error();
		else
			$user = buildUserFromResults($result);
		
		// clean up database connection
		pg_free_result($prep);
		pg_free_result($result);
		pg_close($dbconn);

		return $user;
	}

	/* Create a new user in the database. Returns user created, null if error (Billy)*/
	function createUserInDatabase($name, $email, $pass) {
		// database connection
		$dbconn = openConnection();
		if ($dbconn == FALSE) {
			echo "Not able to connect to database...";
			return null;
		}
		
		// create salted hash
		$hashedPassword = hash('sha384', $name . $pass);
		
		// create stored procedure to prevent sql attacks
		$prep = pg_prepare($dbconn, "createuser", 
				'INSERT INTO users (user_name, password, email) VALUES ($1, $2, $3);');
		$result = pg_execute($dbconn, "createuser", array($name, $hashedPassword, $email));
		if ($result == FALSE)
			echo "FAILED TO execute " . pg_last_error();

		// clean up database connection			
		pg_free_result($prep);
		pg_free_result($result);
		pg_close($dbconn);
	
		return getUserFromName($name);
	}
?>
