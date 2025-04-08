<?php 
require_once("config.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Token.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Logger.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Mailer.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/functions.php");


if(!isset($_GET['data']) || !isset($_GET['signiture'])){
    Redirect("/?error=Hiányzó paraméter a megerősítéshez!");
}


// email megerősítő adatok ellenőrzése
$data = Token::CheckLinkToken($conn, htmlspecialchars($_GET['data']), htmlspecialchars($_GET['signiture']));


if($data['success'] && $data['data']['action'] == "confirm-email"){
    // új jogosultság lekérdezése
    $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?");
    $stmt->execute(["basic"]);
    $role_id = $stmt->fetch(PDO::FETCH_ASSOC)["id"];

    // email megerősítése
    $stmt = $conn->prepare("UPDATE `users` SET `role_id` = ? WHERE `users`.`id` = ?");
    $stmt->execute([$role_id, $data['data']['user_id']]);

    Token::UseToken($conn, $data['token_id']);

    // Naplózás
    $logger = new Logger($conn);
    $logger->LogUserAction($data['data']['user_id'], "Confirm Email", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true]);

    // felhasználói email lekérdezése
    $stmt = $conn->prepare("SELECT `users`.`email` FROM `users` WHERE `users`.`id` = ?");
    $stmt->execute([$data['data']['user_id']]);

    $email = $stmt->fetch(PDO::FETCH_ASSOC)['email'];

    // email értesítés
    EmailConfirmedEmail($email);
}

?>

<!DOCTYPE html>
<html lang="hu">

<head>
    <!-- META tagek -->
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="<?php echo($route["description"]); ?>">

    <!-- Oldal címe -->
    <title><?php echo($route['name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">

    <!-- Stílus -->
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        
        .container {
            max-width: 600px;
            margin: 20px auto;
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        
        .header {
            text-align: center;
            padding-bottom: 20px;
        }
        
        .header img {
            max-width: 150px;
        }
        
        .content {
            font-size: 16px;
            line-height: 1.6;
            color: #333;
            text-align: center;
        }
        
        .footer {
            text-align: center;
            font-size: 12px;
            color: #777;
            padding-top: 20px;
        }
        
        .button {
            display: block;
            background-color: #3fccbc;
            color: #fff !important;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px auto 0;
            width: fit-content;
        }
        
        .top-margin {
            margin-top: 20px;
        }
        
        .appointment {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }
        
        .appointment p {
            margin: 0;
        }
    </style>
</head>

<body>
    <?php if($data['success'] && $data["data"]['action'] == "confirm-email") { ?>
        <?php if(isset($_SESSION['confirmEmail'])) {unset($_SESSION["confirmEmail"]);} ?>
        <div class="container">
            <div class="header">
                <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
            </div>
            <div class="content">
                <h1>Sikeres megerősítés!</h1>
                <p>Hamarosan átirányítjuk a főoldalra!</p>
                <p>Átirányítás: <span id="countdown">5</span> mp</p>
            </div>
        </div>

        <!-- Átirányítás időzítő -->
        <script>
            var counter = 5
            setInterval(() => {
                counter--;
                if(counter < 0){
                    Redirect("<?php echo($GLOBALS["DEFAULT_URL"])?>/bejelentkezes");
                } else {
                    $("#countdown").text(counter);
                }
            }, 1000);
        </script>
    <?php } else { ?>
        <?php if($data["success"] && $data['data']['action'] != "confirm-email"){ ?>
            <div class="container">
                <div class="header">
                <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
                </div>
                <div class="content">
                    <h1>Sikertelen művelet!</h1>
                    <p>Hiba: Érvénytelen link</p>
                    <p>Új link kéréséhez jelentkezzen be felhasználójába</p>
                    <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/bejelentkezes" class="button">Bejelentkezés</a>
                </div>
            </div>
        <?php } else { ?>
            <div class="container">
                <div class="header">
                <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
                </div>
                <div class="content">
                    <h1>Sikertelen művelet!</h1>
                    <p>Hiba: <?php echo($data['error']['message']); ?></p>
                    <p>Új link kéréséhez jelentkezzen be felhasználójába</p>
                    <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/bejelentkezes" class="button">Bejelentkezés</a>
                </div>
            </div>
        <?php } ?>
    <?php }?>


    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/jquery-3.7.1.min.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/redirect.js"></script>
</body>