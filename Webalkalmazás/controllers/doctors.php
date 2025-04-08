<?php 
require_once("config.php");
require_once("apiaccess.php");
$url = $GLOBALS['API_DEFAULT_URL'] . "/doctors/GetDoctors";

$doctors_response = ApiAccessGet($url);

$doctors_data = array();

$services = array();

foreach($doctors_response["data"]["doctors"] as $doctor){ // orvosok rendezése cégek alapján
    if($doctor["company_name"] != null && $doctor['name'] != null && $doctor['company_active']){
        $doctors_data[$doctor["company_name"]][] = $doctor;
        foreach($doctor["services"] as $service){
            if(!in_array($service, $services)){
                $services[] = $service;
            }
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
        <div class="page-title">
            <h1>Orvosaink</h1>
        </div>
        <div class="filter-section">
            <div class="filter-section-container max-content">
                <div class="filter-container">
                    <div>
                        <p onclick="OpenFilters()">Szűrő</p>
                        <img src="./assets/static/filter_icon_b.png" alt="szűrés" onclick="OpenFilters()">
                    </div>
                    <div class="filters hidden" id="filters-container">
                        <div class="filter">
                            <p>Cég:</p>
                            <select name="ceg" id="companyFilter" class="filterSetting" onchange="filterResults()">
                                <option value="">Összes cég</option>
                                <?php foreach(array_keys($doctors_data) as $company){ ?>
                                    <option value="<?php echo($company); ?>"><?php echo($company); ?></option>
                                <?php } ?>
                            </select>
                        </div>

                        <div class="filter">
                            <p>Szolgáltatás:</p>
                            <select name="szolgaltatas" id="serviceFilter" class="filterSetting" onchange="filterResults()">
                                <option value="">Összes szolgáltatás</option>
                                <?php foreach($services as $service){ ?>
                                    <option value="<?php echo(mb_strtolower($service)); ?>"><?php echo($service); ?></option>
                                <?php } ?>
                            </select>
                        </div>
                    </div>
                </div>
                <div class="search-container">
                    <input type="text" id="search" placeholder="Keresés...">
                    <img src="./assets/static/search_icon_b.svg" alt="keresés">
                </div>
            </div>
        </div>

        <p class="hidden max-content" id="filter-indicator">Keresés eredményei:</p>

        <?php foreach(array_keys($doctors_data) as $key){ ?>
            <h2 class="max-content title-bottom-margin companyName"><?php echo($key); ?></h2>
            <div class="cards-container grid-3 max-content">
                <?php foreach($doctors_data[$key] as $doctor) {?>
                    <div class="medium-card-container card-padding" data-company="<?php echo($key); ?>">
                        <div class="card-image-circle">
                            <?php if($doctor["profile_image"] !== null) { ?>
                                <img src="<?php echo($doctor["profile_image"]["image"]); ?>" alt="<?php echo($doctor["profile_image"]["image_alt"]); ?>" title="<?php echo($doctor["profile_image"]["image_title"]); ?>">
                            <?php } else { ?>
                                <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/doctor_placeholder.png" alt="helyettesítő kép" title="Helyettesítő kép">
                            <?php }?>
                        </div>
                        <div class="card-details">
                            <div class="main">
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
            <?php if(array_key_last($doctors_data) != $key) { ?>
                <div class="max-content separator">
                    <hr>
                </div>
            <?php } ?>
        <?php } ?>
    </main>


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
            

            let doctors = document.querySelectorAll(".medium-card-container");

            doctors.forEach(doctor => {
                let doctorName = doctor.querySelector(".medium-card-container h3").textContent.toLowerCase();
                let doctorService = doctor.querySelector(".medium-card-container p").textContent.toLowerCase();

                let companyFilterBool = true;
                let serviceFilterBool = true;

                if("ceg" in filters && filters['ceg'] != ""){
                    if(doctor.dataset.company != filters['ceg']){
                        companyFilterBool = false;
                    }
                }

                if("szolgaltatas" in filters && filters['szolgaltatas'] != ""){
                    if(!doctorService.includes(filters['szolgaltatas'])){
                        serviceFilterBool = false;
                    }
                }

                if ((doctorName.includes(searchText) || doctorService.includes(searchText)) && companyFilterBool && serviceFilterBool) {
                    doctor.classList.remove("hidden");
                } else {
                    doctor.classList.add("hidden");
                }
            });

            // címek eltűntetése
            if("ceg" in filters && filters['ceg'] != ""){
                let titles = document.querySelectorAll(".companyName");
                titles.forEach(title => {
                    if(title.innerText != filters['ceg']){
                        title.classList.add("hidden");
                    } else {
                        title.classList.remove("hidden");
                    }
                });
            } else {
                let titles = document.querySelectorAll(".companyName");
                titles.forEach(title => {
                        title.classList.remove("hidden");
                });
            }

            // elválasztók eltűntetése
            if("ceg" in filters && filters['ceg'] != ""){
                let separators = document.querySelectorAll(".separator");
                separators.forEach(separator => {
                        separator.classList.add("hidden");
                });
            } else {
                let separators = document.querySelectorAll(".separator");
                separators.forEach(separator => {
                        separator.classList.remove("hidden");
                });
            }
        }
    </script>

    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/filter.js"></script>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>
</body>