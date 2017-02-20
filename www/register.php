<?php
/*
 * COSC 470 2016 Team B project
 * Team: Ben, Billy, Corey, Daniel, Marc
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Creating an account
 * 
 * Versions:
 * BT-75As a user: I want to be able to sign-up for an account 
*/
	require_once ('page_template.inc');
	require_once ('userDatabase.inc');
	
	// Start out assuming not signed up and no issues with last registration attempt
	$user = null;
	$errorMessage = null;
	
	// See if sign up fields have been posted (in which case we are creating user)
	if (	(isset($_POST["user"])) &&
			(isset($_POST["email"])) &&
			(isset($_POST["password"])) &&
			(isset($_POST["pass2"])) ) {
		// verify passwords at least 4 characters
		$pass1 = $_POST["password"];
		if (strlen($pass1) < 4)
			$errorMessage = "Password must be at least 4 characters";
		// verify passwords match
		if (strcmp($pass1, $_POST["pass2"]) != 0 )
			$errorMessage = "Passwords must match";

		// verify user doen't already exist
		$user = getUserFromName($_POST["user"]);
		if ($user != null) {
			$errorMessage = "User name is already in use!";
		} else {
			// verify email doesn't already exist
			$user = getUserFromEmail($_POST["email"]);
			if ($user != null) {
				$errorMessage = "Email is already in use!";
			}
		}
		
		// if sign-in form is valid then add user to the database
		if ($errorMessage == null) {
			$user = createUserInDatabase($_POST["user"], $_POST["email"], $pass1);
			if ($user != null)
				setcookie("User", $user->name, time()+604800, "/");
			else
				$errorMessage = "Unable to create account. Try again later.";
		}
			else $user = null;
	}
			
?>

<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Crossing Streams - Home</title>
<link rel="stylesheet" type="text/css" href="css/styles.css" />
<?php
	writeHeadScripts(HEADFLAG_NONE);
?>
</head>
<body>
<?php
	writeHeader(HEADERFLAG_NONE);
	if ($user == null) {
		echo "
<h1>Please fill out the form to register</h1>
<form action='register.php' name='register' method='post' target='_self' id='register'>
  <p> ";

		if ($errorMessage != null)
			echo "<p>$errorMessage</p>\n";
		
		echo "
    <label>user name: <input name='user' type='text' id='user' size='20' required> </label> <br />
    <label>&nbsp; Email: <input name='email' type='email' id='email' size='50' required> </label> <br />
    <label>password: <input name='password' type='password' id='password' size='50' required> </label> <br />
    <label>verify password: <input name='pass2' type='password' id='pass2' size='50' required> </label> <br />
  </p>
  <p>
  <input type='submit' name='Submit' value='Submit' />
  </p>

</form>
";

	} else {

		echo "
<h1>Welcome to Crossing Streams</h1>
<p>
User ID: $user->userID <br />
User Name: $user->name <br />
Password: $user->password <br />
EMail: $user->email
</p>
<span class=\"nav_btn\"><a href=\"index.php\">Continue</a></span>
		";		
	}
?>
<p>
  <?php
	writeFooter(FOOTERFLAG_NONE);
?>
</p>
<p>&nbsp;</p>
</body>
</html>