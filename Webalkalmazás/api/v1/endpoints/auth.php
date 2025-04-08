<?php


if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    if($function == "login"){ // bejelentkezési kérés feldolgozása
        // adatok kinyerése a kérésből
        $input = json_decode(file_get_contents('php://input'), true);

        $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
        $password = isset($input['password']) ? htmlspecialchars($input['password']) : null;
        
        // hiányzó adat estén visszatérés hibával
        if ($email === null || $password === null) {
            Response::error(400, "Hiányzó email vagy jelszó!", $input);
        }

        // jelszó titkosítása
        $passwordHash = hash("sha256", $password);
        
        // Felhasználó ellenőrzése
        $stmt = $conn->prepare("SELECT `users`.`id` AS 'user_id', `users`.`name` AS 'user_name', `users`.`email` AS 'user_email', JSON_UNQUOTE(JSON_EXTRACT(`roles`.`tags`, '$.role_identifier')) AS 'role_identifier', JSON_UNQUOTE(JSON_EXTRACT(`roles`.`tags`, '$.web_login')) AS 'web_login' FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` WHERE BINARY `users`.`password` = ? AND BINARY `users`.`email` = ? AND `users`.`deleted` = ?");
        $stmt->execute([$passwordHash, $email, false]);
        $user = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($user) {
            $data = $user;
            switch($user['role_identifier']){
                case "hospital":
                    // Felhasználóhoz tartozó kórház azonosítója
                    $stmt = $conn->prepare("SELECT `user_hospital`.`hospital_id` AS 'hospital_id', `hospitals`.`name` AS 'hospital_name', `hospitals`.`company_id` AS 'company_id' FROM `user_hospital` INNER JOIN `hospitals` ON `user_hospital`.`hospital_id` = `hospitals`.`id` WHERE `user_hospital`.`user_id` = ?");
                    $stmt->execute([$user['user_id']]);
                    $resp = $stmt->fetch(PDO::FETCH_ASSOC);
                    $data['hospital_id'] = ($resp) ? $resp['hospital_id'] : null;
                    $data['company_id'] = ($resp) ? $resp['company_id'] : null;
                    $data['institution_name'] = ($resp) ? $resp['hospital_name'] : null;
                    break;
                case "company":
                    // Felhasználóhoz tartozó kórház azonosítója
                    $stmt = $conn->prepare("SELECT `user_company`.`company_id` AS 'company_id', `company`.`name` AS 'company_name' FROM `user_company` INNER JOIN `company` ON `user_company`.`company_id` = `company`.`id` WHERE `user_company`.`user_id` = ?");
                    $stmt->execute([$user['user_id']]);
                    $resp = $stmt->fetch(PDO::FETCH_ASSOC);
                    $data['company_id'] = ($resp) ? $resp['company_id'] : null;
                    $data['institution_name'] = ($resp) ? $resp['company_name'] : null;
                    break;
                case "default";
                    // email megerősítésre vár
                    $data['confirmEmail'] = true;
                    break;
                }
            // token generálása a felhasználónak
            $data['token'] = Token::generateToken($conn, $user['user_id']);
            
            $logger->LogUserAction($user['user_id'], "login", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true]);
            Response::success($data, "Sikeres azonosítás!");
        } else {
            Response::error(401, "Érvénytelen hitelesítő adatok!");
        }
    } else if($function == "register") { //regisztáció kérés feldolgozása
        // adatok kinyerése a kérésből
        $input = json_decode(file_get_contents('php://input'), true);

        $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;
        $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
        $password = isset($input['password']) ? htmlspecialchars($input['password']) : null;
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
        if($email === null || strlen(trim($email)) == 0){
            $errorFields[] = "email";
        }
        if($password === null || strlen(trim($password)) == 0){
            $errorFields[] = "password";
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

        if(count($errorFields) != 0){
            Response::error(400, "A pirossal jelölt mező(k) kitöltése kötelező!", ['errorFields' => $errorFields]);
        }

        // kitöltések szintaktikai ellenőrzése - ha rendben van továbblép, egyébként üzenetben választ küld
        ValidateName($name); // név ellenőrzés
        ValidateEmail($email); // email ellenőrzése
        ValidatePassword($password); // jelszó ellenőrzése
        ValidatePhonenumber($phone); // telefonszám ellenőrzése
        
        if($taj != null) {
            ValidateTaj($taj); // TAJ szám ellenőrzése
        }
        ValidateAddress($address); // Cím ellenőrzése
        ValidateGender($gender); // Nem ellenőrzése
        ValidateAge($birthdate); // Életkor ellenőrzése


        if($hasTB == "true" && $taj != null){
            $hasTB = 1;
        } else {
            $hasTB = 0;
        }

        // további szükséges adatok beszerzése
        $stmt = $conn->prepare("SELECT `id` FROM `roles` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ?");
        $stmt->execute(["default"]);
        $role_id = $stmt->fetch(PDO::FETCH_ASSOC)["id"];
        $passwordHash = hash("sha256", $password);

        $user = false;

        try{

            // Felhasználó létrehozása
            $conn->prepare("START TRANSACTION")->execute();

            // Alap felhasználó létrehozása
            $stmt = $conn->prepare("INSERT INTO `users` (`name`, `email`, `password`, `role_id`) VALUES (?, ?, ?, ?)");
            $stmt->execute([$name, $email, $passwordHash, $role_id]);
            // Létrehozott felhasználó ID lekérése
            $user_id = $conn->lastInsertId();
            // Felhasználó adatainak feltöltése
            $stmt = $conn->prepare("INSERT INTO `user_data` (`user_id`, `gender_id`, `phone`, `TAJ`, `address`, `birthdate`, `hasTB`) VALUES (?, ?, ?, ?, ?, ?, ?)");
            $stmt->execute([$user_id, $gender, $phone, $taj, $address, $birthdate, $hasTB]);

            // eseménynapló frissítése
            $logger->LogUserAction($user_id, "Registration", $_SERVER["REMOTE_ADDR"], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "method" => "Registration Form"]);
            
            // email küldése
            $confirmToken = Token::GenerateLinkToken($conn, $user_id, "confirm-email", 120);
            if(NewRegistrationEmail($email, $confirmToken)){
                // műveletek lezárása
                $conn->prepare("COMMIT")->execute();
                $user = true;
            } else{
                $conn->prepare("ROLLBACK")->execute();
            }
            
        } catch (Exception $e){
            $conn->prepare("ROLLBACK")->execute();

            // hibák lehetőségének ellenőrzése
            // létező email
            $stmt = $conn->prepare("SELECT `id` FROM `users` WHERE `users`.`email` = ?");
            $stmt->execute([$email]);

            if($stmt->fetch() != false){
                Response::error(500, "Ez az email cím már foglalt!");
            }

            // létező taj
            if($taj !== null){
                $stmt = $conn->prepare("SELECT `user_id` FROM `user_data` WHERE `user_data`.`TAJ` = ?");
                $stmt->execute([$taj]);

                if($stmt->fetch() != false){
                    Response::error(500, "Ez a TAJ szám már foglalt!");
                }
            }
            
            // Valami egyéb hiba
            Response::error(500, "Nem sikerült a felhasználót létrehozni!", $e);
        }
        
        if ($user) {
            $data = array();

            $data["user_id"] = $user_id;
            $data["user_name"] = $name;
            $data["role_identifier"] = "default";
            // token generálása a felhasználónak
            $data["token"] = Token::generateToken($conn, $user_id);

            Response::success($data, "Sikeres regisztráció!");
        }
    } else if ($function == "refreshToken"){
        if(isset($_SERVER['HTTP_AUTHORIZATION'])){
            // autentikáció tokennel
            $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
            $accessToken = str_replace('Bearer ', '', $authHeader);

            // $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
            // $requestingUser = $checkAccess['user_id'];
            
            // adatok kinyerése
            $input = json_decode(file_get_contents('php://input'), true);
            $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
            
            if($user_id === null || !isset($accessToken) || trim($accessToken) == ""){
                Response::error(400, "Hiányzó azonosító!");
            }
            $requestingUser = $user_id;

            // adatok validálása a token alapján
            $stmt = $conn->prepare("SELECT `tokens`.`id` FROM `tokens` WHERE `tokens`.`user_id` = ? AND `tokens`.`access_token` = ? AND `tokens`.`expires_at` < NOW()");
            $stmt->execute([$user_id, $accessToken]);

            if($stmt->rowCount() == 1){
                $data = array("token" => Token::generateToken($conn, $user_id));
                $logger->LogUserAction($requestingUser, "refresh token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true]);
                Response::success($data);
            } else {
                $stmt = $conn->prepare("SELECT `tokens`.`expires_at` AS 'expires_at' FROM `tokens` WHERE `tokens`.`user_id` = ? AND `tokens`.`access_token` = ?");
                $stmt->execute([$user_id, $accessToken]);
                $token = $stmt->fetch(PDO::FETCH_ASSOC);
                if($token) {
                    $data = array("token" => ["access_token" => $accessToken, "expiresAt" => $token["expires_at"], 'level' => 'private']);
                    $logger->LogUserAction($requestingUser, "refresh token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true]);
                    Response::success($data);
                } else {
                    $logger->LogUserAction($requestingUser, "refresh token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                    Response::error(400, "Hiba lépett fel a token frissítésekor!");
                }
            }
        } else {
            Response::error(403, "Nincs jogosultsága az erőforráshoz!", ["message" => "A fejlécben token használata kötelező!"]);
        }
    } else if ($function == "ForgotPasswordEmail") {
        // adatok kinyerése

        $input = json_decode(file_get_contents('php://input'), true);
        $email = isset($input['email']) ? htmlspecialchars($input['email']) : null;
        
        if($email === null){
            Response::error(401, "Hiányzó email!");
        }

        ValidateEmail($email);

        $stmt = $conn->prepare("SELECT `users`.`id` FROM `users` WHERE `users`.`email` = ? AND `users`.`deleted` = ?");
        $stmt->execute([$email, false]);

        $user = $stmt->fetch(PDO::FETCH_ASSOC);

        if($user){
            $logger->LogUserAction($user['id'], "Request new password", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true]);
            $resetPasswordToken = Token::GenerateLinkToken($conn, $user['id'], "forgot-password", 60);
            ForgotPasswordEmail($email, $resetPasswordToken);
        }

        Response::success();
    } else if ($function == "SetNewPassword") {
        // adatok kinyerése
        $input = json_decode(file_get_contents('php://input'), true);
        $password = isset($input['password']) ? htmlspecialchars($input['password']) : null;
        $passwordAgain = isset($input['passwordAgain']) ? htmlspecialchars($input['passwordAgain']) : null;
        $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
        $token_id = isset($input['token_id']) ? htmlspecialchars($input['token_id']) : null;
        
        if($password === null || $passwordAgain === null || $user_id === null || $token_id === null){
            Response::error(401, "Hiányzó adat(ok)!");
        }

        if($password != $passwordAgain){
            Response::error(400, "A két jelszó nem egyezik!");
        }

        ValidatePassword($password);

        $conn->prepare("START TRANSACTION")->execute();

        $stmt = $conn->prepare("UPDATE `users` SET `users`.`password` = ? WHERE `users`.`id` = ?");
        $stmt->execute([hash("sha256",$password), $user_id]);

        $logger->LogUserAction($user_id, "change forgot password", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true]);

        $stmt = $conn->prepare("SELECT `users`.`email` FROM `users` WHERE `users`.`id` = ? AND `users`.`deleted` = ?");
        $stmt->execute([$user_id, false]);

        $user = $stmt->fetch(PDO::FETCH_ASSOC);

        if($user){
            $conn->prepare("COMMIT")->execute();
            Token::UseToken($conn, $token_id);
            UpdatePasswordEmail($user['email']);
            Response::success();
        } else {
            $conn->prepare("ROLLBACK")->execute();
            Response::error(500, "Érvénytelen művelet!");
        }
    } else if ($function == "AcceptInvite"){
        // adatok kinyerése
        $input = json_decode(file_get_contents('php://input'), true);
        $name = isset($input['name']) ? htmlspecialchars($input['name']) : null;
        $password = isset($input['password']) ? htmlspecialchars($input['password']) : null;
        $birthdate = isset($input['birthdate']) ? htmlspecialchars($input['birthdate']) : null;
        $user_id = isset($input['user_id']) ? htmlspecialchars($input['user_id']) : null;
        $token_id = isset($input['token_id']) ? htmlspecialchars($input['token_id']) : null;
        
        if($name === null || $password === null || $user_id === null || $token_id === null){
            Response::error(401, "Hiányzó adat(ok)!");
        }

        
        // adatok ellenőrzése
        ValidateName($name);
        ValidatePassword($password);

        if($birthdate != null){
            ValidateAge($birthdate);
        }

        $stmt = $conn->prepare("UPDATE `users` SET `users`.`name` = ?, `users`.`password` = ? WHERE `users`.`id` = ?");
        $stmt->execute([$name ,hash("sha256",$password), $user_id]);

        if($birthdate != null){
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
            
            // születési idő rögzítése
            $stmt = $conn->prepare("UPDATE `doctors` SET `doctors`.`birthdate` = ?, `doctors`.`slug` = ? WHERE `doctors`.`user_id` = ?");
            $stmt->execute([$birthdate, $slug, $user_id]);
        }

        Token::UseToken($conn, $token_id);
        $logger->LogUserAction($user_id, "Accept invite", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, $user_id, ["success" => true]);

        Response::success();
    } else {
        Response::error(404, "Ismeretlen végpont!", ["endpoint" => explode("?", $requestUri)[0]]);
    }
} else {
    Response::error(400, "Hibás metódus típus!", ["method" => $_SERVER['REQUEST_METHOD']]);
}
?>