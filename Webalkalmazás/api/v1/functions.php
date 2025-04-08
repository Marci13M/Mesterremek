<?php

// regisztrációs adatok validálása
function ValidateName($name) {
    // név ellenőrzése hosszra, tagszámra és betűhasználatra
    if (count(explode(" ", trim($name))) < 2) {
        Response::error(400, "A névnek legalább 2 tagból kell állnia!", ['errorFields' => array("name")]);
    } else if (strlen($name) > 65){
        Response::error(400, "A név legfeljebb 65 karakterből állhat!", ['errorFields' => array("name")]);
    } else if (!preg_match("/^[a-zA-Z áéíóöőúüű ÁÉÍÓÖŐÚÜŰ]+$/", $name)) {
        Response::error(400, "A név kizárólag betűkből állhat!", ['errorFields' => array("name")]);
    }
}

function ValidateEmail($email) {
    // email formátumának ellenőrzése beépített ellenőzréssel
    if(!filter_var($email, FILTER_VALIDATE_EMAIL)){
        Response::error(400, "Az email formátuma nem megfelelő!", ['errorFields' => array("email")]);
    }
}

function ValidatePassword($password) {
    // jelszó ellenőzrése karakterszámra és karakterfajtára
    if(strlen($password) < 8){
        Response::error(400, "A jelszónak legalább 8 karakter hosszúnak kell lennie!", ['errorFields' => array("password")]);
    } else if(!preg_match('/[A-Z]/', $password)){
        Response::error(400, "A jelszónak tartalmaznia kell nagybetűt!", ['errorFields' => array("password")]);
    }else if(!preg_match('/[a-z]/', $password)){
        Response::error(400, "A jelszónak tartalmaznia kell kisbetűt!", ['errorFields' => array("password")]);
    } else if(!preg_match('/[0-9]/', $password)){
        Response::error(400, "A jelszónak tartalmaznia kell számot!", ['errorFields' => array("password")]);
    } else if(!preg_match('/[^A-Za-z0-9]/', $password)) {
        Response::error(400, "A jelszónak tartalmaznia kell különleges karaktert!", ['errorFields' => array("password")]);
    }
}

function ValidatePhonenumber($phonenumber){
    // Telefonszám ellenőrzése
    if(preg_match("/[A-Za-z]/", $phonenumber) || preg_match("/[^A-Za-z0-9]/", $phonenumber)) {
        Response::error(400, "A telefonszám csak számokból állhat!", ['errorFields' => array("phone")]);
    } else if(strlen($phonenumber) < 10 || strlen($phonenumber) > 11){
        Response::error(400, "A telefonszámnak 10 vagy 11 karakter hosszúnak kell lennie!", ['errorFields' => array("phone")]);
    } else if(substr($phonenumber, 0, 2) != "06"){
        Response::error(400, "A telefonszámnak 06-tal kell kezdődnie!", ["errorFields" => array('phone')]);
    }
}

function ValidateTaj($taj){
    // TAJ szám ellenőrzése
    if(strlen($taj) != 9 || preg_match("/[A-Za-z]/", $taj) || preg_match("/[^A-Za-z0-9]/", $taj)) {
        Response::error(400, "A TAJ szám 9 számjegyből kell álljon!", ['errorFields' => array("taj")]);
    } else if(((3 * $taj[0] + 7 * $taj[1] + 3 * $taj[2] + 7 * $taj[3] + 3 * $taj[4] + 7 * $taj[5] + 3 * $taj[6] + 7 * $taj[7]) % 10) != $taj[8]) {
        Response::error(400, "Hibás vagy nem létező TAJ szám!", ['errorFields' => array("taj")]);
    } else if($taj == "000000000"){
        Response::error(400, "Hibás vagy nem létező TAJ szám!", ['errorFields' => array("taj")]);
    }
}

