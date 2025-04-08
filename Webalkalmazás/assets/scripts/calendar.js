// // mintaadat
// const doctor_id = 2;

// fordítótábla
var servicesTranslate = new Map();

// színek
const colors = [
    "#D32F2F", // Dark Red
    "#1976D2", // Bright Blue
    "#388E3C", // Dark Green
    "#F5AA3B", // Golden Yellow
    "#8E24AA", // Deep Purple
    "#0288A3", // Teal
    "#F57C00", // Orange
    "#C2185B", // Dark Magenta
    "#FF4081" // Neon Pink
];

var selectedColor = null;
var selectedService = null;
var selectedHoliday = null;
var selectedWorktime = null;
var selectedReservation = null;


// Naptárhoz tartozó adatok deklarálása

const calendarDates = document.querySelector('.calendar-dates');
const monthYear = document.getElementById('month-year');
const prevMonthBtn = document.getElementById('prev-month');
const nextMonthBtn = document.getElementById('next-month');

let currentDate = new Date();
let currentMonth = currentDate.getMonth();
let currentYear = currentDate.getFullYear();

var selectedDate = new Date();

var from = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate() - selectedDate.getDay() + (selectedDate.getDay() == 0 ? -6 : 1));
var to = new Date(from);
to.setDate(to.getDate() + 7);

const months = [
    'január', 'február', 'március', 'április', 'május', 'június',
    'július', 'augusztus', 'szeptember', 'október', 'november', 'december'
];

function renderCalendar(month, year) {
    calendarDates.innerHTML = '';
    monthYear.textContent = `${year}. ${months[month]}`;

    const today = new Date();

    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    // Üres mezők
    for (let i = 0; i < firstDay - 1; i++) {
        const blank = document.createElement('div');
        blank.classList.add("blank");
        calendarDates.appendChild(blank);
    }

    // Napok feltöltése
    for (let i = 1; i <= daysInMonth; i++) {
        const day = document.createElement('div');
        day.textContent = i;

        // mai nap kiemelése
        if (i === today.getDate() && year === today.getFullYear() && month === today.getMonth()) {
            day.classList.add('current-date');
        }
        if (i === selectedDate.getDate() && year === selectedDate.getFullYear() && month === selectedDate.getMonth()) {
            day.classList.add('selected-date');
        }
        calendarDates.appendChild(day);
    }

    // Üres mezők
    var rest = 7 - (calendarDates.childElementCount) % 7;
    for (let i = 0; i < rest && rest != 7; i++) {
        const blank = document.createElement('div');
        blank.classList.add("blank");
        calendarDates.appendChild(blank);
    }
}

renderCalendar(currentMonth, currentYear);


// előző hónap
prevMonthBtn.addEventListener('click', () => {
    currentMonth--;
    if (currentMonth < 0) {
        currentMonth = 11;
        currentYear--;
    }

    renderCalendar(currentMonth, currentYear);
});

// következő hónap
nextMonthBtn.addEventListener('click', () => {
    currentMonth++;
    if (currentMonth > 11) {
        currentMonth = 0;
        currentYear++;
    }

    renderCalendar(currentMonth, currentYear);
});

// nap kiválasztása
calendarDates.addEventListener('mousedown', (e) => {
    if (e.target.textContent !== '' && !e.target.classList.contains("inactive")) {
        for (var i = 0; i < calendarDates.children.length; i++) {
            calendarDates.children[i].classList.remove("selected-date");
        }
        e.target.classList.add("selected-date");
        selectedDate = new Date(currentYear, currentMonth, e.target.textContent);
        from = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate() - selectedDate.getDay() + (selectedDate.getDay() == 0 ? -6 : 1));
        to = new Date(from);
        to.setDate(to.getDate() + 7);

        LoadMainCalendar();
    }
});


// fő naptár

// fejléc betöltése
const mainCalendarHeader = $("#main-calendar-header");

function LoadHeader() {
    // első nap a héten
    const firstDay = selectedDate.getDate() - selectedDate.getDay() + (selectedDate.getDay() == 0 ? -6 : 1);
    var currDate = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), firstDay);

    // napok a héten
    const daysOfWeek = ["Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat", "Vasárnap"];

    //html generálása
    var insertHTML = "<div><p><p></div>";

    for (let i = 0; i < 7; i++) {
        insertHTML += '<div class="day-header"><p>' + daysOfWeek[i] + '</p><p>' + (currDate.getDate()) + '</p></div>';
        currDate.setDate(currDate.getDate() + 1);
    }

    // html frissítés
    mainCalendarHeader.html(insertHTML);
}

