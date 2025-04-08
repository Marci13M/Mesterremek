<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if(preg_match("/^GetReservation(\?|$)/", $function) ? true : false){ // lefoglalt időpont lekérdezése
            if(count($_GET) == 1 && isset($_GET['reservation_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //foglalás id lekérdezése
                $reservation_id = htmlspecialchars($_GET['reservation_id']);

                // foglalás lekérdezése
                $stmt = $conn->prepare("SELECT `reservations`.`datetime` AS 'reservation_datetime', `reservations`.`price` AS 'reservation_price', `reservations`.`type` AS 'reservation_type', `reservations`.`fulfilled` AS 'reservations_fulfilled', `users`.`name` AS 'user_name', `hospitals`.`name` AS 'hospital_name', `hospital_service`.`duration` AS 'duration', `services`.`name` AS 'service_name', `services`.`description` AS 'service_description' FROM `reservations` INNER JOIN `users` ON `reservations`.`user_id` = `users`.`id` INNER JOIN `hospitals` ON `reservations`.`hospital_id` = `hospitals`.`id` INNER JOIN `services` ON `reservations`.`service_id` = `services`.`id` INNER JOIN `hospital_service` ON (`hospital_service`.`hospital_id` = `hospitals`.`id` AND `hospital_service`.`service_id` = `services`.`id`) WHERE `reservations`.`id` = ? AND `reservations`.`deleted` = ?");
                $stmt->execute([$reservation_id, false]);
                
                $reservation = $stmt->fetch(PDO::FETCH_ASSOC);

                if($reservation !== false){
                    $respData = [
                        "reservation" => $reservation
                    ];

                    // válasz küldése
                    $logger->LogUserAction($requestingUser, "view reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "reservation_id" => $reservation_id]);
                    Response::success($respData);
                } else {
                    $logger->LogUserAction($requestingUser, "view reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "reservation_id" => $reservation_id]);
                    Response::error(404, "Nincs ilyen lefoglalt időpont!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetUserReservation(\?|$)/", $function) ? true : false){
            if(count($_GET) == 2 && isset($_GET['user_id']) && isset($_GET['reservation_id'])){
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //foglalás id lekérdezése
                $reservation_id = htmlspecialchars($_GET['reservation_id']);
                $user_id = htmlspecialchars($_GET['user_id']);

                // foglalás lekérdezése
                $stmt = $conn->prepare("SELECT `reservations`.`datetime` AS 'reservation_datetime', `reservations`.`price` AS 'reservation_price', `reservations`.`fulfilled` AS 'reservation_fulfilled', `users`.`name` AS 'doctor_name', `hospitals`.`name` AS 'hospital_name', `hospitals`.`address` AS 'hospital_address', `services`.`name` AS 'service_name', `services`.`description` AS 'service_description', `hospital_service`.`duration` AS 'service_duration', `reservations`.`price` AS 'service_price' FROM `reservations` INNER JOIN `users` ON `reservations`.`doctor_id` = `users`.`id` INNER JOIN `hospitals` ON `reservations`.`hospital_id` = `hospitals`.`id` INNER JOIN `services` ON `reservations`.`service_id` = `services`.`id` INNER JOIN `hospital_service` ON (`reservations`.`service_id` = `hospital_service`.`service_id` AND `reservations`.`hospital_id` = `hospital_service`.`hospital_id`) WHERE `reservations`.`id` = ? AND `reservations`.`user_id` = ? AND `reservations`.`deleted` = ?;");
                $stmt->execute([$reservation_id, $user_id, false]);

                $reservation = $stmt->fetch(PDO::FETCH_ASSOC);

                if($reservation !== false){
                    $respData = [
                        "reservation" => $reservation
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs ilyen lefoglalt időpont!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        }else if(preg_match("/^GetUserAllReservation(\?|$)/", $function) ? true : false){ // felhasználó lefoglalt időpontjainak lekérdezése
            if(count($_GET) == 1 && isset($_GET['user_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //felhasználó id lekérdezése
                $user_id = htmlspecialchars($_GET['user_id']);

                // foglalások lekérdezése
                $stmt = $conn->prepare("SELECT `reservations`.`id` AS 'reservation_id',  `reservations`.`datetime` AS 'reservation_datetime', `reservations`.`reserved_at` AS 'reserved_at', `users`.`name` AS 'doctor_name', `services`.`name` AS 'service_name' FROM `reservations` INNER JOIN `users` ON `reservations`.`doctor_id` = `users`.`id` INNER JOIN `services` ON `reservations`.`service_id` = `services`.`id` WHERE `reservations`.`user_id` = ? AND `reservations`.`deleted` = ? ORDER BY `reservations`.`datetime` DESC;");
                $stmt->execute([$user_id, false]);
                
                $reservations = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if(count($reservations) !== 0){
                    $respData = [
                        "reservations" => $reservations,
                        "total_reservations" => count($reservations)
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs a felhasználónak lefoglalt időpontja!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetFreeAppointmentTimes(\?|$)/", $function) ? true : false){ // Szabad időpontok lekérdezése
            if(count($_GET) == 5 && isset($_GET['service_id']) && isset($_GET['hospital_id']) && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // azonosítók lekérdezése
                $service_id = htmlspecialchars($_GET['service_id']);
                $hospital_id = htmlspecialchars($_GET['hospital_id']);
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);
                $sqlTo = date_add(date_create($to), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

                // foglalt időpontok lekérdezése lekérdezése
                $stmt = $conn->prepare("SELECT `reservations`.`datetime` AS 'datetime', `hospital_service`.`duration` AS 'duration' FROM `reservations` INNER JOIN `hospital_service` ON (`hospital_service`.`hospital_id` = `reservations`.`hospital_id` AND `hospital_service`.`service_id` = `reservations`.`service_id`) WHERE `reservations`.`doctor_id` = ? AND `reservations`.`hospital_id` = ? AND (`reservations`.`datetime` BETWEEN ? AND ?) AND `reservations`.`deleted` = ? ORDER BY `reservations`.`datetime` ASC;");
                $stmt->execute([$doctor_id, $hospital_id, $from, $sqlTo, false]);

                // foglalt időpontok
                $reservedTimes = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $reservationDateTime = new DateTime($row['datetime']); // teljes dátum és idő
                    $reservationDate = $reservationDateTime->format("Y-m-d"); // dátum
                    $reservationTime = $reservationDateTime->format("H:i"); // idő
                    $reservedTimes[$reservationDate][] = array($reservationTime, $row['duration']); // asszociatív tömbbe helyezés
                }

                // orvos munkaidejének lekérdezése
                $stmt = $conn->prepare("SELECT `worktimes`.`date` AS 'date', `worktimes`.`from_time` AS 'from', `worktimes`.`to_time` AS 'to' FROM `worktimes` INNER JOIN `users` ON `worktimes`.`doctor_id` = `users`.`id` WHERE `worktimes`.`doctor_id` = ? AND `worktimes`.`hospital_id` = ? AND `worktimes`.`service_id` = ? AND (`worktimes`.`date` BETWEEN ? AND ?) AND `users`.`deleted` = ? AND `worktimes`.`deleted` = ?");
                $stmt->execute([$doctor_id, $hospital_id, $service_id, $from, $to, false, false]);

                // munkaórák
                $worktimes = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $worktimes[$row['date']][] = array($row['from'], $row['to']); // asszociatív tömbbe helyezés
                }

                //kezelés időtartamának lekérdezése
                $stmt = $conn->prepare("SELECT `hospital_service`.`duration` AS 'duration' FROM `hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` WHERE `hospital_service`.`service_id` = ? AND `hospital_service`.`hospital_id` = ? AND `services`.`deleted` = ?");
                $stmt->execute([$service_id, $hospital_id, false]);

                $serviceDuration = $stmt->fetch(PDO::FETCH_ASSOC)['duration'];

                // szabad időpontok keresése
                $freeTimes = array();

                foreach($worktimes as $key => $worktimeDay){
                    foreach($worktimeDay as $worktime){
                        $workStartTime = date_create($key . " " . $worktime[0]);
                        $workEndTime = date_sub(date_create($key . " " . $worktime[1]), date_interval_create_from_date_string($serviceDuration . " minutes")); // csökkentjük a vizsgált kezelés idejével
                        $currentCheckTime = $workStartTime; // éppen vizsgált idő beállítása
                        
                        // adott napra történt e foglalás
                        if(in_array($key, array_keys($reservedTimes))){
                            $reservedForTheDay = $reservedTimes[$key];
                            while($currentCheckTime <= $workEndTime){
                                if(count($reservedForTheDay) > 0){
                                    $endTime = date_create($currentCheckTime->format('Y-m-d H:i:s'));
                                    date_add($endTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                                    $reservationStartDate = date_create($key . " " . $reservedForTheDay[0][0]);
                                    $reservationEndDate = clone $reservationStartDate;
                                    date_add($reservationEndDate, date_interval_create_from_date_string($reservedForTheDay[0][1] . " minutes"));
                                    
                                    if($currentCheckTime != $reservationStartDate && $endTime <= $reservationStartDate && $endTime <= $workEndTime){ // hogyha a viszáglt idő nem egyenlő a foglalás kezdő idejével és a viszgált idő vége kisebb vagy egyenlő a kezelés kezdetével, akkor hozzáadjuk szabad időpontnak
                                        if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                            $freeTimes[$key][] = $currentCheckTime->format("H:i");
                                        }
                                    } else { // foglaltnak jelezzük az időpontot
                                        if($endTime >= $reservationEndDate){
                                            array_splice($reservedForTheDay, 0, 1);
                                        }
                                        $freeTimes[$key][] = "reserved";
                                    }
                                } else {
                                    if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                        $freeTimes[$key][] = $currentCheckTime->format("H:i");
                                    }
                                }
                                date_add($currentCheckTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                            }
                        } else { // amikor nincs még foglalás azon napok feltöltése
                            while($currentCheckTime <= $workEndTime){
                                if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                    $freeTimes[$key][] = $currentCheckTime->format("H:i");
                                }
                                date_add($currentCheckTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                            }
                        }
                    }
                }
                
                if(count($freeTimes) !== 0){
                    $respData = [
                        "free_times" => $freeTimes
                        // "reserved_times" => $reservedTimes
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs szabad időpont a megadott intervallumban!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        }  else if(preg_match("/^GetAppointmentTimeDetails(\?|$)/", $function) ? true : false){ // Szabad időpontok lekérdezése
            if(count($_GET) == 4 && isset($_GET['service_id']) && isset($_GET['hospital_id']) && isset($_GET['doctor_id']) && isset($_GET['user_id'])){
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // azonosítók lekérdezése
                $service_id = htmlspecialchars($_GET['service_id']);
                $hospital_id = htmlspecialchars($_GET['hospital_id']);
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $user_id = htmlspecialchars($_GET['user_id']);

                // felhasználó adatainak lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` AS 'name', `users`.`email` AS 'email', `user_data`.`phone` FROM `users` INNER JOIN `user_data` ON `user_data`.`user_id` = `users`.`id` WHERE `users`.`id` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$user_id, false]);

                $userData = $stmt->fetch(PDO::FETCH_ASSOC);

                if($userData !== false){
                    // szolgáltatás lekérdezése a kórházra nézve
                    $stmt = $conn->prepare("SELECT `hospital_service`.`price` AS 'price', `hospitals`.`address` AS 'address', `services`.`name` FROM `hospitals` INNER JOIN (`hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id`) ON `hospital_service`.`hospital_id` = `hospitals`.`id` WHERE `hospitals`.`id` = ? AND `services`.`id` = ? AND `hospitals`.`deleted` = ? AND `services`.`deleted` = ? AND `hospital_service`.`active` = ?;");
                    $stmt->execute([$hospital_id, $service_id, false, false, true]);

                    $serviceData = $stmt->fetch(PDO::FETCH_ASSOC);
                    
                    if($serviceData !== false){
                        $serviceData['price'] = number_format($serviceData['price'], 0, '', '.');
                        // orvos adatainak lekérdezése
                        $stmt = $conn->prepare("SELECT `users`.`name` AS 'name' FROM `users` INNER JOIN `doctors` ON `doctors`.`user_id` = `users`.`id` INNER JOIN (`doctor_hospital` INNER JOIN `hospitals` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id`) ON `doctor_hospital`.`user_id` = `users`.`id` INNER JOIN `doctor_service` ON `doctor_service`.`user_id` = `users`.`id` WHERE `doctors`.`user_id` = ? AND `doctor_service`.`service_id` = ? AND `doctor_hospital`.`hospital_id` = ? AND `users`.`deleted` = ?;");
                        $stmt->execute([$doctor_id, $service_id, $hospital_id, false]);

                        $doctorData = $stmt->fetch(PDO::FETCH_ASSOC);

                        if($doctorData !== false){
                            $respData = [
                                "user" => $userData,
                                "service" => $serviceData,
                                "doctor" => $doctorData
                            ];

                            Response::success($respData);
                        } else {
                            Response::error(404, "A kórháznál ez az orvos nem látja el ezt a szolgáltatást!");
                        }
                    } else {
                        Response::error(404, "Nincs ilyen szolgáltatása a kórháznak!");
                    }
                } else {
                    Response::error(404, "Nincs jogosultsága a foglaláshoz!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "POST"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "NewReservation") {

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $date = isset($input['date']) ? htmlspecialchars($input['date']) : null;
            $time = isset($input['time']) ? htmlspecialchars($input['time']) : null;
            $type = isset($input['type']) ? htmlspecialchars($input['type']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($service_id === null || $user_id === null || $hospital_id === null || $doctor_id === null || $date === null || $type === null || $time === null) {
                Response::error(400, "Hiányzó azonosító(k)!", ["service_id" => $service_id, "user_id" => $user_id, "hospital_id" => $hospital_id, "doctor_id" => $doctor_id, "date" => $date, "time" => $time, "type" => $type]);
            }
            

            // szolgáltaás árának és idejének lekérdezése
            $stmt = $conn->prepare("SELECT `hospital_service`.`price` AS 'price', `hospital_service`.`duration` AS 'duration' FROM `hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` WHERE `hospital_service`.`hospital_id` = ? AND `hospital_service`.`service_id` = ? AND `hospital_service`.`active` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$hospital_id, $service_id, true, false]);
            $serviceData = $stmt->fetch(PDO::FETCH_ASSOC);

            $datetime = $date . ' ' . $time;

            $service_price = $serviceData['price'];
            $serviceDuration = $serviceData['duration'];

            $date = date_create($datetime)->format("Y-m-d");
            $time = date_create($datetime)->format("H:i");

            $sqlTo = date_add(date_create($date), date_interval_create_from_date_string("1 days"))->format("Y-m-d");

            // időpont meglétének ellenőrzése
            // foglalt időpontok lekérdezése lekérdezése
            $stmt = $conn->prepare("SELECT `reservations`.`datetime` AS 'datetime', `hospital_service`.`duration` AS 'duration' FROM `reservations` INNER JOIN `hospital_service` ON (`hospital_service`.`hospital_id` = `reservations`.`hospital_id` AND `hospital_service`.`service_id` = `reservations`.`service_id`) WHERE `reservations`.`doctor_id` = ? AND `reservations`.`hospital_id` = ? AND (`reservations`.`datetime` BETWEEN ? AND ?) AND `reservations`.`deleted` = ? ORDER BY `reservations`.`datetime` ASC;");
            $stmt->execute([$doctor_id, $hospital_id, $date, $sqlTo, false]);

            // foglalt időpontok
            $reservedTimes = array();
            while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                $reservationDateTime = new DateTime($row['datetime']); // teljes dátum és idő
                $reservationDate = $reservationDateTime->format("Y-m-d"); // dátum
                $reservationTime = $reservationDateTime->format("H:i"); // idő
                $reservedTimes[$reservationDate][] = array($reservationTime, $row['duration']); // asszociatív tömbbe helyezés
            }

            // orvos munkaidejének lekérdezése
            $stmt = $conn->prepare("SELECT `worktimes`.`date` AS 'date', `worktimes`.`from_time` AS 'from', `worktimes`.`to_time` AS 'to' FROM `worktimes` INNER JOIN `users` ON `worktimes`.`doctor_id` = `users`.`id` WHERE `worktimes`.`doctor_id` = ? AND `worktimes`.`hospital_id` = ? AND `worktimes`.`service_id` = ? AND (`worktimes`.`date` BETWEEN ? AND ?) AND `users`.`deleted` = ? AND `worktimes`.`deleted` = ?");
            $stmt->execute([$doctor_id, $hospital_id, $service_id, $date, $date, false, false]);

            // munkaórák
            $worktimes = array();
            while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                $worktimes[$row['date']][] = array($row['from'], $row['to']); // asszociatív tömbbe helyezés
            }

            //kezelés időtartamának lekérdezése
            $stmt = $conn->prepare("SELECT `hospital_service`.`duration` AS 'duration', `services`.`name` AS 'service_name' FROM `hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` WHERE `hospital_service`.`service_id` = ? AND `hospital_service`.`hospital_id` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$service_id, $hospital_id, false]);

            $service_data = $stmt->fetch(PDO::FETCH_ASSOC);

            $serviceDuration = $service_data['duration'];
            $serviceName = $service_data['service_name'];

            // szabad időpontok keresése
            $freeTimes = array();

            foreach($worktimes as $key => $worktimeDay){
                foreach($worktimeDay as $worktime){
                    $workStartTime = date_create($key . " " . $worktime[0]);
                    $workEndTime = date_sub(date_create($key . " " . $worktime[1]), date_interval_create_from_date_string($serviceDuration . " minutes")); // csökkentjük a vizsgált kezelés idejével
                    $currentCheckTime = $workStartTime; // éppen vizsgált idő beállítása
                    
                    // adott napra történt e foglalás
                    if(in_array($key, array_keys($reservedTimes))){
                        $reservedForTheDay = $reservedTimes[$key];
                        while($currentCheckTime <= $workEndTime){
                            if(count($reservedForTheDay) > 0){
                                $endTime = date_create($currentCheckTime->format('Y-m-d H:i:s'));
                                date_add($endTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                                $reservationStartDate = date_create($key . " " . $reservedForTheDay[0][0]);
                                $reservationEndDate = clone $reservationStartDate;
                                date_add($reservationEndDate, date_interval_create_from_date_string($reservedForTheDay[0][1] . " minutes"));

                                if($currentCheckTime != $reservationStartDate && $endTime <= $reservationStartDate && $endTime <= $workEndTime){ // hogyha a viszáglt idő nem egyenlő a foglalás kezdő idejével és a viszgált idő vége kisebb vagy egyenlő a kezelés kezdetével, akkor hozzáadjuk szabad időpontnak
                                    if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                        $freeTimes[$key][] = $currentCheckTime->format("H:i");
                                    }
                                } else { // foglaltnak jelezzük az időpontot
                                    if($endTime >= $reservationEndDate){
                                        array_splice($reservedForTheDay, 0, 1);
                                    }
                                    $freeTimes[$key][] = "reserved";
                                }
                            } else {
                                if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                    $freeTimes[$key][] = $currentCheckTime->format("H:i");
                                }
                            }
                        date_add($currentCheckTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                        }
                    } else { // amikor nincs még foglalás azon napok feltöltése
                        while($currentCheckTime <= $workEndTime){
                            if($currentCheckTime > date_create("now")){ // nem lehet a jelenlegi időpontnál régebbre foglalni
                                $freeTimes[$key][] = $currentCheckTime->format("H:i");
                            }
                            date_add($currentCheckTime, date_interval_create_from_date_string($serviceDuration . " minutes"));
                        }
                    }
                }
            }

            if(count($freeTimes) !== 0){
                if(in_array($time, $freeTimes[$date])){
                    // időpont lefoglalása
                    $stmt= $conn->prepare("INSERT INTO `reservations` (`datetime`, `user_id`, `hospital_id`, `doctor_id`, `service_id`, `type`, `price`) VALUES (?, ?, ?, ?, ?, ?, ?)");
                    $stmt->execute([$datetime, $user_id, $hospital_id, $doctor_id, $service_id, $type, $service_price]);

                    $reservation_id = $conn->lastInsertId();

                    // kórház címének lekérdezése
                    $stmt = $conn->prepare("SELECT `hospitals`.`address` FROM `hospitals` WHERE `hospitals`.`id` = ?");
                    $stmt->execute([$hospital_id]);

                    $hospitalAddress = $stmt->fetch(PDO::FETCH_ASSOC)['address'];

                    // felhasználó email címének lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`email` FROM `users` WHERE `users`.`id` = ?");
                    $stmt->execute([$user_id]);

                    $userEmail = $stmt->fetch(PDO::FETCH_ASSOC)["email"];

                    // email értesítés küldése
                    newReservationEmail($userEmail, $reservation_id, $datetime, $hospitalAddress, $serviceDuration, $serviceName);
                    
                    // válasz küldése
                    $logger->LogUserAction($requestingUser, "new reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "reservation_id" => $reservation_id]);
                    Response::success([], "Sikeres foglalás!");
                } else {
                    $logger->LogUserAction($requestingUser, "new reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "error" => "Already taken"]);
                    Response::error(404, "Az időpont már nem foglalható!");
                }
            } else {
                $logger->LogUserAction($requestingUser, "new reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "error" => "Past reservation time"]);
                Response::error(404, "Az időpont már nem foglalható!");
            }
        } else if($function == "FinishReservation"){ // időpont késznek nyilvánítása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $reservation_id = isset($input['reservation_id']) ? htmlspecialchars($input['reservation_id']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($reservation_id === null) {
                Response::error(400, "Hiányzó azonosító!");
            }

            // feltételek ellenőrzése (nem lehet a múltban a kezdete)
            $stmt = $conn->prepare("SELECT `reservations`.`datetime` FROM `reservations` WHERE `reservations`.`id` = ?;");
            $stmt->execute([$reservation_id]);

            $reservation = $stmt->fetch(PDO::FETCH_ASSOC);

            if($reservation != false && strtotime($reservation['datetime']) < strtotime("now")){
                // időpont frissítése        
                $stmt = $conn->prepare("UPDATE `reservations` SET `reservations`.`fulfilled` = true WHERE `reservations`.`id` = ?;");
                $stmt->execute([$reservation_id]);
                $user = $stmt->fetch();
                
                $logger->LogUserAction($requestingUser, "Finish reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $reservation_id, ["success" => true, "message" => "fulfilled reservation"]);
                Response::success();
            } else {
                Response::error(400, "A rendelés még nem kezdődött el");
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "CancelReservation"){ // foglalás törlése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosító kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $reservation_id = isset($input['reservation_id']) ? htmlspecialchars($input['reservation_id']) : null;
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;

            if($reservation_id === null || $user_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //foglalás adatok lekérdezése az emailhez
            $stmt = $conn->prepare("SELECT `reservations`.`datetime` AS 'datetime', `services`.`name` AS 'service_name', `hospitals`.`address` AS 'hospital_address' FROM `reservations` INNER JOIN `hospitals` ON `hospitals`.`id` = `reservations`.`hospital_id` INNER JOIN `services` ON `services`.`id` = `reservations`.`service_id` WHERE `reservations`.`id` = ? AND `reservations`.`user_id` = ?");
            $stmt->execute([$reservation_id, $user_id]);

            $respData = $stmt->fetch(PDO::FETCH_ASSOC);
            if($respData){
                $date = $respData['datetime'];
                $serviceName = $respData['service_name'];
                $hospitalAddress = $respData['hospital_address'];

                if(strtotime($date) > strtotime("now")){
                    // felhasználó email címének lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`email` AS 'email' FROM `users` WHERE `users`.`id` = ?");
                    $stmt->execute([$user_id]);

                    $email = $stmt->fetch(PDO::FETCH_ASSOC)['email'];
                    
                    //foglalás törlése azonosító alapján
                    $stmt = $conn->prepare("UPDATE `reservations` SET `reservations`.`deleted` = 1 WHERE `reservations`.`id` = ? AND `reservations`.`user_id` = ?");
                    $stmt->execute([$reservation_id, $user_id]);

                    // email értesítés
                    CancelReservationEmail($email, $date, $hospitalAddress, $serviceName);
                    
                    $logger->LogUserAction($requestingUser, "Cancel reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $reservation_id, ["success" => true]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Cancel reservation", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "error" => "Past cancel time"]);
                    Response::error(401, "Az időpont már nem mondható le!");
                }
            } else {
                Response::error(404, "Nincs ilyen foglalás!");
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