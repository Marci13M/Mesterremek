<?php 
require_once("config.php");
require_once("apiaccess.php");


if(!isset($params[0]) || $params[0] == ""){
    Redirect("/profil?error=Nincs kiválasztva foglalás!");
}


if(!isset($_SESSION["user_id"])){
    Redirect("/bejelentkezes?warning=Kérjük jelentkezzen be!&redirect_url=" . urlencode($GLOBALS['DEFAULT_URL'] . "/foglalasok/" .$params[0]));
}


$url = $GLOBALS['API_DEFAULT_URL'] . "/appointments/GetUserReservation?user_id=" . $_SESSION['user_id'] . "&reservation_id=" . htmlspecialchars($params[0]);

$data = ApiAccessGet($url);

if($data['success']){
    $reservation_data = $data['data']['reservation'];
} else {
    Redirect("/profil?error=A foglalás nem tekinthető meg!");
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
    <title><?php echo($reservation_data['service_name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/footer.css">
</head>

<body>
<?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php') ?>

    <main>
        <div class="reservation-details-container max-content">
            <h2>Foglalás részletei</h2>
            <div class="grid-4">
                <div>
                    <p>Kezelés időpontja:</p>
                    <p class="bold"><?php echo($reservation_data['reservation_datetime']); ?></p>
                </div>
                <div>
                    <p>Kezelést végző személy:</p>
                    <p class="bold"><?php echo($reservation_data['doctor_name']); ?></p>
                </div>
                <div>
                    <p>Kezelés időtartama</p>
                    <p class="bold"><?php echo($reservation_data['service_duration']); ?> perc</p>
                </div>
                <div>
                    <p>Kezelés megnevezése</p>
                    <p class="bold"><?php echo($reservation_data['service_name']); ?></p>
                </div>
            </div>
            <div>
                <p>Kezelés helyszíne</p>
                <p class="bold"><?php echo($reservation_data['hospital_address']); ?></p>
            </div>
            <div>
                <p>Kezelés végző intézmény:</p>
                <p class="bold"><?php echo($reservation_data['hospital_name']); ?></p>
            </div>
            <div>
                <p>Kezelés ára</p>
                <p class="bold"><?php echo(number_format($reservation_data['service_price'], 0, '', '.')); ?> Ft</p>
            </div>
            <div>
                <p>Kezelés leírása</p>
                <p><?php echo($reservation_data['service_description']); ?></p>
            </div>
            <div class="buttons-container">
                <a href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/profil" class="btn">Vissza az adatlapra</a>
                <?php if($reservation_data['reservation_fulfilled'] == false && strtotime("now") <= strtotime($reservation_data["reservation_datetime"])) { ?>
                <button type="button" class="delete" name='open-modal-btn' data-modal='delete-details-modal'>Időpont lemondása</button>
                <?php } ?>
            </div>
        </div>
    </main>

    <!-- Felugró ablak -->
    <div class="popup-container" id="delete-details-modal">
        <div class="popup">
            <h2>Biztosan törli az időpontot?</h2>
            <div class="popup-buttons equal">
                <button type="button" name="close-open-modal-btn" data-modal='delete-details-modal'>Mégse</button>
                <button type="button" class="delete" id="delete-reservation" onclick="DeleteReservation(<?php echo(htmlspecialchars($params[0])); ?>)">
                    <span class="btn-text">Időpont törlése</span>
                    <div class="dot-spinner hidden">
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
            </div>
        </div>
    </div>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/reservation.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/modal.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/redirect.js"></script>
</body>