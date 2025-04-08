<?php
require_once("config.php");
require_once($GLOBALS['DEFAULT_PATH'] . "/api/v1/classes/Token.php");


// hozzáférés ellenőrzése

// jelszóváltoztatás adatok ellenőrzése
if((isset($_GET['data']) && isset($_GET['signiture'])) || (isset($_SESSION['view-calendar']) && $_SESSION['view-calendar'])){
    if((isset($_GET['data']) && isset($_GET['signiture']))){
        session_unset();
        $data = Token::CheckLinkToken($conn, htmlspecialchars($_GET['data']), htmlspecialchars($_GET['signiture']));
        if($data['success']){
            $_SESSION['user_id'] = $data["data"]["user_id"];
            $_SESSION['view-calendar'] = true;
            $_SESSION['logged_in'] = true;
            $_SESSION['token'] = $data["data"]["data"]["token"];
            Token::UseToken($conn, $data['token_id']);
            if (!empty($_SERVER['QUERY_STRING'])) {
                Redirect(str_replace("/mesterremek", "", strtok($_SERVER["REQUEST_URI"], '?')));
            }
        } else {
            unset($_SESSION['view-calendar']);
            Redirect("/?error=" . $data['error']['message']);
        }
    }
} else {
    unset($_SESSION['view-calendar']);
    Redirect("/?error=Nincs jogosultsága az oldal megtekintéséhez");
}

?>
<!DOCTYPE html>
<html lang="hu">

<head>
    <!-- META tagek -->
    <meta charset="UTF-8"> 
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <!-- Oldal címe -->
    <title><?php echo($route['name']); ?> - CareCompass</title>

    <!-- Stíluslapok -->
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/base.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/elements.css">
    <link rel="stylesheet" href="<?php echo($GLOBALS['DEFAULT_URL']) ?>/assets/style/calendar.css">
</head>

