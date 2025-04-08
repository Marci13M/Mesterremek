<?php 
require_once("config.php");
require_once("apiaccess.php");

if(isset($_SESSION['user_id']) && isset($_SESSION['logged_in']) && $_SESSION['logged_in'] == true){
    Redirect();
}


$url = $GLOBALS['API_DEFAULT_URL'] . "/data/general/GetGenders";

$data = ApiAccessGet($url);

if($data['success']){
    $genders = $data['data']['genders'];
} else {
    echo("Nincs nem hozzáadva az adatbázishoz");
    exit;
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
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/auth.css">
</head>

<body>

    <!-- jquery -->
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/jquery-3.7.1.min.js"></script>


    <div class="container" id="container">
        <div class="form-container sign-up">
            <form id="registration">
                <h1>Regisztráció</h1>
                <div class="input-requirements">
                    <input type="text" placeholder="Név" id="registration-name">
                    <div class="requirements">A névnek legalább két tagból kell állnia!</div>
                </div>
                <div class="input-requirements">
                    <input type="email" placeholder="Email" id="registration-email">
                    <div class="requirements">Csak létező email címet adhat meg!</div>
                </div>
                <div class="input-requirements">
                    <input type="password" placeholder="Jelszó" id="registration-password">
                    <div class="requirements">A jelszó legalább 8 karakter hosszú, tartalmaznia kell kis és nagy betűt, számot, illetve különleges karaktert!</div>
                </div>
                <div class="input-requirements">
                    <input type="text" placeholder="Telefonszám" id="registration-phone">
                    <div class="requirements">Csak számokat tartalmazhat és 11 karakterből kell állnia!</div>
                </div>
                <div class="input-requirements">
                    <input type="text" placeholder="TAJ szám" id="registration-taj">
                    <div class="requirements">Csak számokat tartalmazhat és 9 karakterből kell állnia!</div>
                </div>
                <div class="input-requirements">
                    <input type="text" placeholder="Lakcím" id="registration-address">
                    <div class="requirements">Helyes formátum: irányítószám település, közterület neve közterület jellege házszám. Pl.: 0000 Mintafalva, Minta u. 0.</div>
                </div>
                <select name="gender" id="registration-gender">
                    <option value="0" disabled selected hidden>Nem</option>
                    <?php foreach($genders as $gender) { ?>
                        <option value="<?php echo($gender["id"]); ?>"><?php echo($gender["name"]); ?></option>
                    <?php } ?>
                </select>
                <div class="form-group">
                    <label for="birthdate">Születési dátum:</label>
                    <input type="date" id="registration-birthdate" min="1908-01-01">
                </div>
                <div class="form-group">
                    <label for="hasTB">Rendelkezik TB-vel?</label>
                    <select name="hasTB" id="registration-hasTB">
                        <option value="true">Igen</option>
                        <option value="false">Nem</option>
                    </select>
                </div>
                <button type="submit" id="registration-btn">
                    <span class="btn-text">Regisztráció</span>
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
                <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>">Vissza a főoldalra</a>
            </form>
        </div>
        <div class="form-container sign-in">
            <form id="login">
                <h1>Bejelentkezés</h1>
                <input type="text" placeholder="Email" id="login-email">
                <input type="password" placeholder="Jelszó" id="login-password">
                <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/elfelejtett-jelszo">Elfelejtette a jelszavát?</a>
                <button type="submit" id="login-btn">
                    <span class="btn-text">Bejelentkezés</span>
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
                <a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>">Vissza a főoldalra</a>
            </form>
        </div>
        <div class="toggle-container">
            <div class="toggle">
                <div class="toggle-panel toggle-left">
                    <h1>Üdvözöljük újra!</h1>
                    <p>Jelentkezzen be adataival, hogy minden funkcionalitáshoz hozzáférést kapjon</p>
                    <button class="button-hidden" id="loginBtn">Bejelentkezés</button>
                </div>
                <div class="toggle-panel toggle-right">
                    <h1>Üdvözöljük!</h1>
                    <p>Regisztráljon az adataival, hogy minden funkcionalitáshoz hozzáférést kapjon</p>
                    <button class="button-hidden" id="registerBtn">Regisztráció</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>
    
    
    <!-- Szkriptek -->
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/auth.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/redirect.js"></script>
</body>

</html>