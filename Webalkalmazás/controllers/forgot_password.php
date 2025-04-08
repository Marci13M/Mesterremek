<?php 
require_once("config.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Token.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Logger.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Mailer.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/functions.php");

// felhasználó kijelentkeztetése minden esetben
if(isset($_SESSION['user_id']) && (isset($_SESSION['logged_in']) && $_SESSION['logged_in'] == true)){
    Redirect("/kijelentkezes?redirect_url=" . urlencode((isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://" . $_SERVER['HTTP_HOST'] . $_SERVER['REQUEST_URI']));
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

        .content .inputs{
            display: flex;
            flex-direction: column;
            gap: 10px;
            max-width: 300px;
            margin: 20px auto;
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
            outline: 2px solid #3fccbc;
            outline-offset: -2px;
            color: #fff !important;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px auto 0;
            width: fit-content;
            transition: all 200ms ease;
        }

        .button:hover,
        .button:focus{
            background-color: transparent;
            color: #3fccbc !important;
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
    <?php 
        if(isset($_GET['data']) && isset($_GET['signiture'])){
            // jelszóváltoztatás adatok ellenőrzése
            $data = Token::CheckLinkToken($conn, htmlspecialchars($_GET['data']), htmlspecialchars($_GET['signiture']));
            if($data["success"] && $data['data']['action'] == "forgot-password"){
                $_SESSION['user_id'] = $data["data"]["user_id"];
            ?>
            <!-- Form megjelenítése változtatáshoz -->
                <div class="container">
                    <div class="header">
                        <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
                    </div>
                    <div class="content" id="content">
                        <h1>Új jelszó beállítása</h1>
                        <div class="inputs">
                            <input type="password" name="password" id="password" placeholder="Új jelszó">
                            <input type="password" name="password-again" id="password-again" placeholder="Új jelszó ismét">
                        </div>

                        <button type="button" class="button" onclick="setNewPassword()" id="btn">
                            <span class="btn-text">Jelszó megváltoztatása</span>
                            <div class="dot-spinner hidden light">
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                                <div class="dot-spinner__dot"></div>
                            </div>
                        </button>
                        <a href="<?php echo($GLOBALS['DEFAULT_URL']); ?>" style="color: var(--dark-grey-color); box-sizing: content-box; font-size: 0.8rem;">Vissza a főoldalra</a>
                    </div>
                </div>

                <script>
                    async function setNewPassword(){
                        var password = $("#password").val(); // jelszó cím
                        var passwordAgain = $("#password-again").val(); // jelszó cím

                        if(password.length != 0 || passwordAgain.length != 0){
                            try {
                                // Disable button and show spinner
                                $("#btn").prop("disabled", true);
                                $("#btn").find("span.btn-text").addClass("hidden") // Hide text
                                $("#btn").find("div.dot-spinner").removeClass("hidden"); // Show spinner
                                //adatok lekérdezése
                                var requestURL = `${BASE_URL}/apiaccess.php`;
                                
                                
                                var requestData = {
                                    "data": {
                                        "password": password,
                                        "passwordAgain": passwordAgain,
                                        "token_id": <?php echo($data['token_id']); ?>
                                    },
                                    "url": `${API_URL}/auth/SetNewPassword`,
                                    "includeUserId": true
                                }
                                
                                const response = await fetch(requestURL, {
                                method: "POST",
                                headers: {
                                    'X-Requested-With': 'XMLHttpRequest'
                                },
                                body: JSON.stringify(requestData)
                            });
                            const data = await response.json();
                            
                            if(data.success){
                                $("#password").remove();
                                $("#password-again").remove();
                                $("#btn").replaceWith("<p>Sikeres jelszóváltoztatás! Hamarosan átirányítjuk!</p><p>Átirányítás: <span id='countdown'>5</span> mp</p>");
                                // Átirányítás időzítő
                                var counter = 5
                                setInterval(() => {
                                    counter--;
                                    if(counter < 0){
                                        Redirect("<?php echo($GLOBALS["DEFAULT_URL"])?>/bejelentkezes");
                                    } else {
                                        $("#countdown").text(counter);
                                    }
                                }, 1000);
                            } else {
                                throw Error(data.error.message);
                            }
                        } catch (error) {
                            AddMessage("error", error.message);
                        } finally {
                            // Disable button and show spinner
                            $("#btn").prop("disabled", false);
                            $("#btn").find("span.btn-text").removeClass("hidden") // Hide text
                            $("#btn").find("div.dot-spinner").addClass("hidden"); // Show spinner
                        }
                    } else {
                        AddMessage("error", "Adjon meg egy új jelszót!");
                    }
                    }
                </script>
            <?php } else { ?>
                <!-- Form megjelenítése hibával -->
                <?php if($data["success"] && $data['data']['action'] != "forget-password"){ ?>
                    <div class="container">
                        <div class="header">
                        <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
                        </div>
                        <div class="content">
                            <h1>Sikertelen művelet!</h1>
                            <p>Hiba: Érvénytelen link</p>
                            <p>Új link kérését a bejelentkezés felületéről tudja megtenni</p>
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
                            <p>Új link kéréséhez kattintson az alábbi gombra</p>
                            <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/elfelejtett-jelszo" class="button">Új link igénylése</a>
                        </div>
                    </div>
                <?php } ?>
            <?php } ?>
    <?php } else { ?>
        <div class="container">
            <div class="header">
                <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_db.png" alt="sötétkék logó" title="Céglogó">
            </div>
            <div class="content" id="content">
                <h1>Új jelszó igénylése</h1>
                <p>Adja meg email címét: </p>
                <div class="inputs">
                    <input type="email" name="email" id="email" placeholder="pelda@pelda.hu">
                </div>

                <button type="button" class="button" onclick="newPassword()" id="btn">
                    <span class="btn-text">Igénylés</span>
                    <div class="dot-spinner hidden light">
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                        <div class="dot-spinner__dot"></div>
                    </div>
                </button>
                <a href="<?php echo($GLOBALS['DEFAULT_URL']); ?>" style="color: var(--dark-grey-color); box-sizing: content-box; font-size: 0.8rem;">Vissza a főoldalra</a>
            </div>
        </div>

        <script>
            async function newPassword(){
                var email = $("#email").val(); // email cím
                
                if(email.length != 0){
                    // Disable button and show spinner
                    $("#btn").prop("disabled", true);
                    $("#btn").find("span.btn-text").addClass("hidden") // Hide text
                    $("#btn").find("div.dot-spinner").removeClass("hidden"); // Show spinner
                    
                    try {
                        //adatok lekérdezése a felugró ablakhoz
                        var requestURL = `${BASE_URL}/apiaccess.php`;
                        
                        
                        var requestData = {
                            "data": {
                                "email": email
                            },
                            "url": `${API_URL}/auth/ForgotPasswordEmail`
                        }
                        
                        const response = await fetch(requestURL, {
                            method: "POST",
                            headers: {
                                'X-Requested-With': 'XMLHttpRequest'
                            },
                            body: JSON.stringify(requestData)
                        });
                        const data = await response.json();
                        
                        if(data.success){
                            // AddMessage("success", "Sikeres igénylés!");
                            $("#btn").replaceWith("<p>Emailben elküldtük a változtatáshoz szükséges linket</p>");
                        } else {
                            throw Error(data.error.message);
                        }
                    } catch (error){
                        AddMessage("error", error.message);
                    } finally {
                        // Disable button and show spinner
                        $("#btn").prop("disabled", false);
                        $("#btn").find("span.btn-text").removeClass("hidden") // Hide text
                        $("#btn").find("div.dot-spinner").addClass("hidden"); // Show spinner
                    }
                } else {
                    AddMessage("error", "Adjon meg email címet!");
                }
            }
        </script>
    <?php } ?>

    
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>


    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/jquery-3.7.1.min.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/redirect.js"></script>
</body>