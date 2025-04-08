<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($type == "doctor") {
            if(preg_match("/^languages(\?|$)/", $function) ? true : false){ // még nem hozzáadott nyelvek lekérdezése
                if(count($_GET) == 1 && isset($_GET['doctor_id'])){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
                    
                    //orvos azonosítójának lekérdezése
                    $doctor_id = htmlspecialchars($_GET['doctor_id']);
    
                    //nyelvek lekérdezése
                    $stmt = $conn->prepare("SELECT `languages`.`id` AS 'id', `languages`.`language` AS 'language' FROM `languages` WHERE `languages`.`id` NOT IN (SELECT `doctor_language`.`language_id` FROM `doctor_language` WHERE `doctor_language`.`user_id` = ?);");
                    $stmt->execute([$doctor_id]);
    
                    $languages = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "languages" => ($languages !== false) ? $languages : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^educations(\?|$)/", $function) ? true : false){ // még nem hozzáadott tanulmányok lekérdezése
                if(count($_GET) == 1 && isset($_GET['doctor_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);
                    
                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //orvos azonosító lekérdezése
                    $doctor_id = htmlspecialchars($_GET['doctor_id']);
    
                    // tanulmányok lekérdezése
                    $stmt = $conn->prepare("SELECT `educations`.`id` AS 'id', `educations`.`name` AS 'name' FROM `educations` WHERE `educations`.`id` NOT IN (SELECT `doctor_education`.`education_id` FROM `doctor_education` WHERE `doctor_education`.`user_id` = ?);");
                    $stmt->execute([$doctor_id]);
                    
                    $educations = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "educations" => ($educations !== false) ? $educations : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^services(\?|$)/", $function) ? true : false){ // még nem hozzáadott szolgáltatások lekérdezése
                if(count($_GET) == 1 && isset($_GET['doctor_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //orvos azonosító lekérdezése
                    $doctor_id = htmlspecialchars($_GET['doctor_id']);
    
                    // szolgáltatások lekérdezése
                    $stmt = $conn->prepare("SELECT `services`.`id` AS 'id', `services`.`name` AS 'name' FROM `services` WHERE `services`.`id` NOT IN (SELECT `doctor_service`.`service_id` FROM `doctor_service` WHERE `doctor_service`.`user_id` = ?)");
                    $stmt->execute([$doctor_id]);
                    
                    $services = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "services" => ($services !== false) ? $services : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else if($type == "hospital") {
            if(preg_match("/^services(\?|$)/", $function) ? true : false){ // még nem hozzáadott szolgáltatások lekérdezése
                if(count($_GET) == 2 && isset($_GET['hospital_id']) && isset($_GET['company_id'])){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
                    
                    //orvos azonosítójának lekérdezése
                    $hospital_id = htmlspecialchars($_GET['hospital_id']);
                    $company_id = htmlspecialchars($_GET['company_id']);
    
                    //szolgáltatások lekérdezése
                    $stmt = $conn->prepare("SELECT `services`.`id` AS 'id', `services`.`name` AS 'service' FROM `services` WHERE `services`.`id` NOT IN (SELECT `company_service`.`service_id` FROM `company_service` LEFT JOIN `hospital_service` ON `hospital_service`.`service_id` = `company_service`.`service_id` WHERE `hospital_service`.`hospital_id` = ? AND `company_service`.`company_id` = ?) AND `services`.`id` IN (SELECT `company_service`.`service_id` FROM `company_service` WHERE `company_service`.`company_id` = ?);");
                    $stmt->execute([$hospital_id, $company_id, $company_id]);
    
                    $services = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "services" => ($services !== false) ? $services : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^doctors(\?|$)/", $function) ? true : false){ // még nem hozzáadott orvosok lekérdezése
                if(count($_GET) == 2 && isset($_GET['hospital_id']) && isset($_GET['company_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //orvos azonosító lekérdezése
                    $hospital_id = htmlspecialchars($_GET['hospital_id']);
                    $company_id = htmlspecialchars($_GET['company_id']);
    
                    // orvosok lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name' FROM `users` WHERE `users`.`id` NOT IN (SELECT `doctor_company`.`user_id` FROM `doctor_company` LEFT JOIN `doctor_hospital` ON `doctor_hospital`.`user_id` = `doctor_company`.`user_id` WHERE `doctor_hospital`.`hospital_id` = ? AND `doctor_company`.`company_id` = ?) AND `users`.`id` IN (SELECT `doctor_company`.`user_id` FROM `doctor_company` WHERE `doctor_company`.`company_id` = ?);");
                    $stmt->execute([$hospital_id, $company_id, $company_id]);
                    
                    $doctors = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "doctors" => ($doctors !== false) ? $doctors : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            }  else if(preg_match("/^allDoctors(\?|$)/", $function) ? true : false){ // Kórházhoz tartozó orvosok lekérdezése
                if(count($_GET) == 1 && isset($_GET['hospital_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //orvos azonosító lekérdezése
                    $hospital_id = htmlspecialchars($_GET['hospital_id']);
    
                    // orvosok lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name' FROM `users` INNER JOIN `doctor_hospital` ON `doctor_hospital`.`user_id` = `users`.`id` WHERE `doctor_hospital`.`hospital_id` = ?");
                    $stmt->execute([$hospital_id]);
                    
                    $hospitals = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "hospitals" => ($hospitals !== false) ? $hospitals : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            }
        } else if($type == "company") {
            if($function == "roles"){ // cég által kiosztható jogosultságok lekérdezése
                if(count($_GET) == 0){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //jogosultságok lekérdezése
                    $stmt = $conn->prepare("SELECT `roles`.`id` AS 'id', `roles`.`name` AS 'name' FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND `roles`.`deleted` = ?");
                    $stmt->execute(["admin", "basic", "default", false]);
    
                    $roles = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "roles" => ($roles !== false) ? $roles : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^services(\?|$)/", $function) ? true : false){ // céghez még nem tartozó szolgáltatások lekérdezése
                if(count($_GET) == 1 && isset($_GET['company_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //cég id lekérdezése
                    $company_id = htmlspecialchars($_GET['company_id']);
    
                    // szolgáltatások lekérdezése
                    $stmt = $conn->prepare("SELECT `services`.`id` AS 'id', `services`.`name` AS 'name' FROM `services` WHERE `services`.`id` NOT IN (SELECT `company_service`.`service_id` FROM `company_service` WHERE `company_service`.`company_id` = ? AND `company_service`.`deleted` = ?)");
                    $stmt->execute([$company_id, false]);
                    
                    $services = $stmt->fetchAll(PDO::FETCH_ASSOC);
                    
                    $respData = [
                        "services" => ($services !== false) ? $services : null
                    ];

                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^hospitals(\?|$)/", $function) ? true : false){ // céghez tartozó kórházak lekérdezése
                if(count($_GET) == 1 && isset($_GET['company_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //cég id lekérdezése
                    $company_id = htmlspecialchars($_GET['company_id']);
    
                    // kórházak lekérdezése
                    $stmt = $conn->prepare("SELECT `hospitals`.`id` AS 'id', `hospitals`.`name` AS 'name' FROM `hospitals` WHERE `hospitals`.`company_id` = ? AND `hospitals`.`deleted` = ?");
                    $stmt->execute([$company_id, false]);
                    
                    $hospitals = $stmt->fetchAll(PDO::FETCH_ASSOC);
                    
                    $respData = [
                        "hospitals" => ($hospitals !== false) ? $hospitals : null
                    ];

                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            }  else if(preg_match("/^doctors(\?|$)/", $function) ? true : false){ // céghez nem tartozó orvosok lekérdezése
                if(count($_GET) == 1 && isset($_GET['company_id'])){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //cég id lekérdezése
                    $company_id = htmlspecialchars($_GET['company_id']);
    
                    // orovosok lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name' FROM `users` WHERE `users`.`id` NOT IN (SELECT `doctor_company`.`user_id` FROM `doctor_company` WHERE `doctor_company`.`company_id` = ?) AND `users`.`id` IN (SELECT `doctors`.`user_id` FROM `doctors`) AND `users`.`name` IS NOT NULL AND `users`.`deleted` = ?;");
                    $stmt->execute([$company_id, false]);
                    
                    $doctors = $stmt->fetchAll(PDO::FETCH_ASSOC);
                    
                    $respData = [
                        "doctors" => ($doctors !== false) ? $doctors : null
                    ];

                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else if($type == "admin"){
            if($function == "roles"){ //jogusultságok lekérdezése
                if(count($_GET) == 0){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //jogosultságok lekérdezése
                    $stmt = $conn->prepare("SELECT `roles`.`id` AS 'id', `roles`.`name` AS 'name' FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND `roles`.`deleted` = ?");
                    $stmt->execute(["basic", "default", false]);
    
                    $roles = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "roles" => ($roles !== false) ? $roles : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "companies"){ //cégek lekérdezése
                if(count($_GET) == 0){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //cégek lekérdezése
                    $stmt = $conn->prepare("SELECT `company`.`id` AS 'id', `company`.`name` AS 'name' FROM `company`");
                    $stmt->execute([]);
    
                    $companies = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "companies" => ($companies !== false) ? $companies : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "hospitals"){ //kórházak lekérdezése
                if(count($_GET) == 0){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //kórházak lekérdezése
                    $stmt = $conn->prepare("SELECT `hospitals`.`id` AS 'id', `hospitals`.`name` AS 'name' FROM `hospitals`");
                    $stmt->execute([]);
    
                    $hospitals = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "hospitals" => ($hospitals !== false) ? $hospitals : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "doctors"){ //orvosok lekérdezése
                if(count($_GET) == 0){
    
                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    
    
                    //orvosok lekérdezése
                    $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name' FROM `users` INNER JOIN `doctors` ON `doctors`.`user_id` = `users`.`id`");
                    $stmt->execute([]);
    
                    $doctors = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "doctors" => ($doctors !== false) ? $doctors : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else if($type == "translate"){
            if(preg_match("/^GetServiceIdFromSlug(\?|$)/", $function) ? true : false){
                if(count($_GET) == 1 && isset($_GET['slug'])){

                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    //slug kigyűjtése
                    $slug = htmlspecialchars($_GET['slug']);

    
                    //szolgáltatás azonosító lekérdezése
                    $stmt = $conn->prepare("SELECT `services`.`id` AS 'id' FROM `services` WHERE `services`.`slug` = ?");
                    $stmt->execute([$slug]);
    
                    $service_id = $stmt->fetch(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "service_id" => ($service_id !== false) ? $service_id['id'] : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^GetDoctorIdFromSlug(\?|$)/", $function) ? true : false){
                if(count($_GET) == 1 && isset($_GET['slug'])){

                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    //slug kigyűjtése
                    $slug = htmlspecialchars($_GET['slug']);

    
                    //orvos azonosító lekérdezése
                    $stmt = $conn->prepare("SELECT `doctors`.`user_id` AS 'id' FROM `doctors` WHERE `doctors`.`slug` = ?");
                    $stmt->execute([$slug]);
    
                    $doctor_id = $stmt->fetch(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "doctor_id" => ($doctor_id !== false) ? $doctor_id['id'] : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if(preg_match("/^GetHospitalIdFromSlug(\?|$)/", $function) ? true : false){
                if(count($_GET) == 1 && isset($_GET['slug'])){

                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    //slug kigyűjtése
                    $slug = htmlspecialchars($_GET['slug']);

    
                    //orvos azonosító lekérdezése
                    $stmt = $conn->prepare("SELECT `hospitals`.`id` AS 'id' FROM `hospitals` WHERE `hospitals`.`slug` = ?");
                    $stmt->execute([$slug]);
    
                    $hospital_id = $stmt->fetch(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "hospital_id" => ($hospital_id !== false) ? $hospital_id['id'] : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            }else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else if ($type == "general"){
            if($function == "GetGenders"){
                if(count($_GET) == 0){

                    //autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);
                    
                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    //nemek lekérdezése
                    $stmt = $conn->prepare("SELECT `genders`.`id` AS 'id', `genders`.`gender_name` AS 'name' FROM `genders`");
                    $stmt->execute();
    
                    $genders = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "genders" => ($genders !== false) ? $genders : null
                    ];
    
                    //válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "GetZipCodes"){ // Irányítószámok és települések lekérdezése
                if(count($_GET) == 0){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);
                    
                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    // Zip lekérdezése
                    $stmt = $conn->prepare("SELECT `zip_codes`.`zip` AS 'zip', `zip_codes`.`name` AS 'name' FROM `zip_codes`");
                    $stmt->execute();
                    
                    $zip_codes = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "zip_codes" => ($zip_codes !== false) ? $zip_codes : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "Languages") { // nyelvek lekérdezése
                if(count($_GET) == 0){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];
    

                    // Nyelvek lekérdezése
                    $stmt = $conn->prepare("SELECT `languages`.`id` AS 'id', `languages`.`language` AS 'name' FROM `languages`");
                    $stmt->execute();
                    
                    $languages = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "languages" => ($languages !== false) ? $languages : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            } else if($function == "Educations") { // tanulmányok lekérdezése
                if(count($_GET) == 0){
    
                    // autentikáció tokennel
                    $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                    $accessToken = str_replace('Bearer ', '', $authHeader);

                    $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                    $requestingUser = $checkAccess['user_id'];

    
                    // Nyelvek lekérdezése
                    $stmt = $conn->prepare("SELECT `educations`.`id` AS 'id', `educations`.`name` AS 'name' FROM `educations`");
                    $stmt->execute();
                    
                    $educations = $stmt->fetchAll(PDO::FETCH_ASSOC);
    
                    $respData = [
                        "educations" => ($educations !== false) ? $educations : null
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(400, "Hibás kérés!");
                }
            }else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "POST") {
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){ 
        if($type == "general") {
            if($function == "AddLanguage") {
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // adatok kinyerése a kérésből
                $input = json_decode(file_get_contents('php://input'), true);

                $language = isset($input['language']) ? htmlspecialchars($input['language']) : null;

                
                // hiányzó adat estén visszatérés hibával
                if ($language === null) {
                    Response::error(400, "Hiányzó nyelv");
                }

                // nyelv meglétének ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `languages` WHERE `language` = ?");
                $stmt->execute([$language]);

                if($stmt->fetch(PDO::FETCH_ASSOC) == false) {
                    // nyelv hozzáadása
                    $stmt = $conn->prepare("INSERT INTO `languages` (language) VALUES (?)");
                    $stmt->execute([$language]);

                    $language_id = $conn->lastInsertId();

                    $logger->LogUserAction($requestingUser, "Add language", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $language_id, ["success" => true]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add language", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                    Response::success([], "A nyelvet már hozzáadták!");
                }
            } else if($function == "AddEducation") {
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // adatok kinyerése a kérésből
                $input = json_decode(file_get_contents('php://input'), true);

                $education = isset($input['education']) ? htmlspecialchars($input['education']) : null;

                
                // hiányzó adat estén visszatérés hibával
                if ($education === null) {
                    Response::error(400, "Hiányzó tanulmány");
                }

                // nyelv meglétének ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `educations` WHERE `name` = ?");
                $stmt->execute([$education]);

                if($stmt->fetch(PDO::FETCH_ASSOC) == false) {
                    // nyelv hozzáadása
                    $stmt = $conn->prepare("INSERT INTO `educations` (name) VALUES (?)");
                    $stmt->execute([$education]);

                    $education_id = $conn->lastInsertId();
                    $logger->LogUserAction($requestingUser, "Add eductaion", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $education_id, ["success" => true]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add education", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added"]);
                    Response::success([], "A tanulmányt már hozzáadták!");
                }
            }else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($type == "general") {
            if($function == "DeleteLanguage") {
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // adatok kinyerése a kérésből
                $input = json_decode(file_get_contents('php://input'), true);

                $language_id = isset($input['language_id']) ? htmlspecialchars($input['language_id']) : null;

                
                // hiányzó adat estén visszatérés hibával
                if ($language_id === null) {
                    Response::error(400, "Hiányzó azonosító!");
                }

                // nyelv meglétének ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `languages` WHERE `languages`.`id` = ?");
                $stmt->execute([$language_id]);

                $language = $stmt->fetch(PDO::FETCH_ASSOC);

                if($language) {
                    // nyelv törlése az orvosoktól
                    $stmt = $conn->prepare("DELETE FROM `doctor_language` WHERE `doctor_language`.`language_id` = ?");
                    $stmt->execute([$language_id]);

                    // nyelv törlése
                    $stmt = $conn->prepare("DELETE FROM `languages` WHERE `languages`.`id` = ?");
                    $stmt->execute([$language_id]);

                    $logger->LogUserAction($requestingUser, "Delete language", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "language" => $language['language']]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Delete language", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "message" => "Not exists"]);
                    Response::error(404, "A nyelv nem létezik!");
                }
            } else if($function == "DeleteEducation") {
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // adatok kinyerése a kérésből
                $input = json_decode(file_get_contents('php://input'), true);

                $education_id = isset($input['education_id']) ? htmlspecialchars($input['education_id']) : null;

                
                // hiányzó adat estén visszatérés hibával
                if ($education_id === null) {
                    Response::error(400, "Hiányzó azonosító!");
                }

                // oktatás meglétének ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `educations` WHERE `id` = ?");
                $stmt->execute([$education_id]);

                $education = $stmt->fetch(PDO::FETCH_ASSOC);

                if($education) {
                    // oktatás törlése az orvosoktól
                    $stmt = $conn->prepare("DELETE FROM `doctor_education` WHERE `doctor_education`.`education_id` = ?");
                    $stmt->execute([$education_id]);

                    // oktatás hozzáadása
                    $stmt = $conn->prepare("DELETE FROM `educations` WHERE `educations`.`id` = ?");
                    $stmt->execute([$education_id]);

                    $logger->LogUserAction($requestingUser, "Delete eductaion", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "education" => $education['name']]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Delete education", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "message" => "Not exists"]);
                    Response::error(404, "A tanulmány nem létezik!");
                }
            }else {
                Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
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