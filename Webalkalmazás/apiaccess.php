<?php
require_once("./config.php");
require_once("./api/v1/classes/Response.php");

function CheckApiAccessToken(){ // visszatérés a tokennel
    //token ellenőrzése, ha nincs, lekérése
    if(isset($_SESSION['token']) && $_SESSION['token'] != null && strtotime($_SESSION['token']['expiresAt']['date']) > strtotime('now')){
        $token = $_SESSION['token']['access_token'];
    } else { // token lekérdezése
        if((isset($_SESSION['token']) && $_SESSION['token']['level'] == 'public') || !isset($_SESSION['logged_in']) || !isset($_SESSION['user_id']) || $_SESSION['logged_in'] == false){ // új nyilvános token lekérdezése
            
            // cURL beállítások
            $ch = curl_init($GLOBALS['API_DEFAULT_URL'] . '/public/public_token');
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_HTTPHEADER, [
                "Content-Type: application/json"
            ]);

            $response = curl_exec($ch);
            if (curl_errno($ch)) {
                echo 'Error: ' . curl_error($ch);
            }
            curl_close($ch);

            // Válasz feldolgozása
            $data = json_decode($response, true);
            $_SESSION['token'] = $data['data'];
            $token = $_SESSION['token']['access_token'];
        } else { // privát token frissítése
            $sendData = [
                "user_id" => $_SESSION['user_id']
            ];

            // cURL beállítások
            $ch = curl_init($GLOBALS['API_DEFAULT_URL'] . '/auth/refreshToken');
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_POST, true); // POST kérés
            curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($sendData));
            curl_setopt($ch, CURLOPT_HTTPHEADER, [
                "Authorization: Bearer " . $_SESSION['token']['access_token'],
                "Content-Type: application/json"
            ]);

            $response = curl_exec($ch);
            if (curl_errno($ch)) {
                echo 'Error: ' . curl_error($ch);
            }
            curl_close($ch);

            // Válasz feldolgozása
            $data = json_decode($response, true);
            $_SESSION['token'] = $data['data']['token'];
            $token = $_SESSION['token']['access_token'];
        }
    }
    return $token;
}

function CurlRequest($url, $method = 'GET', $data = null){
    $token = CheckApiAccessToken();

    // cURL beállítások
    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    
    // POST és DELETE kérés kiegészítése
    if($method == "POST"){
        curl_setopt($ch, CURLOPT_POST, true); // POST kérés
        curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    } else if ($method == "DELETE"){
        curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "DELETE"); // DELETE metódus
        curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    }

    // fejlécek beállítása
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        "Authorization: Bearer $token",
        "Content-Type: application/json"
    ]);


    // kérés elküldése
    $response = curl_exec($ch);
    if (curl_errno($ch)) {
        echo 'Error: ' . curl_error($ch);
    }
    curl_close($ch);

    // Válasz feldolgozása
    $data = json_decode($response, true);
    
    if(isset($data['success']) && $data['success'] == false && isset($data['error']) && isset($data['error']['details']["token_detail"]) && $data['error']['details']["token_detail"] == false){
        $_SESSION['token']['expiresAt']['date'] = date("Y-m-d H:i", strtotime("now"));
        return CurlRequest($url, $method, $data);
    } else {
        return $data;
    }
}

function ApiAccessGet($url, $write = false){
    $response = CurlRequest($url);
    if($write){
        echo(json_encode($response));
        exit;
    } else {
        return $response;
    }
}

function ApiAccessPost($url, $data, $write = false){
    $response = CurlRequest($url, "POST", $data);
    if($write){
        echo(json_encode($response));
        exit;
    } else {
        return $response;
    }
}

function ApiAccessDelete($url, $data, $write = false){
    $response = CurlRequest($url, "DELETE", $data);
    if($write){
        echo(json_encode($response));
        exit;
    } else {
        return $response;
    }
}


// Api hozzáférés biztosítása javascript XMLHttpRequest részére
if(isset($_SERVER['HTTP_X_REQUESTED_WITH']) && $_SERVER['HTTP_X_REQUESTED_WITH'] == 'XMLHttpRequest'){
    // Szétbontjuk a kérést a lehetséges típusokra
    $requestMethod = $_SERVER['REQUEST_METHOD'];

    switch($requestMethod){
        case 'GET':
            // GET kérés továbbítása
            // cél url meglétének ellenőrzés
            if(isset($_GET['url']) && $_GET['url'] != ""){
                echo(json_encode(ApiAccessGet(($_GET['url']), true)));
            } else{
                Response::error(400, "Hibás kérés!");
            }
            break;
        case 'POST':
            // POST kérés továbbítása
            // cél url kinyerése
            $input = json_decode(file_get_contents('php://input'), true);
            if(isset($input['url']) && $input['url'] != "" && isset($input['data'])){
                if(isset($input['includeUserId']) && $input['includeUserId']){
                    $input["data"]["user_id"] = $_SESSION['user_id'];
                }
                if(isset($input['setSession'])){
                    $response = ApiAccessPost($input['url'], $input['data'], false);

                    if($response['success']){   
                        switch($input["setSession"]){
                            case "login":
                                if($response["data"]["web_login"] == true){
                                    $_SESSION['user_id'] = $response["data"]["user_id"];
                                    $_SESSION['logged_in'] = true;
                                    $_SESSION['user_name'] = $response["data"]["user_name"];
                                    $_SESSION['token'] = $response["data"]["token"];

                                    if(isset($response["data"]["confirmEmail"])){
                                        $_SESSION["confirmEmail"] = $response["data"]["confirmEmail"];
                                    }
                                    echo(json_encode(["success" => true, "message" => $response["message"], "detail" => $response['data']['web_login']]));
                                } else {
                                    echo(json_encode(["success" => false, "error" => ["code" => 401, "message" => "Nincs jogosultsága a bejelentkezéshez!", "details" => []]]));
                                }
                                break;
                            case "register":
                                $_SESSION['user_id'] = $response["data"]["user_id"];
                                $_SESSION['logged_in'] = true;
                                $_SESSION['user_name'] = $response["data"]["user_name"];
                                $_SESSION['token'] = $response["data"]["token"];
                                $_SESSION['confirmEmail'] = true;
                                echo(json_encode(["success" => true, "message" => $response["message"]]));
                                break;
                            case "update":
                                $_SESSION['user_name'] = $input["data"]["name"];
                                echo(json_encode(["success" => true, "message" => $response["message"]]));
                            }
                    } else {
                        echo(json_encode($response));
                    }
                } else {
                    ApiAccessPost($input['url'], $input['data'], true);
                }
            } else {
                Response::error(400, "Hibás kérés! apiaccess");
            }
            break;
        case 'DELETE':
            // DELETE kérés továbbítása
            // cél url kinyerése
            $input = json_decode(file_get_contents('php://input'), true);

            if(isset($input['url']) && $input['url'] != "" && isset($input['data'])){
                if(isset($input['includeUserId']) && $input['includeUserId']){
                    $input["data"]["user_id"] = $_SESSION['user_id'];
                }
                ApiAccessDelete(($input['url']), $input['data'], true);
            } else {
                Response::error(400, "Hibás kérés!");
            }
            break;
        default:
            Response::error(400, "Hibás típusú kérés!");
            break;
    }
}
?>