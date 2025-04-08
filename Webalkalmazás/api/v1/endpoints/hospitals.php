<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET') {
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "GetHospitals"){ // kórházak lekérdezése
            if(count($_GET) == 0){
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //kórházak lekérdezése
                $stmt = $conn->prepare("SELECT `company`.`name` AS 'company_name', `hospitals`.`id` AS 'hospital_id', `hospitals`.`name` AS 'hospital_name', `hospitals`.`email` AS 'hospital_email', `hospitals`.`address` AS 'hospital_address', `hospitals`.`slug` AS 'slug', `hospitals`.`active` AS 'hospital_active' FROM `hospitals` INNER JOIN `company` ON `hospitals`.`company_id` = `company`.`id` WHERE `hospitals`.`deleted` = ? AND `company`.`active` = ? AND `company`.`deleted` = ?");
                $stmt->execute([false, true, false]);

                $hospitalsSum = 0;
                $hospitals = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    // borítókép lekérdezése
                    $cover_img_stmt = $conn->prepare("SELECT `images`.`id` AS 'id', `images`.`image` AS 'image', `images`.`visible` AS 'visible', `images`.`position` AS 'position', `images`.`title` AS 'title', `images`.`alt` AS 'alt' FROM `images` WHERE `images`.`type` = ? AND `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`deleted` = ?");
                    $cover_img_stmt->execute(["cover", "hospital", $row['hospital_id'], false]);

                    // profilkép lekérdezése
                    $profile_img_stmt = $conn->prepare("SELECT `images`.`id` AS 'id', `images`.`image` AS 'image', `images`.`visible` AS 'visible', `images`.`position` AS 'position', `images`.`title` AS 'title', `images`.`alt` AS 'alt' FROM `images` WHERE `images`.`type` = ? AND `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`deleted` = ?");
                    $profile_img_stmt->execute(["profile", "hospital", $row['hospital_id'], false]);

                    $cover_image = $cover_img_stmt->fetch(PDO::FETCH_ASSOC);
                    $profile_image = $profile_img_stmt->fetch(PDO::FETCH_ASSOC);

                    $images = [
                        "cover_img" => ($cover_image) ? $cover_image : null,
                        "profile_img" => ($profile_image) ? $profile_image : null
                    ];

                    $row['images'] = $images;

                    $hospitalsSum += 1;
                    $hospitals[] = $row;
                }

                $respData = [
                    "hospitals" => $hospitals,
                    "total_hospitals" => $hospitalsSum
                ];

                // válasz megjelenítése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetHospital(\?|$)/", $function) ? true : false){ // kórház adatlapjának lekérdezése
            if(count($_GET) == 1 && isset($_GET['hospital_id'])){ // kórház lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //kórház id lekérdezése
                $hospital_id = htmlspecialchars($_GET['hospital_id']);

                //kórház alapadatok lekérdezése
                $stmt = $conn->prepare("SELECT `hospitals`.`id` AS 'hospital_id', `hospitals`.`name` AS 'hospital_name', `company`.`id` AS 'company_id', `company`.`name` AS 'company_name', `hospitals`.`address` AS 'hospital_address', `hospitals`.`open_hours` AS 'hospital_open_hours', `hospitals`.`description` AS 'hospital_description', `hospitals`.`email` AS 'hospital_email' FROM `hospitals` INNER JOIN `company` ON `hospitals`.`company_id` = `company`.`id` WHERE `hospitals`.`id` = ? AND `hospitals`.`deleted` = ?;");
                $stmt->execute([$hospital_id, false]);


                $hospital = $stmt->fetch(PDO::FETCH_ASSOC); // alapadatok elmentése

                if($hospital !== false){ // ha kapunk választ
                    //kórház telefonszámai
                    $stmt = $conn->prepare("SELECT `phonenumber`.`id`, `phonenumber`.`phone_number`, `phonenumber`.`type`, `phonenumber`.`public` FROM `phonenumber` INNER JOIN `hospital_phonenumber` ON `hospital_phonenumber`.`phonenumber_id` = `phonenumber`.`id` WHERE `hospital_phonenumber`.`hospital_id` = ? AND `phonenumber`.`deleted` = ?");
                    $stmt->execute([$hospital_id, false]);

                    $phonenumbers = array();
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $phonenumbers[] = $row;
                    }

                    $hospital['phonenumbers'] = $phonenumbers;

                    // kórház borítóképe
                    $stmt = $conn->prepare("SELECT `images`.`id` AS 'image_id', `images`.`title` AS 'image_title', `images`.`alt` AS 'image_alt', `images`.`visible` AS 'visible', `images`.`image` AS 'image' FROM `images` WHERE `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`type` = ? AND `images`.`deleted` = ?");
                    $stmt->execute(["hospital", $hospital_id, "cover", false]);

                    $cover_image = $stmt->fetch(PDO::FETCH_ASSOC);

                    $hospital['images']['cover_image'] = ($cover_image != false) ? $cover_image : ["image_id" => null, "image_title" => null, "image_alt" => null, "image_visible" => null, "image" => null];

                    //kórház profilképe
                    $stmt = $conn->prepare("SELECT `images`.`id` AS 'image_id', `images`.`title` AS 'image_title', `images`.`alt` AS 'image_alt', `images`.`visible` AS 'visible', `images`.`image` FROM `images` WHERE `images`.`owner_type` = ? AND `images`.`owner_id` = ? AND `images`.`type` = ? AND `images`.`deleted` = ?");
                    $stmt->execute(["hospital", $hospital_id, "profile", false]);

                    $profile_image = $stmt->fetch(PDO::FETCH_ASSOC);

                    $hospital['images']['profile_image'] = ($profile_image != false) ? $profile_image : ["image_id" => null, "image_title" => null, "image_alt" => null, "image_visible" => null, "image" => null];

                    $respData = [
                        "hospital" => $hospital
                    ];

                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "Nincs ilyen kórház!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetDoctors(\?|$)/", $function) ? true : false){ // kórház orvosainak lekérdezése
            if(count($_GET) == 1 && isset($_GET['hospital_id'])){ // kórház lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //kórház id lekérdezése
                $hospital_id = htmlspecialchars($_GET['hospital_id']);

                //orvosok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'doctor_id', `users`.`name` AS 'name', `users`.`email` AS 'email', `doctors`.`slug` AS 'slug', `images`.`image` AS 'image', `images`.`alt` AS 'image_alt', `images`.`title` AS 'image_title' FROM `users` INNER JOIN `doctors` ON `doctors`.`user_id` = `users`.`id` INNER JOIN `doctor_hospital` ON `doctor_hospital`.`user_id` = `users`.`id` LEFT JOIN `images` ON `images`.`owner_id` = `users`.`id` WHERE (`images`.`owner_type` = ? OR `images`.`owner_type` IS NULL) AND `doctor_hospital`.`hospital_id` = ? AND `users`.`deleted` = ? AND (`images`.`deleted` = ? OR `images`.`deleted` IS NULL);");
                $stmt->execute(["doctor", $hospital_id, false, false]);

                $hospital_doctors = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    // orvos szolgáltatásai
                    $profStmt = $conn->prepare("SELECT `services`.`name` FROM (`services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id`) INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company_service`.`company_id` INNER JOIN `hospital_service` ON (`hospital_service`.`service_id` = `company_service`.`service_id` AND `hospital_service`.`hospital_id` = `hospitals`.`id`) INNER JOIN `doctor_hospital` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id` INNER JOIN `doctor_service` ON (`doctor_service`.`user_id` = `doctor_hospital`.`user_id` AND `doctor_service`.`service_id` = `company_service`.`service_id`) WHERE `hospital_service`.`hospital_id` = ? AND `doctor_service`.`user_id` = ? AND `hospital_service`.`active` = ? AND `hospitals`.`active` = ? AND `company_service`.`deleted` = ? AND `services`.`deleted` = ?;");
                    $profStmt->execute([$hospital_id, $row['doctor_id'], true, true, false, false]);

                    $services = array();

                    while($profRow = $profStmt->fetch(PDO::FETCH_ASSOC)){
                        $services[] = $profRow["name"];
                    }
                    
                    $row['services'] = $services;
                    $hospital_doctors[] = $row;
                }

                if($hospital_doctors !== false) { // ha kapunk választ
                    $respData = [
                        "hospital_doctors" => $hospital_doctors
                    ];

                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "A kórháznak nincs orvosa!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetModerators(\?|$)/", $function) ? true : false) { // kórház moderátorainak lekérdezése
            if(count($_GET) == 1 && isset($_GET['hospital_id'])){ // kórház lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //kórház id lekérdezése
                $hospital_id = htmlspecialchars($_GET['hospital_id']);

                //orvosok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`email` AS 'user_email', `users`.`name` AS 'user_name', `users`.`id` AS 'user_id' FROM `users` INNER JOIN (`hospitals` INNER JOIN `user_hospital` ON `user_hospital`.`hospital_id` = `hospitals`.`id`) ON `user_hospital`.`user_id` = `users`.`id` WHERE `hospitals`.`id` = ? AND `hospitals`.`deleted` = ?  AND `users`.`deleted` = ?");
                $stmt->execute([$hospital_id, false, false]);

                $hospital_moderators = $stmt->fetchAll(PDO::FETCH_ASSOC); // alapadatok elmentése

                if($hospital_moderators !== false) { // ha kapunk választ
                    $respData = [
                        "hospital_moderators" => $hospital_moderators
                    ];

                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "A kórháznak nincs moderátora!");
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
        if($function == "Register"){ // Kórház regisztrációja

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
            $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;
            $address = isset($input['address']) ? htmlspecialchars($input['address']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($email === null || $name === null || $address === null || $company_id === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            

            //slug ellenőrzése
            $slug = RemoveAccents(strtolower(str_replace(" ", "-", $name)));

            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `slug` = ?");
            $stmt->execute([$slug]);
            $slugResp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($slugResp !== false) {
                // slugok lekérdezése
                $stmt = $conn->prepare("SELECT `slug` FROM `hospitals`");
                $stmt->execute();

                $slugs = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $slugs[] = $row['slug'];
                }

                $counter = 1;
                $uniqueSlug = "";
                do {
                    $uniqueSlug = $slug . "-" . $counter;
                    $counter++;
                } while (in_array($uniqueSlug, $slugs));

                $slug = $uniqueSlug;
            }

            // név ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `hospitals`.`name` = ?");
            $stmt->execute([$name]);

            if($stmt->fetch(PDO::FETCH_ASSOC) !== false) {
                Response::error(500, "Ezzel az névvel nem hozható létre kórház!");
            }


            // Kórház létrehozása        
            $stmt = $conn->prepare("INSERT INTO `hospitals` (email, name, address, company_id, slug) VALUES (?, ?, ?, ?, ?);");
            $stmt->execute([$email, $name, $address, $company_id, $slug]);
            $user = $stmt->fetch();

            $hospital_id = $conn->lastInsertId();

            // Email küldése a kórház regisztrációjáról
            RegisterHospitalEmail($email);

            $logger->LogUserAction($requestingUser, "Register hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true]);
            Response::success([], "Kórház sikersen létrehozva!");
        } else if($function == "UpdateProfile"){ // Kórház módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;
            $address = isset($input['address']) ? htmlspecialchars($input['address']) : null;
            $open_hours = isset($input['open_hours']) ? $input['open_hours'] : null;


            // adatok ellenőrzése
            if($hospital_id === null || $description === null || $address === null || $open_hours === null){
                Response::error(400, "Hiányzó adatok!");
            }

            ValidateAddress($address); // cím ellenőrzése

            //kórház létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `hospitals`.`address`, `hospitals`.`open_hours`, `hospitals`.`description` FROM `hospitals` WHERE `hospitals`.`id` = ? AND `hospitals`.`deleted` = ?");
            $stmt->execute([$hospital_id, false]);

            $hospital = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($hospital['description'] != $description){
                    $modifiedData[] = array("old_description" => $hospital['description'], "new_description" => $description);
                }
                if($hospital['address'] != $address){
                    $modifiedData[] = array("old_address" => $hospital['address'], "new_address" => $address);
                }
                if($hospital['open_hours'] != $open_hours){
                    $modifiedData[] = array("old_open_hours" => $hospital['open_hours'], "new_open_hours" => $open_hours);
                }

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `hospitals` SET `description` = ?, `address` = ?, `open_hours` = ? WHERE `hospitals`.`id` = ?");
                    $stmt->execute([$description, $address, $open_hours, $hospital_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update profile", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true, "profile_type" => "hospital", "modifiedData" => $modifiedData]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen kórház!");
            }
        } else if($function == "UpdateProfileImage") {

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $old_image_id = isset($input['old_image_id']) ? htmlspecialchars($input['old_image_id']) : null;
            $image = isset($input['image']) ? htmlspecialchars($input['image']) : null;
            $image_title = isset($input['image_title']) ? htmlspecialchars($input['image_title']) : null;
            $image_alt = isset($input['image_alt']) ? htmlspecialchars($input['image_alt']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $old_image_id === null || $image === null || $image_title === null || $image_alt === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //kórház létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospitals` WHERE `hospitals`.`id` = ? AND `hospitals`.`deleted` = ?");
            $stmt->execute([$hospital_id, false]);

            $hospital = $stmt->fetch(PDO::FETCH_ASSOC);

            // kép létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `images`.`image` FROM `images` WHERE `images`.`id` = ? AND `images`.`deleted` = ?");
            $stmt->execute([$old_image_id, false]);

            $image_resp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital){
                if($image_resp && $image_resp['image'] != $image){
                    // régi kép törlése
                    $stmt = $conn->prepare("UPDATE `images` SET `images`.`deleted` = ? WHERE `images`.`id` = ?");
                    $stmt->execute([true, $old_image_id]);
                } else if($image_resp && $image_resp['image'] == $image) {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }

                //adatok módosítása
                $stmt = $conn->prepare("INSERT INTO `images` (image, type, owner_type, owner_id, title, alt) VALUES (?, ?, ?, ?, ?, ?)");
                $stmt->execute([$image, "profile", "hospital", $hospital_id, $image_title, $image_alt]);
                
                $image_id = $conn->lastInsertId();

                // eseménynapló frissítése
                $logger->LogUserAction($requestingUser, "Update profile image", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true, "profile_type" => "hospital", "old_image_id" => $old_image_id, "new_image_id" => $image_id]);
            

                Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen kórház!");
            }
        } else if($function == "UpdateCoverImage") {

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $old_image_id = isset($input['old_image_id']) ? htmlspecialchars($input['old_image_id']) : null;
            $image = isset($input['image']) ? htmlspecialchars($input['image']) : null;
            $image_title = isset($input['image_title']) ? htmlspecialchars($input['image_title']) : null;
            $image_alt = isset($input['image_alt']) ? htmlspecialchars($input['image_alt']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $old_image_id === null || $image === null || $image_title === null || $image_alt === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //kórház létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospitals` WHERE `hospitals`.`id` = ? AND `hospitals`.`deleted` = ?");
            $stmt->execute([$hospital_id, false]);

            $hospital = $stmt->fetch(PDO::FETCH_ASSOC);

            // kép létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `images`.`image` FROM `images` WHERE `images`.`id` = ? AND `images`.`deleted` = ?");
            $stmt->execute([$old_image_id, false]);

            $image_resp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital){
                if($image_resp && $image_resp['image'] != $image){
                    // régi kép törlése
                    $stmt = $conn->prepare("UPDATE `images` SET `images`.`deleted` = ? WHERE `images`.`id` = ?");
                    $stmt->execute([true, $old_image_id]);
                } else if($image_resp && $image_resp['image'] == $image) {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
                
                //adatok módosítása
                $stmt = $conn->prepare("INSERT INTO `images` (image, type, owner_type, owner_id, title, alt) VALUES (?, ?, ?, ?, ?, ?)");
                $stmt->execute([$image, "cover", "hospital", $hospital_id, $image_title, $image_alt]);
                
                $image_id = $conn->lastInsertId();
                
                // eseménynapló frissítése
                $logger->LogUserAction($requestingUser, "Update cover image", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true, "profile_type" => "hospital", "old_image_id" => $old_image_id, "new_image_id" => $image_id]);
                
                Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen kórház!");
            }
        } else if($function == "ChangePhonenumberActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? htmlspecialchars($input['phonenumber_id']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $phonenumber_id === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //telefonszám - orvos összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospitals` INNER JOIN `hospital_phonenumber` ON `hospital_phonenumber`.`hospital_id` = `hospitals`.`id` INNER JOIN `phonenumber` ON `phonenumber`.`id` = `hospital_phonenumber`.`phonenumber_id` WHERE `hospitals`.`id` = ? AND `hospital_phonenumber`.`phonenumber_id` = ? AND `hospitals`.`deleted` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$hospital_id, $phonenumber_id, false, false]);

            $hospital_phonenumber = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital_phonenumber){
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `phonenumber` SET `public` = NOT `public` WHERE `phonenumber`.`id` = ?");
                    $stmt->execute([$phonenumber_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Change phonenumber active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $phonenumber_id, ["success" => true]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen telefonszám!");
            }
        } else if($function == "ChangeActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $company_id === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //kórház - cég összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospitals` WHERE `hospitals`.`id` = ? AND `hospitals`.`company_id` = ? AND `hospitals`.`deleted` = ?");
            $stmt->execute([$hospital_id, $company_id, false]);

            $company_hospital = $stmt->fetch(PDO::FETCH_ASSOC);

            if($company_hospital){
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `hospitals` SET `active` = NOT `active` WHERE `hospitals`.`id` = ?");
                    $stmt->execute([$hospital_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Change hospital active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen kórház!");
            }
        } else if($function == "AddDoctor"){ // orvos hozzáadása a kórházhoz

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;

            if($doctor_id === null || $hospital_id === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            //kórház lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `id` = ?");
            $stmt->execute([$hospital_id]);

            if($stmt->fetchAll() == false){
                Response::error(404, "Nincs ilyen kórház!");
            }

            //orvos lekérdezése
            $stmt = $conn->prepare("SELECT `user_id`, `email` FROM `doctors` INNER JOIN `users` ON `doctors`.`user_id` = `users`.`id` WHERE `user_id` = ?");
            $stmt->execute([$doctor_id]);
            
            $doctor = $stmt->fetch(PDO::FETCH_ASSOC);
            
            if($doctor === false){
                Response::error(404, "Nincs ilyen orvos!");
            }

            //kapcsolat meglétének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_hospital` WHERE `user_id` = ? AND `hospital_id` = ?");
            $stmt->execute([$doctor_id, $hospital_id]);

            if($stmt->fetch() == false){
                //orvos kórházhoz rendelése
                $stmt = $conn->prepare("INSERT INTO `doctor_hospital` (`user_id`, `hospital_id`) VALUES (?, ?)");
                $stmt->execute([$doctor_id, $hospital_id]);
                
                if($stmt->rowCount() == 1){
                    AddDoctorToHospitalEmail($doctor['email']);

                    $logger->LogUserAction($requestingUser, "Add doctor to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "doctor_id" => $doctor_id, "hospital_id" => $hospital_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add doctor to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(500, "Nem sikerült az orvost a kórházhoz rendelni!");
                }
            } else { // korábban összekapcsolt
                $logger->LogUserAction($requestingUser, "Add doctor to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success(200, "Orvos korábban hozzárendelve a kórházhoz!");
            }
        } else if($function == "AddService"){ // szolgáltatás hozzáadása a kórházhoz

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];
            

            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $duration = isset($input['duration']) ? htmlspecialchars($input['duration']) : null;
            $price = isset($input['price']) ? htmlspecialchars($input['price']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;

            if($service_id === null || $hospital_id === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            //kórház lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `id` = ?");
            $stmt->execute([$hospital_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false){
                Response::error(404, "Nincs ilyen kórház!");
            }

            //szolgáltatás lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `services` WHERE `id` = ?");
            $stmt->execute([$service_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false){
                Response::error(404, "Nincs ilyen szolgáltatás!");
            }

            //kapcsolat meglétének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospital_service` WHERE `service_id` = ? AND `hospital_id` = ?");
            $stmt->execute([$service_id, $hospital_id]);

            if($stmt->fetch() == false){
                //orvos kórházhoz rendelése
                $stmt = $conn->prepare("INSERT INTO `hospital_service` (`service_id`, `hospital_id`, `description`, `price`, `duration`) VALUES (?, ?, ?, ?, ?)");
                $stmt->execute([$service_id, $hospital_id, $description, $price, $duration]);
                
                if($stmt->rowCount() == 1){
                    $logger->LogUserAction($requestingUser, "Add service to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "hospital_id" => $hospital_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add service to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(500, "Nem sikerült a szolgáltatást a kórházhoz rendelni!");
                }
            } else { // korábban összekapcsolt
                $logger->LogUserAction($requestingUser, "Add service to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success(200, "A szolgáltatás korábban hozzárendelve a kórházhoz!");
            }
        } else if($function == "AddPhonenumber"){ // telefonszám hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $phonenumber = isset($input['phonenumber']) ? htmlspecialchars($input['phonenumber']) : null;

            if($hospital_id === null || $phonenumber === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // orvos létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `hospitals`.`id` = ?");
            $stmt->execute([$hospital_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen kórház!");
            }
            
            // telefonszám létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `phonenumber` WHERE `phonenumber`.`phone_number` = ? AND `phonenumber`.`type` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$phonenumber, "hospital", false]);
            $phonenumberData = $stmt->fetch(PDO::FETCH_ASSOC);
            

            if($phonenumberData == false) {
                // telefonszám létrehozása
                $stmt = $conn->prepare("INSERT INTO `phonenumber` (`phone_number`, `type`) VALUES (?, ?)");
                $stmt->execute([$phonenumber, "hospital"]);

                $phonenumber_id = $conn->lastInsertId();

                // telefonszám hozzáadása
                $stmt = $conn->prepare("INSERT INTO `hospital_phonenumber` (`hospital_id`, `phonenumber_id`) VALUES (?, ?)");
                if($stmt->execute([$hospital_id, $phonenumber_id])){
                    Response::success();
                } else {
                    Response::error(500, "Hiba lépett fel!");
                }
            } else {
                // hozzárendelés ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `hospital_phonenumber` WHERE `hospital_phonenumber`.`phonenumber_id` = ? AND `hospital_phonenumber`.`hospital_id` = ?");
                $stmt->execute([$phonenumberData['id'], $hospital_id]);
                if($stmt->fetchAll(PDO::FETCH_ASSOC) != false) {
                    $logger->LogUserAction($requestingUser, "Add phonenumber to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                    Response::success();
                } else{
                    // telefonszám hozzáadása
                    $stmt = $conn->prepare("INSERT INTO `hospital_phonenumber` (`hospital_id`, `phonenumber_id`) VALUES (?, ?)");
                    if($stmt->execute([$hospital_id, $phonenumberData['id']])){
                        $logger->LogUserAction($requestingUser, "Add phonenumber to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "hospital_id" => $hospital_id]);
                        Response::success();
                    } else {
                        $logger->LogUserAction($requestingUser, "Add phonenumber to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                        Response::error(500, "Hiba lépett fel!");
                    }
                }
            }
        } else if($function == "AddModerator"){ // Moderátor hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;

            if($email === null || $hospital_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // kórház létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `hospitals` WHERE `hospitals`.`id` = ?");
            $stmt->execute([$hospital_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen kórház!");
            }

            // email lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `users` WHERE `users`.`email` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(500, "Ezzel az emaillel már regisztráltak felhasználót!");
            }
            
            // jogosultság lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?");
            $stmt->execute(["hospital"]);
            $role_id = $stmt->fetch(PDO::FETCH_ASSOC)["id"];


            // felhasználó létrehozása
            $stmt = $conn->prepare("INSERT INTO `users` (`email`, `role_id`) VALUES (?, ?)");
            
            if($stmt->execute([$email, $role_id])) {
                $user_id = $conn->lastInsertId();

                // küldő nevének lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` FROM `users` WHERE `users`.`id` = ?");
                $stmt->execute([$requestingUser]);

                $userName = $stmt->fetch(PDO::FETCH_ASSOC)['name'];
                
                // felhasználónak email küldése
                $inviteLink = Token::GenerateLinkToken($conn, $user_id, "user-invitation", 1440, ["user_email" => $email, "invite_author" => $userName, "user_type" => "moderator"]);
                InviteUserEmail($email, $inviteLink);


                // felhasználó - kórház összerendelés
                $stmt = $conn->prepare("INSERT INTO `user_hospital` (`user_id`, `hospital_id`) VALUES (?, ?)");
                $stmt->execute([$user_id, $hospital_id]);

                $logger->LogUserAction($requestingUser, "Add moderator to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "moderator_id" => $user_id, "hospital_id" => $hospital_id]);
                Response::success();
            } else {
                $logger->LogUserAction($requestingUser, "Add moderator to hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                Response::error(500, "Hiba lépett fel!");
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "Delete"){ // kórház eltávolítása a rendszerből
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //kórház id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;

            if($hospital_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }

            //kórház törlése id alapján
            $stmt = $conn->prepare("UPDATE `hospitals` SET `hospitals`.`deleted` = true WHERE `hospitals`.`id` = ?");
            $stmt->execute([$hospital_id]);

            $logger->LogUserAction($requestingUser, "Delete hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $hospital_id, ["success" => true]);
            Response::success();
        } else if($function == "RemoveDoctor"){ //orvos eltávolítása a kórházból

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;

            if($hospital_id === null || $doctor_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos eltávolítása a kórházból
            $stmt = $conn->prepare("DELETE FROM `doctor_hospital` WHERE `user_id` = ? AND `hospital_id` = ?");
            $stmt->execute([$doctor_id, $hospital_id]);

            $logger->LogUserAction($requestingUser, "Remove doctor from hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "doctor_id" => $doctor_id, "hospital_id" => $hospital_id]);
            Response::success();
        }  else if($function == "RemoveModerator"){ // moderátor eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $moderator_id = isset($input['moderator_id']) ? htmlspecialchars($input['moderator_id']) : null;

            if($hospital_id === null || $moderator_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //moderátor - kórház eltávolítása
            $stmt = $conn->prepare("DELETE FROM `user_hospital` WHERE `user_id` = ? AND `hospital_id` = ?");
            $stmt->execute([$moderator_id, $hospital_id]);

            // moderátor törlése
            $stmt = $conn->prepare("UPDATE `users` SET `deleted` = true WHERE `users`.`id` = ?");
            $stmt->execute([$moderator_id]);

            $logger->LogUserAction($requestingUser, "Remove moderator from hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "moderator_id" => $moderator_id, "hospital_id" => $hospital_id]);
            Response::success();
        } else if($function == "DeletePhonenumber"){ // Telefonszám törlése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? htmlspecialchars($input['phonenumber_id']) : null;

            if($hospital_id === null || $phonenumber_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospital_phonenumber` WHERE `hospital_id` = ? AND `phonenumber_id` = ?");
            $stmt->execute([$hospital_id, $phonenumber_id]);

            if($stmt->fetchAll(PDO::FETCH_ASSOC) != false) {
               // telefonszám törlése
                $stmt = $conn->prepare("UPDATE `phonenumber` SET `deleted` = true WHERE `id` = ?");
                $stmt->execute([$phonenumber_id]);

                $logger->LogUserAction($requestingUser, "Remove phonenumber from hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "hospital_id" => $hospital_id]);
                Response::success();
            } else {
                Response::error(500, "Nem törölhető a telefonszám!");
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