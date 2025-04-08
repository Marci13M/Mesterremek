<?php 
require_once("config.php");
require_once("apiaccess.php");

if(!isset($_SESSION['user_id'])){
    Redirect("/bejelentkezes?redirect_url=" . urlencode($GLOBALS["DEFAULT_URL"] . "/profil") . "&warning=Kérjük jelentkezzen be!");
}

$data = ApiAccessGet($GLOBALS['API_DEFAULT_URL'] . "/users/GetUser?user_id=" . $_SESSION['user_id']);
$reservationData = ApiAccessGet($GLOBALS['API_DEFAULT_URL'] . "/appointments/GetUserAllReservation?user_id=" . $_SESSION['user_id']);

// foglalások közül a következőt kiválasztja és a legutóbbi foglalási időt is kiválasztja
$latestReservationTime = "-";
$nextAppointmentTime = "-";

if($data["success"]){
    $user_data = $data["data"]["user"];

} else {
    Redirect("/kijelentkezes?redirect_url=" . urlencode($GLOBALS["DEFAULT_URL"] . "/bejelentkezes?redirect_url=" . urlencode($GLOBALS["DEFAULT_URL"] . "/profil")));
    exit();
}

if($reservationData["success"]){
    $reservation_data = $reservationData["data"]["reservations"];

    $moment = strtotime("now");

    foreach($reservation_data as $reservation){
        $currentReservationTime = strtotime($reservation["reservation_datetime"]);
        $currentLatestReservedAt = strtotime($reservation["reserved_at"]);

        if($latestReservationTime == "-" || $latestReservationTime < $currentLatestReservedAt){
            $latestReservationTime = $currentLatestReservedAt;
        }
        if($currentReservationTime > $moment && ($nextAppointmentTime > $currentLatestReservedAt || $nextAppointmentTime == "-")){
            $nextAppointmentTime = $currentReservationTime;
        }
    }
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
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/footer.css">
</head>

<body>
    <?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php') ?>

    <main>
        <div class="user-data-section max-content">
            <div class="user-data-list">
                <div>
                    <p>Név</p>
                    <p class="bold" id="user-name"><?php echo($user_data['user_name']); ?></p>
                </div>
                <div>
                    <p>Jelszó</p>
                    <button class="text-like" onclick="OpenPasswordChangeModal();" name="open-modal-btn" data-modal="change-password-modal">Jelszó megváltoztatása</button>
                </div>
                <div>
                    <p>Email</p>
                    <p class="bold"><?php echo($user_data['user_email']); ?></p>
                </div>
                <div>
                    <p>Telefonszám</p>
                    <p class="bold" id="user-phone"><?php echo($user_data['user_phone']); ?></p>
                </div>
                <div>
                    <p>Lakcím</p>
                    <p class="bold" id="user-address"><?php echo($user_data["user_address"]); ?></p>
                </div>
                <div class="grid-2">
                    <div>
                        <p>Születési dátum</p>
                        <p class="bold" id="user-birthdate"><?php echo($user_data["user_birthdate"]); ?></p>
                    </div>
                    <div>
                        <p>Nem</p>
                        <p class="bold" id="user-gender"><?php echo($user_data["gender_name"]); ?></p>
                    </div>
                </div>
                <div class="grid-2">
                    <div>
                        <p>TAJ szám</p>
                        <p class="bold" id="user-taj"><?php echo($user_data["user_taj"]); ?></p>
                    </div>
                    <div>
                        <p>Társadalombiztosítás</p>
                        <p class="bold" id="user-tb"><?php echo($user_data["user_hasTB"]? "Igen" : "Nem"); ?></p>
                    </div>
                </div>
                <div class="buttons-container" id="edit-buttons">
                    <button type="button" name="open-modal-btn" data-modal="update-user-modal" class="no-focus" onclick="Edit();">Szerkesztés</button>
                </div>
            </div>
            <div class="reservations-section">
                <h2>Foglalásaim</h2>
                <div class="grid-2">
                    <div>
                        <p>Legutóbbi foglalás ideje</p>
                        <p class="bold"><?php echo(($latestReservationTime == "-")? "-" : date("Y-m-d H:i", $latestReservationTime)); ?></p>
                    </div>
                    <div>
                        <p>Következő időpontom</p>
                        <p class="bold"><?php echo(($nextAppointmentTime == "-")? "-" : date("Y-m-d H:i", $nextAppointmentTime)); ?></p>
                    </div>
                </div>
                <div class="reservations-container">
                    <?php if(isset($reservation_data)) { ?>
                        <?php foreach($reservation_data as $reservation){ ?>
                            <div class="reservation">
                                <div>
                                    <p class="bold"><?php echo($reservation["service_name"]); ?></p>
                                    <p><?php echo($reservation["doctor_name"]); ?></p>
                                </div>
                                <p><?php echo($reservation["reservation_datetime"]); ?></p>
                                <a href="<?php echo($GLOBALS["DEFAULT_URL"]) ?>/foglalasok/<?php echo($reservation['reservation_id']); ?>">Megtekintés</a>
                            </div>
                        <?php } ?>
                    <?php } else { ?>
                        <p>Még nincs lefoglalt időpont</p>
                    <?php } ?>
                </div>
            </div>
        </div>
    </main>


    <!-- Felugró ablak -->
    <div class="popup-container" id="change-password-modal">
        <div class="popup">
            <h2>Jelszó megváltoztatása</h2>
            <div class="named-data-container" id="password-change-inputs">
                <input type="password" name="old-password" id="old-password" placeholder="Jelenlegi jelszó">
                <input type="password" name="new-password" id="new-password" placeholder="Új jelszó">
                <input type="password" name="new-password-again" id="new-password-again" placeholder="Új jelszó ismét">
            </div>
            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="change-password-modal" class="btn-fill">Mégse</button>
                <button type="button" onclick="ChangePassword();" id="change-password">
                    <span class="btn-text">Jelszó megváltoztatása</span>
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

    <!-- Felugró ablak -->
    <div class="popup-container" id="update-user-modal">
        <div class="popup">
            <h2>Felhasználó módosítása</h2>
            <div class="form-flow-column grid-column-2 edit-profile max-width" id="update">
                <div>
                    <p>Név</p>
                    <input type='text' id='user-name-input' value="" placeholder="Név">
                </div>
                <div>
                    <p>Telefonszám</p>
                    <input type='tel' id='user-phone-input' value="" placeholder="Telefonszám">
                </div>
                <div class="grid-span-2">
                    <p>Lakcím</p>
                    <input type='text' id='user-address-input' value="" placeholder="Lakcím">
                </div>
                <div>
                    <p>Nem</p>
                    <select id="user-gender-input" class="fill">
                        
                    </select>
                </div>
                <div>
                    <p>Rendelkezik TB-vel?</p>
                    <select id='user-hasTB-input' class="fill">
                        
                    </select>
                </div>
                <div>
                    <p>Születési dátum</p>
                    <input type='date' id='user-birthdate-input' value="" placeholder="Születési dátum">
                </div>
                <div>
                    <p>TAJ szám</p>
                    <input type='text' id='user-taj-input' value="" placeholder="TAJ szám">
                </div>
            </div>
            <div class="popup-buttons">
                <button type="button" class="btn-fill" name="close-open-modal-btn" data-modal="update-user-modal">Elvetés</button>
                <button type="button" onclick="UpdateUser();" id="update-profile">
                    <span class="btn-text">Módosítások mentése</span>
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

    <script>
        var userId = <?php echo($_SESSION['user_id']); ?>;
    </script>

    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/profile.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/modal.js"></script>
</body>