function ValidateAddress($address){
    global $conn;
    //cím ellenőrzése
    // Regex minta létrehozása
    $pattern = '/^(\d{4})\s([A-ZÁÉÍÓÖŐÚÜŰa-záéíóöőúüű-]+),\s([A-ZÁÉÍÓÖŐÚÜŰa-záéíóöőúüű\s]+(?:\s(?:u\.|út|tér|krt|utca|sor|dűlő|köz|park|liget|rakpart))?)\s(\d+[A-Za-z\/-]*(?:-\d+)?\.?)(?:\s*(?:emelet|kapu|ajtó)\s*(\d+[A-Za-z\/-]*)?)?$/u';

    if (preg_match($pattern, $address, $matches)) {
        $zip_code = $matches[1];
        $city = $matches[2];

        // Irányítószám ellenőrzése
        $stmt = $conn->prepare("SELECT COUNT(*) FROM zip_codes WHERE zip = ? AND name = ?");
        $stmt->execute([$zip_code, $city]);
        $exists = $stmt->fetchColumn();

        if (!$exists) {
            Response::error(400, "Hibás irányítószám és település kombináció!", ['errorFields' => array("address")]);
        }

    } else {
        Response::error(400, "Hibás címformátum", ['errorFields' => array("address")]);
    }
}

function ValidateGender($gender){
    // Nem ellenőrzése
    if($gender == 0){
        Response::error(400, "Nem kiválasztása kötelező!", ['errorFields' => array("gender")]);
    }
}

function ValidateAge($birthdate){
    // életkor ellenőrzése (elmúlt e 16 éves)
    if(strtotime($birthdate) > strtotime("-16 year", time())){
        Response::error(400, "Legalább 16 évesnek kell lennie a regisztrációhoz!", ['errorFields' => array("birthdate")]);
    }
}


// ékezetek eltávolítása
function RemoveAccents($string) {
    $accents = [
        'á'=>'a', 'é'=>'e', 'í'=>'i', 'ó'=>'o', 'ö'=>'o', 'ő'=>'o', 'ú'=>'u', 'ü'=>'u', 'ű'=>'u',
        'Á'=>'A', 'É'=>'E', 'Í'=>'I', 'Ó'=>'O', 'Ö'=>'O', 'Ő'=>'O', 'Ú'=>'U', 'Ü'=>'U', 'Ű'=>'U',
        'ñ'=>'n', 'ç'=>'c'
    ];
    return strtr($string, $accents);
}


// kapcsolat oldal

function sendContactFormEmail($data, $conn){
    
    $data = json_decode($data, true);
    
    $name = isset($data['name']) ? htmlspecialchars($data['name']) : null;
    $email = isset($data['email']) ? htmlspecialchars($data['email']) : null;
    $phonenumber = isset($data['phonenumber']) ? htmlspecialchars($data['phonenumber']) : null;
    $subject = isset($data['subject']) ? htmlspecialchars($data['subject']) : null;
    $message = isset($data['message']) ? htmlspecialchars($data['message']) : null;

    $errorFields = [];
    if ($name === null || strlen(trim($name)) == 0) {
        $errorFields[] = "name";
    }
    if($email === null || strlen(trim($email)) == 0){
        $errorFields[] = "email";
    }
    if($phonenumber === null || strlen(trim($phonenumber)) == 0) {
        $errorFields[] = "phone";
    }
    if($subject === null || strlen(trim($subject)) == 0) {
        $errorFields[] = "subject";
    }
    if($message === null || strlen(trim($message)) == 0) {
        $errorFields[] = "message";
    }

    if(count($errorFields) != 0){
        Response::error(400, "A pirossal jelölt mező(k) kitöltése kötelező!", ['errorFields' => $errorFields]);
    }

    // adatok validálása
    ValidateName($name);
    ValidateEmail($email);
    ValidatePhonenumber($phonenumber);


    //admin emailek begyűjtése
    $stmt = $conn->prepare("SELECT `email` FROM `users` INNER JOIN `roles` ON `users`.`role_id` = `roles`.`id` WHERE JSON_EXTRACT(`tags`, '$.role_identifier') = ? AND `users`.`deleted` = ?");
    $stmt->execute(['admin', false]);

    $admin_emails = array();
    while($row = $stmt->fetch(PDO::FETCH_ASSOC)){
        $admin_emails[] = $row['email'];
    }

    // email kiküldése a feladónak
    ContactFormSenderEmail($email, $name, $phonenumber, $subject, $message);

    //emailek kiküldése adminoknak
    foreach($admin_emails as $admin_email){
        ContactFormRecieverEmail($admin_email, $email, $name, $phonenumber, $subject, $message);
    }
    Response::success([], "Üzenetét megkaptuk!");
}