// szolgáltatások betöltése
async function LoadServices() {
    servicesTranslate.clear();
    const services = $('#services');

    // alap lehetőség - szabadság
    var insertHTML = '<div class="calendar-service"><div class="service-color" style="background-color: #7c7c7c;"></div><p class="service-name">Szabadság</p><img src="' + BASE_URL + '/assets/static/plus_b.png" alt="Új időpont" class="service-add" onclick="OpenModal(\'holiday-details-modal\');"></div>';

    // szolgáltatások lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/services/GetDoctorServices?doctor_id=${doctor_id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){
        data.data.services.forEach(service => {
            if(service.service_color == null){
                insertHTML += '<div class="calendar-service"><div class="service-color" onclick="OpenChangeColor(' + service.service_id +')" style="background-image: url(' + BASE_URL + '/assets/static/no-color.png); background-size: cover; background-position: center;"></div><p class="service-name">' + service.service_name +'</p><img src="' + BASE_URL +'/assets/static/plus_b.png" alt="Új időpont" class="service-add"  onclick="OpenAddWorktime(\'' + service.service_id + '\', \'' + service.service_name + '\');"></div>';
            } else {
                insertHTML += '<div class="calendar-service"><div class="service-color" onclick="OpenChangeColor(' + service.service_id +')"  style="background-color: ' + service.service_color + ';"></div><p class="service-name">' + service.service_name +'</p><img src="' + BASE_URL +'/assets/static/plus_b.png" alt="Új időpont" class="service-add" onclick="OpenAddWorktime(\'' + service.service_id + '\', \'' + service.service_name + '\');"></div>';
            }
            servicesTranslate.set(service.service_id, service.service_color);
        });
    } else {
        AddMessage("error", "Hiba történt a lekérdezéskor");
    }

    // html frissítése
    services.html(insertHTML);
}


// orvos adatinak betöltése
async function LoadDoctorInfo(){
    const doctorImage = $('#doctor-image');
    const doctorName = $('#doctor-name');

    // orvos adatainak lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/doctors/GetDoctor?doctor_id=${doctor_id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){
        doctorName.text(data.data.doctor.doctor_name);
        if(data.data.doctor.profile_image !== null){
            doctorImage.attr("src", data.data.doctor.profile_image.image);
        }
    } else {
        AddMessage("error", "Hiba történt a lekérdezéskor");
    }
}


// naptár teljes betöltése
async function LoadMainCalendar(){
    // fejléc betöltése
    LoadHeader();

    // tartalom ürítése
    for (let i = 0; i < 7; i++) {
        $('#day-' + (i + 1) + "-events").html("");
    }

    // tartalom betöltése
    // szabadnapok betöltése
    LoadHolidays(from, to);

    // rendelések betöltése
    LoadWorktimes(from, to);

    // lefoglalt időpontok betöltése
    LoadReservations(from, to);
}


// szabadság betöltése
async function LoadHolidays(from, to) {
    // változók
    const fromString = `${from.getFullYear().toString().padStart(2, "0")}-${(from.getMonth() + 1).toString().padStart(2, "0")}-${from.getDate().toString().padStart(2, "0")}`;
    const toString = `${to.getFullYear().toString().padStart(2, "0")}-${(to.getMonth() + 1).toString().padStart(2, "0")}-${to.getDate().toString().padStart(2, "0")}`;

    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetHolidays?doctor_id=${doctor_id}&from=${fromString}&to=${toString}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){ // siker esetén betöltés
        data.data.holidays.forEach(holiday => {
            // változók létrehozása
            var holidayStart = new Date(holiday.start_date + "T00:00:00");
            var holidayEnd = new Date(holiday.end_date + "T00:00:00");

            var checkDate = new Date(from);

            if(holidayStart < checkDate){
                holidayStart = new Date(checkDate);
            }
            for(let i = 0; i < 7; i++){
                if(checkDate.getTime() == holidayStart.getTime()){
                    $('#day-' + (i + 1) + "-events").append("<div class='holiday' onclick='OpenDeleteHoliday(" + holiday.id + ")'></div>"); // szabadságolt nap hozzáadása
                    if(holidayStart < holidayEnd){
                        holidayStart.setDate(holidayStart.getDate() + 1);
                    }
                }
                checkDate.setDate(checkDate.getDate() + 1);
            }
        });
    } else {
        AddMessage("error", "Hiba történt a lekérdezéskor");
    }
}

