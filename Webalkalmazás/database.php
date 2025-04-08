<?php
class Database {
    private $host = "";
    private $database = "";
    private $username = "";
    private $password = "";
    private static $pdo;

    public function __construct($host, $database, $username, $password){
        $this->host = $host;
        $this->database = $database;
        $this->username = $username;
        $this->password = $password;
    }
    
    public function connect() {
        if (!self::$pdo) {
            try {
                self::$pdo = new PDO("mysql:host=" . $this->host . ";dbname=" . $this->database, $this->username, $this->password);
                self::$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            } catch (PDOException $e) {
                die("Database connection failed: " . $e->getMessage());
            }
        }
        return self::$pdo;
    }
}