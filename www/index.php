<?php
	require_once ('page_template.inc');
	require_once ('userDatabase.inc');
	
	$user = null;
	$loginErr = null;

	// determine if user already logged in via cookie	
	if (isset($_COOKIE["User"])) {
		$user = getUserFromName($_COOKIE["User"]);
	}
	// if not, see if user signing in via email/password post
	if ($user == null) {
		if ((isset($_POST["email"])) && (isset($_POST["password"]))){
			$user = getUserFromEmail($_POST["email"]);
			if ($user == null)
				$loginErr = "Email or password is incorrect";
			if ($user->checkPassword($_POST["password"]) == TRUE)		
				setcookie("User", $user->name, time()+604800, "/");
			else {
				$user = null;
				$loginErr = "Email or password is incorrect";
			}
		}
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
	// if logged in write logout and menu
	if ($user != null) {
		$user->writeLogoutLine();
		writeMenu(MENUFLAG_NO_HOME);
	} 
?>
<h1>Welcome to Crossing Streams</h1>

<?php
if($user == null) {
	if ($loginErr != null)
		echo "<p>$loginErr</p>";
	echo "
<form action='index.php' name='login' method='post' target='_self' id='login'>
  <p>Use your email address and password to log-in:</p>
  <p> 
    <label>&nbsp; Email: <input name='email' type='email' id='email' size='50' required> </label> <br />
    <label>Password: <input name='password' type='password' id='password' size='50' required> </label>
  </p>
  <p>
  <input type='submit' name='Submit' value='Submit' />
  </p>
  <p>Don't have an account? Don't worry, you can also </p>
  <span class='nav_btn'> <a href='register.php'>
  Create a new Account</a></span>
</form>
";
} 
?>

<?php 
	if ($user != null) {
		echo "
	<span class='nav_btn'> <a href='downloads/CrossingStreams.jar'>
	Download the game!</a></span>";
	} else {
		echo "<p> you must be signed in to dowload the game.</p>";
	}
?>
<p>Screenshots and other promo info goes here...</p>
<p>
  <?php
	writeFooter(FOOTERFLAG_NONE);
?>
</p>
<p>&nbsp;</p>
</body>
</html>