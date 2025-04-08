<!DOCTYPE html>
<html lang="hu">

<head>
    <!-- META tagek -->
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <!-- Oldal címe -->
    <title>CareCompass - Főoldal</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/footer.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/landing.css">

</head>

<body>
    <!-- Navigáció -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . '/includes/navigation.php') ?>

    <main>
        <!-- Képnek és oldalcímnek konténer -->
        <div class="rectangle-container">
            <div class="rectangle">
                <h1>CareCompass</h1>
                <h2>Szolgáltatások egy helyen</h2>
                <div class="button-position">
                    <button onclick="location.href='<?php echo($GLOBALS['DEFAULT_URL'] . '/szolgaltatasok'); ?>'" class="btn-fill">Részletek</button>
                </div>
            </div>
        </div>
        <!-- Kép slidernek konténer -->
        <div class="slider-container">
            <div class="slider">
                <img src="assets/images/landing/1.jpg" alt="Kép 1">
                <img src="assets/images/landing/2.jpg" alt="Kép 2">
                <img src="assets/images/landing/3.jpg" alt="Kép 3">
            </div>
            <button class="prev" onclick="moveSlide(-1)">&#10094;</button>
            <button class="next" onclick="moveSlide(1)">&#10095;</button>
            </div>
        </div>
        <!-- Értékelés címnek konténer -->
        <div class="rating-container">
            <h2>Rólunk mondták</h2>
        </div>
        <!-- Értékelések kártyáknak konténer -->
        <div class="cards-container">
            <div class="card">
                <img src="assets/images/landing/geza.png" alt="Kép egy elégedett ügyfélről">
                <div class="card-text">
                    <h1>Géza Bácsi</h1>
                    <p>A CareCompass használatával sikerült gyorsan megtalálnom a nekem megfelelő szolgáltatást!</p>
                </div>
            </div>
            <div class="card">
                <img src="assets/images/landing/lajos.png" alt="Kép egy elégedett ügyfélről">
                <div class="card-text">
                    <h1>O Lajos</h1>
                    <p>A CareCompass használatával sikerült gyorsan megtalálnom a nekem megfelelő szolgáltatást!</p>
                </div>
            </div>
            <div class="card">
                <img src="assets/images/landing/zoltan.png" alt="Kép egy elégedett ügyfélről">
                <div class="card-text">
                    <h1>Boro Zoltán</h1>
                    <p>A CareCompass használatával sikerült gyorsan megtalálnom a nekem megfelelő szolgáltatást!</p>
                </div>
            </div>
        </div>
    </main>

    <!-- Footer -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/footer.php"); ?>

    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <!-- JavaScript szkriptek importálása -->
    <script src="assets/scripts/landing.js"></script>
</body>

</html>