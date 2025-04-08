<?php 
require_once("config.php");
require_once("apiaccess.php");

if(!isset($params[0]) || $params[0] == ""){
    Redirect("/orvosok?error=Nincs kiválasztva orvos!");
}

$data = ApiAccessGet($GLOBALS['API_DEFAULT_URL'] . "/data/translate/GetDoctorIdFromSlug?slug=" . htmlspecialchars($params[0]));

if($data['data']['doctor_id'] !== null){
    $doctor_id = $data['data']['doctor_id'];
} else {
    Redirect("/orvosok?error=Nem találtunk ilyen orvost!");
}

$url = $GLOBALS['API_DEFAULT_URL'] . "/doctors/GetDoctor?doctor_id=" . $doctor_id;

$data = ApiAccessGet($url);

if($data['success']){
    $doctor_data = $data['data']['doctor']; 
} else {
    Redirect("/orvosok?error=Nem találtunk ilyen orvost!");
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
    <title><?php echo($doctor_data['doctor_name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/footer.css">
</head>

<body>
    <?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php') ?>

    <main>
        <div class="doctor-data-container max-content">
            <div class="doctor-profile">
                <div class="details">
                    <?php if($doctor_data["profile_image"] !== null) { ?>
                        <img src="<?php echo($doctor_data["profile_image"]["image"]); ?>" alt="<?php echo($doctor_data["profile_image"]["alt"]); ?>" title="<?php echo($doctor_data["profile_image"]["title"]); ?>" class="doctor-profile-image">
                    <?php } else { ?>
                        <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/doctor_placeholder.png" alt="helyettesítő kép" title="Helyettesítő kép">
                    <?php } ?>
                    <div>
                        <h2><?php echo($doctor_data["doctor_name"]); ?></h2>
                        <ul>
                            <?php foreach($doctor_data["services"] as $service) { ?>
                                <li><?php echo($service["name"]); ?></li>
                            <?php } ?>
                        </ul>
                    </div>
                </div>
                <!-- <div class="review">
                    <p>Értékelés: 4.8</p>
                    <a href="#">Visszajelzések</a>
                </div> -->
            </div>
            <div class="doctor-description">
                <div>
                    <h2>Bemutatkozás</h2>
                    <p><?php echo($doctor_data["doctor_introduction"]); ?></p>
                </div>
                <div>
                    <h2>Tanulmányok</h2>
                    <ul>
                            <?php foreach($doctor_data["educations"] as $education) { ?>
                                <li><?php echo($education["name"]); ?></li>
                            <?php } ?>
                        </ul>
                </div>
                <div>
                    <h2>Nyelvismeret</h2>
                    <ul>
                            <?php foreach($doctor_data["languages"] as $language) { ?>
                                <li><?php echo($language["name"]); ?></li>
                            <?php } ?>
                        </ul>
                </div>
                <div class="not-flex">
                    <h2>Szakrendelések</h2>
                    <div class="cards-container grid-3">
                        <?php foreach($doctor_data["services"] as $service) {?>
                            <div class="small-card-container">
                                <h3 class="card-title"><?php echo($service["name"]); ?></h3>
                                <a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/szolgaltatasok/" . $service["slug"] . "?orvos=" . str_replace(" ", "+", mb_strtolower($doctor_data["doctor_name"]))); ?>" class="card-link">Megtekintés</a>
                            </div>
                        <?php } ?>
                    </div>
                </div>
            </div>
        </div>
    </main>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>

</body>