// munkaidő betöltése
async function LoadWorktimes(from, to){
    // változók
    const fromString = `${from.getFullYear().toString().padStart(2, "0")}-${(from.getMonth() + 1).toString().padStart(2, "0")}-${from.getDate().toString().padStart(2, "0")}`;
    const toString = `${to.getFullYear().toString().padStart(2, "0")}-${(to.getMonth() + 1).toString().padStart(2, "0")}-${to.getDate().toString().padStart(2, "0")}`;

    // adatok lekérezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetWorktimes?doctor_id=${doctor_id}&from=${fromString}&to=${toString}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){
        data.data.worktimes.forEach(worktime => {
            // változók létrehozása
            var worktimeDate = new Date(worktime.date + "T00:00:00");
            var worktimeStart = new Date(worktime.date + "T" + worktime.from_time);
            var worktimeEnd = new Date(worktime.date + "T" + worktime.to_time);
            let hours = (worktimeEnd.getTime() - worktimeStart.getTime()) / (1000 * 60 * 60);
            
            var checkDate = new Date(from);

            var hourHeight = getComputedStyle(document.querySelector(".main-calendar-body")).getPropertyValue("--hour-height").trim().replace("px", ""); //css magasság lekérdezése

            for(let i = 0; i < 7; i++){
                if(checkDate.getTime() == worktimeDate.getTime()){
                    var startHour = (worktimeStart.getTime() - worktimeDate.getTime()) / (1000 * 60 * 60) + 0.5; // pozició kiszámítása
                    if(servicesTranslate.get(worktime.service_id) != null){
                        $('#day-' + (i + 1) + "-events").append(`<div class='service' onclick='OpenDeleteWorktime(${worktime.id})' style='height: ${hours * hourHeight}px; top: ${startHour * hourHeight}px; background-color: ${servicesTranslate.get(worktime.service_id)};'></div>`); // munkaidő beillesztése
                    } else {
                        $('#day-' + (i + 1) + "-events").append(`<div class='service outline' onclick='OpenDeleteWorktime(${worktime.id})' style='height: ${hours * hourHeight}px; top: ${startHour * hourHeight}px; background-color: ${servicesTranslate.get(worktime.service_id)};'></div>`); // munkaidő beillesztése
                    }
                }
                checkDate.setDate(checkDate.getDate() + 1);
            }
        });
    } else {
        AddMessage("error", "Hiba történt a lekérdezéskor");
    }
}


