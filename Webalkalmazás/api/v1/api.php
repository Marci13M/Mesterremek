<?php

// error_reporting(E_ALL);
// ini_set('display_errors', 1);

require_once '../../config.php';

// Adatbázis osztály meghívása
require_once './database/database.php';

// Response osztály meghívása
require_once "./classes/Response.php";

// Logger osztály meghívása
require_once "./classes/Logger.php";

// Token osztály meghívása
require_once "./classes/Token.php";

// Mailer osztály meghívása
require_once "./classes/Mailer.php";

// Naptáresemény generátor meghívása
require_once "./classes/ICS.php";

// Függvények betöltése
require_once "./functions.php";



// Új adatbázis kapcsolat létrehozása
$db = new Database($GLOBALS['database_host'], $GLOBALS['database_name'], $GLOBALS['database_user'], $GLOBALS['database_password']);
$conn = $db->connect();

// Új naplózó létrehozása
$logger = new Logger($conn);

// Az útvonal kinyerése
$requestUri = $_SERVER['REQUEST_URI'];
$requestUri = str_replace("/mesterremek", "", $requestUri); //kiszedi a link azon részét, amely rámutat a györkérmappára, amiben a program elhelyezkedik
// var_dump($requestUri);
$uriSegments = explode('/', trim($requestUri, '/')); // backslash kicserélése / jelre

// Ellenőrizzük, hogy a megfelelő útvonalon vagyunk
if ($uriSegments[0] === 'api' && $uriSegments[1] === 'v1' && $uriSegments[2] === 'api.php') {
    $resource = $uriSegments[3] ?? null; // Erőforrás
    $method = $_SERVER['REQUEST_METHOD']; // Lekérdezzük a HTTP metódust

    switch ($resource) {
        case 'auth': // publikusan elérhető
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/auth.php'; // Az autentikációs logika
            break;
        case 'statistics': // privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/statistics.php'; // A statisztikák logikája
            break;
        case 'doctors': // privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/doctors.php'; // Orvosadatok logikája
            break;
        case 'hospitals': //privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/hospitals.php'; // Kórházak logikája
            break;
        case 'companies': //privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/companies.php'; // Cégek logikája
            break;
        case 'users': //privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/users.php'; // Felhasználók logikája
            break;
        case 'services': //privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/services.php'; // Szolgáltatások logikája
            break;
        case 'appointments': //privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/appointments.php'; // időpontok logikája
            break;
        case 'data': //privát végpont
            $type = $uriSegments[4] ?? null;
            $function = $uriSegments[5] ?? null;
            require_once './endpoints/data.php'; // adatok logikája
            break;
        case 'calendar': // privát végpont
            $function = $uriSegments[4] ?? null;
            require_once './endpoints/calendar.php'; // naptár logikája
            break;
        case 'contact': // nyilvános végpont
            // szerveren futó javascript hívhat meg, ellenőrzés
            if($_SERVER['SERVER_ADDR'] === $_SERVER["REMOTE_ADDR"] && $_SERVER['REQUEST_METHOD'] == "POST"){
                // üzenet tartalmi ellenőrzése
                sendContactFormEmail(file_get_contents("php://input"), $conn); // adatok kinyerése
            } else {
                Response::error(403, "Nincs megfelelő jogosultsága");
            }
            break;
        case 'public': // publikusan elérhető
            require './public/public_token.php'; // hozzáférési token generálása
            break;
        default:
            Response::error(404, "Invalid endpoint");
            break;
    }
} else {
    Response::error(404, "API version not specified");
}