<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "GetServices"){ //szolgáltatások lekérdezése
            if(count($_GET) == 0){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //szolgáltatások lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `services`.`description` AS 'service_description', `services`.`slug` AS 'slug' FROM `services` WHERE `services`.`deleted` = ? ORDER BY `name` ASC");
                $stmt->execute([false]);

                $servicesSum = 0;
                $services = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $servicesSum += 1;
                    $services[] = $row;
                }

                $respData = [
                    "services" => $services,
                    "total_services" => $servicesSum
                ];

                //válasz küldése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetService(\?|$)/", $function) ? true : false){ // adott szolgáltatás lekérdezése 
            if(count($_GET) == 1 && isset($_GET['service_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //szolgáltatás id lekérdezése
                $service_id = htmlspecialchars($_GET['service_id']);

                // szolgáltatás lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `services`.`description` AS 'service_description' FROM `services` WHERE `services`.`id` = ? AND `services`.`deleted` = ?");
                $stmt->execute([$service_id, false]);
                
                $service = $stmt->fetch(PDO::FETCH_ASSOC);

                if($service !== false){
                    $respData = [
                        "service" => $service
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs ilyen szolgáltatás!");
                }
            }
        } else if(preg_match("/^GetDoctorServices(\?|$)/", $function) ? true : false){ // orvoshoz tartozó szolgáltatások lekérdezése
            if(count($_GET) == 1 && isset($_GET['doctor_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //orvos id lekérdezése
                $doctor_id = htmlspecialchars($_GET['doctor_id']);

                // szolgáltatások lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `doctor_service`.`color` AS 'service_color' FROM `services` INNER JOIN `doctor_service` ON `doctor_service`.`service_id` = `services`.`id` WHERE `doctor_service`.`user_id` = ? AND `services`.`deleted` = ? ORDER BY `services`.`name` ASC");
                $stmt->execute([$doctor_id, false]);
                
                $services = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if(count($services) > 0){
                    $respData = [
                        "services" => $services
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs az orvoshoz tartozó szolgáltatás!");
                }
            }
        }  else if(preg_match("/^GetDoctorsWithService(\?|$)/", $function) ? true : false){ // szolgáltatást ellátó orvosok
            if(count($_GET) == 1 && isset($_GET['service_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //szolgáltatás id lekérdezése
                $service_id = htmlspecialchars($_GET['service_id']);

                // szolgáltatások lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` AS 'doctor_name', `users`.`id` AS 'doctor_id', `images`.`image` AS 'doctor_profil', `images`.`title` AS 'doctor_profil_title', `images`.`alt` AS 'doctor_profil_alt', `hospitals`.`id` AS 'hospital_id', `hospitals`.`address` AS 'hospital_address', `hospitals`.`name` AS 'hospital_name', `hospital_service`.`price` AS 'service_price' FROM `users` INNER JOIN `doctors` ON `doctors`.`user_id` = `users`.`id` LEFT JOIN  `images` ON `users`.`id` = `images`.`owner_id` INNER JOIN (`hospitals` INNER JOIN `hospital_service` ON `hospital_service`.`hospital_id` = `hospitals`.`id` INNER JOIN `company_service` ON (`company_service`.`service_id` = `hospital_service`.`service_id` AND `company_service`.`company_id` = `hospitals`.`company_id`) INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` INNER JOIN `doctor_hospital` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id`) ON `doctor_hospital`.`user_id` = `doctors`.`user_id` INNER JOIN `doctor_service` ON (`doctor_service`.`user_id` = `doctors`.`user_id` AND `doctor_service`.`service_id` = `services`.`id`) WHERE `doctor_service`.`service_id` = ? AND (`images`.`owner_type` = ? OR `images`.`owner_type` IS NULL) AND `hospital_service`.`active` = ? AND `hospitals`.`active` = ? AND `services`.`deleted` = ? AND (`images`.`deleted` = ?  OR `images`.`deleted` IS NULL) AND `company_service`.`deleted` = ?;");
                $stmt->execute([$service_id, "doctor", true, true, false, false, false]);

                $doctors = array();
                $doctorsSum = 0;


                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $doctorsSum += 1;
                    $doctor = $row;

                    $doctor_id = $row['doctor_id'];
                    // orvos szakterületeinek ellenőrzése
                        $profStmt = $conn->prepare("SELECT `services`.`name` FROM (`services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id`) INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company_service`.`company_id` INNER JOIN `hospital_service` ON (`hospital_service`.`service_id` = `company_service`.`service_id` AND `hospital_service`.`hospital_id` = `hospitals`.`id`) INNER JOIN `doctor_hospital` ON `doctor_hospital`.`hospital_id` = `hospitals`.`id` INNER JOIN `doctor_service` ON (`doctor_service`.`user_id` = `doctor_hospital`.`user_id` AND `doctor_service`.`service_id` = `company_service`.`service_id`) WHERE `hospital_service`.`hospital_id` = ? AND `doctor_service`.`user_id` = ? AND `hospital_service`.`active` = ? AND `hospitals`.`active` = ? AND `company_service`.`deleted` = ? AND `services`.`deleted` = ?;");
                        $profStmt->execute([$row["hospital_id"], $doctor_id, true, true, false, false]);

                        $services = array(); //szakterületek

                        while($profRow = $profStmt->fetch(PDO::FETCH_ASSOC)){
                            $services[] = $profRow['name'];
                        }
                        $knownServices[$doctor_id] = $services;
                        $doctor['services'] = $services;
                    $doctors[] = $doctor;
                }

                if(count($doctors) > 0){
                    $respData = [
                        "doctors" => $doctors,
                        "total_doctors" => $doctorsSum
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs a szolgáltatáshoz tartozó orvos!");
                }
            }
        }else if(preg_match("/^GetHospitalServices(\?|$)/", $function) ? true : false){ // kórházhoz tartozó szolgáltatások lekérdezése
            if(count($_GET) == 1 && isset($_GET['hospital_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //kórház id lekérdezése
                $hospital_id = htmlspecialchars($_GET['hospital_id']);

                // szolgáltatások lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `hospital_service`.`description` AS 'service_description', `hospital_service`.`price` AS 'service_price', `hospital_service`.`duration` AS 'service_duration', `hospital_service`.`active` AS 'service_active', `services`.`slug` AS 'slug' FROM `services` INNER JOIN `hospital_service` ON `hospital_service`.`service_id` = `services`.`id` INNER JOIN `hospitals` ON `hospital_service`.`hospital_id` = `hospitals`.`id` INNER JOIN `company_service` ON (`company_service`.`company_id` = `hospitals`.`company_id` AND `company_service`.`service_id` = `hospital_service`.`service_id`) WHERE `hospital_service`.`hospital_id` = ? AND `services`.`deleted` = ? AND `company_service`.`deleted` = ?;");
                $stmt->execute([$hospital_id, false, false]);
                
                $services = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if(count($services) > 0){
                    $respData = [
                        "services" => $services
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs a kórházhoz tartozó szolgáltatás!");
                }
            }
        } else if(preg_match("/^GetCompanyServices(\?|$)/", $function) ? true : false){ // céghez tartozó szolgáltatások lekérdezése
            if(count($_GET) == 1 && isset($_GET['company_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cég id lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);

                // szolgáltatások lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `company_service`.`description` AS 'service_description', `company_service`.`price` AS 'service_price', `company_service`.`duration` AS 'service_duration' FROM `services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id` WHERE `company_service`.`company_id` = ? AND `company_service`.`deleted` = ? AND `services`.`deleted` = ?");
                $stmt->execute([$company_id, false, false]);
                
                $services = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if(count($services) > 0){
                    $respData = [
                        "services" => $services
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs a céghez tartozó szolgáltatás!");
                }
            }
        } else if(preg_match("/^GetCompanyTemporaryServices(\?|$)/", $function) ? true : false){ // céghez tartozó szolgáltatások lekérdezése
            if(count($_GET) == 2 && isset($_GET['company_id']) && isset($_GET['service_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //azonosítók lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);
                $service_id = htmlspecialchars($_GET['service_id']);

                // mintaszolgáltatás lekérdezése
                $stmt = $conn->prepare("SELECT `services`.`id` AS 'service_id', `services`.`name` AS 'service_name', `company_service`.`price` AS 'service_price', `company_service`.`duration` AS 'service_duration', `company_service`.`description` AS 'service_description' FROM `services` INNER JOIN `company_service` ON `company_service`.`service_id` = `services`.`id` WHERE `company_service`.`company_id` = ? AND `services`.`id` = ? AND `services`.`deleted` = ?");
                $stmt->execute([$company_id, $service_id, false]);
                
                $temporaryService = $stmt->fetch(PDO::FETCH_ASSOC);

                if($temporaryService !== false){
                    $respData = [
                        "temporary_service" => $temporaryService
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs a cégnek ilyen mintaszolgáltatása!");
                }
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "POST"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "Add"){ // szolgáltatás hozzáadása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($name === null || $description === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            // slug létezés ellenőrzése
            $slug = RemoveAccents(strtolower(str_replace(" ", "-", $name)));

            $stmt = $conn->prepare("SELECT `id` FROM `services` WHERE `slug` = ?");
            $stmt->execute([$slug]);
            $slugResp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($slugResp !== false) {
                // slugok lekérdezése
                $stmt = $conn->prepare("SELECT `slug` FROM `services`");
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
            

            // szolgáltatás létrehozása        
            $stmt = $conn->prepare("INSERT INTO `services` (name, description, slug) VALUES (?, ?, ?);");
            $stmt->execute([$name, $description, $slug]);
            
            $service_id = $conn->lastInsertId();

            $logger->LogUserAction($requestingUser, "Add service", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id]);
            Response::success([], "Szolgáltatás sikersen létrehozva!");
        } else if($function == "AddCompanyTemporaryService"){ // mintaszolgáltatás hozzáadása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;
            $duration = isset($input['duration']) ? htmlspecialchars($input['duration']) : null;
            $price = isset($input['price']) ? htmlspecialchars($input['price']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($service_id === null || $company_id === null || $description === null || $duration === null || $price === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            
            // szolgáltatás lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `services` WHERE `id` = ?");
            $stmt->execute([$service_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) == false){
                Response::error(404, "Nincs ilyen szolgáltatás!");
            }

            // cég lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) == false){
                Response::error(404, "Nincs ilyen cég!");
            }

            // mintaszolgáltatás létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `company_service` WHERE `company_id` = ? AND `service_id` = ?");
            $stmt->execute([$company_id, $service_id]);

            if($stmt->fetch() === false){   
                // mintaszolgáltatás létrehozása
                $stmt = $conn->prepare("INSERT INTO `company_service` (company_id, service_id, description, duration, price) VALUES (?, ?, ?, ?, ?);");
                $stmt->execute([$company_id, $service_id, $description, $duration, $price]);

                $logger->LogUserAction($requestingUser, "Add company temporary service", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "company_id" => $company_id]);
                Response::success([], "Mintaszolgáltatás sikeresen létrehozva");
            } else {
                $logger->LogUserAction($requestingUser, "Add company temporary service", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                Response::success([], "Mintaszolgáltatás korábban hozzáadva a céghez!");
            }
        } else if($function == "UpdateService"){ // szolgáltatás módosítása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;
            $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;


            // adatok ellenőrzése
            if($service_id === null || $description === null || $name === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //szolgáltatás - kórház összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `services` WHERE `services`.`id` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$service_id, false]);

            $service = $stmt->fetch(PDO::FETCH_ASSOC);

            if($service){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($service['description'] != $description){
                    $modifiedData[] = array("old_description" => $service['description'], "new_description" => $description);
                }
                if($service['name'] != $name){
                    $modifiedData[] = array("old_name" => $service['name'], "new_name" => $name);
                }

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `services` SET `description` = ?, `name` = ? WHERE `services`.`id` = ?");
                    $stmt->execute([$description, $name, $service_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update service", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $service_id, ["success" => true, "modifiedData" => $modifiedData]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen szolgáltatás!");
            }
            
        } else if($function == "ChangeHospitalServiceActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $service_id === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //szolgáltatás - kórház összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` WHERE `hospital_service`.`hospital_id` = ? AND `hospital_service`.`service_id` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$hospital_id, $service_id, false]);

            $hospital_service = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital_service){
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `hospital_service` SET `active` = NOT `active` WHERE `hospital_service`.`service_id` = ? AND `hospital_service`.`hospital_id` = ?");
                    $stmt->execute([$service_id, $hospital_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Change service active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "hospital_id" => $hospital_id, "service_id" => $service_id]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen szolgáltatás!");
            }
        } else if($function == "UpdateHospitalService") { // kórház szolgáltatás módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $hospital_id = isset($input['hospital_id']) ? htmlspecialchars($input['hospital_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;
            $price = isset($input['price']) ? htmlspecialchars($input['price']) : null;
            $duration = isset($input['duration']) ? htmlspecialchars($input['duration']) : null;


            // adatok ellenőrzése
            if($hospital_id === null || $service_id === null || $description === null || $price === null || $duration === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //szolgáltatás - kórház összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT `hospital_service`.`description`, `price`, `duration` FROM `hospital_service` INNER JOIN `services` ON `hospital_service`.`service_id` = `services`.`id` WHERE `hospital_service`.`hospital_id` = ? AND `hospital_service`.`service_id` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$hospital_id, $service_id, false]);

            $hospital_service = $stmt->fetch(PDO::FETCH_ASSOC);

            if($hospital_service){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($hospital_service['description'] != $description){
                    $modifiedData[] = array("old_description" => $hospital_service['description'], "new_description" => $description);
                }
                if($hospital_service['price'] != $price){
                    $modifiedData[] = array("old_price" => $hospital_service['price'], "new_price" => $price);
                }
                if($hospital_service['duration'] != $duration){
                    $modifiedData[] = array("old_duration" => $hospital_service['duration'], "new_duration" => $duration);
                }

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `hospital_service` SET `description` = ?, `price` = ?, `duration` = ? WHERE `hospital_service`.`hospital_id` = ? AND `hospital_service`.`service_id` = ?");
                    $stmt->execute([$description, $price, $duration, $hospital_id, $service_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update hospital service", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "modifiedData" => $modifiedData]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen szolgáltatás!");
            }
        } else if($function == "UpdateCompanyService") { // kórház szolgáltatás módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;
            $price = isset($input['price']) ? htmlspecialchars($input['price']) : null;
            $duration = isset($input['duration']) ? htmlspecialchars($input['duration']) : null;


            // adatok ellenőrzése
            if($company_id === null || $service_id === null || $description === null || $price === null || $duration === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //szolgáltatás - cég összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT `company_service`.`description`, `price`, `duration` FROM `company_service` INNER JOIN `services` ON `company_service`.`service_id` = `services`.`id` WHERE `company_service`.`company_id` = ? AND `company_service`.`service_id` = ? AND `services`.`deleted` = ?");
            $stmt->execute([$company_id, $service_id, false]);

            $company_service = $stmt->fetch(PDO::FETCH_ASSOC);

            if($company_service){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($company_service['description'] != $description){
                    $modifiedData[] = array("old_description" => $company_service['description'], "new_description" => $description);
                }
                if($company_service['price'] != $price){
                    $modifiedData[] = array("old_price" => $company_service['price'], "new_price" => $price);
                }
                if($company_service['duration'] != $duration){
                    $modifiedData[] = array("old_duration" => $company_service['duration'], "new_duration" => $duration);
                }

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `company_service` SET `description` = ?, `price` = ?, `duration` = ? WHERE `company_service`.`company_id` = ? AND `company_service`.`service_id` = ?");
                    $stmt->execute([$description, $price, $duration, $company_id, $service_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update company service", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "modifiedData" => $modifiedData]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen szolgáltatás!");
            }

        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if ($function == "Delete") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //adatok kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;

            if($service_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }

            $stmt = $conn->prepare("UPDATE `services` SET `deleted` = true WHERE `services`.`id` = ?");
            $stmt->execute([$service_id]);

            $logger->LogUserAction($requestingUser, "Delete hospital", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $service_id, ["success" => true]);
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