// Foglalt időpontok betöltése
async function LoadReservations(from, to){
    // változók
    const fromString = `${from.getFullYear().toString().padStart(2, "0")}-${(from.getMonth() + 1).toString().padStart(2, "0")}-${from.getDate().toString().padStart(2, "0")}`;
    const toString = `${to.getFullYear().toString().padStart(2, "0")}-${(to.getMonth() + 1).toString().padStart(2, "0")}-${to.getDate().toString().padStart(2, "0")}`;

    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetReservedTimes?doctor_id=${doctor_id}&from=${fromString}&to=${toString}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){ // siker esetén adatok betöltése
        data.data.reservations.forEach(reservation => {
            // változók létrehozása
            var reservationDate = new Date(reservation.datetime.split(' ')[0] + "T00:00:00");
            var reservationStart = new Date(reservation.datetime.split(' ')[0] + "T" + reservation.datetime.split(' ')[1]);
            var reservationEnd = new Date(reservationStart);
            reservationEnd.setMinutes(reservationEnd.getMinutes() + reservation.duration);
            let hours = (reservationEnd.getTime() - reservationStart.getTime()) / (1000 * 60 * 60);
            
            var checkDate = new Date(from);

            var hourHeight = getComputedStyle(document.querySelector(".main-calendar-body")).getPropertyValue("--hour-height").trim().replace("px", ""); // css magasság lekérdezése

            for(let i = 0; i < 7; i++){
                if(checkDate.getTime() == reservationDate.getTime()){
                    var startHour = (reservationStart.getTime() - reservationDate.getTime()) / (1000 * 60 * 60) + 0.5;
                    if(reservation.fulfilled){
                        $('#day-' + (i + 1) + "-events").append(`<div class='event fulfilled' onclick="OpenReservation(${reservation.id})" style='height: ${hours * hourHeight}px; top: ${startHour * hourHeight}px;'><p class='title'>${reservation.name}</p><p class='description'>${reservationStart.getHours().toString().padStart(2, "0") + ':' + reservationStart.getMinutes().toString().padStart(2, "0") + ' - ' + reservationEnd.getHours().toString().padStart(2, "0") + ':' + reservationEnd.getMinutes().toString().padStart(2, "0")}</p></div>`); // lefoglalt időpont betöltése
                    } else {
                        $('#day-' + (i + 1) + "-events").append(`<div class='event' onclick="OpenReservation(${reservation.id})" style='height: ${hours * hourHeight}px; top: ${startHour * hourHeight}px;'><p class='title'>${reservation.name}</p><p class='description'>${reservationStart.getHours().toString().padStart(2, "0") + ':' + reservationStart.getMinutes().toString().padStart(2, "0") + ' - ' + reservationEnd.getHours().toString().padStart(2, "0") + ':' + reservationEnd.getMinutes().toString().padStart(2, "0")}</p></div>`); // lefoglalt időpont betöltése
                    }
                    var c = Array.from($('#day-' + (i + 1) + "-events").children().last().children());
                    var totalHeight = 0;
                    c.forEach(element => {
                        totalHeight += element.offsetHeight;
                    });
                    if(totalHeight > (hours *hourHeight)){
                        $('#day-' + (i + 1) + "-events").children().last().children().remove();
                    }
                }
                checkDate.setDate(checkDate.getDate() + 1);
            }
        });
    } else {
        AddMessage("error", "Hiba történt a lekérdezéskor");
    }
}


// színválasztó megnyitása
function OpenChangeColor(id){
    selectedService = id;
    OpenModal("change-service-color-modal");
    // adatok betöltése
    const currentColor = $("#current-color");
    if(servicesTranslate.get(id) == null){
        // üres kép betöltése
        currentColor.replaceWith('<div class="current-color" id="current-color" style="background-image: url(' + BASE_URL + '/assets/static/no-color.png); background-size: cover; background-position: center;"></div>');
    } else {
        // szín beállítása
        currentColor.replaceWith('<div class="current-color" id="current-color" style="background-color: ' + servicesTranslate.get(id) + '"></div>');
    }
    selectedColor = servicesTranslate.get(id);

    // összes szín betöltése
    const colorsContainer = $("#colors-container");
    colorsContainer.html("");
    colors.forEach(color => {
        if(color != selectedColor){
            colorsContainer.append("<div class='color' id='" + color + "' style='background-color: " + color + "' onclick='SelectColor(this)' data-color='" + color + "'></div>");
        } else {
            colorsContainer.append("<div class='color selected' id='" + color + "' style='background-color: " + color + "' onclick='SelectColor(this)' data-color='" + color + "'></div>");
        }
    });
}

// szín kiválasztása
function SelectColor(element){
    if(selectedColor != null && colors.includes(selectedColor)){
        // régi kijelölés törlése
        document.getElementById(selectedColor).classList.remove("selected");
        element.classList.add("selected");
    } else {
        element.classList.add("selected");
    }
    selectedColor = element.dataset.color;
}


// szín elmentése
async function SaveColor(){
    if(selectedColor !== null){

        //adatok feltöltése
        var requestURL = `${BASE_URL}/apiaccess.php`;
        var requestData = {
            "data": {
                "service_id": selectedService,
                "color": selectedColor
            },
            "url": `${API_URL}/calendar/ChangeServiceColor`, 
            "includeUserId": true
        }
        
        const response = await fetch(requestURL, {
            method: "POST",
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(requestData)
        });
        const data = await response.json();

        if(data.success){
            CloseModal("change-service-color-modal");
            AddMessage("success", "Sikeres színváltoztatás");
            await LoadServices();
            LoadMainCalendar();
        } else {
            AddMessage("error", "Hiba lépett fel a mentés közben");
        }
    } else {
        AddMessage("warning", "Válasszon színt!");
    }
}


