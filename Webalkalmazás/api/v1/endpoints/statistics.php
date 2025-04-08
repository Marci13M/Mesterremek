<?php

if ($_SERVER['REQUEST_METHOD'] == 'GET') {
    if((isset($_SERVER['HTTP_AUTHORIZATION']) && strlen($_SERVER['HTTP_AUTHORIZATION']) > 0)){

        if(preg_match("/^DoctorReservations(\?|$)/", $function) ? true : false){ // orvos foglalásainak lekérdezése
            if(count($_GET) == 3 && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){ // orvosadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont rendelései
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, COUNT(*) AS `sum` FROM `reservations` WHERE `reservations`.`doctor_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([$doctor_id, false, $from, $to]);

                $reservationsSum = 0;
                $reservations = array();
                while($row = $stmt->fetch()){
                    $reservationsSum += $row['sum'];
                    $reservations[] = array("date" => $row['datetime'], "reservations" => $row['sum']);
                }

                //kért időpontot megelőző azonos idzőszak rendelések számának lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT COUNT(*) AS `sum` FROM `reservations` WHERE `reservations`.`doctor_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$doctor_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $reservationsSum;
                } else {
                    $prevNum = $prevNum['sum'];
                }
                if($prevNum == 0 || $reservationsSum == 0){
                    $change = 0;
                } else{
                    $change = $reservationsSum / $prevNum;
                }

                $respData = [
                    "reservations" => $reservations,
                    "total_reservations" => $reservationsSum,
                    "reservation_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            } else {
                Response::error(400, "Hibás kérés!");
            }
        } else if(preg_match("/^HospitalReservations(\?|$)/", $function) ? true : false){ // Kóráház foglalásainak lekérdezése
            if(count($_GET) == 3 && isset($_GET['hospital_id']) && isset($_GET['from']) && isset($_GET['to'])){ // kórházadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $hospital_id = htmlspecialchars($_GET['hospital_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont rendelései
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, COUNT(*) AS `sum` FROM `reservations` WHERE `reservations`.`hospital_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([$hospital_id, false, $from, $to]);

                $reservationsSum = 0;
                $reservations = array();
                while($row = $stmt->fetch()){
                    $reservationsSum += $row['sum'];
                    $reservations[] = array("date" => $row['datetime'], "reservations" => $row['sum']);
                }

                //kért időpontot megelőző azonos idzőszak rendelések számának lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT COUNT(*) AS `sum` FROM `reservations` WHERE `reservations`.`hospital_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$hospital_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch()['sum'];
                if($prevNum == 0){
                    $prevNum = $reservationsSum;
                }
                if($prevNum == 0 || $reservationsSum == 0){
                    $change = 0;
                } else{
                    $change = $reservationsSum / $prevNum;
                }

                $respData = [
                    "reservations" => $reservations,
                    "total_reservations" => $reservationsSum,
                    "reservation_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        }else if(preg_match("/^CompanyReservations(\?|$)/", $function) ? true : false){ // Cég foglalásainak lekérdezése
            if(count($_GET) == 3 && isset($_GET['company_id']) && isset($_GET['from']) && isset($_GET['to'])){ // cégadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $company_id = htmlspecialchars($_GET['company_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont rendelései
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, COUNT(*) as 'sum' FROM `reservations` INNER JOIN (`company` INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company`.`id`) ON `reservations`.`hospital_id` = `hospitals`.`id` WHERE `company`.`id` = ? AND `reservations`.`deleted` = ? GROUP BY `hospitals`.`id`, YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`) HAVING  `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$company_id, false, $from, $to]);

                $reservationsSum = 0;
                $reservations = array();
                while($row = $stmt->fetch()){
                    $reservationsSum += $row['sum'];
                    $reservations[] = array("date" => $row['datetime'], "reservations" => $row['sum']);
                }

                //kért időpontot megelőző azonos idzőszak rendelések számának lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT COUNT(*) AS `sum` FROM `reservations` INNER JOIN (`company` INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company`.`id`) ON `reservations`.`hospital_id` = `hospitals`.`id` WHERE `hospitals`.`id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY `hospitals`.`id`");
                $stmt->execute([$company_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $reservationsSum;
                } else{
                    $prevNum = $prevNum["sum"];
                }
                if($prevNum == 0 || $reservationsSum == 0){
                    $change = 0;
                } else{
                    $change = $reservationsSum / $prevNum;
                }

                $respData = [
                    "reservations" => $reservations,
                    "total_reservations" => $reservationsSum,
                    "reservation_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        }else if(preg_match("/^TotalReservations(\?|$)/", $function) ? true : false){ // Összes foglalás lekérdezése
            if(count($_GET) == 2 && isset($_GET['from']) && isset($_GET['to'])){ // kórházadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont rendelései
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, COUNT(*) AS 'sum' FROM `reservations` WHERE `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([false, $from, $to]);

                $reservationsSum = 0;
                $reservations = array();
                while($row = $stmt->fetch()){
                    $reservationsSum += $row['sum'];
                    $reservations[] = array("date" => $row['datetime'], "reservations" => $row['sum']);
                }

                //kért időpontot megelőző azonos idzőszak rendelések számának lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT COUNT(*) AS `sum` FROM `reservations` WHERE `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $reservationsSum;
                } else{
                    $prevNum = $prevNum['sum'];
                }
                if($prevNum == 0 || $reservationsSum == 0){
                    $change = 0;
                } else{
                    $change = $reservationsSum / $prevNum;
                }

                $respData = [
                    "reservations" => $reservations,
                    "total_reservations" => $reservationsSum,
                    "reservation_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        } else if(preg_match("/^DoctorIncome(\?|$)/", $function) ? true : false){ // Kórház bevételek
            if(count($_GET) == 3 && isset($_GET['doctor_id']) && isset($_GET['from']) && isset($_GET['to'])){ // kórházadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $doctor_id = htmlspecialchars($_GET['doctor_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont bevételei
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`doctor_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([$doctor_id, false, $from, $to]);

                $totalIncome = 0;
                $incomes = array();
                while($row = $stmt->fetch()){
                    $totalIncome += $row['income'];
                    $incomes[] = array("date" => $row['datetime'], "income" => $row['income']);
                }

                //kért időpontot megelőző azonos idzőszak bevételeinek lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`doctor_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$doctor_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása

                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $totalIncome;
                } else{
                    $prevNum = $prevNum["income"];
                }
                if($prevNum == 0 || $totalIncome == 0){
                    $change = 0;
                } else{
                    $change = $totalIncome / $prevNum;
                }

                $respData = [
                    "incomes" => $incomes,
                    "total_income" => $totalIncome,
                    "income_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        } else if(preg_match("/^HospitalIncome(\?|$)/", $function) ? true : false){ // Kórház bevételek
            if(count($_GET) == 3 && isset($_GET['hospital_id']) && isset($_GET['from']) && isset($_GET['to'])){ // kórházadat lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $hospital_id = htmlspecialchars($_GET['hospital_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont bevételei
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`hospital_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([$hospital_id, false, $from, $to]);

                $totalIncome = 0;
                $incomes = array();
                while($row = $stmt->fetch()){
                    $totalIncome += $row['income'];
                    $incomes[] = array("date" => $row['datetime'], "income" => $row['income']);
                }

                //kért időpontot megelőző azonos idzőszak bevételeinek lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`hospital_id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$hospital_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása

                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $totalIncome;
                } else{
                    $prevNum = $prevNum["income"];
                }
                if($prevNum == 0 || $totalIncome == 0){
                    $change = 0;
                } else{
                    $change = $totalIncome / $prevNum;
                }

                $respData = [
                    "incomes" => $incomes,
                    "total_income" => $totalIncome,
                    "income_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        } else if(preg_match("/^CompanyIncome(\?|$)/", $function) ? true : false){ // Cég bevételek
            if(count($_GET) == 3 && isset($_GET['company_id']) && isset($_GET['from']) && isset($_GET['to'])){ // cégadatok lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $company_id = htmlspecialchars($_GET['company_id']);
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont bevételei
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, SUM(`reservations`.`price`) as 'income' FROM `reservations` INNER JOIN (`company` INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company`.`id`) ON `reservations`.`hospital_id` = `hospitals`.`id` WHERE `company`.`id` = ? AND `reservations`.`deleted` = ? GROUP BY `hospitals`.`id`, YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`) HAVING `reservations`.`datetime` BETWEEN ? AND ?");
                $stmt->execute([$company_id, false, $from, $to]);

                $totalIncome = 0;
                $incomes = array();
                while($row = $stmt->fetch()){
                    $totalIncome += $row['income'];
                    $incomes[] = array("date" => $row['datetime'], "income" => $row['income']);
                }

                //kért időpontot megelőző azonos idzőszak bevételeinek lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT SUM(`reservations`.`price`) AS `income` FROM `reservations` INNER JOIN (`company` INNER JOIN `hospitals` ON `hospitals`.`company_id` = `company`.`id`) ON `reservations`.`hospital_id` = `hospitals`.`id` WHERE `company`.`id` = ? AND `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY `company`.`id`");
                $stmt->execute([$company_id, false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $totalIncome;
                } else{
                    $prevNum = $prevNum["income"];
                }
                if($prevNum == 0 || $totalIncome == 0){
                    $change = 0;
                } else{
                    $change = $totalIncome / $prevNum;
                }

                $respData = [
                    "incomes" => $incomes,
                    "total_income" => $totalIncome,
                    "income_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
            }
        }  else if(preg_match("/^TotalIncome(\?|$)/", $function) ? true : false){ // Összesített bevételek
            if(count($_GET) == 2 && isset($_GET['from']) && isset($_GET['to'])){ // bevételek lekérdezése
                
                // autentikáció tokennel
                $authHeader = $_SERVER['HTTP_AUTHORIZATION'];
                $accessToken = str_replace('Bearer ', '', $authHeader);

                $checkAccess = Token::CheckToken($conn, $accessToken, explode("?", $requestUri)[0]);
                $requestingUser = $checkAccess['user_id'];


                // statisztikai lekérdezés
                $from = htmlspecialchars($_GET['from']);
                $to = htmlspecialchars($_GET['to']);

                //kért időpont bevételei
                $stmt = $conn->prepare("SELECT `reservations`.`datetime`, SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([false, $from, $to]);

                $totalIncome = 0;
                $incomes = array();
                while($row = $stmt->fetch()){
                    $totalIncome += $row['income'];
                    $incomes[] = array("date" => $row['datetime'], "income" => $row['income']);
                }

                //kért időpontot megelőző azonos idzőszak bevételeinek lekérdezése
                $prevFrom = date("Y-m-d", strtotime($from . " - " . round((strtotime($to) - strtotime($from)) / (60*60*24))+1 . " day"));
                $stmt = $conn->prepare("SELECT SUM(`reservations`.`price`) AS `income` FROM `reservations` WHERE `reservations`.`deleted` = ? AND `reservations`.`datetime` BETWEEN ? AND ? GROUP BY YEAR(`reservations`.`datetime`), MONTH(`reservations`.`datetime`), DAY(`reservations`.`datetime`)");
                $stmt->execute([false, $prevFrom, date("Y-m-d", strtotime($from . " - 1 day"))]);

                // tendencia kiszámítása
                $prevNum = $stmt->fetch();
                if($prevNum == false){
                    $prevNum = $totalIncome;
                } else{
                    $prevNum = $prevNum['income'];
                }
                if($prevNum == 0 || $totalIncome == 0){
                    $change = 0;
                } else{
                    $change = $totalIncome / $prevNum;
                }

                $respData = [
                    "incomes" => $incomes,
                    "total_income" => $totalIncome,
                    "income_change" => $change
                ];

                $metadata = [
                    "from" => $from,
                    "to" => $to,
                    "prevFrom" => $prevFrom,
                    "prevTo" => date("Y-m-d", strtotime($from . " - 1 day"))
                ];

                // válasz megjelenítése
                Response::success($respData, "Sikeres kérés!", $metadata);
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