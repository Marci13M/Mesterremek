<?php
// kapcsolódás az adatbázishoz
require_once("config.php");
require_once ("database.php");

$db = new Database($GLOBALS["database_host"], $GLOBALS["database_name"], $GLOBALS["database_user"], $GLOBALS["database_password"]);
$conn = $db->connect();

// Aktuálisan kért útvonal előkészítése, fölösleges részek leszedése
$requestUri = trim(parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH), '/');
$basePath = ""; // Adjust to project base path
// $requestPath = str_replace($basePath . "/", "", $requestUri);
$requestPath = $requestUri;

if($basePath == $requestPath){
    $requestPath = "/";
}

// Útvonal kereésse az adatbázisban
$stmt = $conn->prepare("SELECT * FROM endpoints WHERE endpoint = ? AND active = 1 AND deleted = 0 LIMIT 1");
$stmt->execute([$requestPath]);
$route = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$route) {
    // Ha nincs pontos egyezés, keresés paraméter alapján
    $stmt = $conn->prepare("SELECT * FROM endpoints WHERE endpoint LIKE '%:%' AND active = 1 AND deleted = 0");
    $stmt->execute();
    $dynamicRoutes = $stmt->fetchAll(PDO::FETCH_ASSOC);

    foreach ($dynamicRoutes as $dynamicRoute) {
        $pattern = preg_replace('/:\w+/', '([^/]+)', $dynamicRoute['endpoint']); // pareméter helyettesítése Regex-el
        $pattern = str_replace('/', '\/', $pattern);
        if (preg_match('/^' . $pattern . '$/', $requestPath, $matches)) {
            $route = $dynamicRoute;
            array_shift($matches); // Remove the full match
            $params = $matches; // Extract parameters
        }
    }
} 

if ($route) {
    // Egyezés esetén betöltjük a vezérlőt
    $controllerPath = __DIR__ . '/controllers/' . $route['controller'];
    if (file_exists($controllerPath)) {
        require_once $controllerPath;
    } else {
        http_response_code(500);
        ?>
        <!DOCTYPE html>
        <html lang="hu">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>500 - Szerverhiba - CareCompass</title>
        
            <!-- Stíluslapok -->
            <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
            <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
        </head>
        <body>
            <main style="font-size: 150%">

                <div class="max-width" style="width: fit-content; margin: 0 auto; display: flex; align-items: center; flex-direction: column; gap: 30px;">
                    
                    <h1>500</h1>
                    <p>Belső hiba történt!</p>
                    <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>">Vissza a főoldalra</a>
                </div>
            </main>
        </body>
        </html>
        <?php
    }
} else {
    // Nincs ilyen útvonal
    http_response_code(404);
    ?>
    <!DOCTYPE html>
    <html lang="hu">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>404 - Oldal nem található - CareCompass</title>
    
        <!-- Stíluslapok -->
        <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
        <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    </head>
    <body>
        <main style="font-size: 150%">

            <div class="max-width" style="width: fit-content; margin: 0 auto; display: flex; align-items: center; flex-direction: column; gap: 30px;">
                
                <h1>404</h1>
                <p>A keresett oldal nem található!</p>
                <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>">Vissza a főoldalra</a>
            </div>
        </main>
    </body>
    </html>
    <?php
}
?>
