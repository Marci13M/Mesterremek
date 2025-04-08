<?php

require_once("config.php");

session_start();
session_unset();
session_destroy();

if(isset($_GET["redirect_url"])){
    header("location: " . htmlspecialchars($_GET["redirect_url"]));
} else {
    header("location: " . $GLOBALS["DEFAULT_URL"]);
}