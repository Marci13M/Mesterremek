<?php

// válasz osztály létrehozása az egységes válaszokhoz
class Response {
    // Sikeres válasz státuszkód és token támogatással
    public static function success($data = [], $message = 'Sikeres kérés!', $meta = [], $httpCode = 200) {
        // http_response_code($httpCode); // Státuszkód beállítása
        $meta['response_timestamp'] = time(); // válasz ideje
        $response = json_encode([
            'success' => true,
            'data' => $data,
            'meta' => $meta,
            'message' => $message
        ]);
        self::send($response);
    }

    // Hibás válasz státuszkóddal és opcionális tokenkezeléssel
    public static function error($httpCode, $message, $details = []) {
        // http_response_code($httpCode); // Státuszkód beállítása
        $meta['response_timestamp'] = time(); // válasz ideje

        $response = json_encode([
            'success' => false,
            'error' => [
                'code' => $httpCode,
                'message' => $message,
                'details' => $details
            ]
        ]);
        self::send($response);
    }

    private static function send($response) {
        header('Content-Type: application/json');
        echo $response;
        exit;
    }
}