// szabadság hozzáadása
async function AddHoliday() {
    var start = $("#holiday-start");
    var end = $("#holiday-end");
    var note = $("#holiday-note");


    //adatok feltöltése
    var requestURL = `${BASE_URL}/apiaccess.php`;
    var requestData = {
        "data": {
            "start": start.val(),
            "end": end.val(),
            "note": note.val()
        },
        "url": `${API_URL}/calendar/AddHoliday`, 
        "includeUserId": true
    }
    
    const response = await fetch(requestURL, {
        method: "POST",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(requestData)
    });
    const data = await response.json();

    if(data.success){
        CloseModal("holiday-details-modal");

        // adatok visszaállítása
        const today = new Date();
        today.setDate(today.getDate() + 1);
        const todayString = `${today.getFullYear().toString().padStart(2, "0")}-${(today.getMonth() + 1).toString().padStart(2, "0")}-${today.getDate().toString().padStart(2, "0")}`;
        start.val(todayString);
        end.val(todayString);
        note.val("");

        AddMessage("success", "Sikeres szabadságolás");
        LoadMainCalendar();
    } else {
        AddMessage("error", data.error.message);
    }
}

//munkamodal megnyitása
async function OpenAddWorktime(id, name){
    selectedService = id;
    
    // szolgáltatás nevének betöltése
    $("#selected-service-name").text(name);

    // kórházak betöltése
    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetHospitals?doctor_id=${doctor_id}&service_id=${id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    const data = await response.json();

    if(data.success){
        $("#selected-hospital").html("");
        if(data.data.hospitals.length == 0){
            $("#selected-hospital").append("<option disabled selected>Nincs kórház</option>")
        } else {
            data.data.hospitals.forEach(hospital => {
                $("#selected-hospital").append("<option value='" + hospital.id+ "'>" + hospital.name + "</option>");
            });
        }
    } else {
        AddMessage("error", "Sikertelen lekérdezés");
    }

    // felugró ablak megnyitása
    OpenModal("service-details-modal");
}


// munkanap hozzáadása
async function AddWorkday() {
    var hospital = $("#selected-hospital");
    var date = $("#selected-date");
    var from = $("#selected-date-start");
    var to = $("#selected-date-end");


    //adatok feltöltése
    var requestURL = `${BASE_URL}/apiaccess.php`;
    var requestData = {
        "data": {
            "service_id": selectedService,
            "hospital_id": hospital.val(),
            "date": date.val(),
            "startTime": from.val(),
            "endTime": to.val()
        },
        "url": `${API_URL}/calendar/AddWorkday`, 
        "includeUserId": true
    }
    
    const response = await fetch(requestURL, {
        method: "POST",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(requestData)
    });
    const data = await response.json();

    if(data.success){
        CloseModal("service-details-modal");

        // adatok visszaállítása
        const today = new Date();
        today.setDate(today.getDate() + 1);
        const todayString = `${today.getFullYear().toString().padStart(2, "0")}-${(today.getMonth() + 1).toString().padStart(2, "0")}-${today.getDate().toString().padStart(2, "0")}`;
        date.val(todayString);
        from.val("08:00");
        to.val("16:00");

        AddMessage("success", "Sikeres rögzítés");
        LoadMainCalendar();
    } else {
        AddMessage("error", data.error.message);
    }
}


// szabadság áttekintése
async function OpenDeleteHoliday(holiday_id){
    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetHolidayInfo?doctor_id=${doctor_id}&holiday_id=${holiday_id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    
    const data = await response.json();
    
    if(data.success){
        // felugró ablak betöltése
        selectedHoliday = holiday_id;
        $("#holiday-start-info").text(data.data.holiday.start_date);
        $("#holiday-end-info").text(data.data.holiday.end_date);
        $("#holiday-note-info").text(data.data.holiday.note);

        OpenModal("delete-holiday-modal");
    } else {
        AddMessage("error", data.error.message);
    }
}


// szabadság törlése
async function DeleteHoliday() {
    //adatok feltöltése
    var requestURL = `${BASE_URL}/apiaccess.php`;
    var requestData = {
        "data": {
            "holiday_id": selectedHoliday
        },
        "url": `${API_URL}/calendar/DeleteHoliday`, 
        "includeUserId": true
    }
    
    const response = await fetch(requestURL, {
        method: "DELETE",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(requestData)
    });
    const data = await response.json();

    if(data.success){
        CloseModal("delete-holiday-modal");

        AddMessage("success", "Szabadság törölve");
        LoadMainCalendar();
    } else {
        AddMessage("error", data.error.message);
    }
}



// munkaidő áttekintése
async function OpenDeleteWorktime(worktime_id){
    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/calendar/GetWorktimeInfo?doctor_id=${doctor_id}&worktime_id=${worktime_id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    
    const data = await response.json();
    
    if(data.success){
        // felugró ablak betöltése
        selectedWorktime = worktime_id;
        $("#service-name").text(data.data.worktime.service_name);
        $("#service-hospital").text(data.data.worktime.hospital_name);
        $("#service-date").text(data.data.worktime.date);
        $("#service-from").text(data.data.worktime.from);
        $("#service-to").text(data.data.worktime.to);

        OpenModal("delete-service-modal");
    } else {
        AddMessage("error", data.error.message);
    }
}


// munkaidő törlése
async function DeleteWorkday() {
    //adatok feltöltése
    var requestURL = `${BASE_URL}/apiaccess.php`;
    var requestData = {
        "data": {
            "worktime_id": selectedWorktime
        },
        "url": `${API_URL}/calendar/DeleteWorkday`, 
        "includeUserId": true
    }
    
    const response = await fetch(requestURL, {
        method: "DELETE",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(requestData)
    });
    const data = await response.json();

    if(data.success){
        CloseModal("delete-service-modal");

        AddMessage("success", "Munkaidő törölve");
        LoadMainCalendar();
    } else {
        AddMessage("error", data.error.message);
    }
}


// időpont lekérdezése
async function OpenReservation(reservation_id) {
    // adatok lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/appointments/GetReservation?reservation_id=${reservation_id}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    
    const data = await response.json();
    
    if(data.success){
        // felugró ablak betöltése
        selectedReservation = reservation_id;
        var from = new Date(data.data.reservation.reservation_datetime);
        var to = new Date(from.getTime() + data.data.reservation.duration * 60000);
        var date = from.toISOString().split('T')[0];

        $("#reservation-username").text(data.data.reservation.user_name);
        $("#reservation-name").text(data.data.reservation.service_name);
        $("#reservation-date").text(date);
        $("#reservation-from").text(from.toTimeString().slice(0, 5));
        $("#reservation-to").text(to.toTimeString().slice(0, 5));
        $("#reservation-location").text(data.data.reservation.hospital_name);

        OpenModal("reservation-details-modal");
    } else {
        AddMessage("error", data.error.message);
    }
}


// időpont lezárása
async function FinishReservation() {
    //adatok feltöltése
    var requestURL = `${BASE_URL}/apiaccess.php`;
    var requestData = {
        "data": {
            "reservation_id": selectedReservation
        },
        "url": `${API_URL}/appointments/FinishReservation`, 
        "includeUserId": true
    }
    
    const response = await fetch(requestURL, {
        method: "POST",
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(requestData)
    });
    const data = await response.json();

    if(data.success){
        CloseModal("reservation-details-modal");

        AddMessage("success", "Időpont lezárva");
        LoadMainCalendar();
    } else {
        AddMessage("error", data.error.message);
    }
}


// oldal alap betöltése
async function LoadPage() {
    await LoadServices();
    LoadDoctorInfo();
    LoadMainCalendar();
}

LoadPage();

// naptári napok ne lehessenenek a múltban
// mai nap
const today = new Date();
today.setDate(today.getDate() + 1);
const todayString = `${today.getFullYear().toString().padStart(2, "0")}-${(today.getMonth() + 1).toString().padStart(2, "0")}-${today.getDate().toString().padStart(2, "0")}`;

// szabadság
$("#holiday-start").attr({
    "min": todayString
});
$("#holiday-end").attr({
    "min": todayString
});
$("#holiday-start").val(todayString);
$("#holiday-end").val(todayString);

// munkaidő
$("#selected-date").attr({
    "min": todayString
});
$("#selected-date").val(todayString);