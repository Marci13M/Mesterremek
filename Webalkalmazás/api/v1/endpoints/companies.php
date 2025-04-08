<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "GetCompanies"){ //cégek lekérdezése
            if(count($_GET) == 0){

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cégek lekérdezése
                $stmt = $conn->prepare("SELECT `company`.`id` AS 'company_id', `company`.`name` AS 'company_name', `company`.`director_name` AS 'company_director_name', `company`.`address` AS 'company_address', `company`.`email` AS 'company_email', `company`.`active` AS 'company_active' FROM `company` WHERE `company`.`deleted` = ?");
                $stmt->execute([false]);

                $companiesSum = 0;
                $companies = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $companiesSum += 1;
                    $companies[] = $row;
                }

                $respData = [
                    "companies" => $companies,
                    "total_companies" => $companiesSum
                ];

                //válasz küldése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetHospitals(\?|$)/", $function) ? true : false){ // céghez tartozó kórházak lekérdezése
            if(count($_GET) == 1 && isset($_GET['company_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cég id lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);

                // kórházak lekérdezése
                $stmt = $conn->prepare("SELECT `hospitals`.`id` AS 'hospital_id', `hospitals`.`name` AS 'hospital_name', `hospitals`.`address` AS 'hospital_address', `hospitals`.`email` AS 'hospital_email', `hospitals`.`active` AS 'hospital_active' FROM `hospitals` WHERE `hospitals`.`company_id` = ? AND `hospitals`.`deleted` = ?");
                $stmt->execute([$company_id, false]);
                
                $hospitals = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if($hospitals !== false){
                    $respData = [
                        "company_hospitals" => $hospitals
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "A cégnek nincs kórháza!");
                }
            }
        } else if(preg_match("/^GetDoctors(\?|$)/", $function) ? true : false){ // céghez tartozó orvosok lekérdezése
            if(count($_GET) == 1 && isset($_GET['company_id'])){

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cég id lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);

                // orvosok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'doctor_id', `users`.`name` AS 'doctor_name', `users`.`email` AS 'doctor_email' FROM `users` INNER JOIN `doctor_company` ON `doctor_company`.`user_id` = `users`.`id` WHERE `doctor_company`.`company_id` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$company_id, false]);
                
                $doctors = $stmt->fetchAll(PDO::FETCH_ASSOC);

                if($doctors !== false){
                    $respData = [
                        "company_doctors" => $doctors
                    ];

                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "A cégnek nincs orvosa!");
                }
            }
        } else if(preg_match("/^GetCompany(\?|$)/", $function) ? true : false){ // kórház adatlapjának lekérdezése
            if(count($_GET) == 1 && isset($_GET['company_id'])){ // cég lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cég id lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);

                //cég alapadatok lekérdezése
                $stmt = $conn->prepare("SELECT `company`.`id` AS 'company_id', `company`.`name` AS 'company_name', `company`.`company_register_number` AS 'company_register_number', `company`.`tax_number` AS 'company_tax_number', `company`.`director_name` AS 'company_director_name', `company`.`email` AS 'company_email', `company`.`address` AS 'company_address', `company`.`active` AS 'company_active' FROM `company` WHERE `company`.`id` = ? AND `company`.`deleted` = ?");
                $stmt->execute([$company_id, false]);


                $company = $stmt->fetch(PDO::FETCH_ASSOC); // alapadatok elmentése

                if($company !== false){ // ha kapunk választ
                    // cég telefonszámainak lekérdezése
                    $stmt = $conn->prepare("SELECT `phonenumber`.`id`, `phonenumber`.`phone_number`, `phonenumber`.`type`, `phonenumber`.`public` FROM `phonenumber` INNER JOIN `company_phonenumber` ON `company_phonenumber`.`phonenumber_id` = `phonenumber`.`id` WHERE `company_phonenumber`.`company_id` = ? AND `phonenumber`.`deleted` = ?");
                    $stmt->execute([$company_id, false]);

                    $phonenumbers = array();

                    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                        $phonenumbers[] = $row;
                    }

                    $company["phonenumbers"] = $phonenumbers;

                    // cég logójának lekérdezése
                    $stmt = $conn->prepare("SELECT `images`.`id` AS 'image_id', `images`.`image` AS 'image', `images`.`title` AS 'image_title', `images`.`alt` AS 'image_alt' FROM `images` WHERE `images`.`owner_type` = ? AND `images`.`type` = ? AND `images`.`owner_id` = ? AND `images`.`deleted` = ?");
                    $stmt->execute(["company", "logo", $company_id, false]);

                    $image = $stmt->fetch(PDO::FETCH_ASSOC);

                    if($image !== false){
                        $company['images']['logo'] = $image;
                    } else {
                        $company['images']['logo'] = ["image_id" => null, "image" => null, "image_title" => null, "image_alt" => null];
                    }

                    $respData = [
                        "company" => $company
                    ];

                    // válasz megjelenítése
                    Response::success($respData);
                } else{
                    Response::error(404, "Nincs ilyen cég!");
                }
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetModerators(\?|$)/", $function) ? true : false){ // cég moderátorainak lekérdezése
            if(count($_GET) == 1 && isset($_GET['company_id'])){ // cég lekérdezése

                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);
    
                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                //cég id lekérdezése
                $company_id = htmlspecialchars($_GET['company_id']);

                //cég moderátorainak lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'user_id', `users`.`name` AS 'user_name', `users`.`email` AS 'user_email', `company`.`name` AS 'institution_name' FROM `users` INNER JOIN (`company` INNER JOIN `user_company` ON `user_company`.`company_id` = `company`.`id`) ON `user_company`.`user_id` = `users`.`id` WHERE `company`.`id` = ? AND `company`.`deleted` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$company_id, false, false]);

                $company_moderators = array();

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $row['role_name'] = "Cég";
                    $company_moderators[] = $row;
                }

                // cég kórházaihoz tartozó moderátorok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'user_id', `users`.`name` AS 'user_name', `users`.`email` AS 'user_email', `roles`.`name` AS 'role_name', `hospitals`.`name` AS 'institution_name', `hospitals`.`id` AS 'institution_id' FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` INNER JOIN (`user_hospital` INNER JOIN `hospitals` ON `user_hospital`.`hospital_id` = `hospitals`.`id`) ON `user_hospital`.`user_id` = `users`.`id` WHERE `hospitals`.`id` IN (SELECT `hospitals`.`id` FROM `hospitals` WHERE `hospitals`.`company_id` = ?) AND `hospitals`.`deleted` = ? AND `users`.`deleted` = ?;");
                $stmt->execute([$company_id, false, false]);

                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $row['role_name'] = "Kórház";
                    $company_moderators[] = $row;;
                }

                $respData = [
                    "moderators" => $company_moderators
                ];

                Response::success($respData);
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
        if($function == "Register"){ // Cég regisztrációja

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
            $registry_number = isset($input['registry_number']) ? htmlspecialchars($input['registry_number']) : null;
            $tax_number = isset($input['tax_number']) ? htmlspecialchars($input['tax_number']) : null;
            $director_name = isset($input['director_name']) ? htmlspecialchars($input['director_name']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($email === null || $name === null || $address === null || $registry_number === null || $tax_number === null || $director_name === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            
            // adatok ellenőrzése (tax_number, register_number, email)
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `email` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(400, "Ezzel az email címmel már regisztráltak céget");
            }

            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `tax_number` = ?");
            $stmt->execute([$tax_number]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(400, "Ezzel az adószámmal már regisztráltak céget");
            }

            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `company_register_number` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(400, "Ezzel az cégjegyzékszámmal már regisztráltak céget");
            }


            // Cég létrehozása        
            $stmt = $conn->prepare("INSERT INTO `company` (name, company_register_number, tax_number, director_name, email, address) VALUES (?, ?, ?, ?, ?, ?);");
            $stmt->execute([$name, $registry_number, $tax_number, $director_name, $email, $address]);
            $user = $stmt->fetch();

            $company_id = $conn->lastInsertId();

            // Email küldése a kórház regisztrációjáról
            RegisterCompanyEmail($email);

            $logger->LogUserAction($requestingUser, "register company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $company_id, ["success" => true]);
            Response::success([], "Cég sikersen létrehozva!");
        } else if($function == "UpdateProfile"){ // cég módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $address = isset($input['address']) ? htmlspecialchars($input['address']) : null;
            $company_register_number = isset($input['company_register_number']) ? htmlspecialchars($input['company_register_number']) : null;
            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
            $director_name = isset($input['director_name']) ? htmlspecialchars($input['director_name']) : null;
            $tax_number = isset($input['tax_number']) ? htmlspecialchars($input['tax_number']) : null;


            // adatok ellenőrzése
            if($company_id === null || $address === null || $company_register_number === null || $email === null || $director_name === null || $tax_number === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //cég létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `company`.`company_register_number`, `company`.`tax_number`, `company`.`director_name`, `company`.`email`, `company`.`address` FROM `company` WHERE `company`.`id` = ? AND `company`.`deleted` = ?");
            $stmt->execute([$company_id, false]);

            $company = $stmt->fetch(PDO::FETCH_ASSOC);

            if($company){
                // mődosítások ellenőrzése
                $modifiedData = array();

                if($company['address'] != $address){
                    $modifiedData[] = array("old_address" => $company['address'], "new_address" => $address);
                }
                if($company['company_register_number'] != $company_register_number){
                    $modifiedData[] = array("old_company_register_number" => $company['company_register_number'], "new_company_register_number" => $company_register_number);
                }
                if($company['email'] != $email){
                    $modifiedData[] = array("old_email" => $company['email'], "new_email" => $email);
                }
                if($company['director_name'] != $director_name){
                    $modifiedData[] = array("old_director_name" => $company['director_name'], "new_director_name" => $director_name);
                }
                if($company['tax_number'] != $tax_number){
                    $modifiedData[] = array("old_tax_number" => $company['tax_number'], "new_tax_number" => $tax_number);
                }

                if(count($modifiedData) > 0) {
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `company` SET `company_register_number` = ?, `tax_number` = ?, `director_name` = ?, `email` = ?, `address` = ? WHERE `company`.`id` = ?");
                    $stmt->execute([$company_register_number, $tax_number, $director_name, $email, $address, $company_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Update profile", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $company_id, ["success" => true, "profile_type" => "company", "modifiedData" => $modifiedData]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
            } else{
                Response::error(400, "Nincs ilyen cég!");
            }
        } else if($function == "ChangePhonenumberActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? htmlspecialchars($input['phonenumber_id']) : null;


            // adatok ellenőrzése
            if($company_id === null || $phonenumber_id === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //telefonszám - cég összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `company` INNER JOIN `company_phonenumber` ON `company_phonenumber`.`company_id` = `company`.`id` INNER JOIN `phonenumber` ON `phonenumber`.`id` = `company_phonenumber`.`phonenumber_id` WHERE `company`.`id` = ? AND `company_phonenumber`.`phonenumber_id` = ? AND `company`.`deleted` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$company_id, $phonenumber_id, false, false]);

            $company_phonenumber = $stmt->fetch(PDO::FETCH_ASSOC);

            if($company_phonenumber){
                    //adatok módosítása
                    $stmt = $conn->prepare("UPDATE `phonenumber` SET `public` = NOT `public` WHERE `phonenumber`.`id` = ?");
                    $stmt->execute([$phonenumber_id]);
                    
                    // eseménynapló frissítése
                    $logger->LogUserAction($requestingUser, "Change phonenumber active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $phonenumber_id, ["success" => true]);
                    
                    Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen cég!");
            }
        } else if($function == "ChangeActiveState") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;


            // adatok ellenőrzése
            if($company_id === null){
                Response::error(400, "Hiányzó adat!");
            }

            $stmt = $conn->prepare("UPDATE `company` SET `active` = NOT `active` WHERE `company`.`id` = ?");
            $stmt->execute([$company_id]);
            
            // eseménynapló frissítése
            $logger->LogUserAction($requestingUser, "Change company active state", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $company_id, ["success" => true]);
            
            Response::success(200, "Sikeres adatmódosítás!");
        } else if($function == "UpdateLogo") {
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $old_image_id = isset($input['old_image_id']) ? htmlspecialchars($input['old_image_id']) : null;
            $image = isset($input['image']) ? htmlspecialchars($input['image']) : null;
            $image_title = isset($input['image_title']) ? htmlspecialchars($input['image_title']) : null;
            $image_alt = isset($input['image_alt']) ? htmlspecialchars($input['image_alt']) : null;


            // adatok ellenőrzése
            if($company_id === null || $old_image_id === null || $image === null || $image_title === null || $image_alt === null){
                Response::error(400, "Hiányzó adatok!");
            }

            //kórház létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `company` WHERE `company`.`id` = ? AND `company`.`deleted` = ?");
            $stmt->execute([$company_id, false]);

            $company = $stmt->fetch(PDO::FETCH_ASSOC);

            // kép létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `images`.`image` FROM `images` WHERE `images`.`id` = ? AND `images`.`deleted` = ?");
            $stmt->execute([$old_image_id, false]);

            $image_resp = $stmt->fetch(PDO::FETCH_ASSOC);

            if($company){
                if($image_resp && $image_resp['image'] != $image){
                    // régi kép törlése
                    $stmt = $conn->prepare("UPDATE `images` SET `images`.`deleted` = ? WHERE `images`.`id` = ?");
                    $stmt->execute([true, $old_image_id]);
                } else if($image_resp && $image_resp['image'] == $image) {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
                //adatok módosítása
                $stmt = $conn->prepare("INSERT INTO `images` (image, type, owner_type, owner_id, title, alt) VALUES (?, ?, ?, ?, ?, ?)");
                $stmt->execute([$image, "logo", "company", $company_id, $image_title, $image_alt]);
                
                $image_id = $conn->lastInsertId();
                
                // eseménynapló frissítése
                $logger->LogUserAction($requestingUser, "Update logo image", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $company_id, ["success" => true, "profile_type" => "company", "old_image_id" => $old_image_id, "new_image_id" => $image_id]);
                
                Response::success(200, "Sikeres adatmódosítás!");
            } else{
                Response::error(400, "Nincs ilyen cég!");
            }
        } else if($function == "AddDoctor"){ // orvos hozzáadása a céghez

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;

            if($doctor_id === null || $company_id === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            //cég lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch() == false){
                Response::error(404, "Nincs ilyen cég!");
            }

            //orvos lekérdezése
            $stmt = $conn->prepare("SELECT `user_id`, `email` FROM `doctors` INNER JOIN `users` ON `doctors`.`user_id` = `users`.`id` WHERE `user_id` = ?");
            $stmt->execute([$doctor_id]);

            $doctor = $stmt->fetch(PDO::FETCH_ASSOC);
            if($doctor == false){
                Response::error(404, "Nincs ilyen orvos!");
            }

            //kapcsolat meglétének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `doctor_company` WHERE `user_id` = ? AND `company_id` = ?");
            $stmt->execute([$doctor_id, $company_id]);

            if($stmt->fetch() === false){
                //orvos céghez rendelése
                $stmt = $conn->prepare("INSERT INTO `doctor_company` (`user_id`, `company_id`) VALUES (?, ?)");
                $stmt->execute([$doctor_id, $company_id]);
                
                if($stmt->rowCount() == 1){
                    AddDoctorToCompanyEmail($doctor['email']);

                    $logger->LogUserAction($requestingUser, "Add doctor to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "doctor_id" => $doctor_id, "company_id" => $company_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add doctor to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "doctor_id" => $doctor_id, "company_id" => $company_id]);
                    Response::error(400, "Nem sikerült az orvost a céghez rendelni!");
                }
            } else { // korábban összekapcsolt
                $logger->LogUserAction($requestingUser, "Add doctor to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added", "doctor_id" => $doctor_id, "company_id" => $company_id]);
                Response::success(200, "Orvos korábban hozzárendelve a céghez!");
            }
        } else if($function == "AddService"){ // szolgáltatás hozzáadása a céghez

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];
            

            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $price = isset($input['price']) ? htmlspecialchars($input['price']) : null;
            $duration = isset($input['duration']) ? htmlspecialchars($input['duration']) : null;
            $description = isset($input['description']) ? htmlspecialchars($input['description']) : null;

            if($service_id === null || $company_id === null || $price === null || $duration === null || $description === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }

            //cég lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch() == false){
                Response::error(404, "Nincs ilyen cég!");
            }

            //szolgáltatás lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `services` WHERE `id` = ?");
            $stmt->execute([$service_id]);

            if($stmt->fetch() == false){
                Response::error(404, "Nincs ilyen szolgáltatás!");
            }

            //kapcsolat meglétének ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `company_service` WHERE `service_id` = ? AND `company_id` = ?");
            $stmt->execute([$service_id, $company_id]);

            if($stmt->fetch() === false){
                //szolgáltatás céghez rendelése
                $stmt = $conn->prepare("INSERT INTO `company_service` (`service_id`, `company_id`, `price`, `duration`, `description`) VALUES (?, ?, ?, ?, ?)");
                $stmt->execute([$service_id, $company_id, $price, $duration, $description]);
                
                if($stmt->rowCount() == 1){
                    $logger->LogUserAction($requestingUser, "Add service to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "company_id" => $company_id]);
                    Response::success();
                } else {
                    $logger->LogUserAction($requestingUser, "Add service to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "service_id" => $service_id, "company_id" => $company_id]);
                    Response::error(400, "Nem sikerült a szolgáltatást a céghez rendelni!");
                }
            } else { // korábban összekapcsolt
                $stmt = $conn->prepare("UPDATE `company_service` SET `price` = ?, `duration` = ?, `description` = ?, `deleted` = false WHERE `service_id` = ? AND `company_id` = ?");
                $stmt->execute([$price, $duration, $description, $service_id, $company_id]);

                $logger->LogUserAction($requestingUser, "Add service to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "message" => "Already added", "service_id" => $service_id, "company_id" => $company_id]);
                Response::success(200, "A szolgáltatás korábban hozzárendelve a céghez!");
            }
        } else if($function == "RegisterDoctor"){ // orvos hozzáadása a céghez
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;

            if($email === null || $company_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            ValidateEmail($email);

            // cég létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `company`.`id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen cég!");
            }

            $stmt = $conn->prepare("SELECT `id` FROM `users` WHERE `users`.`email` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(500, "Ezzel az email címmel már regisztráltak felhasználót.");
            }
            
            // jogosultság lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?");
            $stmt->execute(["doctor"]);
            $role_id = $stmt->fetch(PDO::FETCH_ASSOC)["id"];


            // felhasználó létrehozása
            $stmt = $conn->prepare("INSERT INTO `users` (`email`, `role_id`) VALUES (?, ?)");
            $doctor = $stmt->execute([$email, $role_id]);

            $user_id = $conn->lastInsertId();

            // doktor adatok létrehozása
            $stmt = $conn->prepare("INSERT INTO `doctors` (user_id) VALUES (?)");
            
            if($stmt->execute([$user_id])) {
                // küldő nevének lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` FROM `users` WHERE `users`.`id` = ?");
                $stmt->execute([$requestingUser]);

                $userName = $stmt->fetch(PDO::FETCH_ASSOC)['name'];
                
                // felhasználónak email küldése
                $inviteLink = Token::GenerateLinkToken($conn, $user_id, "user-invitation", 1440, ["user_email" => $email, "invite_author" => $userName, "user_type" => "doctor"]);
                InviteUserEmail($email, $inviteLink);


                // felhasználó - cég összerendelés
                $stmt = $conn->prepare("INSERT INTO `doctor_company` (`user_id`, `company_id`) VALUES (?, ?)");
                $stmt->execute([$user_id, $company_id]);

                $logger->LogUserAction($requestingUser, "Register doctor to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "doctor_id" => $user_id]);
                Response::success();
            } else {
                $logger->LogUserAction($requestingUser, "Register doctor to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                Response::error(500, "Hiba lépett fel!");
            }
        } else if($function == "AddPhonenumber"){ // telefonszám hozzáadása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            // adatok feldolgozása
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $phonenumber = isset($input['phonenumber']) ? htmlspecialchars($input['phonenumber']) : null;

            if($company_id === null || $phonenumber === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // cég létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `company`.`id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen cég!");
            }
            
            // telefonszám létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `phonenumber` WHERE `phonenumber`.`phone_number` = ? AND `phonenumber`.`type` = ? AND `phonenumber`.`deleted` = ?");
            $stmt->execute([$phonenumber, "company", false]);
            $phonenumberData = $stmt->fetch(PDO::FETCH_ASSOC);
            

            if($phonenumberData == false) {
                // telefonszám létrehozása
                $stmt = $conn->prepare("INSERT INTO `phonenumber` (`phone_number`, `type`) VALUES (?, ?)");
                $stmt->execute([$phonenumber, "company"]);

                // telefonszám hozzáadása
                $phonenumber_id = $conn->lastInsertId();

                $stmt = $conn->prepare("INSERT INTO `company_phonenumber` (`company_id`, `phonenumber_id`) VALUES (?, ?)");
                if($stmt->execute([$company_id, $phonenumber_id])){
                    Response::success();
                } else {
                    Response::error(500, "Hiba lépett fel!");
                }
            } else {
                // hozzárendelés ellenőrzése
                $stmt = $conn->prepare("SELECT * FROM `company_phonenumber` WHERE `company_phonenumber`.`phonenumber_id` = ? AND `company_phonenumber`.`company_id` = ?");
                $check = $stmt->execute([$phonenumberData['id'], $company_id]);
                if($check) {
                    Response::success();
                } else{
                    // telefonszám hozzáadása
                    $stmt = $conn->prepare("INSERT INTO `company_phonenumber` (`company_id`, `phonenumber_id`) VALUES (?, ?)");
                    if($stmt->execute([$company_id, $phonenumberData['id']])){
                        $logger->LogUserAction($requestingUser, "Add phonenumber", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "company_id" => $company_id]);
                        Response::success();
                    } else {
                        $logger->LogUserAction($requestingUser, "Add phonenumber", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
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
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;

            if($email === null || $company_id === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // cég létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `company` WHERE `company`.`id` = ?");
            $stmt->execute([$company_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) === false) {
                Response::error(404, "Nincs ilyen cég!");
            }

            // email létezésének ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `users` WHERE `users`.`email` = ?");
            $stmt->execute([$email]);

            $user = $stmt->fetch(PDO::FETCH_ASSOC);
            if($user){
                Response::error(400, "Ezzel az email címmel már regisztráltak felhasználót!");
            }
            
            // jogosultság lekérdezése
            $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?");
            $stmt->execute(["company"]);
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

                // felhasználó - cég összerendelés
                $stmt = $conn->prepare("INSERT INTO `user_company` (`user_id`, `company_id`) VALUES (?, ?)");
                $stmt->execute([$user_id, $company_id]);

                $logger->LogUserAction($requestingUser, "Add moderator to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "user_id" => $user_id, "company_id" => $company_id]);
                Response::success();
            } else {
                $logger->LogUserAction($requestingUser, "Add moderator to company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
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
        if($function == "Delete"){ // cég eltávolítása a rendszerből
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //cég id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;

            if($company_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }

            //cég törlése id alapján
            $stmt = $conn->prepare("UPDATE `company` SET `company`.`deleted` = true WHERE `company`.`id` = ?");
            $stmt->execute([$company_id]);

            $logger->LogUserAction($requestingUser, "Delete company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $company_id, ["success" => true]);
            Response::success();
        } else if($function == "RemoveDoctor"){ //orvos eltávolítása a cégtől

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $doctor_id = isset($input['doctor_id']) ? htmlspecialchars($input['doctor_id']) : null;

            if($company_id === null || $doctor_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //orvos eltávolítása a cégből
            $stmt = $conn->prepare("DELETE FROM `doctor_company` WHERE `user_id` = ? AND `company_id` = ?");
            $stmt->execute([$doctor_id, $company_id]);

            // orvos eltávolítása az összes cégtől
            $stmt = $conn->prepare("DELETE FROM `doctor_hospital` WHERE `user_id` = ? AND EXISTS (SELECT 1 FROM `hospitals` WHERE `hospitals`.`id` = `doctor_hospital`.`hospital_id` AND `hospitals`.`company_id` = ?);");
            $stmt->execute([$doctor_id, $company_id]);

            $logger->LogUserAction($requestingUser, "Remove doctor from company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "doctor_id" => $doctor_id, "company_id" => $company_id]);
            Response::success();
        } else if($function == "RemoveService"){ // cég szolgáltatás deaktiválása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $service_id = isset($input['service_id']) ? htmlspecialchars($input['service_id']) : null;

            if($company_id === null || $service_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //szolgáltatás törlése
            $stmt = $conn->prepare("UPDATE `company_service` SET `deleted` = true WHERE `service_id` = ? AND `company_id` = ?");
            $stmt->execute([$service_id, $company_id]);

            $logger->LogUserAction($requestingUser, "Remove service from company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "service_id" => $service_id, "company_id" => $company_id]);
            Response::success();
        }  else if($function == "RemoveModerator"){ // moderátor eltávolítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $moderator_id = isset($input['moderator_id']) ? htmlspecialchars($input['moderator_id']) : null;

            if($company_id === null || $moderator_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            //moderátor - kórház eltávolítása
            $stmt = $conn->prepare("DELETE FROM `user_company` WHERE `user_id` = ? AND `company_id` = ?");
            $stmt->execute([$moderator_id, $company_id]);

            // moderátor törlése
            $stmt = $conn->prepare("UPDATE `users` SET `deleted` = true WHERE `users`.`id` = ?");
            $stmt->execute([$moderator_id]);

            $logger->LogUserAction($requestingUser, "Remove moderator from company", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "moderator_id" => $moderator_id, "company_id" => $company_id]);
            Response::success();
        } else if($function == "DeletePhonenumber"){ // Telefonszám törlése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);
    
            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //azonosítók kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $company_id = isset($input['company_id']) ? htmlspecialchars($input['company_id']) : null;
            $phonenumber_id = isset($input['phonenumber_id']) ? htmlspecialchars($input['phonenumber_id']) : null;

            if($company_id === null || $phonenumber_id === null){
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // összerendelés ellenőrzése
            $stmt = $conn->prepare("SELECT * FROM `company_phonenumber` WHERE `company_id` = ? AND `phonenumber_id` = ?");
            $stmt->execute([$company_id, $phonenumber_id]);

            if($stmt->fetch(PDO::FETCH_ASSOC) != false) {
               // telefonszám törlése
                $stmt = $conn->prepare("UPDATE `phonenumber` SET `deleted` = true WHERE `id` = ?");
                $stmt->execute([$phonenumber_id]);
                $logger->LogUserAction($requestingUser, "Delete phonenumber", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "phonenumber_id" => $phonenumber_id, "company_id" => $company_id]);
                Response::success();
            } else {
                Response::error(400, "Nem törölhető a telefonszám!");
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