// Emailezéssel kapcsolatos funkciók
function ContactFormSenderEmail($email, $name, $phonenumber, $subject, $message){ // kapcsolat oldal küldőnek menő üzenet

    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents('./email_templates/contactFormSender.html');
    $template = str_replace('{{name}}', $name, $template);
    $template = str_replace('{{phonenumber}}', $phonenumber, $template);
    $template = str_replace('{{subject}}', $subject, $template);
    $template = str_replace('{{message}}', $message, $template);


    $resp = Mailer::sendMail($email, "Üzenetét megkaptuk!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function ContactFormRecieverEmail($admin_email, $email, $name, $phonenumber, $subject, $message){ // admin felhasználók értesítése kapcsolat oldali üzenetről

    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/contactFormReciever.html');
    $template = str_replace('{{name}}', $name, $template);
    $template = str_replace('{{email}}', $email, $template);
    $template = str_replace('{{phonenumber}}', $phonenumber, $template);
    $template = str_replace('{{subject}}', $subject, $template);
    $template = str_replace('{{message}}', $message, $template);


    $resp = Mailer::sendMail($admin_email, "Új üzenet érkezett!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function UpdatePasswordEmail($email){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/updatePassword.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/profil", $template);


    $resp = Mailer::sendMail($email, "Sikeres jelszóváltoztatás!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function UpdateUserEmail($email){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/updateUser.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/profil", $template);


    $resp = Mailer::sendMail($email, "Sikeres felhasználó módosítás!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}


function newReservationEmail($email, $id, $date, $location, $duration, $service){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/newReservation.html');
    $template = str_replace('{{date}}', $date, $template);
    $template = str_replace('{{location}}', $location, $template);
    $template = str_replace('{{duration}}', $duration, $template);
    $template = str_replace('{{service}}', $service, $template);
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/foglalasok/" . $id, $template);

    // Example reservation details
    $event_title = $service;
    $event_description = "";
    $event_location = $location;
    $start_time = $date; // YYYY-MM-DD HH:MM:SS format
    $end_time = date_add(date_create($date), date_interval_create_from_date_string($duration . " minute"))->format("Y-m-d H:i");

    // Create ICS event
    $ics = new ICSGenerator($event_title, $event_description, $event_location, $start_time, $end_time);
    $ics_content = $ics->generateICS();

    // Save it as a file
    $filename = "reservation_" . time() . ".ics";
    file_put_contents($filename, $ics_content);

    $resp = Mailer::sendMail($email, "Sikeres időpontfoglalás!", $template, $filename);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function CancelReservationEmail($email, $date, $location, $service){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/cancelReservation.html');
    $template = str_replace('{{date}}', $date, $template);
    $template = str_replace('{{location}}', $location, $template);
    $template = str_replace('{{service}}', $service, $template);
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/szolgaltatasok", $template);


    $resp = Mailer::sendMail($email, "Az időpont lemondásra került!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function NewRegistrationEmail($email, $token){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/newRegistration.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/email-megerositese?data=" . $token["data"] . "&signiture=" . $token['signiture'], $template);


    $resp = Mailer::sendMail($email, "Erősítse meg email címét!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function EmailConfirmedEmail($email){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/confirmedEmail.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/szolgaltatasok", $template);


    $resp = Mailer::sendMail($email, "Sikeres megerősítés!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function ForgotPasswordEmail($email, $token){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/forgotPassword.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/elfelejtett-jelszo?data=" . $token["data"] . "&signiture=" . $token['signiture'], $template);


    $resp = Mailer::sendMail($email, "Elfelejtett jelszó helyreállítása!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function RegisterHospitalEmail($email){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/newHospital.html');
    

    $resp = Mailer::sendMail($email, "Sikeres kórház regisztráció!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function RegisterCompanyEmail($email){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/newCompany.html');
    

    $resp = Mailer::sendMail($email, "Sikeres cégregisztráció!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function AddDoctorToCompanyEmail($email) {
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/addDoctorCompany.html');
    

    $resp = Mailer::sendMail($email, "Új céghez regisztrálták!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function AddDoctorToHospitalEmail($email) {
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/addDoctorHospital.html');
    

    $resp = Mailer::sendMail($email, "Új kórházhoz regisztrálták!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}

function InviteUserEmail($email, $token){
    // sablon betöltése és adatok helyettesítése
    $template = file_get_contents($GLOBALS['DEFAULT_PATH'] . '/api/v1/email_templates/inviteUser.html');
    $template = str_replace('{{link}}', $GLOBALS['DEFAULT_URL'] . "/meghivo?data=" . $token["data"] . "&signiture=" . $token['signiture'], $template);


    $resp = Mailer::sendMail($email, "Meghívója érkezett!", $template);

    if($resp){
        return true;
    } else {
        return $resp;
    }
}