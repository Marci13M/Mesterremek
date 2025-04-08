<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if(preg_match("/^GetReservedTimes(\?|$)/", $function) ? true : false){ //naptári lefoglalt időpontok lekérdezése
            if(count($_GET) == 3 && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);
                $sqlTo = date_add(date_create($to), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

                //foglalások lekérdezése
                $stmt = $conn->prepare("SELECT `reservations`.`id`, `reservations`.`datetime`, `reservations`.`fulfilled`, `hospital_service`.`duration`, `users`.`name`, `hospitals`.`name` AS 'hospital_name', `hospitals`.`address` FROM `reservations` INNER JOIN `hospital_service` ON `reservations`.`service_id` = `hospital_service`.`service_id` AND `reservations`.`hospital_id` = `hospital_service`.`hospital_id` INNER JOIN `users` ON `reservations`.`user_id` = `users`.`id` INNER JOIN `hospitals` ON `reservations`.`hospital_id` = `hospitals`.`id` WHERE `doctor_id` = ? AND (`reservations`.`datetime` BETWEEN ? AND ?) AND `reservations`.`deleted` = ? ORDER BY `reservations`.`datetime` ASC;");
                $stmt->execute([$doctor_id, $from, $sqlTo, false]);
                
                $reservations = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $reservations[] = $row;
                }

                //válasz küldése
                Response::success(["reservations" => $reservations]);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetHolidays(\?|$)/", $function) ? true : false){ //szabadságok lekérdezése
            if(count($_GET) == 3 && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);
                $sqlTo = date_add(date_create($to), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

                //szabadságok lekérdezése
                $stmt = $conn->prepare("SELECT `holidays`.`id`, `holidays`.`start_date`, `holidays`.`end_date` FROM `holidays` WHERE `holidays`.`doctor_id` = ? AND ((`holidays`.`start_date` BETWEEN ? AND ?) OR (`holidays`.`end_date` BETWEEN ? AND ?) OR (`holidays`.`start_date` <= ? AND `holidays`.`end_date` >= ?)) AND `holidays`.`deleted` = ?");
                $stmt->execute([$doctor_id, $from, $sqlTo, $from, $sqlTo, $from, $to, false]);

                $holidays = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $holidays[] = $row;
                }

                //válasz küldése
                Response::success(["holidays" => $holidays]);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetWorktimes(\?|$)/", $function) ? true : false){ //szolgáltatás munkaidő lekérdezése
            if(count($_GET) == 3 && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);
                $sqlTo = date_add(date_create($to), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

                //munkaidő lekérdezése
                $stmt = $conn->prepare("SELECT `worktimes`.`id`, `worktimes`.`service_id`, `worktimes`.`date`, `worktimes`.`from_time`, `worktimes`.`to_time` FROM `worktimes` WHERE `worktimes`.`doctor_id` = ? AND (`worktimes`.`date` BETWEEN ? AND ?) AND `worktimes`.`deleted` = ? ORDER BY `worktimes`.`date` ASC");
                $stmt->execute([$doctor_id, $from, $sqlTo, false]);

                $worktimes = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $worktimes[] = $row;
                }

                //válasz küldése
                Response::success(["worktimes" => $worktimes]);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetHospitals(\?|$)/", $function) ? true : false){ //szolgáltatás munkaidő lekérdezése
            if(count($_GET) == 2 && isset($_GET['doctor_id']) && isset($_GET["service_id"])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $service_id = htmlspecialchars($_GET['service_id']);

                //kórházak lekérdezése
                $stmt = $conn->prepare("SELECT `hospitals`.`id`, `hospitals`.`name` FROM `doctor_hospital` INNER JOIN `hospitals` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id` INNER JOIN `hospital_service` ON `hospital_service`.`hospital_id` = `hospitals`.`id` WHERE `doctor_hospital`.`user_id` = ? AND `hospital_service`.`service_id` = ? AND `hospitals`.`deleted` = ?");
                $stmt->execute([$doctor_id, $service_id, false]);

                $hospitals = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $hospitals[] = $row;
                }

                //válasz küldése
                Response::success(["hospitals" => $hospitals]);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetHolidayInfo(\?|$)/", $function) ? true : false){ // szabadság adatainak lekérdezése
            if(count($_GET) == 2 && isset($_GET['doctor_id']) && isset($_GET['holiday_id'])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $holiday_id = htmlspecialchars($_GET['holiday_id']);

                //szabadság lekérdezése
                $stmt = $conn->prepare("SELECT `holidays`.`start_date`, `holidays`.`end_date`, `holidays`.`note` FROM `holidays` WHERE `holidays`.`doctor_id` = ? AND `holidays`.`id` = ? AND `holidays`.`deleted` = ?");
                $stmt->execute([$doctor_id, $holiday_id, false]);

                $holiday = $stmt->fetch(PDO::FETCH_ASSOC);

                //válasz küldése
                if($holiday){
                    Response::success(["holiday" => $holiday]);
                } else {
                    Response::error(404, "Nem találtunk ilyen szabadságot");
                }

            } else {
                Response::error(400, "Hibás kérés!", [count($_GET)]);
            }
        } else if(preg_match("/^GetWorktimeInfo(\?|$)/", $function) ? true : false){ // munkaidő adatainak lekérdezése
            if(count($_GET) == 2 && isset($_GET['doctor_id']) && isset($_GET['worktime_id'])){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];

                //adatok
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $worktime_id = htmlspecialchars($_GET['worktime_id']);

                //munkaidő lekérdezése
                $stmt = $conn->prepare("SELECT `hospitals`.`name` AS 'hospital_name', `services`.`name` AS 'service_name', `worktimes`.`date`, `worktimes`.`from_time` AS 'from', `worktimes`.`to_time` AS 'to' FROM `worktimes` INNER JOIN `hospitals` ON `worktimes`.`hospital_id` = `hospitals`.`id` INNER JOIN `services` ON `worktimes`.`service_id` = `services`.`id` WHERE `worktimes`.`doctor_id` = ? AND `worktimes`.`id` = ? AND `worktimes`.`deleted` = ?");
                $stmt->execute([$doctor_id, $worktime_id, false]);

                $worktime = $stmt->fetch(PDO::FETCH_ASSOC);

                //válasz küldése
                if($worktime){
                    Response::success(["worktime" => $worktime]);
                } else {
                    Response::error(404, "Nem találtunk ilyen munkaidő");
                }

            } else {
                Response::error(400, "Hibás kérés!", [count($_GET)]);
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "POST"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "ChangeServiceColor"){ // Szolgáltatás szín változtatása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $color = isset($input['color']) ? htmlspecialchars($input['color']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($user_id === null || $service_id === null || $color === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            
            //összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_service` WHERE `doctor_service`.`user_id` = ? AND `doctor_service`.`service_id` = ?");
            $stmt->execute([$user_id, $service_id]);

            $doctor_service = $stmt->fetch(PDO::FETCH_ASSOC);

            if($doctor_service){
                // szín változás ellenőrzése
                if($doctor_service['color'] != $color){
                    // szín cseréje
                    $stmt = $conn->prepare("UPDATE `doctor_service` SET `doctor_service`.`color` = ? WHERE `doctor_service`.`user_id` = ? AND `doctor_service`.`service_id` = ?");
                    $stmt->execute([$color, $user_id, $service_id]);
                    
                    $logger->LogUserAction($requestingUser, "Change service color", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "doctor_id" => $user_id, "old_color" => $doctor_service['color'], "new_color" => $color]);
                    Response::success();
                } else {
                    Response::success();
                }
            } else {
                Response::error(404, "Nem találtunk ilyen szolgáltatást!");
            }
        } else if($function == "AddHoliday"){ // Szabadság hozzáadása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $start = isset($input['start']) ? htmlspecialchars($input['start']) : null;
            $end = isset($input['end']) ? htmlspecialchars($input['end']) : null;
            $note = isset($input['note']) ? htmlspecialchars($input['note']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($user_id === null || $start === null || $end === null || $note === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            
            // megjegyzés ellenőrzése
            if(strlen(trim($note)) == 0){
                Response::error(400, "Megjegyzés kitöltése kötelező!");
            }

            $sqlEnd = date_add(date_create($end), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

            // szabadnapok lerdezése - ne legyen átfedés
            $stmt = $conn->prepare("SELECT `holidays`.`start_date`, `holidays`.`end_date` FROM `holidays` WHERE `holidays`.`doctor_id` = ? AND ((`holidays`.`start_date` BETWEEN ? AND ?) OR (`holidays`.`end_date` BETWEEN ? AND ?) OR (`holidays`.`start_date` <= ? AND `holidays`.`end_date` >= ?)) AND `holidays`.`deleted` = ?");
            $stmt->execute([$user_id, $start, $end, $start, $end, $start, $end, false]);

            $holidays = $stmt->fetch(PDO::FETCH_ASSOC);
            if(!$holidays){
                // nincs belelógó szabadság
                // szabadság ellenőrzése időpontilag
                if(strtotime($start) <= strtotime($end)){
                    if(strtotime($start) > strtotime("now")){
                        // szabadság ellenőrzése rögzített munkanapokat tekintve
                        $stmt = $conn->prepare("SELECT `worktimes`.`date` FROM `worktimes` WHERE (`worktimes`.`date` BETWEEN ? AND ?) AND `worktimes`.`doctor_id` = ? AND `worktimes`.`deleted` = ?");
                        $stmt->execute([$start, $sqlEnd, $user_id, false]);

                        $worktimes = $stmt->fetch(PDO::FETCH_ASSOC);
                        if(!$worktimes){
                            // szabadság rögzítése
                            $stmt = $conn->prepare("INSERT INTO `holidays` (doctor_id, start_date, end_date, note) VALUES (?, ?, ?, ?)");
                            if($stmt->execute([$user_id, $start, $end, $note])){
                                $holiday_id = $conn->lastInsertId();

                                $logger->LogUserAction($requestingUser, "Add holiday", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $holiday_id, ["success" => true]);
                                Response::success();
                            } else {
                                Response::error(500, "Nem sikerült rögzíteni");
                            }
                        } else {
                            Response::error(400, "Ebben az időszakban már van rögzített munkanap");
                        }
                    } else {
                        Response::error(400, "A szabadság nem kezdődhet a múltban vagy a mai napon");
                    }
                } else {
                    Response::error(400, "A kezdet nem lehet a vége előtt");
                }

                
            } else {
                Response::error(400, "Ebben az idősávban már van szabadnap rögzítve");
            }
        } else if($function == "AddWorkday"){ // Munkanap hozzáadása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $startTime = isset($input['startTime']) ? htmlspecialchars($input['startTime']) : null;
            $endTime = isset($input['endTime']) ? htmlspecialchars($input['endTime']) : null;
            $date = isset($input['date']) ? htmlspecialchars($input['date']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($user_id === null || $service_id === null || $hospital_id === null || $startTime === null || $endTime === null || $date === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            if($startTime == $endTime){
                Response::error(400, "A munka kezdete és vége nem egyezhetnek");
            }

            // összerendelések ellenőrzése
            // orvos - kórház
            $stmt = $conn->prepare("SELECT * FROM `doctor_hospital` WHERE `doctor_hospital`.`user_id` = ? AND `doctor_hospital`.`hospital_id` = ?");
            $stmt->execute([$user_id, $hospital_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) == false){
                Response::error(404, "Az orvoshoz nem tartozik ilyen kórház");
            }

            // orvos - szolgáltatás
            $stmt = $conn->prepare("SELECT * FROM `doctor_service` WHERE `doctor_service`.`user_id` = ? AND `doctor_service`.`service_id` = ?");
            $stmt->execute([$user_id, $service_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) == false){
                Response::error(404, "Az orvoshoz nem tartozik ilyen szolgáltatás");
            }

            // szabadnapok lerdezése - ne legyen átfedés
            $stmt = $conn->prepare("SELECT `holidays`.`start_date`, `holidays`.`end_date` FROM `holidays` WHERE `holidays`.`doctor_id` = ? AND (? BETWEEN start_date AND end_date) AND `holidays`.`deleted` = ?");
            $stmt->execute([$user_id, $date, false]);

            $holidays = $stmt->fetch(PDO::FETCH_ASSOC);
            if(!$holidays){
                // nincs belelógó szabadság
                // munkanap ellenőrzése időpontilag
                if(strtotime($date . " " . $startTime) <= strtotime($date . " " . $endTime)){
                    if(strtotime($date) > strtotime("now")){
                        // munkanap ellenőrzése rögzített munkanapokat tekintve
                        $stmt = $conn->prepare("SELECT `worktimes`.`id` FROM `worktimes` WHERE (? < `worktimes`.`to_time` AND ? > `worktimes`.`from_time`) AND `worktimes`.`date` = ? AND `worktimes`.`doctor_id` = ? AND `worktimes`.`deleted` = ?");
                        $stmt->execute([$startTime, $endTime, $date, $user_id, false]);

                        $worktimes = $stmt->fetch(PDO::FETCH_ASSOC);
                        if(!$worktimes){
                            // kórház nyitvatartás ellenőrzése
                            $stmt = $conn->prepare("SELECT `hospitals`.`open_hours` FROM `hospitals` WHERE `hospitals`.`id` = ?");
                            $stmt->execute([$hospital_id]);

                            $open_hours = $stmt->fetch(PDO::FETCH_ASSOC);

                            if($open_hours){
                                // napi nyitvatartás ellenőrzése
                                $open_hours_json = json_decode($open_hours['open_hours']);
                                $dayNum = date("N", strtotime($date)) - 1;

                                if(isset($open_hours_json->$dayNum)){
                                    $day_open = $open_hours_json->$dayNum;
                                    if(strtotime($date . ' ' . $day_open[0]) <= strtotime($date . " " . $startTime) && strtotime($date . ' ' . $day_open[1]) >= strtotime($date . " " . $endTime)){
                                        // munkanap rögzítése
                                        $stmt = $conn->prepare("INSERT INTO `worktimes` (doctor_id, service_id, hospital_id, date, from_time, to_time) VALUES (?, ?, ?, ?, ?, ?)");
                                        if($stmt->execute([$user_id, $service_id, $hospital_id, $date, $startTime, $endTime])){
                                            $worktime_id = $conn->lastInsertId();

                                            $logger->LogUserAction($requestingUser, "Add worktime", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $worktime_id, ["success" => true]);
                                            Response::success();
                                        } else {
                                            Response::error(500, "Nem sikerült rögzíteni");
                                        }
                                    } else {
                                        Response::error(400, "A megadott idő a nyitvatartáson kívül esik");
                                    }
                                } else {
                                    Response::error(400, "Nincs nyitva a kórház a megadott napon");
                                }
                            } else {
                                Response::error(400, "Nincs nyitvatartása a kórháznak");
                            }
                        } else {
                            Response::error(400, "Ebben az időszakban már van rögzített munkaidő");
                        }
                    } else {
                        Response::error(400, "A munkanap nem kezdődhet a múltban vagy a mai napon");
                    }
                } else {
                    Response::error(400, "A kezdet nem lehet a vége előtt");
                }
            } else {
                Response::error(400, "Ebben az idősávbanvan rögzített szabadnap!");
            }
        } else if($function == "OpenCalendar"){
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($user_id === null) {
                Response::error(400, "Hiányzó adat!");
            }

            //orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctors` INNER JOIN `users` ON `doctors`.`user_id` = `users`.`id`  WHERE `doctors`.`user_id` = ? AND `users`.`deleted` = ?");
            $stmt->execute([$user_id, false]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                // token generálása, adatok visszaküldése
                $token = Token::generateToken($conn, $user_id, "+30 second");
                $link = Token::GenerateLinkToken($conn, $user_id, "view-calendar", 0.5, ["token" => $token]);
                Response::success(['link' => $GLOBALS['DEFAULT_URL'] . "/naptar?data=" . $link['data'] . "&signiture=" . $link['signiture']]);
            } else {
                Response::error(404, "Nincs ilyen orvos!");
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "DeleteHoliday"){ // szabadság törlése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //adatok kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $holiday_id = isset($input['holiday_id']) ? htmlspecialchars($input['holiday_id']) : null;

            if($user_id === null || $holiday_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // feltételek ellenőrzése (nem lehet múltban, nem kezdődhet a mai napon)
            // szabadság lekérdezése
            $stmt = $conn->prepare("SELECT `holidays`.`start_date`, `holidays`.`end_date` FROM `holidays` WHERE `holidays`.`id` = ? AND `holidays`.`doctor_id` = ? AND `holidays`.`deleted` = ?");
            $stmt->execute([$holiday_id, $user_id, false]);

            $holiday = $stmt->fetch(PDO::FETCH_ASSOC);

            if($holiday){
                // szűrés napokra
                if(strtotime($holiday['start_date']) > strtotime("now")){
                    // szabadság törlése
                    $stmt = $conn->prepare("UPDATE `holidays` SET `holidays`.`deleted` = true WHERE `holidays`.`id` = ?");
                    $stmt->execute([$holiday_id]);

                    $logger->LogUserAction($requestingUser, "Delete holiday", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $holiday_id, ["success" => true]);
                    Response::success();
                } else {
                    Response::error(500, "A szabadság nem törölhető");
                }
            } else {
                Response::error(404, "Nem találtunk ilyen szabadságot");
            } 
        } else if($function == "DeleteWorkday"){ // munkanap törlése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //adatok kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $worktime_id = isset($input['worktime_id']) ? htmlspecialchars($input['worktime_id']) : null;

            if($user_id === null || $worktime_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // feltételek ellenőrzése (nem lehet múltban, nem kezdődhet a mai napon, nem lehet foglalt időpont arra a napra)
            // munkanap lekérdezése
            $stmt = $conn->prepare("SELECT `worktimes`.`date`, `worktimes`.`from_time`, `worktimes`.`to_time` FROM `worktimes` WHERE `worktimes`.`id` = ? AND `worktimes`.`doctor_id` = ? AND `worktimes`.`deleted` = ?");
            $stmt->execute([$worktime_id, $user_id, false]);

            $worktime = $stmt->fetch(PDO::FETCH_ASSOC);

            if($worktime){
                // szűrés napokra
                if(strtotime($worktime['date']) > strtotime("now")){
                    // szűrés foglalásra
                    // $to = date_add(date_create($worktime['date']), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

                    $stmt = $conn->prepare("SELECT `reservations`.`id` FROM `reservations` WHERE (`reservations`.`datetime` >= ? AND `reservations`.`datetime` <= ?) AND `reservations`.`doctor_id` = ? AND `reservations`.`deleted` = ?");
                    $stmt->execute([$worktime['date'] . " " . $worktime['from_time'], $worktime['date'] . " " . $worktime['to_time'], $user_id, false]);

                    $reservations = $stmt->fetch(PDO::FETCH_ASSOC);
                    if($reservations == false){
                        // munkanap törlése
                        $stmt = $conn->prepare("UPDATE `worktimes` SET `worktimes`.`deleted` = true WHERE `worktimes`.`id` = ?");
                        $stmt->execute([$worktime_id]);

                        $logger->LogUserAction($requestingUser, "Delete worktime", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $worktime_id, ["success" => true]);
                        Response::success([$reservations]);
                    } else {
                        Response::error(500, "Az időintervallumon már van lefoglalt időpont");
                    }
                } else {
                    Response::error(500, "A munkaidő nem törölhető");
                }
            } else {
                Response::error(404, "Nem találtunk ilyen munkaidőt");
            } 
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else {
    Response::error(400, "Hibás metódus típus!", ["method" => $_SERVER['REQUEST_METHOD']]);
}
?>