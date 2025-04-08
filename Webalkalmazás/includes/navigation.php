<?php 
require_once("config.php");

if(isset($_SESSION['view-calendar']) && $_SESSION['view-calendar'] == true){
    Redirect("/kijelentkezes"); 
}

// menü lekérdezése adatbázisból
$db = new Database($GLOBALS["database_host"], $GLOBALS["database_name"], $GLOBALS["database_user"], $GLOBALS["database_password"]);
$conn = $db->connect();

$stmt = $conn->prepare("SELECT `endpoints`.`id` AS 'id', `endpoints`.`name` AS 'name', `endpoints`.`endpoint` AS 'endpoint', `endpoints`.`parent_id` AS 'parent_id', `endpoints`.`dynamic_source` FROM `endpoints` WHERE `endpoints`.`active` = ? AND `endpoints`.`menu_item` = ? AND `endpoints`.`deleted` = ? ORDER BY `endpoints`.`position` ASC");
$stmt->execute([true, true, false]);

$menuItems = $stmt->fetchAll(PDO::FETCH_ASSOC);

function getDynamicOptions($source) {
    global $conn;
    $stmt = $conn->prepare("SELECT `id`, `name`, `slug` FROM `" . $source . "` WHERE deleted = ? ORDER BY `name` ASC");
    $stmt->execute([false]);
    return $stmt->fetchAll(PDO::FETCH_ASSOC);
}

function buildMenu(array $menuItems, $parentId = null) {
    $menu = [];

    foreach ($menuItems as $item) {
        if ($item['parent_id'] == $parentId) {
            if(!empty($item["dynamic_source"])){
                $item["dynamic_options"] = getDynamicOptions($item['dynamic_source']);
            }
            $item['children'] = buildMenu($menuItems, $item['id']); // Find children
            $menu[] = $item;
        }
    }

    return $menu;
}

$menuTree = buildMenu($menuItems);

?>

<nav>
    <div class="nav-container max-width" data-open="false">

        <div class="nav-elements-container icon-container">
            <a href="<?php echo($GLOBALS['DEFAULT_URL']); ?>"><img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_w.png" alt="fehér logó" title="Céglogó" class="nav-logo"></a>
            <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/menu_icon_w.svg" alt="hamburger menü ikon" title="Menü ikon" class="hamburger-icon" id="phone-nav-opener">
        </div>
        <div class="nav-elements-container menu-options phone">
            <ul class="nav-list-items">
                <?php foreach($menuTree as $item){ ?>
                    <li class="nav-list-item">
                        <?php if(!empty($item["children"])){ ?> <!-- ha van lenyíló menü -->
                            <p href="" class="nav-link bold dropdown-opener" <?php if(!empty($item["children"])){ echo("id='drop-1'"); } ?>><?php echo($item['name']); ?></p>
                            <div class="nav-dropdown" id="drop-1-drop">
                                <ul class="nav-list-items grid-column-3">
                                    <li class="nav-list-item padding-small"><a href="<?php echo($GLOBALS['DEFAULT_URL']) . '/' . $item['endpoint'] ?>" class="nav-link">Összes</a></li>
                                    <?php foreach($item["children"][0]["dynamic_options"] as $option) { ?>
                                        <li class="nav-list-item padding-small"><a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/" .str_replace(":" . explode(":", $item["children"][0]['endpoint'])[1], $option['slug'],$item["children"][0]['endpoint'])); ?>" class="nav-link"><?php echo($option["name"]); ?></a></li>
                                    <?php } ?>
                                </ul>
                            </div>
                        <?php } else { ?>
                            <a href="<?php echo($GLOBALS['DEFAULT_URL']) . '/' . $item['endpoint'] ?>" class="nav-link bold dropdown-opener" <?php if(!empty($item["children"])){ echo("id='drop-1'"); } ?>><?php echo($item['name']); ?></a>
                        <?php } ?>
                    </li>
                <?php } ?>
            </ul>
        </div>
        <div class="nav-elements-container user-action phone">
            <!-- Bejelentkezés ellenőrzése -->
            <?php if(isset($_SESSION["logged_in"]) && $_SESSION["logged_in"] && isset($_SESSION['user_id']) && !empty($_SESSION['user_id'])){?>
                <div class="relative-position">
                    <div class="nav-link dropdown-opener user-login" id="drop-2">
                        <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/user_w.png" alt="user_w.png" title="Felhasználó ikon" class="icon">
                        <p class="bold" id="navigation-username"><?php echo($_SESSION["user_name"]); ?></p>
                    </div>
                    <div class="nav-dropdown width-100" id="drop-2-drop">
                        <ul class="nav-list-items grid-column-1">
                            <li class="nav-list-item padding-small"><a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/profil" class="nav-link bold">Adataim</a></li>
                            <li class="nav-list-item padding-small"><a href="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/kijelentkezes" class="nav-link bold logout">Kijelentkezés</a></li>
                        </ul>
                    </div>
                </div>

            <?php } else { ?>
                <a href="<?php echo($GLOBALS["DEFAULT_URL"] . "/bejelentkezes"); ?>" class="nav-list-item nav-link bold">Bejelentkezés</a>
            <?php } ?>
        </div>
    </div>
</nav>


<?php if(isset($_SESSION['confirmEmail']) && $_SESSION['confirmEmail'] == true) { ?>

    <div class="headline-info-container">
        <div class="headline-info max-width">
            <p>Erősítse meg email címét!</p>
            <button onclick="sendEmailConfirmation();">Email újraküldése</button>
        </div>
    </div>

    <script>
        async function sendEmailConfirmation(){
            console.log("katt");
            //adatok lekérdezése
            var requestURL = `<?php echo($GLOBALS['DEFAULT_URL']); ?>/apiaccess.php`;
        
            var requestData = {
                "data": {},
                "url": `<?php echo($GLOBALS['API_DEFAULT_URL']); ?>/users/ResendEmailConfirm`,
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
                AddMessage("success", data.message);
            } else {
                AddMessage("error", data.error.message);
            }
        }
    </script>

<?php } ?>
<!-- Szkriptek -->
<script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/jquery-3.7.1.min.js"></script>
<script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/navigation.js"></script>