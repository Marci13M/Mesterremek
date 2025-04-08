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
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/navigation.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/style/footer.css">
</head>

<body>

    <?php require_once('./includes/navigation.php') ?>

    <main>
        <div class="page-title white">
            <h1>Rólunk</h1>
        </div>
        
        <div class="text-section max-content">
            <h2>A CareCompass története</h2>
            <p>A CareCompass története nem nyúlik olyan régre vissza. 2024 szeptemberében a 3 alapító tag elhatározta, hogy valami olyat szeretne létrehozni amivel megkönnyítheti az emberek dolgát. Hatalmas lelkesedéssel vágtunk bele ebbe a projektbe, hiszen tudtuk ezzel valami olyat hoznánk létre ami megváltoztathatja az emberek mindennapjait. Nagy igyekezettel fejlesztettük/fejlesztjük a CareCompass-t hiszen tudjuk mennyire fontos felhasználóinknak.</p>
            <p>Fejlesztésünk első fázisaiban átgondoltuk céljaink, megterveztük mit szeretnénk létrehozni. Hosszú folyamat volt, de egy-két nehézséget leszámítva hamar elő álltunk egy olyan tervvel aminek segítségevel elkezdhettük a fejlesztést.</p>
            <p>A CareCompass történetében a második fázisa maga a fejlesztést jelentette. Rengeteg munka, lemondás, de hatalmas lelkesedés jellemezte ezt az időszakot. Sok probléma merült fel, de mindegyiket közös erővel leküzdöttük, hisz számunkra ez a projekt jelentette az álmunkat.</p>
            <p>Végül amikor már a fejlesztés végén jártunk, láttuk a fényt az alagút végén és egyre jobban állt össze az a program hármas, amivel rengeteg ember mindennapjait könnyíthetjük meg. Folyamatosan fejlesztjük a CareCompass-t új funkciókkal, mivel szeretnénk felhasználóinknak minél jobb élményt nyújtani.</p>
        </div>
        <div class="text-section max-content">
            <h2>Alapítók</h2>
            <div class="figures-container grid-3">
                <figure>
                    <img src="./assets/images/profil/babi_levente.jpg" alt="Bábi Levente" title="Bábi Levente">
                    <figcaption>Bábi Levente</figcaption>
                </figure>
                <figure>
                    <img src="./assets/images/profil/nyikonyuk_gergo.jpg" alt="Nyikonyuk Gergő" title="Nyikonyuk Gergő">
                    <figcaption>Nyikonyuk Gergő</figcaption>
                </figure>
                <figure>
                    <img src="./assets/images/profil/szlonkai_marcell.jpg" alt="Szlonkai Marcell" title="Szlonkai Marcell">
                    <figcaption>Szlonkai Marcell</figcaption>
                </figure>
            </div>
        </div>

        <div class="text-section max-content">
            <h2>Célkitűzésünk</h2>
            <p>A CareCompass célja, hogy minden felhasználójának segítségét nyújtson aki magánegészségügyben szeretne számára leginkább megfelelő szolgáltatást találni. Cégünk megalakuláskor egy volt számunkra a lényeg, megkönnyítsük azoknak az embereknek az életét akik nem tudnak kiigazodni a rengeteg kórház szolgáltatásai közt.</p>
            <p>Számunkra CareCompass több mint egy program vagy épp mint egy weboldal, ez egy olyan szolgáltatás amivel emberek mindennapjait könnyíthetjük meg. Mindig is az volt a fő célkitűzésünk, hogy olyat alkossunk amit mi is szívesen használnánk és számunkra is segítséget nyújtana.</p>
            <p>Alapítóink nevében mondhatom, hogy a fő célunkat elértük, de számunkra határ a csillagos ég, addig fogjuk fejleszteni a CareCompass-t amíg minden felhasználónk számára a lehető legjobb élményt nem biztosítjuk.</p>
        </div>
    </main>


    <?php require_once("./includes/footer.php"); ?>
</body>