<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET') {
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "GetDoctors"){ // orvosok lekérdezése
            if(count($_GET) == 0){ // orvosok lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //orvosok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id`, `users`.`name`,`users`.`email`, `doctors`.`slug` AS 'slug', `company`.`name` AS 'company_name', `company`.`id` AS 'company_id', `company`.`active` AS `company_active` FROM `users` INNER JOIN `doctors` ON `users`.`id` = `doctors`.`user_id` LEFT JOIN (`doctor_company` INNER JOIN `company` ON `doctor_company`.`company_id` = `company`.`id`) ON `doctor_company`.`user_id` = `doctors`.`user_id` WHERE `users`.`deleted` = ?;");
                $stmt->execute([false]);

                $doctorsSum = 0;
                $doctors = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $doctorsSum += 1;
                    // orvos szolgáltatásinak lekérdezése
                    $servStmt = $conn->prepare("SELECT DISTINCT `services`.`name` FROM (`services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id`) INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company_service`.`company_id` INNER JOIN `hospital_service` ON (`hospital_service`.`service_id` = `company_service`.`service_id` AND `hospital_service`.`hospital_id` = `hospitals`.`id`) INNER JOIN `doctor_hospital` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id` INNER JOIN `doctor_service` ON (`doctor_service`.`user_id` = `doctor_hospital`.`user_id` AND `doctor_service`.`service_id` = `company_service`.`service_id`) WHERE `company_service`.`company_id` = ? AND `doctor_service`.`user_id` = ? AND `hospital_service`.`active` = ? AND `hospitals`.`active` = ? AND `company_service`.`deleted` = ? AND `services`.`deleted` = ?;");
                    $servStmt->execute([$row["company_id"], $row['id'], true, true, false, false]);

                    $services = array();
                    while($serv = $servStmt->fetch(PDO::FETCH_ASSOC)){
                        $services[] = $serv['name'];
                    }

                    //szakterületek hozzáadása
                    $row["services"] = $services;
                    
                    // profilkép lekérdezése
                    $imgStmt = $conn->prepare("SELECT `images`.`id` AS 'id', `images`.`image` AS 'image', `images`.`visible` AS 'visible', `images`.`position` AS 'position', `images`.`title` AS 'title', `images`.`alt` AS 'alt' FROM `images` WHERE `images`.`type` = ? AND `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`deleted` = ?");
                    $imgStmt->execute(["profile", "doctor", $row['id'], false]);

                    $profile_image = $imgStmt->fetch(PDO::FETCH_ASSOC);
                    $row["profile_image"] = ($profile_image) ? $profile_image : null;
                    
                    $doctors[] = $row;
                }

                $respData = [
                    "doctors" => $doctors,
                    "total_doctors" => $doctorsSum
                ];

                // válasz megjelenítése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetDoctor(\?|$)/", $function) ? true : false){ // orvos adatlapjának lekérdezése
            if(count($_GET) == 1 && isset($_GET['doctor_id'])){ // orvos lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //orvos id lekérdezése
                $doctor_id = htmlspecialchars($_GET['doctor_id']);

                //orvos alapadatok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` AS 'doctor_name', `users`.`email` AS 'email', `roles`.`name` AS 'role_name', `doctors`.`user_id` AS 'doctor_id', `doctors`.`birthdate` AS 'doctor_birthdate', `doctors`.`birthdate_visible` AS 'doctor_birthdate_visible', `doctors`.`introduction` AS 'doctor_introduction' FROM `doctors` INNER JOIN (`users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id`) ON `doctors`.`user_id` = `users`.`id` WHERE  `users`.`id` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$doctor_id, false]);

                $doctor = $stmt->fetch(PDO::FETCH_ASSOC); // alapadatok elmentése

                if($doctor !== false){ // ha az orvos nincs törölve
                    //orvos tanulmányai
                    $stmt = $conn->prepare("SELECT `educations`.`id`, `educations`.`name` FROM `educations` INNER JOIN `doctor_education` ON `educations`.`id` = `doctor_education`.`education_id` WHERE `doctor_education`.`user_id` = ?");
                    $stmt->execute([$doctor_id]);
                    
                    
                    $eductaions = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $eductaions[] = $row;
                    }
                    
                    $doctor['educations'] = $eductaions;
                    
                    //orvos telefonszámai
                    $stmt = $conn->prepare("SELECT `phonenumber`.`id`, `phonenumber`.`phone_number`, `phonenumber`.`type`, `phonenumber`.`public` FROM `phonenumber` INNER JOIN `doctor_phonenumber` ON `doctor_phonenumber`.`phonenumber_id` = `phonenumber`.`id` WHERE `doctor_phonenumber`.`user_id` = ? AND `phonenumber`.`deleted` = ?");
                    $stmt->execute([$doctor_id, false]);
                    
                    $phonenumbers = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $phonenumbers[] = $row;
                    }
                    
                    $doctor['phonenumbers'] = $phonenumbers;
                    

                    // orvos nyelvismerete
                    $stmt = $conn->prepare("SELECT `languages`.`id` AS 'id', `languages`.`language` AS 'name' FROM `languages` INNER JOIN `doctor_language` ON `doctor_language`.`language_id` = `languages`.`id` WHERE `doctor_language`.`user_id` = ? ORDER BY `languages`.`language` ASC;");
                    $stmt->execute([$doctor_id]);
                    
                    $languages = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $languages[] = $row;
                    }
                    
                    $doctor['languages'] = $languages;
                    

                    // orvos szolgáltatásai
                    $stmt = $conn->prepare("SELECT DISTINCT `services`.`id`, `services`.`name`, `services`.`slug` FROM (`services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id`) INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company_service`.`company_id` INNER JOIN `hospital_service` ON (`hospital_service`.`service_id` = `company_service`.`service_id` AND `hospital_service`.`hospital_id` = `hospitals`.`id`) INNER JOIN `doctor_hospital` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id` INNER JOIN `doctor_service` ON (`doctor_service`.`user_id` = `doctor_hospital`.`user_id` AND `doctor_service`.`service_id` = `company_service`.`service_id`) WHERE `doctor_service`.`user_id` = ? AND `hospital_service`.`active` = ? AND `hospitals`.`active` = ? AND `company_service`.`deleted` = ? AND `services`.`deleted` = ?;");
                    $stmt->execute([$doctor_id, true, true, false, false]);
                    
                    $services = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $services[] = $row;
                    }
                    
                    $doctor['services'] = $services;

                    // orvos profilképe
                    $stmt = $conn->prepare("SELECT `images`.`id` AS 'id', `images`.`image` AS 'image', `images`.`visible` AS 'visible', `images`.`position` AS 'position', `images`.`title` AS 'title', `images`.`alt` AS 'alt' FROM `images` WHERE `images`.`type` = ? AND `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`deleted` = ?");
                    $stmt->execute(["profile", "doctor", $doctor_id, false]);

                    $profile_image = $stmt->fetch(PDO::FETCH_ASSOC);

                    $doctor["profile_image"] = ($profile_image) ? $profile_image : null;


                    $respData = [
                        "doctor" => $doctor
                    ];
    
                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "Nincs ilyen orvos!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetServices(\?|$)/", $function) ? true : false){ // orvos összes szolgáltatás lekérdezése
            if(count($_GET) == 1 && isset($_GET['doctor_id'])){ // orvos lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //orvos id lekérdezése
                $doctor_id = htmlspecialchars($_GET['doctor_id']);

                //orvos alapadatok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` FROM `doctors` INNER JOIN (`users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id`) ON `doctors`.`user_id` = `users`.`id` WHERE  `users`.`id` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$doctor_id, false]);

                $doctor = $stmt->fetch(PDO::FETCH_ASSOC); // alapadatok elmentése

                if($doctor !== false){ // ha az orvos nincs törölve

                    // orvos szolgáltatásai
                    $stmt = $conn->prepare("SELECT `services`.`id` AS 'id', `services`.`name` AS 'name', `services`.`slug` AS 'slug' FROM `services` INNER JOIN `doctor_service` ON `doctor_service`.`service_id` = `services`.`id` WHERE `doctor_service`.`user_id` = ? AND `services`.`deleted` = ? ORDER BY `services`.`name` ASC;");
                    $stmt->execute([$doctor_id, false]);
                    
                    $services = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $services[] = $row;
                    }

                    $respData = [
                        "services" => $services
                    ];
    
                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "Nincs ilyen orvos!");
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
        if($function == "Add"){ // orvos hozzáadás kérés feldolgozása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($email === null) {
                Response::error(400, "Hiányzó email");
            }
            

            // Jogosultság azonosító kinyerése
            $stmt = $conn->prepare("SELECT `roles`.`id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?;");
            $stmt->execute(["doctor"]);
            $response = $stmt->fetch(PDO::FETCH_ASSOC);
            $role_id = ($response != false) ? $response['id'] : null;
            if($role_id === null){
                Response::error(406, "Nem létezik orvos jogosultság!");
            }

            // email létezésének lekérdezése
            $stmt = $conn->prepare("SELECT `users`.`id` FROM `users` WHERE `users`.`email` = ?");
            $stmt->execute([$email]);

            $user = $stmt->fetch(PDO::FETCH_ASSOC);

            if(!$user){
                // Felhasználó létrehozása        
                $stmt = $conn->prepare("INSERT INTO `users` (email, role_id) VALUES (?, ?);");
                $stmt->execute([$email, $role_id]);

                $doctor_id = $conn->lastInsertId();

                // Orvos üres adatok létrehozása
                $stmt = $conn->prepare("INSERT INTO `doctors` (user_id) VALUES (?);");
                $stmt->execute([$doctor_id]);


                //email küldése
                // küldő nevének lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` FROM `users` WHERE `users`.`id` = ?");
                $stmt->execute([$requestingUser]);

                $userName = $stmt->fetch(PDO::FETCH_ASSOC)['name'];
                
                // felhasználónak email küldése
                $inviteLink = Token::GenerateLinkToken($conn, $doctor_id, "user-invitation", 1440, ["user_email" => $email, "invite_author" => $userName, "user_type" => "doctor"]);
                InviteUserEmail($email, $inviteLink);

                $logger->LogUserAction($requestingUser, "Add doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $doctor_id, ["success" => true]);
                Response::success([], "Felhasználó sikersen létrehozva!");
            } else {
                Response::error(400, "Ezt az emailt már regisztrálták!");
            }
        } else if($function == "UpdateProfile"){ // orvos profil módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $introduction = isset($input['introduction']) ? htmlspecialchars($input['introduction']) : null;
            $birthdate = isset($input['birthdate']) ? htmlspecialchars($input['birthdate']) : null;
            $birthdate_visible = isset($input['birthdate_visible']) ? htmlspecialchars($input['birthdate_visible']) : null;


            // adatok ellenőrzése
            if($doctor_id === null || $introduction === null || $birthdate === null || $birthdate_visible === null){
                Response::error(400, "Hiányzó adatok!");
            }

            ValidateAge($birthdate);

            //orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `doctors`.`introduction`, `doctors`.`birthdate_visible`, `doctors`.`birthdate`, `users`.`email` FROM `doctors` INNER JOIN `users` ON `doctors`.`user_id` = `users`.`id` WHERE `doctors`.`user_id` = ? AND `users`.`deleted` = ?");
            $stmt->execute([$doctor_id, false]);

            $doctor = $stmt->fetch(PDO::FETCH_ASSOC);

            if($doctor){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($doctor['birthdate'] != $birthdate){
                    $modifiedData[] = array("old_birthdate" => $doctor['birthdate'], "new_birthdate" => $birthdate);
                }
                if($doctor['birthdate_visible'] != $birthdate_visible){
                    $modifiedData[] = array("old_birthdate_visible" => $doctor['birthdate_visible'], "new_birthdate_visible" => $birthdate_visible);
                }
                if($doctor['introduction'] != $introduction){
                    $modifiedData[] = array("old_introduction" => $doctor['introduction'], "new_introduction" => $introduction);
                }

                $birthdate_visible = ($birthdate_visible == true) ? 1 : 0;

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `doctors` SET `birthdate` = ?, `birthdate_visible` = ?, `introduction` = ? WHERE `doctors`.`user_id` = ?");
                    $stmt->execute([$birthdate, $birthdate_visible, $introduction, $doctor_id]);
                    
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update profile", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $doctor_id, ["success" => true, "profile_type" => "doctor", "modifiedData" => $modifiedData]);
                
                    // email értesítés
                    UpdateUserEmail($doctor['email']);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen orvos!");
            }
        } else if($function == "UpdateProfileImage") {

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $old_image_id = isset($input['old_image_id']) ? htmlspecialchars($input['old_image_id']) : null;
            $image = isset($input['image']) ? htmlspecialchars($input['image']) : null;
            $image_title = isset($input['image_title']) ? htmlspecialchars($input['image_title']) : null;
            $image_alt = isset($input['image_alt']) ? htmlspecialchars($input['image_alt']) : null;


            // adatok ellenőrzése
            if($doctor_id === null || $old_image_id === null || $image === null || $image_title === null || $image_alt === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `users`.`email` FROM `doctors` INNER JOIN `users` ON `doctors`.`user_id` = `users`.`id` WHERE `doctors`.`user_id` = ? AND `users`.`deleted` = ?");
            $stmt->execute([$doctor_id, false]);

            $doctor = $stmt->fetch(PDO::FETCH_ASSOC);

            // kép létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `images`.`image` FROM `images` WHERE `images`.`id` = ? AND `images`.`deleted` = ?");
            $stmt->execute([$old_image_id, false]);

            $image_resp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($doctor){
                if($image_resp && $image_resp['image'] != $image){
                    // régi kép törlése
                    $stmt = $conn->prepare("UPDATE `images` SET `images`.`deleted` = ? WHERE `images`.`id` = ?");
                    $stmt->execute([true, $old_image_id]);
                } else if($image_resp && $image_resp['image'] == $image) {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
                //adatok módosítása
                $stmt = $conn->prepare("INSERT INTO `images` (image, type, owner_type, owner_id, title, alt) VALUES (?, ?, ?, ?, ?, ?)");
                $stmt->execute([$image, "profile", "doctor", $doctor_id, $image_title, $image_alt]);
                
                $image_id = $conn->lastInsertId();

                // régi kép törlése
                $stmt = $conn->prepare("UPDATE `images` SET `images`.`deleted` = ? WHERE `images`.`id` = ?");
                $stmt->execute([true, $old_image_id]);
                
                // eseménynapló frissítése
                $logger->LogUserAction($requestingUser, "Update profile image", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $doctor_id, ["success" => true, "profile_type" => "doctor", "old_image_id" => $old_image_id, "new_image_id" => $image_id]);
                
                Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen orvos vagy kép!");
            }
        } else if($function == "ChangePhonenumberActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? json_decode(htmlspecialchars($input['phonenumber_id'])) : null;


            // adatok ellenőrzése
            if($doctor_id === null || $phonenumber_id === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //telefonszám - orvos összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctors` INNER JOIN `users` ON `users`.`id` = `doctors`.`user_id` INNER JOIN `doctor_phonenumber` ON `doctor_phonenumber`.`user_id` = `users`.`id` INNER JOIN `phonenumber` ON `phonenumber`.`id` = `doctor_phonenumber`.`phonenumber_id` WHERE `doctors`.`user_id` = ? AND `doctor_phonenumber`.`phonenumber_id` = ? AND `users`.`deleted` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$doctor_id, $phonenumber_id, false, false]);

            $doctor_phonenumber = $stmt->fetch(PDO::FETCH_ASSOC);

            if($doctor_phonenumber){
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `phonenumber` SET `public` = NOT `public` WHERE `phonenumber`.`id` = ?");
                    $stmt->execute([$phonenumber_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Change phonenumber active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $phonenumber_id, ["success" => true]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen telfonszám!");
            }
        } else if($function == "AddLanguage"){ // nyelv hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $language_id = isset($input['language_id']) ? htmlspecialchars($input['language_id']) : null;

            if($doctor_id === null || $language_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `user_id` FROM `doctors` WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$doctor_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen orvos!");
            }
            
            // nyelv létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `languages` WHERE `languages`.`id` = ?");
            $stmt->execute([$language_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen nyelv!");
            }

            // hozzárendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_language` WHERE `doctor_language`.`language_id` = ? AND `doctor_language`.`user_id` = ?");
            $stmt->execute([$language_id, $doctor_id]);
            if($stmt->fetch() != false) {
                $logger->LogUserAction($requestingUser, "Add language to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success();
            } else{
                 // nyelv hozzáadása
                $stmt = $conn->prepare("INSERT INTO `doctor_language` (`user_id`, `language_id`) VALUES (?, ?)");
                if($stmt->execute([$doctor_id, $language_id])){
                    $logger->LogUserAction($requestingUser, "Add language to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "language_id" => $language_id, "doctor_id" => $doctor_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add language to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(500, "Hiba lépett fel!");
                }
            }
        } else if($function == "AddService"){ // szolgáltatás hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;

            if($doctor_id === null || $service_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `user_id` FROM `doctors` WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$doctor_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen orvos!");
            }
            
            // szolgáltatás létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `services` WHERE `services`.`id` = ?");
            $service = $stmt->execute([$service_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen szolgáltatás!");
            }

            // hozzárendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_service` WHERE `doctor_service`.`service_id` = ? AND `doctor_service`.`user_id` = ?");
            $stmt->execute([$service_id, $doctor_id]);
            if($stmt->fetch() != false) {
                $logger->LogUserAction($requestingUser, "Add service to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success();
            } else{
                 // szolgáltatás hozzáadása
                $stmt = $conn->prepare("INSERT INTO `doctor_service` (`user_id`, `service_id`) VALUES (?, ?)");
                if($stmt->execute([$doctor_id, $service_id])){
                    $logger->LogUserAction($requestingUser, "Add service to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "doctor_id" => $doctor_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add service to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(500, "Hiba lépett fel!");
                }
            }
        } else if($function == "AddEducation"){ // tanulmány hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $education_id = isset($input['education_id']) ? htmlspecialchars($input['education_id']) : null;

            if($doctor_id === null || $education_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `user_id` FROM `doctors` WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$doctor_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen orvos!");
            }
            
            // tanulmány létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `educations` WHERE `educations`.`id` = ?");
            $education = $stmt->execute([$education_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen tanulmány!");
            }

            // hozzárendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_education` WHERE `doctor_education`.`education_id` = ? AND `doctor_education`.`user_id` = ?");
            $stmt->execute([$education_id, $doctor_id]);
            if($stmt->fetch() != false) {
                $logger->LogUserAction($requestingUser, "Add education to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success();
            } else{
                 // tanulmány hozzáadása
                $stmt = $conn->prepare("INSERT INTO `doctor_education` (`user_id`, `education_id`) VALUES (?, ?)");
                if($stmt->execute([$doctor_id, $education_id])){
                    $logger->LogUserAction($requestingUser, "Add education to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "education_id" => $education_id, "doctor_id" => $doctor_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add education to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(500, "Hiba lépett fel!");
                }
            }
        } else if($function == "AddPhonenumber"){ // telefonszám hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $phonenumber = isset($input['phonenumber']) ? htmlspecialchars($input['phonenumber']) : null;

            if($doctor_id === null || $phonenumber === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `user_id` FROM `doctors` WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$doctor_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen orvos!");
            }
            
            // telefonszám létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `phonenumber` WHERE `phonenumber`.`phone_number` = ? AND `phonenumber`.`type` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$phonenumber, "doctor", false]);
            $phonenumberData = $stmt->fetch(PDO::FETCH_ASSOC);
            

            if($phonenumberData == false) {
                // telefonszám létrehozása
                $stmt = $conn->prepare("INSERT INTO `phonenumber` (`phone_number`, `type`) VALUES (?, ?)");
                $stmt->execute([$phonenumber, "doctor"]);

                $phonenumber_id = $conn->lastInsertId();

                // telefonszám hozzáadása
                $stmt = $conn->prepare("INSERT INTO `doctor_phonenumber` (`user_id`, `phonenumber_id`) VALUES (?, ?)");
                if($stmt->execute([$doctor_id, $phonenumber_id])){
                    Response::success();
                } else {
                    Response::error(500, "Hiba lépett fel!");
                }
            } else {
                // hozzárendelés ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `doctor_phonenumber` WHERE `doctor_phonenumber`.`phonenumber_id` = ? AND `doctor_phonenumber`.`user_id` = ?");
                $stmt->execute([$phonenumberData['id'], $doctor_id]);
                if($stmt->fetch() != false) {
                    $logger->LogUserAction($requestingUser, "Add phonenumber to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                    Response::success();
                } else{
                    // telefonszám hozzáadása
                    $stmt = $conn->prepare("INSERT INTO `doctor_phonenumber` (`user_id`, `phonenumber_id`) VALUES (?, ?)");
                    if($stmt->execute([$doctor_id, $phonenumberData['id']])){
                        $logger->LogUserAction($requestingUser, "Add phonenumber to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "doctor_id" => $doctor_id]);
                        Response::success();
                    } else {
                        $logger->LogUserAction($requestingUser, "Add phonenumber to doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                        Response::error(500, "Hiba lépett fel!");
                    }
                }
                
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "Delete"){ // orvos eltávolítása a rendszerből
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //orvos id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;

            if($doctor_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }

            //orvos törlése id alapján
            $stmt = $conn->prepare("UPDATE `users` INNER JOIN `doctors` ON `doctors`.`user_id` = `users`.`id` SET `users`.`deleted` = true WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$doctor_id]);

            if($stmt->rowCount() == 1){
                $logger->LogUserAction($requestingUser, "Delete doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $doctor_id, ["success" => true]);
                Response::success();
            } else {
                $logger->LogUserAction($requestingUser, "Delete doctor", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "error" => "Not found", "doctor_id" => $doctor_id]);
                Response::error(404, "Nincs ilyen azonosítóval orvos a rendszerben!");
            }
        } else if($function == "DeleteLanguage"){ // orvos nyelvismeretének eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //orvos id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $language_id = isset($input['language_id']) ? htmlspecialchars($input['language_id']) : null;

            if($doctor_id === null && $language_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos nyelvismeretének törlése
            $stmt = $conn->prepare("DELETE FROM `doctor_language` WHERE `doctor_language`.`user_id` = ? AND `doctor_language`.`language_id` = ?");
            $stmt->execute([$doctor_id, $language_id]);

            $logger->LogUserAction($requestingUser, "Delete doctor language", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "language_id" => $language_id, "doctor_id" => $doctor_id]);
            Response::success();
        } else if($function == "DeleteEducation"){ // orvos tanulmányának eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //orvos id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $education_id = isset($input['education_id']) ? htmlspecialchars($input['education_id']) : null;

            if($doctor_id === null && $education_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos tanulmány törlése
            $stmt = $conn->prepare("DELETE FROM `doctor_education` WHERE `doctor_education`.`user_id` = ? AND `doctor_education`.`education_id` = ?");
            $stmt->execute([$doctor_id, $education_id]);

            $logger->LogUserAction($requestingUser, "Delete doctor eduction", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "education_id" => $education_id, "doctor_id" => $doctor_id]);
            Response::success();
        } else if($function == "DeleteService"){ // orvos szolgáltatásának eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //orvos id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;

            if($doctor_id === null && $service_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos szolgáltatás törlése
            $stmt = $conn->prepare("DELETE FROM `doctor_service` WHERE `doctor_service`.`user_id` = ? AND `doctor_service`.`service_id` = ?");
            $stmt->execute([$doctor_id, $service_id]);

            $logger->LogUserAction($requestingUser, "Delete doctor service", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "doctor_id" => $doctor_id]);
            Response::success();
        }  else if($function == "DeletePhonenumber"){ // orvos telefonszám eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //orvos id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? htmlspecialchars($input['phonenumber_id']) : null;

            if($doctor_id === null && $phonenumber_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos nyelvismeretének törlése
            $stmt = $conn->prepare("UPDATE `phonenumber` INNER JOIN `doctor_phonenumber` ON `doctor_phonenumber`.`phonenumber_id` = `phonenumber`.`id` SET `phonenumber`.`deleted` = true WHERE `phonenumber`.`id` = ? AND `doctor_phonenumber`.`user_id` = ?;");
            $stmt->execute([$phonenumber_id, $doctor_id]);

            $logger->LogUserAction($requestingUser, "Delete doctor phonenumber", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "doctor_id" => $doctor_id]);
            Response::success();
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