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
$GLOBALS['email_username'] = ""; // ide kerül az email cím
$GLOBALS['email_password'] = ""; // ide kerül a jelszó
$GLOBALS['email_port'] = 465; // ide kerül a port
$GLOBALS['email_host'] = ""; // ide kerül a kiszolgáló címe


function Redirect($url = "/"){
    header("location: " . $GLOBALS["DEFAULT_URL"] . $url);
    exit;
}