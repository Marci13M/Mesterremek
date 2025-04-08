<?php 
ini_set('session.cookie_secure', 1);
ini_set('session.cookie_httponly', 1);
session_start();


$GLOBALS['API_DEFAULT_URL'] = 'http://localhost/api/v1/api.php';
$GLOBALS['DEFAULT_URL'] = "http://localhost";
$GLOBALS['DEFAULT_PATH'] = realpath(dirname(__FILE__));

// adatbázis adatok
$GLOBALS["database_host"] = "localhost";
$GLOBALS["database_name"] = "carecompass";
$GLOBALS["database_user"] = "root";
$GLOBALS["database_password"] = "";

// email
$GLOBALS['email_username'] = "carecompass@nyikonyuk.hu";
$GLOBALS['email_password'] = "?tDeh^Yv=t88"; //awrclqvgxouheabk
$GLOBALS['email_port'] = 465;
$GLOBALS['email_host'] = "mail.nyikonyuk.hu";


function Redirect($url = "/"){
    header("location: " . $GLOBALS["DEFAULT_URL"] . $url);
    exit;
}