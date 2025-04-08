<?php
if ($_SERVER['REQUEST_METHOD'] == 'GET'){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "GetAdmins") {
            if(count($_GET) == 0){
            
                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];
                
                
                //adminok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'user_id', `users`.`name` AS 'user_name', `users`.`email` AS 'user_email' FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ? AND `users`.`deleted` = ?");
                $stmt->execute(["admin", false]);
                
                $adminsSum = 0;
                $admins = array();
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $adminsSum += 1;
                    $admins[] = $row;
                }
                
                $respData = [
                    "admins" => $admins,
                    "total_admins" => $adminsSum
                ];

                //válasz küldése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if($function == "GetAllModerators") {
            if(count($_GET) == 0){
            
                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];
                
                
                //cégmoderátorok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name', `users`.`email` AS 'email', `company`.`id` AS 'institution_id', `company`.`name` AS 'institution', `roles`.`name` AS 'role' FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` INNER JOIN (`company` INNER JOIN `user_company` ON `user_company`.`company_id` = `company`.`id`) ON `user_company`.`user_id` = `users`.`id`");
                $stmt->execute([]);
                
                $moderators = array();
                
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $moderators[] = $row;
                }

                //kórházmoderátorok lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'id', `users`.`name` AS 'name', `users`.`email` AS 'email', `hospitals`.`id` AS 'institution_id', `hospitals`.`name` AS 'institution', `roles`.`name` AS 'role' FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` INNER JOIN (`hospitals` INNER JOIN `user_hospital` ON `user_hospital`.`hospital_id` = `hospitals`.`id`) ON `user_hospital`.`user_id` = `users`.`id`");
                $stmt->execute([]);
                while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    $moderators[] = $row;
                }
                
                $respData = [
                    "moderators" => (count($moderators) > 0) ? $moderators : null
                ];

                //válasz küldése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if($function == "GetBasicUsers"){
            if(count($_GET) == 0) {

                //autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];
                
                
                // felhasználók lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`id` AS 'user_id', `users`.`name` AS 'name', `users`.`email` AS 'email', `user_data`.`gender_id` AS 'gender_id', `genders`.`gender_name` AS 'gender_name', `user_data`.`phone` AS 'phone', `user_data`.`TAJ` AS 'taj', `user_data`.`address` AS 'address', `user_data`.`birthdate` AS 'birthdate', `user_data`.`hasTB` AS 'hasTB' FROM `users` INNER JOIN `user_data` ON `user_data`.`user_id` = `users`.`id` INNER JOIN `genders` ON `user_data`.`gender_id` = `genders`.`id` WHERE `users`.`deleted` = ?");
                $stmt->execute([false]);

                $users = $stmt->fetchAll(PDO::FETCH_ASSOC);

                $respData = [
                    "users" => (count($users) > 0) ? $users : null
                ];

                //válasz küldése
                Response::success($respData);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^GetUser(\?|$)/", $function) ? true : false){ // Felhasználó lekérdezése
            if(count($_GET) == 1 && isset($_GET['user_id'])){
    
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];
    
    
                //felhasználó id lekérdezése
                $user_id = htmlspecialchars($_GET['user_id']);
    
                // felhasználó lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` AS 'user_name', `users`.`email` AS 'user_email', `roles`.`id` AS 'role_id', `roles`.`name` AS 'role_name', `genders`.`id` AS 'gender_id', `genders`.`gender_name` AS 'gender_name', `user_data`.`phone` AS 'user_phone', `user_data`.`TAJ` AS 'user_taj', `user_data`.`address` AS 'user_address', `user_data`.`birthdate` AS 'user_birthdate', `user_data`.`hasTB` AS 'user_hasTB' FROM `roles` INNER JOIN (`users` INNER JOIN (`user_data` INNER JOIN `genders` ON `user_data`.`gender_id` = `genders`.`id`) ON `user_data`.`user_id` = `users`.`id`) ON `users`.`role_id` = `roles`.`id` WHERE `users`.`id` = ? AND `users`.`deleted` = ?");
                $stmt->execute([$user_id, false]);
                
                $user = $stmt->fetch(PDO::FETCH_ASSOC);
    
                if($user !== false){
                    $respData = [
                        "user" => $user
                    ];
    
                    // válasz küldése
                    Response::success($respData);
                } else {
                    Response::error(404, "Nincs ilyen felhasználó!");
                }
            }
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    }
} else if($_SERVER['REQUEST_METHOD'] == "POST"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if ($function == "UpdatePassword") { // Felhasználó jelszavának cserélése
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $old_password = isset($input['old_password']) ? htmlspecialchars($input['old_password']) : null;
            $new_password = isset($input['new_password']) ? htmlspecialchars($input['new_password']) : null;
            $new_password_again = isset($input['new_password_again']) ? htmlspecialchars($input['new_password_again']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($user_id === null || $old_password === null || $new_password === null || $new_password_again === null) {
                Response::error(400, "Hiányzó azonosító(k)!");
            }

            // jelszavak ellenőrzése
            if($new_password !== $new_password_again) {
                Response::error(400, "Az új jelszavak nem egyeznek!", ["errorFields" => array("new-password", "new-password-again")]);
            }

            if($old_password == $new_password) {
                Response::error(400, "A régi és új jelszó nem egyezhetnek!", ["errorFields" => array("old-password", "new-password")]);
            }

            // új jelszó validálása
            ValidatePassword($new_password);

            // régi jelszó ellenőzrése
            $oldPasswordHash = hash("sha256", $old_password);

            $stmt = $conn->prepare("SELECT `users`.`id`, `users`.`email` FROM `users` WHERE `users`.`id` = ? AND `users`.`password` = ?");
            $stmt->execute([$user_id, $oldPasswordHash]);
            $user = $stmt->fetch(PDO::FETCH_ASSOC);

            if($user) { // jelszó cseréje
                $newPasswordHash = hash("sha256", $new_password);
                $stmt = $conn->prepare("UPDATE `users` SET `users`.`password` = ? WHERE `users`.`id` = ?");
                $stmt->execute([$newPasswordHash, $user_id]);

                //esemény naplózása
                $logger->LogUserAction($user_id, "profile_update", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true, "user_id" => $user_id, "fields_updated" => array("password")]);

                // email értesítés
                UpdatePasswordEmail($user['email']);
                
                Response::success([], "Sikeres módosítás!");
            } else {
                Response::error(400, "Hibás régi jelszó!", ["errorFields" => array("old-password")]);
            }
        } else if($function == "InviteAdmin"){ // Felhasználó meghívása

            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            // adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
            $role_id = isset($input['role_id']) ? htmlspecialchars($input['role_id']) : null;

            
            // hiányzó adat estén visszatérés hibával
            if ($email === null || $role_id === null) {
                Response::error(400, "Hiányzó adat(ok)!");
            }
            
            //email ellenőrzése4
            $stmt = $conn->prepare("SELECT `id` FROM `users` WHERE `email` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch(PDO::FETCH_ASSOC)){
                Response::error(500, "Ezzel az email címmel már regisztráltak felhasználót!");
            }

            //jogosultság ellenőrzése
            $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE `id` = ? AND JSON_EXTRACT(`tags`, '$.role_identifier') != ? AND JSON_EXTRACT(`tags`, '$.role_identifier') != ?");
            $stmt->execute([$role_id, "basic", "doctor"]);

            if($stmt->fetch(PDO::FETCH_ASSOC) == false){
                Response::error(404, "Nincs ilyen jogosultság!");
            }

            // Felhasználó létrehozása 
            $stmt = $conn->prepare("INSERT INTO `users` (`email`, `role_id`) VALUES (?, ?)");
            $stmt->execute([$email, $role_id]);
            
            $user_id = $conn->lastInsertId();


            if($stmt->rowCount() == 1){
                // küldő nevének lekérdezése
                $stmt = $conn->prepare("SELECT `users`.`name` FROM `users` WHERE `users`.`id` = ?");
                $stmt->execute([$requestingUser]);

                $userName = $stmt->fetch(PDO::FETCH_ASSOC)['name'];
                
                // felhasználónak email küldése
                $inviteLink = Token::GenerateLinkToken($conn, $user_id, "user-invitation", 1440, ["user_email" => $email, "invite_author" => $userName, "user_type" => "moderator"]);
                InviteUserEmail($email, $inviteLink);
                

                $logger->LogUserAction($requestingUser, "Invite user", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "user_id" => $user_id]);
                Response::success([], "Felhasználó meghívva!");
            } else {
                Response::error(404, "Nem sikerült a meghívót elküldeni!");
            }
        } else if($function == "UpdateUser"){ // felhasználó módosítása
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;
            $phone = isset($input['phone']) ? htmlspecialchars($input['phone']) : null;
            $taj = isset($input['taj']) ? htmlspecialchars($input['taj']) : null;
            $address = isset($input['address']) ? htmlspecialchars($input['address']) : null;
            $gender = isset($input['gender']) ? htmlspecialchars($input['gender']) : null;
            $birthdate = isset($input['birthdate']) ? htmlspecialchars($input['birthdate']) : null;
            $hasTB = isset($input['hasTB']) ? htmlspecialchars($input['hasTB']) : null;

            
            // hiányzó adat estén visszatérés hibával és hibás mezőkkel
            $errorFields = [];
            if ($name === null || strlen(trim($name)) == 0) {
                $errorFields[] = "name";
            }
            if($phone === null || strlen(trim($phone)) == 0) {
                $errorFields[] = "phone";
            }
            if($address === null || strlen(trim($address)) == 0){
                $errorFields[] = "address";
            }
            if($gender === null){
                $errorFields[] = "gender";
            }
            if($birthdate === null || strlen(trim($birthdate)) == 0){
                $errorFields[] = "birthdate";
            }
            if($hasTB === null){
                $errorFields[] = "hasTB";
            }

            if(strlen(trim($taj)) == 0) {
                $taj = null;
            } 

            if(count($errorFields) != 0){
                Response::error(400, "A pirossal jelölt mező(k) kitöltése kötelező!", ['errorFields' => $errorFields]);
            }

            // kitöltések szintaktikai ellenőrzése - ha rendben van továbblép, egyébként üzenetben választ küld
            ValidateName($name); // név ellenőrzés
            ValidatePhonenumber($phone); // telefonszám ellenőrzése
            if($taj != null) {
                ValidateTaj($taj); // TAJ szám ellenőrzése
            }
            ValidateAddress($address); // Cím ellenőrzése
            ValidateGender($gender); // Nem ellenőrzése
            ValidateAge($birthdate); // Életkor ellenőrzése


            if(($hasTB == "true" || $hasTB == 1 || $hasTB == true) && $taj != null){
                $hasTB = 1;
            } else {
                $hasTB = 0;
            }

            $birthdate = explode('T', $birthdate)[0];

            //Felhasználó régi adatainak lekérdezése
            $stmt = $conn->prepare("SELECT `users`.`name`, `users`.`email`, `user_data`.`gender_id`, `user_data`.`phone`, `user_data`.`TAJ`, `user_data`.`address`, `user_data`.`birthdate`, `user_data`.`hasTB` FROM `users` INNER JOIN `user_data` ON `user_data`.`user_id` = `users`.`id` WHERE `users`.`id` = ?");
            $stmt->execute([$user_id]);

            $user = $stmt->fetch(PDO::FETCH_ASSOC);
            
            if($user){
                // adatok összehasonlítása
                $modifiedData = array();

                if($user["name"] != $name){
                    $modifiedData[] = array("old_name" => $user["name"], "new_name" => $name);
                }
                if($user["gender_id"] != $gender) {
                    $modifiedData[] = array("old_gender" => $user["gender_id"], "new_gender" => $gender);
                }
                if($user["phone"] != $phone) {
                    $modifiedData[] = array("old_phone" => $user["phone"], "new_phone" => $phone);
                }
                if($user["TAJ"] != $taj) {
                    $modifiedData[] = array("old_TAJ" => $user["TAJ"], "new_TAJ" => $taj);
                }
                if($user["address"] != $address) {
                    $modifiedData[] = array("old_address" => $user["address"], "new_address" => $address);
                }
                if($user["birthdate"] != $birthdate) {
                    $modifiedData[] = array("old_birthdate" => $user["birthdate"], "new_birthdate" => $birthdate);
                }
                if($user["hasTB"] != $hasTB) {
                    $modifiedData[] = array("old_hasTB" => $user["hasTB"], "new_hasTB" => $hasTB);
                }

                if(count($modifiedData) > 0) {
                    // taj lekérdezése
                    $stmt = $conn->prepare("SELECT `user_data`.`TAJ` FROM `user_data` WHERE `user_data`.`TAJ` = ? AND `user_data`.`user_id` != ?");
                    $stmt->execute([$taj, $user_id]);
                    $exists = $stmt->fetch(PDO::FETCH_ASSOC);
                    if($exists == false){
                        //adatok módosítása
                        $stmt = $conn->prepare("UPDATE `user_data` SET `gender_id` = ?, `phone` = ?, `TAJ` = ?, `address` = ?, `birthdate` = ?, `hasTB` = ? WHERE `user_data`.`user_id` = ?");
                        $stmt->execute([$gender, $phone, $taj, $address, $birthdate, $hasTB, $user_id]);
                        
                        $stmt = $conn->prepare("UPDATE `users` SET `name` = ? WHERE `users`.`id` = ?");
                        $stmt->execute([$name, $user_id]);
                        
                        // eseménynapló frissítése
                        $method = ($requestingUser == $user_id) ? "Self update": "Admin update";
                        $logger->LogUserAction($requestingUser, "updateProfile", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true, "method" => $method, "modifiedData" => $modifiedData]);
                        
                        // email értesítés
                        UpdateUserEmail($user['email']);
                        
                        Response::success(200, "Sikeres adatmódosítás!");
                    } else {
                        Response::error(400, "Ez a TAJ szám már regisztrálva van!");
                    }
                } else {
                    Response::success(200, "Nem végzett adatmódosítást!");
                }
                
            } else{
                Response::error(404, "Nincs ilyen felhasználó!");
            }
        } else if ($function == "ResendEmailConfirm") {
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];


            //adatok kinyerése a kérésből
            $input = json_decode(file_get_contents('php://input'), true);

            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;

            if($user_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }


            // email lekérdezése
            $stmt = $conn->prepare("SELECT `users`.`email` FROM `users` WHERE `users`.`id` = ? AND `users`.`deleted` = ?");
            $stmt->execute([$user_id, false]);
    
            $user = $stmt->fetch(PDO::FETCH_ASSOC);
    
            if($user){
                // eseménynapló frissítése
                $logger->LogUserAction($requestingUser, "resendConfirmEmail", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true]);
                
                // email küldése
                $confirmToken = Token::GenerateLinkToken($conn, $user_id, "confirm-email", 120);
                if(NewRegistrationEmail($user['email'], $confirmToken)){
                    Response::success([], "Email elküldve!");
                } else{
                    Response::error(500, "Hiba lépett fel a küldéskor");
                }
            } else {
                Response::error(500, "Hiba lépett fel a küldéskor");
            }      
        } else {
            Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
        }
    } else{
        Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
    }
} else if($_SERVER['REQUEST_METHOD'] == "DELETE"){
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){
        if($function == "Delete"){ // Felhasználó eltávolítása a rendszerből
            
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            $requestingUser = $checkAccess['user_id'];

            
            //cég id kinyerése
            $input = json_decode(file_get_contents("php://input"), true); // adatok kinyerése
            
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;

            if($user_id === null){
                Response::error(400, "Hiányzó azonosító!");
            }

            //cég törlése id alapján
            $stmt = $conn->prepare("UPDATE `users` SET `users`.`deleted` = ? WHERE `users`.`id` = ?");
            $stmt->execute([true, $user_id]);

            $logger->LogUserAction($requestingUser, "Delete user", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true]);

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