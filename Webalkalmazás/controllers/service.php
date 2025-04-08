<?php 
require_once("config.php");
require_once("apiaccess.php");

if(!isset($params[0]) || $params[0] == ""){
    Redirect("/szolgaltatasok?error=Nincs kiválasztva szolgáltatás!");
}

$data = ApiAccessGet($GLOBALS['API_DEFAULT_URL'] . "/data/translate/GetServiceIdFromSlug?slug=" . htmlspecialchars($params[0]));

if($data['data']['service_id'] !== null){
    $service_id = $data['data']['service_id'];
} else {
    Redirect("/szolgaltatasok?error=Nem találtunk ilyen szolgáltatást!");
}

$url = $GLOBALS['API_DEFAULT_URL'] . "/services/GetService?service_id=" . $service_id;

$data = ApiAccessGet($url);

if($data['success']){
    $service_data = $data['data']['service'];
} else {
    Redirect("/szolgaltatasok?error=Nem találtunk ilyen szolgáltatást!");
}

$url = $GLOBALS['API_DEFAULT_URL'] . "/services/GetDoctorsWithService?service_id=" . $service_id;

$data = ApiAccessGet($url);

$doctorNames = array();
$hospitalNames = array();

if($data['success']){
    $doctors_data = $data['data']['doctors'];

    foreach($doctors_data as $doctor){ 
        if(!in_array($doctor['doctor_name'], $doctorNames)){
            $doctorNames[] = $doctor['doctor_name'];
        }
        if(!in_array($doctor['hospital_name'], $hospitalNames)){
            $hospitalNames[] = $doctor['hospital_name'];
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
    <title><?php echo($service_data['service_name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/footer.css">
</head>

<body>
<?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php') ?>

    <main>
        <div class="page-title-image grid-2 max-content">
            <div class="image-container">
                <img src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/static/service.jpg" alt="háttérkép" title="Oldal főképe">
            </div>
            <div class="page-description">
                <h1><?php echo($service_data['service_name']); ?></h1>
                <p><?php echo($service_data['service_description']); ?></p>

            </div>
        </div>

        <div class="filter-section">
            <div class="filter-section-container max-content">
                <div class="filter-container">
                    <div>
                        <p onclick="OpenFilters()">Szűrő</p>
                        <img src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/static/filter_icon_b.png" alt="szűrés" onclick="OpenFilters()">
                    </div>
                    <div class="filters hidden" id="filters-container">
                        <div class="filter">
                            <p>Kórház:</p>
                            <select name="korhaz" id="hospitalFilter" class="filterSetting" onchange="filterResults()">
                                <option value="">Összes kórház</option>
                                <?php foreach($hospitalNames as $name){ ?>
                                    <option value="<?php echo(mb_strtolower($name)); ?>"><?php echo($name); ?></option>
                                <?php } ?>
                            </select>
                        </div>

                        <div class="filter">
                            <p>Orvos:</p>
                            <select name="orvos" id="serviceFilter" class="filterSetting" onchange="filterResults()">
                                <option value="">Összes orvos</option>
                                <?php foreach($doctorNames as $name){ ?>
                                    <option value="<?php echo(mb_strtolower($name)); ?>"><?php echo($name); ?></option>
                                <?php } ?>
                            </select>
                        </div>
                    </div>
                </div>
                <div class="search-container">
                    <input type="text" id="search" placeholder="Keresés...">
                    <img src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/static/search_icon_b.svg" alt="keresés">
                </div>
            </div>
        </div>

        <p class="hidden max-content" id="filter-indicator">Keresés eredményei:</p>

        <div class="service-reservations-container max-content" id="service-options">
            <?php if(isset($doctors_data)) { ?>    
                <?php foreach($doctors_data as $doctor){ ?>
                    <div class="service-reservation-container" id="service-option-<?php echo($service_id . $doctor['hospital_id'] . $doctor['doctor_id']); ?>" data-doctor_id="<?php echo($doctor['doctor_id']); ?>" data-hospital_id="<?php echo($doctor['hospital_id']); ?>" data-from_date="<?php echo(date("Y-m-d")); ?>" data-to_date="<?php echo(date("Y-m-d", strtotime("+4 days"))); ?>" data-hospital_name="<?php echo(mb_strtolower($doctor['hospital_name'])); ?>">
                        <div class="doctor-data-container">
                            <div class="doctor-profile">
                                <div class="details">
                                    <?php if($doctor["doctor_profil"] !== null) { ?>
                                        <img src="<?php echo($doctor['doctor_profil']); ?>" alt="<?php echo($doctor['doctor_profil_alt']); ?>" title="<?php echo($doctor['doctor_profil_title']); ?>" class="doctor-profile-image">
                                    <?php } else { ?>
                                        <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/doctor_placeholder.png" alt="helyettesítő kép" title="Helyettesítő kép">
                                    <?php } ?>
                                    
                                </div>
                            </div>
                            <div class="service-details">
                                <div>
                                    <h2><?php echo($doctor['doctor_name']); ?></h2>
                                    <p><?php echo(join(", ", array_map(function ($service) use ($service_data) { return $service == $service_data['service_name'] ? "<span class='bold'>{$service}</span>" : $service; }, $doctor['services']))); ?></p>
                                </div>
                                <div>
                                    <h3>Szolgáltatás helyszíne:</h3>
                                    <p><?php echo($doctor['hospital_address']); ?></p>
                                </div>
                                <div>
                                    <h3>Intézmény:</h3>
                                    <p class="hospitalName"><?php echo($doctor['hospital_name']); ?></p>
                                </div>
                                <div>
                                    <p>Szolgáltatás ára: <span class="bold"><?php echo(number_format($doctor['service_price'], 0, '', '.')); ?> Ft</span></p>
                                </div>
                            </div>
                        </div>
                        <div class="calendar-container" id="calendar-container-<?php echo($service_id . $doctor['hospital_id'] . $doctor['doctor_id']); ?>" >
                            
                        </div>
                    </div>
                <?php } ?>
            <?php } else { ?>
                <p>Jelenleg egyetlen orvos sem lát el ilyen szolgáltatást</p>    
            <?php } ?>
        </div>
    </main>


    <!-- Felugró ablak -->
    <div class="popup-container" id="reservation-details-modal">
        <div class="popup">
            <h2>Foglalás áttekintése</h2>
            <div class="named-data-container">
                <h3>Foglaló adatai</h3>
                <div>
                    <p>Név:</p>
                    <p class="bold" id="reservation-name"></p>
                </div>
                <div>
                    <p>Email:</p>
                    <p class="bold" id="reservation-email"></p>
                </div>
                <div>
                    <p>Telefonszám:</p>
                    <p class="bold" id="reservation-phonenumber"></p>
                </div>
            </div>
            <div class="named-data-container">
                <h3>Foglalás adatai</h3>
                <div>
                    <p>Szolgáltatás:</p>
                    <p class="bold" id="reservation-service-name"></p>
                </div>
                <div>
                    <p>Időpont:</p>
                    <p class="bold" id="reservation-datetime"></p>
                </div>
                <div>
                    <p>Orvos:</p>
                    <p class="bold" id="reservation-doctor-name"></p>
                </div>
                <div>
                    <p>Helyszín:</p>
                    <p class="bold" id="reservation-location"></p>
                </div>
                <div>
                    <p>Ár:</p>
                    <p class="bold" id="reservation-service-price"></p>
                </div>
            </div>
            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="reservation-details-modal">Mégse</button>
                <button type="button" id="reserve-appointment">
                    <span class="btn-text">Foglalás megerősítése</span>
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
            </div>
        </div>
    </div>


    <!-- Felugró ablak -->
    <div class="popup-container" id="more-options-modal">
        <div class="popup">
            <h2>További időpontok</h2>
            <div class="form-flow-column grid-column-2 edit-profile max-width">
                <div class="calendar">
                    <div class="calendar-header">
                        <button id="prev-month">‹</button>
                        <div id="month-year"></div>
                        <button id="next-month">›</button>
                    </div>
                    <div class="calendar-body">
                        <div class="calendar-weekdays">
                            <div>Hét</div>
                            <div>Kedd</div>
                            <div>Szer</div>
                            <div>Csüt</div>
                            <div>Pén</div>
                            <div>Szo</div>
                            <div>Vas</div>
                        </div>
                        <div class="calendar-dates">
                        </div>
                    </div>
                </div>
                <div class="free-times-section">
                    <p>Kiválasztott dátum: <span class="bold" id="selected-datetime"></span></p>
                    <p>Válasszon időpontot!</p>
                    <div class="free-times-container" id="free-times-container">
                        <button type="button" class="free-time">14:00</button>
                        <h3 class="no-free-time">Nincs szabad időpont</h3>
                    </div>
                </div>
            </div>
            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="more-options-modal">Vissza</button>
                <button type="button" id="reserve-appointment-">Foglalás áttekintése</button>
            </div>
        </div>
    </div>

    <script>
        function filterResults() {
            updateURL();
            let searchText = document.getElementById("search").value.toLowerCase();
            let filters = getFilters();

            let filterSet = false;
            let searchSet = false;

            if(searchText.length > 0){
                document.getElementById("filter-indicator").classList.remove("hidden");
                searchSet = true;
            } else {
                document.getElementById("filter-indicator").classList.add("hidden");
                searchSet = false;
            }

            if (Object.values(filters).some(value => value.trim() !== "") || searchSet) {
                document.getElementById("filter-indicator").classList.remove("hidden");
                filterSet = ((Object.values(filters).some(value => value.trim() !== "") && searchSet) || (Object.values(filters).some(value => value.trim() !== "")));
            } else {
                document.getElementById("filter-indicator").classList.add("hidden");
                filterSet = false;
            }

            if(filterSet && searchSet){
                document.getElementById("filter-indicator").innerText = "Szűrés és keresés eredményei:";
            } else if (filterSet) {
                document.getElementById("filter-indicator").innerText = "Szűrés eredményei:";
            } else if (searchSet){
                document.getElementById("filter-indicator").innerText = "Keresés eredményei:";
            }

            let serviceOptions = document.querySelectorAll(".service-reservation-container");

            serviceOptions.forEach(serviceOption => {
                let doctorName = serviceOption.querySelector("h2").textContent.toLowerCase();
                let hospitalName = serviceOption.querySelector(".hospitalName").textContent.toLowerCase();

                let hospitalFilterBool = true;
                let serviceFilterBool = true;

                if("korhaz" in filters && filters['korhaz'] != ""){
                    if(serviceOption.dataset.hospital_name != filters['korhaz']){
                        hospitalFilterBool = false;
                    }
                }

                if("orvos" in filters && filters['orvos'] != ""){
                    if(doctorName != filters['orvos']){
                        serviceFilterBool = false;
                    }
                }

                if ((doctorName.includes(searchText) || hospitalName.includes(searchText)) && hospitalFilterBool && serviceFilterBool) {
                    serviceOption.classList.remove("hidden");
                } else {
                    serviceOption.classList.add("hidden");
                }
            });
        }
    </script>

    <!-- Üzenet -->
     <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>


    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>


    <script>
        var service_id = <?php echo($service_id); ?>;
        var user_id = <?php echo((isset($_SESSION['user_id']) && isset($_SESSION['logged_in']) && $_SESSION['user_id'] !== null && $_SESSION['logged_in']) ? $_SESSION['user_id'] : 'null'); ?>;
    </script>
    
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/appointments.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/filter.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/modal.js"></script>
</body>