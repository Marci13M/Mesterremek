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
        <div class="page-title white">
            <h1>Kapcsolat</h1>
        </div>

        <div class="contact-form-container">
            <h2>Írjon üzenetet</h2>
            <div class="contact-form" id="contact-form">
                <input type="text" name="name" id="name" placeholder="Teljes név">
                <input type="email" name="email" id="email" placeholder="Email cím">
                <input type="phone" name="phone" id="phone" placeholder="Telefonszám">
                <input type="text" name="subject" id="subject" placeholder="Tárgy">
                <textarea name="message" id="message" placeholder="Üzenet"></textarea>
                <p>Az elküldéssel hozzájárulok az adatkezelési tájékoztatóban foglaltakhoz és elolvastam azt!</p>
                <!-- <div class="element-with-label">
                    <input type="checkbox" name="datahandling" id="datahandling">
                    <label for="datahandling">Hozzájárulok az adatkezelési tájékoztatóban foglaltakhoz és elolvastam azt!</label>
                </div> -->
                <button type="button" class="form-submit-button" onclick="sendContactForm()" id="contact-form-btn">
                    <span class="btn-text">Elküld</span>
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

        <div class="section-container">
            <h2>Elérhetőség</h2>
            <div class="company-data-list max-content">
                <div>
                    <p>Cégnév:</p>
                    <p class="bold">CareCompass Kft.</p>
                </div>
                <div>
                    <p>Cím:</p>
                    <p class="bold">0000 Tesztfalva, Teszt utca 0.</p>
                </div>
                <div>
                    <p>Telefon:</p>
                    <p class="bold">+36 30 123 4567</p>
                </div>
                <div>
                    <p>E-mail:</p>
                    <p class="bold">teszt@pelda.hu</p>
                </div>
                <div>
                    <p>Adószám: </p>
                    <p class="bold">12345678-2-10</p>
                </div>
                <div>
                    <p>Cégjegyzékszám:</p>
                    <p class="bold">00-00-000000</p>
                </div>
                <div>
                    <p>Kapcsolattartó neve:</p>
                    <p class="bold">Teszt Elek</p>
                </div>
                <div>
                    <p>Kapcsolattartó telefonszáma:</p>
                    <p class="bold">+36 30 123 4567</p>
                </div>
                <div>
                    <p>Kapcsolattartó e-mail címe:</p>
                    <p class="bold">teszt@pelda.hu</p>
                </div>
            </div>
        </div>
    </main>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>

    <!-- Szkriptek -->
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/scripts/contact.js"></script>
</body>