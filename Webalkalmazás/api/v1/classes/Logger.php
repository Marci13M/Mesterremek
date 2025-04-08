<?php

// Logger osztály létrehozása a egységes és egyszerű naplóbejegyzések létrehozásához
class Logger {
    private $db;

    public function __construct($db) {
        $this->db = $db;
    }

    // sima felhasználó tevékenységnapló bejegyzése
    public function LogUserAction($userId, $action, $ip_address, $device_info, $affected_data_id, $description) {
        $stmt = $this->db->prepare("INSERT INTO user_logs (user_id, action, ip_address, device_info, affected_data_id, description) VALUES (?, ?, ?, ?, ?, ?)");
        $stmt->execute([$userId, $action, $ip_address, $device_info, $affected_data_id, json_encode($description)]);
    }

    public function getUserLogs($userId) {
        $stmt = $this->db->prepare("SELECT * FROM user_logs WHERE user_id = ?");
        $stmt->execute([$userId]);
        return $stmt->fetchAll(PDO::FETCH_ASSOC);
    }
}