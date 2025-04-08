<?php
//Import PHPMailer classes into the global namespace
//These must be at the top of your script, not inside a function
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

require($GLOBALS['DEFAULT_PATH'] . "/api/v1/mailer/src/Exception.php");
require($GLOBALS['DEFAULT_PATH'] . "/api/v1/mailer/src/PHPMailer.php");
require($GLOBALS['DEFAULT_PATH'] . "/api/v1/mailer/src/SMTP.php");

class Mailer{
    static function sendMail($mailTo, $subject, $body, $file = null, $mailFromName="CareCompass") {
        //Create an instance; passing `true` enables exceptions
        $mail = new PHPMailer(true);
        
        try {
            //Server settings
            // $mail->SMTPDebug = SMTP::DEBUG_SERVER;                   //Enable verbose debug output
            $mail->isSMTP();                                            //Send using SMTP
            $mail->isHTML(true);
            $mail->CharSet="UTF-8";
            $mail->Host       = $GLOBALS['email_host'];                    //Set the SMTP server to send through
            $mail->SMTPAuth   = true;                                   //Enable SMTP authentication
            $mail->Username   = $GLOBALS['email_username'];                             //SMTP username
            $mail->Password   = $GLOBALS['email_password'];                         //SMTP password
            $mail->SMTPSecure = "ssl";                                  //Enable implicit TLS encryption
            $mail->Port       = $GLOBALS['email_port'];                                    //TCP port to connect to; use 587 if you have set `SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS`
        
            //Recipients
            $mail->setFrom($GLOBALS['email_username'], $mailFromName);
            $mail->addAddress($mailTo);                                 //Add a recipient
            
            //Content
            $mail->isHTML(true);                                        //Set email format to HTML
            $mail->Subject = $subject;
            $mail->Body    = $body;
            $mail->AltBody = strip_tags($body);

            if($file !== null){
                // Attach the ICS file
                $mail->addAttachment($file);
            }
        
            $mail->send();

            if($file !== null) {
                unlink($file);                
            }
            return true;
        } catch (Exception $e) {
            return $mail->ErrorInfo;
        }
    }
}

