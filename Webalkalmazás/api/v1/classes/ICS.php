<?php

class ICSGenerator {
    private $event_title;
    private $event_description;
    private $event_location;
    private $start_time;
    private $end_time;

    public function __construct($title, $description, $location, $start, $end) {
        $this->event_title = $title;
        $this->event_description = $description;
        $this->event_location = $location;
        $this->start_time = gmdate("Ymd\THis\Z", strtotime($start)); // Convert to UTC
        $this->end_time = gmdate("Ymd\THis\Z", strtotime($end));
    }

    public function generateICS() {
        $ics_content = "BEGIN:VCALENDAR\r\n";
        $ics_content .= "VERSION:2.0\r\n";
        $ics_content .= "PRODID:-//CareCompass//CareCompassReservations//HU\r\n";
        $ics_content .= "BEGIN:VEVENT\r\n";
        $ics_content .= "UID:" . uniqid() . "@carecompass.hu\r\n";
        $ics_content .= "DTSTAMP:" . gmdate("Ymd\THis\Z") . "\r\n";
        $ics_content .= "DTSTART:{$this->start_time}\r\n";
        $ics_content .= "DTEND:{$this->end_time}\r\n";
        $ics_content .= "SUMMARY:" . addslashes($this->event_title) . "\r\n";
        $ics_content .= "DESCRIPTION:" . addslashes($this->event_description) . "\r\n";
        $ics_content .= "LOCATION:" . wordwrap(addslashes($this->event_location), 75, "\r\n ", true) . "\r\n";
        $ics_content .= "END:VEVENT\r\n";
        $ics_content .= "END:VCALENDAR\r\n";

        return $ics_content;
    }
}
?>