<body data-onepage="true">

    <section class="person-details-container">
        <div class="logo-container">
            <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/logo_w.png" alt="logo" title="logo">
        </div>
        <div class="person-container">
            <div class="person-image-container">
                <img src="<?php echo($GLOBALS["DEFAULT_URL"]); ?>/assets/static/doctor_placeholder.png" alt="doctor" title="doctor" id="doctor-image">
            </div>
            <div class="person-detail">
                <h1 id="doctor-name">Név</h1>
            </div>
        </div>
    </section>
    <section class="calendar-section">
        <div class="calendar-options-container">
            <div class="calendar">
                <div class="calendar-header">
                    <button id="prev-month">‹</button>
                    <div id="month-year"></div>
                    <button id="next-month">›</button>
                </div>
                <div class="calendar-body">
                    <div class="calendar-weekdays">
                        <div>H</div>
                        <div>K</div>
                        <div>Sz</div>
                        <div>Cs</div>
                        <div>P</div>
                        <div>Sz</div>
                        <div>V</div>
                    </div>
                    <div class="calendar-dates">

                    </div>
                </div>
            </div>

            <div class="calendar-services" id="services">
                <div class="calendar-service">
                    <div class="service-color"></div>
                    <p class="service-name">Szabadság</p>
                    <img src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/static/plus_b.png" alt="Új időpont" class="service-add" name="open-modal-btn" data-modal="holiday-details-modal">
                </div>
            </div>
        </div>
        <div class="main-calendar-container">
            <div class="main-calendar-header" id="main-calendar-header">
                <!-- JAVASCRIPT TÖLTI FEL -->
            </div>
            <div class="main-calendar-body">
                <div class="calendar-hours">
                    <p>0:00</p>
                    <p>1:00</p>
                    <p>2:00</p>
                    <p>3:00</p>
                    <p>4:00</p>
                    <p>5:00</p>
                    <p>6:00</p>
                    <p>7:00</p>
                    <p>8:00</p>
                    <p>9:00</p>
                    <p>10:00</p>
                    <p>11:00</p>
                    <p>12:00</p>
                    <p>13:00</p>
                    <p>14:00</p>
                    <p>15:00</p>
                    <p>16:00</p>
                    <p>17:00</p>
                    <p>18:00</p>
                    <p>19:00</p>
                    <p>20:00</p>
                    <p>21:00</p>
                    <p>22:00</p>
                    <p>23:00</p>
                </div>
                <div class="calendar-days-container">
                    <div class="calendar-day">
                        <div id="day-1-events">
                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-2-events">
                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-3-events">
                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-4-events">

                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-5-events">

                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-6-events">

                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                    <div class="calendar-day">
                        <div id="day-7-events">

                        </div>


                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                        <div class="line-container">
                            <div class="line"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>


    <!-- Felugró ablak -->
    <div class="popup-container" id="holiday-details-modal">
        <div class="popup">
            <h2>Szabadság beállítása</h2>
            <div class="form-flow-column grid-column-2 edit-profile max-width" id="update">
                <div>
                    <p>Kezdete</p>
                    <input type='date' id='holiday-start' value="">
                </div>
                <div>
                    <p>Vége</p>
                    <input type='date' id='holiday-end' value="">
                </div>
                <div class="grid-span-2">
                    <p>Megjegyzés</p>
                    <textarea id="holiday-note" class="fill" style="resize: none;"></textarea>
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="holiday-details-modal">Mégse</button>
                <button type="button" id="save-holiday" onclick="AddHoliday()">Szabadság rögzítése</button>
            </div>
        </div>
    </div>

    <!-- Felugró ablak -->
    <div class="popup-container" id="service-details-modal">
        <div class="popup">
            <h2>Munkaidő beállítása</h2>
            <p style="margin-top: 20px;">Szolgáltatás: <span id="selected-service-name"></span></p>
            <div class="form-flow-column grid-column-2 edit-profile max-width" id="update">
                <div>
                    <p>Kórház</p>
                    <select type='input' id='selected-hospital' class="input-like">
                    </select>
                </div>
                <div>
                    <p>Dátum</p>
                    <input type='date' id='selected-date'>
                </div>
                <div>
                    <p>Kezdés</p>
                    <input type='time' id='selected-date-start' value="08:00">
                </div>
                <div>
                    <p>Végzés</p>
                    <input type='time' id='selected-date-end' value="16:00">
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="service-details-modal">Mégse</button>
                <button type="button" id="save-worktime" onclick="AddWorkday()">Munkaidő rögzítése</button>
            </div>
        </div>
    </div>

    <!-- Felugró ablak -->
    <div class="popup-container" id="change-service-color-modal">
        <div class="popup">
            <h2>Szín módosítása</h2>
            <div class="color-change">
                <div class="current-setting">
                    <p>Jelenlegi szín:</p>
                    <div class="current-color" id="current-color"></div>
                </div>
                <p>Választható színek</p>
                <div class="colors-container" id="colors-container">
                    <!-- JAVASCRIPT tölti fel -->
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="change-service-color-modal">Mégse</button>
                <button type="button" id="save-color" onclick="SaveColor()">Mentés</button>
            </div>
        </div>
    </div>


    <!-- Felugró ablak -->
    <div class="popup-container" id="delete-service-modal">
        <div class="popup">
            <h2>Munkaidő áttekintése</h2>
            <div class="form-flow-column grid-column-2 show-details max-width">
                <div class="grid-span-2">
                    <p>Kórház</p>
                    <p id="service-hospital" class="bold"></p>
                </div>
                <div>
                    <p>Szolgáltatás</p>
                    <p id="service-name" class="bold"></p>
                </div>
                <div>
                    <p>Dátum</p>
                    <p id="service-date" class="bold"></p>
                </div>
                <div>
                    <p>Kezdés</p>
                    <p id="service-from" class="bold"></p>
                </div>
                <div>
                    <p>Végzés</p>
                    <p id="service-to" class="bold"></p>
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="delete-service-modal">Bezár</button>
                <button type="button" class="delete" id="save-worktime" onclick="DeleteWorkday()">Munkaidő törlése</button>
            </div>
        </div>
    </div>


    <!-- Felugró ablak -->
    <div class="popup-container" id="delete-holiday-modal">
        <div class="popup">
            <h2>Szabadság áttekintése</h2>
            <div class="form-flow-column grid-column-2 show-details max-width" id="update">
                <div>
                    <p>Kezdete</p>
                    <p id="holiday-start-info" class="bold"></p>
                </div>
                <div>
                    <p>Vége</p>
                    <p id="holiday-end-info" class="bold"></p>
                </div>
                <div class="grid-span-2">
                    <p>Megjegyzés</p>
                    <p id="holiday-note-info" class="bold"></p>
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="delete-holiday-modal">Bezár</button>
                <button type="button" class="delete" id="save-holiday" onclick="DeleteHoliday()">Szabadság törlése</button>
            </div>
        </div>
    </div>


    <!-- Felugró ablak -->
    <div class="popup-container" id="reservation-details-modal">
        <div class="popup">
            <h2>Időpont áttekintése</h2>
            <div class="form-flow-column grid-column-2 show-details max-width">
                <div class="grid-span-2">
                    <p>Név</p>
                    <p id="reservation-username" class="bold"></p>
                </div>
                <div>
                    <p>Szolgáltatás</p>
                    <p id="reservation-name" class="bold"></p>
                </div>
                <div>
                    <p>Dátum</p>
                    <p id="reservation-date" class="bold"></p>
                </div>
                <div>
                    <p>Kezdet</p>
                    <p id="reservation-from" class="bold"></p>
                </div>
                <div>
                    <p>Vég</p>
                    <p id="reservation-to" class="bold"></p>
                </div>
                <div class="grid-span-2">
                    <p>Kórház</p>
                    <p id="reservation-location" class="bold"></p>
                </div>
            </div>

            <div class="popup-buttons">
                <button type="button" name="close-open-modal-btn" data-modal="reservation-details-modal">Bezár</button>
                <button type="button" id="save-worktime" onclick="FinishReservation()">Időpont lezárása</button>
            </div>
        </div>
    </div>


    <!-- Üzenet -->
    <?php require_once($GLOBALS['DEFAULT_PATH'] . "/includes/msg.php"); ?>

    <script>
        const doctor_id = <?php echo($_SESSION['user_id']); ?>;
    </script>

    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/jquery-3.7.1.min.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/config.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/calendar.js"></script>
    <script src="<?php echo($GLOBALS['DEFAULT_URL']); ?>/assets/scripts/modal.js"></script>

</body>

</html>