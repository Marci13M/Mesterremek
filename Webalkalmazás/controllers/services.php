<?php 
require_once("config.php");
require_once("apiaccess.php");
$url = $GLOBALS['API_DEFAULT_URL'] . "/services/GetServices";

$services_data = ApiAccessGet($url);

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
            <h1>Szolgáltatások</h1>
        </div>
        <div class="filter-section">
            <div class="filter-section-container max-content">
                <div class="filter-container">
                    <!-- <p>Szűrő</p>
                    <img src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/static/filter_icon_b.png" alt="szűrés"> -->
                </div>
                <div class="search-container">
                    <input type="text" id="search" placeholder="Keresés...">
                    <img src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/static/search_icon_b.svg" alt="keresés">
                </div>
            </div>
        </div>
        
        <p class="hidden max-content" id="filter-indicator">Keresés eredményei:</p>

        <div class="cards-container grid-3 max-content">
            <!-- php kód a dinamikus tartalomgeneráláshoz -->
            <?php 
            foreach($services_data['data']['services'] as $service){ ?>
                <div class="vertical-card-container">
                    <h3><?php echo($service['service_name']); ?></h3>
                    <div class="main">
                        <p>Leírás</p>
                        <p><?php echo($service['service_description']); ?></p>
                    </div>
                    <a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/szolgaltatasok/" . $service['slug']); ?>" class="cta">Tovább</a>
                </div>
            <?php } ?>
        </div>
    </main>

    <script>
        function filterResults() {
            let searchText = document.getElementById("search").value.toLowerCase();

            if(searchText.length > 0){
                document.getElementById("filter-indicator").classList.remove("hidden");
            } else {
                document.getElementById("filter-indicator").classList.add("hidden");
            }

            let services = document.querySelectorAll(".vertical-card-container");

            services.forEach(service => {
                let serviceName = service.querySelector(".vertical-card-container h3").textContent.toLowerCase();

                if (serviceName.includes(searchText)) {
                    service.classList.remove("hidden");
                } else {
                    service.classList.add("hidden");
                }
            });
        }
    </script>

    
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/filter.js"></script>

    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>
</body>