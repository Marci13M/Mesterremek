<?php

//token osztály létrehozás az API hozzáférés megvalósításához
class Token{
    //Token generálása
    public static function generateToken($db, $user_id, $validTime = "+10 minute", $length = 32) {
        $accessToken = bin2hex(random_bytes($length));
        $expiresAt = new DateTime($validTime); // Token élettartama

        
        $stmt = $db->prepare("INSERT INTO tokens (user_id, access_token, expires_at) VALUES (?, ?, ?)");
        $stmt->execute([$user_id, $accessToken, $expiresAt->format('Y-m-d H:i:s')]);

        if($user_id != null){
            //log frissítése
            $logger = new Logger($db);
            $logger->LogUserAction($user_id, "New token requested", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
        }

        return array("access_token" => $accessToken, "expiresAt" => $expiresAt, "level" => "private");
    }

    public static function GenerateLinkToken($db, $user_id, $action, $expiresInMinutes, $data = []){
        $secretKey = hash('sha256', bin2hex(random_bytes(16)));
        $expiresAt = time() + ($expiresInMinutes * 60);
        $data = json_encode([
            'user_id' => $user_id,
            'action' => $action,
            'data' => $data,
            'expires_at' => $expiresAt
        ]);

        $signiture = hash_hmac('sha256', $data, $secretKey);

        $iv = openssl_random_pseudo_bytes(16);
        $encryptedData = openssl_encrypt($data, 'aes-256-cbc', $secretKey, 0, $iv);
        $encryptedData = base64_encode($iv . $encryptedData);

        // rögzítés adatbázisban
        $stmt = $db->prepare("INSERT INTO `link_tokens` (`token`, `secret_key`) VALUES (?, ?)");
        $stmt->execute([$signiture, $secretKey]);

        // válasz küldése
        return array("data" => $encryptedData, "signiture" => $signiture);
    }

    public static function CheckLinkToken($db, $data, $signiture) {
        // token létezésének ellenőrzése
        $stmt = $db->prepare("SELECT `link_tokens`.`id`, `link_tokens`.`secret_key`, `link_tokens`.`used` FROM `link_tokens` WHERE `link_tokens`.`token` = ? AND `link_tokens`.`deleted` = ?");
        $stmt->execute([$signiture, false]);

        $token = $stmt->fetch(PDO::FETCH_ASSOC);

        if($token){ // létezik, ellenőrizni kell
            // adatok dekódolása
            $secretKey = $token["secret_key"];
            $data = base64_decode($data);
            $iv = substr($data, 0, 16);
            $encryptedData = substr($data, 16);

            $decryptedData = openssl_decrypt($encryptedData, 'aes-256-cbc', $secretKey, 0, $iv);

            if(hash_hmac('sha256', $decryptedData, $token['secret_key']) == $signiture){ // aláírás
                if(!$token['used']){ // felhasználtság
                    $decryptedData = json_decode($decryptedData, true);
                    if($decryptedData['expires_at'] > strtotime('now')){ // érvényesség
                        // felhasználó keresés
                        $stmt = $db->prepare("SELECT `users`.`id` FROM `users` WHERE `users`.`id` = ? AND `users`.`deleted` = ?");
                        $stmt->execute([$decryptedData['user_id'], false]);

                        $user = $stmt->fetch(PDO::FETCH_ASSOC);
                        
                        if($user){ // felhasználó
                            return array("success" => true, "data" => $decryptedData, "token_id" => $token['id']);
                        } else {
                            return array("success" => false, "error" => array("message" => "A link már nem érvényes"));
                        }
                    } else {
                        return array("success" => false, "error" => array("message" => "A link lejárt, kérjen újat"));
                    }
                } else {
                    return array("success" => false, "error" => array("message" => "A linket már felhasználták"));
                }
            } else {
                return array("success" => false, "error" => array("message" => "A link érvénytelen vagy már felhasználták"));
            }
        } else { // nem létezik
            return array("success" => false, "error" => array("message" => "Hiba lépett fel, kérjük próbálkozzon később"));
        }
    }

    public static function UseToken($db, $token_id){
        // link használttá nyílvánítása
        $stmt = $db->prepare("UPDATE `link_tokens` SET `link_tokens`.`used` = true WHERE `link_tokens`.`id` = ?");
        $stmt->execute([$token_id]);

        return array("success" => true);
    }

    // Token ellenőrzése
    public static function CheckToken($db, $accessToken, $endpoint){
        // lejárati idő lekérdezése a token páros alapján
        $stmt = $db->prepare("SELECT `tokens`.`id`, `tokens`.`user_id`, `tokens`.`expires_at` FROM `tokens` WHERE `tokens`.`access_token` = ?");
        $stmt->execute([$accessToken]);
        $resp = $stmt->fetch(PDO::FETCH_ASSOC);

        $token_expiration = $resp['expires_at'];
        $user_id = $resp['user_id'];

        //log frissítése
        $logger = new Logger($db);

        // végpontok alapértéke
        $apiEndpoints = array();

        // token létezésének ellenőrzése
        if ($resp == false) {
            $logger->LogUserAction($user_id, "Usage of invalid token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
            // return ["success" => false, "status_code" => 401, "message" => "Érvénytelen token"];
            Response::error(401, "Érvénytelen token", ["token_detail" => false]);
        }

        // lejárati idő ellenőrzése
        $expiresAt = new DateTime($token_expiration);
        if ($expiresAt < new DateTime()) {
            $logger->LogUserAction($user_id, "Usage of expired token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
            // return ["success" => false, "status_code" => 401, "message" => "Lejárt token"];
            Response::error(401, "Lejárt token", ["token_detail" => false]);
        }

        if($user_id != null){
            // Ide kerül a jogosultságellenőzrés logikája
            $stmt = $db->prepare("SELECT `roles`.`tags` FROM `roles` INNER JOIN `users` ON `roles`.`id` = `users`.`role_id` WHERE `users`.`id` = ?");
            $stmt->execute([$user_id]);
            $resp = $stmt->fetch(PDO::FETCH_ASSOC);

            // jogosultság nem létezik
            if($resp == false) {
                $logger->LogUserAction($user_id, "No role found", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false]);
                // return ["success" => false, "status_code" => 401, "message" => "Nincs ilyen jogosultság"];
                Response::error(401, "Nincs ilyen jogosultság");
            }

            $apiEndpoints = json_decode($resp['tags'])->api_endpoints;
        }

        // végpont lekérdezés adatbáziból
        $stmt = $db->prepare("SELECT `api_endpoints`.`id`, `api_endpoints`.`public` FROM `api_endpoints` WHERE `api_endpoints`.`name` = ? AND `api_endpoints`.`deleted` = ?");
        $stmt->execute([$endpoint, false]);
        $endpointResp = $stmt->fetch(PDO::FETCH_ASSOC);

        if($endpointResp == false) {
            $logger->LogUserAction($user_id, "No endpoint found", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "endpoint" => $endpoint]);
            // return ["success" => false, "status_code" => 401, "message" => "Nincs ilyen végpont"];
            Response::error(401, "Nincs ilyen végpont", ["endpoint" => $endpoint]);
        }

        $endpoint_id = $endpointResp['id'];
        $endpoint_public = $endpointResp['public'];

        if($endpoint_public || $apiEndpoints == "ALL" || in_array($endpoint_id, $apiEndpoints)){
            // $logger->LogUserAction($user_id, "Usage of token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => true, "endpoint" => $endpoint]);
            return ["success" => true, "user_id" => $user_id, "message" => "Jóváhagyva"];
            // Response::success(["access_token" => $accessToken, "expiresAt" => $expiresAt, "success" => true], "Access grantend.");
        } else{
            $logger->LogUserAction($user_id, "Usage of insufficient token", $_SERVER['REMOTE_ADDR'], isset($_SERVER['REMOTE_HOST']) ? $_SERVER['REMOTE_HOST'] : null, null, ["success" => false, "endpoint" => $endpoint]);
            // return ["success" => false, "status_code" => 403, "message" => "Nincs megfelelő jogosultsága"];
            Response::error(403, "Nincs megfelelő jogosultsága", ['user_id' => $user_id, 'token' => $accessToken]);
        }
        
    }

}