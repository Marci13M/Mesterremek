<?php 
require_once("config.php");
require_once("apiaccess.php");
$url = $GLOBALS['API_DEFAULT_URL'] . "/hospitals/GetHospitals";

$hospitals = ApiAccessGet($url);

// kórházak rendezése cégek szerint

$hospitals_data = array();

foreach($hospitals['data']['hospitals'] as $hospital) {
    if($hospital['hospital_active']){
        $hospitals_data[$hospital['company_name']][] = $hospital;
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
            <h1>Partnereink</h1>
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
                                <?php foreach(array_keys($hospitals_data) as $company){ ?>
                                    <option value="<?php echo($company); ?>"><?php echo($company); ?></option>
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
        
            <!-- php kód a dinamikus tartalomgeneráláshoz -->
            <?php 
            foreach(array_keys($hospitals_data) as $company){  
            ?>
                <h2 class="max-content title-bottom-margin companyName"><?php echo($company); ?></h2>
                <div class="cards-container grid-2 max-content">
            <?php 
                foreach($hospitals_data[$company] as $hospital){
            ?>
                
                <div class="medium-card-container card-no-gap" data-company="<?php echo($company); ?>">
                    <div class="card-image">
                        <?php if($hospital["images"]["profile_img"] !== null) { ?>
                            <img loading="lazy" src="<?php echo($hospital['images']['profile_img']['image']); ?>" alt="<?php echo($hospital['images']['profile_img']['alt']); ?>" title="<?php echo($hospital['images']['profile_img']['title']); ?>">
                        <?php } else { ?>
                            <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/hospital_placeholder.png" alt="helyettesítő kép" title="Helyettesítő kép">
                        <?php  }?>
                    </div>
                    <div class="card-details card-padding">
                        <div class="main">
                            <h3><?php echo($hospital['hospital_name']); ?></h3>
                            <p><?php echo($hospital['hospital_address']); ?></p>
                        </div>
                        <div class="cta">
                            <a href="<?php echo($GLOBALS['DEFAULT_URL'] . "/korhazak/" . $hospital['slug']); ?>">Megtekintés</a>
                        </div>
                    </div>
                </div>
            <?php } ?>
                <?php if(array_key_last($hospitals_data) != $company) { ?>
                    </div>
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

            let hospitals = document.querySelectorAll(".medium-card-container");

            hospitals.forEach(hospital => {
                let hospitalName = hospital.querySelector(".medium-card-container h3").textContent.toLowerCase();
                let hospitalAddress = hospital.querySelector(".medium-card-container p").textContent.toLowerCase();

                let filterBool = true;

                if("ceg" in filters && filters['ceg'] != ""){
                    if(hospital.dataset.company != filters['ceg']){
                        filterBool = false;
                    }
                }


                if ((hospitalName.includes(searchText) || hospitalAddress.includes(searchText)) && filterBool) {
                    hospital.classList.remove("hidden");
                } else {
                    hospital.classList.add("hidden");
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

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?><?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>
</body>