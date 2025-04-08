<?php 
require_once("config.php");
require_once("apiaccess.php");

if(!isset($params[0]) || $params[0] == ""){
    Redirect("/partnereink?error=Nincs kiválasztva kórház!");
}

$data = ApiAccessGet($GLOBALS['API_DEFAULT_URL'] . "/data/translate/GetHospitalIdFromSlug?slug=" . htmlspecialchars($params[0]));

if($data['data']['hospital_id'] !== null){
    $hospital_id = $data['data']['hospital_id'];
} else {
    Redirect("/partnereink?error=Nem találtunk ilyen kórházat!");
    exit;
}

$url = $GLOBALS['API_DEFAULT_URL'] . "/hospitals/GetHospital?hospital_id=" . $hospital_id;

$data = ApiAccessGet($url);

if($data['success']){
    $hospital_data = $data['data']['hospital'];

    // orvosok lekérdezése
    $url = $GLOBALS["API_DEFAULT_URL"] . "/hospitals/GetDoctors?hospital_id=" . $hospital_id;

    $data = ApiAccessGet($url);

    if($data['success']){
        $hospital_doctors = $data["data"]["hospital_doctors"];
    }

    // szolgáltatások lekrédezése
    $url = $GLOBALS["API_DEFAULT_URL"] . "/services/GetHospitalServices?hospital_id=" . $hospital_id;

    $data = ApiAccessGet($url);

    if($data['success']){
        $hospital_services = $data["data"]["services"];
    }

    // cégadatok lekérdezése
    $company_data = array();
    $url = $GLOBALS["API_DEFAULT_URL"] . "/companies/GetCompany?company_id=" . $hospital_data['company_id'];

    $data = ApiAccessGet($url);

    if($data['success']){
        $company_data = $data["data"]["company"];
    }
} else {
    Redirect("/partnereink?error=Nem találtunk ilyen kórházat!");
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
    <title><?php echo($hospital_data['hospital_name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/footer.css">
</head>

<body>
    
<?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php');?>

    <main>
        <div class="image-title-section">
            <?php if($hospital_data["images"]["cover_image"]['image'] !== null) { ?>
                <img src="<?php echo($hospital_data["images"]["cover_image"]["image"]); ?>" alt="<?php echo($hospital_data["images"]["cover_image"]["image_alt"]); ?>" title="<?php echo($hospital_data["images"]["cover_image"]["image_title"]); ?>">
            <?php } else { ?>
                <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/hospital_wide_placeholder.jpg" alt="helyettesítő kép" title="Helyettesítő kép">
            <?php  }?>
            
            <h1><?php echo($hospital_data["hospital_name"]); ?></h1>
            <?php if($company_data["images"]["logo"]['image'] !== null) { ?>
                <img src="<?php echo($company_data["images"]["logo"]["image"]); ?>" class="logo" alt="<?php echo($company_data["images"]["logo"]["image_alt"]); ?>" title="<?php echo($company_data["images"]["logo"]["image_title"]); ?>">
            <?php } else { ?>
                <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/logo_placeholder.png" class="logo" alt="helyettesítő kép" title="Helyettesítő kép">
            <?php  }?>
        </div>
        <div class="text-section max-content">
            <h2>Bemutatkozás</h2>
            <p><?php echo($hospital_data["hospital_description"]); ?></p>
        </div>

        <div class="hospital-description max-content">
            <div class="section-container">
                <h2>Nyitvatartás</h2>
                <?php $open_hours = json_decode($hospital_data["hospital_open_hours"], true); ?>
                <div class="company-data-list max-content">
                    <div>
                        <p>Hétfő</p>
                        <p class="bold"><?php echo(isset($open_hours[0]) ? $open_hours[0][0] . " - " . $open_hours[0][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Kedd</p>
                        <p class="bold"><?php echo(isset($open_hours[1]) ? $open_hours[1][0] . " - " . $open_hours[1][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Szerda</p>
                        <p class="bold"><?php echo(isset($open_hours[2]) ? $open_hours[2][0] . " - " . $open_hours[2][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Csütörtök</p>
                        <p class="bold"><?php echo(isset($open_hours[3]) ? $open_hours[3][0] . " - " . $open_hours[3][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Péntek</p>
                        <p class="bold"><?php echo(isset($open_hours[4]) ? $open_hours[4][0] . " - " . $open_hours[4][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Szombat</p>
                        <p class="bold"><?php echo(isset($open_hours[5]) ? $open_hours[5][0] . " - " . $open_hours[5][1] : "Zárva"); ?></p>
                    </div>
                    <div>
                        <p>Vasárnap</p>
                        <p class="bold"><?php echo(isset($open_hours[6]) ? $open_hours[6][0] . " - " . $open_hours[6][1] : "Zárva"); ?></p>
                    </div>
                </div>
            </div>
            <div class="doctor-description">
                <div>
                    <h2>Elérhetőségek</h2>
                    <ul>
                        <li><?php echo($hospital_data["hospital_email"]); ?></li>
                        <?php foreach($hospital_data['phonenumbers'] as $phonenumber) {
                            if($phonenumber['public']){ ?>
                            <li><?php echo($phonenumber['phone_number']); ?></li>
                            <?php }
                    }?>
                    </ul>
                </div>
            </div>
        </div>


        <div class="tab-control max-content">
            <div class="controls">
                <button type="button" class="tab-control-btn tab-active" onclick="openTab('services')" id="services-btn">Szolgáltatások</button>
                <button type="button" class="tab-control-btn" onclick="openTab('doctors')" id="doctors-btn">Orvosok</button>
            </div>

            <div class="tab" id="services">
                <?php if(isset($hospital_services)) { ?>
                    <div class="cards-container grid-3 max-content">
                        <?php foreach($hospital_services as $service) { ?>
                            <?php if($service["service_active"]) {?>
                                <div class="small-card-container">
                                    <h3 class="card-title"><?php echo($service["service_name"]); ?></h3>
                                    <a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/szolgaltatasok/" . $service["slug"] . "?korhaz=" . str_replace(" ", "+", mb_strtolower($hospital_data["hospital_name"]))); ?>" class="card-link">Megtekintés</a>
                                </div>
                            <?php } ?>
                        <?php } ?>
                    </div>
                <?php } else { ?>
                    <p>Nincs szolgáltatás rendelve a kórházhoz</p>
                <?php } ?>
            </div>

            <div class="tab" id="doctors" style="display: none;">
                <?php if(isset($hospital_doctors)) { ?>
                    <div class="cards-container grid-3 max-content ">
                        <?php foreach($hospital_doctors as $doctor) { ?>
                            <div class="medium-card-container card-padding ">
                                <div class="card-image-circle ">
                                    <?php if($doctor["image"] !== null) { ?>
                                        <img src="<?php echo($doctor["image"]); ?> " alt="<?php echo($doctor["image_alt"]); ?>" title="<?php echo($doctor["image_title"]); ?>">
                                    <?php } else { ?>
                                        <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/doctor_placeholder.png" alt="helyettesítő kép" title="Helyettesítő kép">
                                    <?php } ?>
                                </div>
                                <div class="card-details ">
                                    <div class="main ">
                                        <h3><?php echo($doctor["name"]); ?></h3>
                                        <p><?php echo(join(", ", $doctor["services"])); ?></p>
                                    </div>
                                    <div class="cta">
                                        <a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/orvosok/" . $doctor["slug"]); ?>">Megtekintés</a>
                                    </div>
                                </div>
                            </div>
                        <?php } ?>
                    </div>
                <?php } else { ?>
                    <p>Nincs orvos rendelve a kórházhoz</p>
                <?php } ?>
            </div>
        </div>

        <div class="section-container">
            <h2>Fenntartó adatai</h2>
            <div class="company-data-list max-content">
                <div>
                    <p>Fenntartó cég neve:</p>
                    <p class="bold"><?php echo($company_data["company_name"]); ?></p>
                </div>
                <div>
                    <p>Igazgató neve:</p>
                    <p class="bold"><?php echo($company_data["company_director_name"]); ?></p>
                </div>
                <div>
                    <p>Adószám:</p>
                    <p class="bold"><?php echo($company_data["company_tax_number"]); ?></p>
                </div>
                <div>
                    <p>Cégjegyzékszám:</p>
                    <p class="bold"><?php echo($company_data["company_register_number"]); ?></p>
                </div>
                <div>
                    <p>Cím: </p>
                    <p class="bold"><?php echo($company_data["company_address"]); ?></p>
                </div>
                <div>
                    <p>Email:</p>
                    <p class="bold"><?php echo($company_data["company_email"]); ?></p>
                </div>
                <div class="company-phonenumbers">
                    <p>Telefonszám(ok)</p>
                    <?php foreach($company_data['phonenumbers'] as $phonenumber) {
                        if($phonenumber['public']){ ?>
                            <p class="bold"><?php echo($phonenumber['phone_number']); ?></p>
                    <?php }
                    }?>
                </div>
                <!-- <div>
                    <p>Orvosok:</p>
                    <p class="bold"></p>
                </div>
                <div>
                    <p>Kórházak:</p>
                    <p class="bold"></p>
                </div>
                <div>
                    <p>Szolgáltatások</p>
                    <p class="bold"></p>
                </div> -->
            </div>
        </div>
    </main>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>


    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/tabcontrol.js"></script>
</body>