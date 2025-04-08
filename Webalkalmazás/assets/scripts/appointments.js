// Naptárhoz tartozó adatok deklarálása

const calendarDates = document.querySelector('.calendar-dates');
const monthYear = document.getElementById('month-year');
const prevMonthBtn = document.getElementById('prev-month');
const nextMonthBtn = document.getElementById('next-month');

let currentDate = new Date();
let currentMonth = currentDate.getMonth();
let currentYear = currentDate.getFullYear();

var selectedDate = new Date();
var selectedHospitalId = 0;
var selectedDoctorId = 0;

const months = [
    'január', 'február', 'március', 'április', 'május', 'június',
    'július', 'augusztus', 'szeptember', 'október', 'november', 'december'
];

function renderCalendar(month, year, data) {
    calendarDates.innerHTML = '';
    monthYear.textContent = `${year}. ${months[month]}`;

    const today = new Date();

    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    var datesWithFreeTime = [];

    if (data.success) {
        datesWithFreeTime = Object.keys(data.data.free_times);

        if (!(datesWithFreeTime.includes(selectedDateString)) && datesWithFreeTime.length > 0) { // ha a kiválasztott nap foglalt, akkor az első szabad beállítása, amennyiben létezik
            selectedDate = new Date(Date.parse(datesWithFreeTime[0]));
        }
    }

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

            var selectedDateString = `${selectedDate.getFullYear().toString().padStart(2, "0")}-${(selectedDate.getMonth() + 1).toString().padStart(2, "0")}-${selectedDate.getDate().toString().padStart(2, "0")}`;
            if (datesWithFreeTime.includes(selectedDateString)) {
                // szabad időpontok betöltése
                const times = Object.keys(data.data.free_times).map((key) => [key, data.data.free_times[key]]).filter(function(a, b) {
                    return a[0] === selectedDateString;
                });
                document.getElementById("free-times-container").innerHTML = "";
                times[0][1].forEach(time => {
                    if (time != "reserved") {
                        document.getElementById("free-times-container").innerHTML += "<button type='button' class='free-time' onclick='OpenReservationDetailModal(" + user_id + ", " + service_id + ", " + selectedHospitalId + ", " + selectedDoctorId + ", \"" + selectedDateString + " " + time + "\")'>" + time + "</button>"
                    }
                });
            } else {
                // üzenet megjelenítése
                document.getElementById("free-times-container").innerHTML = "<h3 class='no-free-time'>Nincs szabad időpont</h3>";
            }
        }

        var dateString = `${currentYear.toString().padStart(2, "0")}-${(currentMonth + 1).toString().padStart(2, "0")}-${i.toString().padStart(2, "0")}`;

        // inaktív atribútum hozzáadása
        if (datesWithFreeTime.length == 0 || !datesWithFreeTime.includes(dateString)) {
            day.classList.add("inactive");
        }

        calendarDates.appendChild(day);
    }

    // Üres mezők
    var rest = 7 - (calendarDates.childElementCount) % 7;
    for (let i = 0; i < rest; i++) {
        const blank = document.createElement('div');
        blank.classList.add("blank");
        calendarDates.appendChild(blank);
    }

    document.getElementById("selected-datetime").innerText = `${selectedDate.getFullYear().toString().padStart(2, "0")}-${(selectedDate.getMonth() + 1).toString().padStart(2, "0")}-${selectedDate.getDate().toString().padStart(2, "0")}`;
}

async function GetFreeTimesForCalendar(year, month, hospitalId, doctorId) {
    var date = new Date();
    var fromDate = new Date(year, month, 1);
    if (date > fromDate) {
        fromDate = date;
    }
    var fromDateString = `${fromDate.getFullYear().toString().padStart(2, "0")}-${(fromDate.getMonth() + 1).toString().padStart(2, "0")}-${fromDate.getDate().toString().padStart(2, "0")}`;
    var toDate = new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, 0);
    var toDateString = `${toDate.getFullYear().toString().padStart(2, "0")}-${(toDate.getMonth() + 1).toString().padStart(2, "0")}-${toDate.getDate().toString().padStart(2, "0")}`;
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/appointments/GetFreeAppointmentTimes?service_id=${service_id}&hospital_id=${hospitalId}&doctor_id=${doctorId}&from=${fromDateString}&to=${toDateString}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    return await response.json();
}

async function GetFreeTimesForDay(year, month, day, hospitalId, doctorId) {
    var date = new Date(year, month, day);
    var dateString = `${date.getFullYear().toString().padStart(2, "0")}-${(date.getMonth() + 1).toString().padStart(2, "0")}-${date.getDate().toString().padStart(2, "0")}`;
    
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
        `${API_URL}/appointments/GetFreeAppointmentTimes?service_id=${service_id}&hospital_id=${hospitalId}&doctor_id=${doctorId}&from=${dateString}&to=${dateString}`
    )}`;
    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
    return await response.json();
}

// előző hónap
prevMonthBtn.addEventListener('click', () => {
    currentMonth--;
    if (currentMonth < 0) {
        currentMonth = 11;
        currentYear--;
    }
    GetFreeTimesForCalendar(currentYear, currentMonth, selectedHospitalId, selectedDoctorId).then((response) => {
        renderCalendar(currentMonth, currentYear, response);

        // felugró ablak megjelenítése
        OpenModal("more-options-modal");
    });
});

// következő hónap
nextMonthBtn.addEventListener('click', () => {
    currentMonth++;
    if (currentMonth > 11) {
        currentMonth = 0;
        currentYear++;
    }
    GetFreeTimesForCalendar(currentYear, currentMonth, selectedHospitalId, selectedDoctorId).then((response) => {
        renderCalendar(currentMonth, currentYear, response);

        // felugró ablak megjelenítése
        OpenModal("more-options-modal");
    });
});

// nap kiválasztása
calendarDates.addEventListener('mousedown', (e) => {
    if (e.target.textContent !== '' && !e.target.classList.contains("inactive")) {
        for (var i = 0; i < calendarDates.children.length; i++) {
            calendarDates.children[i].classList.remove("selected-date");
        }
        e.target.classList.add("selected-date");
        selectedDate = new Date(currentYear, currentMonth, e.target.textContent);
        document.getElementById("selected-datetime").innerText = `${selectedDate.getFullYear().toString().padStart(2, "0")}-${(selectedDate.getMonth() + 1).toString().padStart(2, "0")}-${selectedDate.getDate().toString().padStart(2, "0")}`;
        

        GetFreeTimesForDay(currentYear, currentMonth, e.target.textContent, selectedHospitalId, selectedDoctorId).then((response) => {
            var selectedDateString = `${selectedDate.getFullYear().toString().padStart(2, "0")}-${(selectedDate.getMonth() + 1).toString().padStart(2, "0")}-${selectedDate.getDate().toString().padStart(2, "0")}`;
            if (response.success && Object.keys(response.data.free_times).includes(selectedDateString)) {
                // szabad időpontok betöltése
                const times = Object.keys(response.data.free_times).map((key) => [key, response.data.free_times[key]]).filter(function(a, b) {
                    return a[0] === selectedDateString;
                });
                document.getElementById("free-times-container").innerHTML = "";
                times[0][1].forEach(time => {
                    if (time != "reserved") {
                        document.getElementById("free-times-container").innerHTML += "<button type='button' class='free-time' onclick='OpenReservationDetailModal(" + user_id + ", " + service_id + ", " + selectedHospitalId + ", " + selectedDoctorId + ", \"" + selectedDateString + " " + time + "\")'>" + time + "</button>"
                    }
                });
            } else {
                // üzenet megjelenítése
                document.getElementById("free-times-container").innerHTML = "<h3 class='no-free-time'>Nincs szabad időpont</h3>";
            }
        });
        
    }
});


async function LoadFreeTime(service_id, hospital_id, doctor_id, from_date, to_date) {
    if(hospital_id != undefined && doctor_id != undefined){
        var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
            `${API_URL}/appointments/GetFreeAppointmentTimes?service_id=${service_id}&hospital_id=${hospital_id}&doctor_id=${doctor_id}&from=${from_date}&to=${to_date}`
        )}`;
        const response = await fetch(requestURL, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const data = await response.json();

        const calendarElement = document.getElementById("calendar-container-" + service_id.toString() + hospital_id.toString() + doctor_id.toString());
        calendarElement.innerHTML = "<h3>Legkorábbi időpontok</h3>";
        
        //fejléc generálása
        // napok
        const daysWithName = [
            "vas",
            "hét",
            "kedd",
            "szer",
            "csüt",
            "pén",
            "szo"
        ];
        // hónapok
        const monthsWithName = [
            "jan.",
            "feb.",
            "mar.",
            "ápr.",
            "maj.",
            "jún.",
            "júl.",
            "aug.",
            "szep.",
            "okt.",
            "nov.",
            "dec."
        ];

        //mai nap
        const mydate = new Date();
        var mydateFormatted = `${mydate.getFullYear().toString().padStart(2, "0")}-${(mydate.getMonth() + 1).toString().padStart(2, "0")}-${mydate.getDate().toString().padStart(2, "0")}`;
        const todayDate = Date.parse(mydateFormatted); // mai nap
        const tomorrowDate = Date.parse(mydateFormatted) + 86400000; // holnapi nap

        var from = Date.parse(from_date) // kezdődátum
        var to = Date.parse(to_date); // végdátum
        
        // előző nyíl értékei
        var prevFromDateTime = new Date(from - 86400000);
        var prevToDateTime = new Date(to - 86400000);

        // előző nyíl értékeinek formázása
        var prevFromDate = `${prevFromDateTime.getFullYear().toString().padStart(2, "0")}-${(prevFromDateTime.getMonth() + 1).toString().padStart(2, "0")}-${prevFromDateTime.getDate().toString().padStart(2, "0")}`;
        var prevToDate = `${prevToDateTime.getFullYear().toString().padStart(2, "0")}-${(prevToDateTime.getMonth() + 1).toString().padStart(2, "0")}-${prevToDateTime.getDate().toString().padStart(2, "0")}`;


        // következő nyíl értékei
        var nextFromDateTime = new Date(from + 86400000);
        var nextToDateTime = new Date(to + 86400000);

        // következő nyíl értékeinek formázása
        var nextFromDate = `${nextFromDateTime.getFullYear().toString().padStart(2, "0")}-${(nextFromDateTime.getMonth() + 1).toString().padStart(2, "0")}-${nextFromDateTime.getDate().toString().padStart(2, "0")}`;
        var nextToDate = `${nextToDateTime.getFullYear().toString().padStart(2, "0")}-${(nextToDateTime.getMonth() + 1).toString().padStart(2, "0")}-${nextToDateTime.getDate().toString().padStart(2, "0")}`;

        
        //egymást követő napok generálása
        var days = []; //napok
        for(let i = 0; i < 5; i++){
            const date = new Date(from);
            date.setDate(date.getDate() + i);
            days.push(date.toISOString().split('T')[0]);
        }

        
        var output; // beillesztendő tartalom

        // nyíl letiltása, hogy múltba ne lehessen menni
        if(mydate > from){
            output = '<div class="day-controls"><img src="' + BASE_URL + '/assets/static/left_arrow_g.png" alt="bal" class="day-control disabled"><div class="days-container" id="days-container">';
        } else {
            output = '<div class="day-controls"><img src="'+ BASE_URL + '/assets/static/left_arrow_b.png" alt="bal" class="day-control" onclick="LoadFreeTime(' + service_id + ', ' + hospital_id + ', ' + doctor_id + ', \'' + prevFromDate + '\', \'' + prevToDate + '\')"><div class="days-container" id="days-container">';
        }

        for(var i = 0; i < 5; i++){
            const currentDateTime = from;
            var currentDate = new Date(currentDateTime);
            if(todayDate == currentDateTime){ // ma van
                output += '<div><p class="bold">ma</p><p class="bold">' + monthsWithName[currentDate.getMonth()] + " " + currentDate.getDate() + '</p></div>';
            } else if(tomorrowDate == currentDateTime){
                output += '<div><p>holnap</p><p>' + monthsWithName[currentDate.getMonth()] + " " +currentDate.getDate() + '</p></div>';
            } else {
                output += '<div><p>' + daysWithName[currentDate.getDay()] + '</p><p>' + monthsWithName[currentDate.getMonth()] + " " + currentDate.getDate() + '</p></div>';
            }
            from += 86400000;
        }

        output += '</div><img src="' + BASE_URL + '/assets/static/right_arrow_b.png" alt="jobb" class="day-control" id="next-day-control" onclick="LoadFreeTime(' + service_id + ', ' + hospital_id + ', ' + doctor_id + ', \'' + nextFromDate + '\', \'' + nextToDate + '\')"></div>';

        
        calendarElement.innerHTML += output; // fejléc beillesztése
        
        if(data.success){ // van szabad időpont és a kérés sikeres volt
            const freeTimes = data.data.free_times;

            // szabad időpontok feltöltése 3 elemre, ha nincs annyi és napok összerendelése
            const freeTimesList = days.map(day => {
                const times = freeTimes[day] || [];
                while(times[0] == "reserved" && times.length > 0){ // a nap elejéről kiszedi azokat, amik már lefoglalt időpontok
                    times.shift();
                }
                while(times.length < 3){
                    times.push("-");
                }
                return {
                    date: day,
                    freeTimes: times
                }
            });


            output = '<div class="day-times" id="day-times">';
            freeTimesList.forEach(day => {
                output += '<div class="day">';
                for(var i = 0; i < 3; i++){
                    if(day.freeTimes[i] == "reserved" || day.freeTimes[i] == "-"){
                        output += '<div class="day-time"></div>';
                    } else {
                        output += '<div class="day-time"><p class="datetime" onclick="OpenReservationDetailModal(' + user_id + ', ' + service_id + ', ' + hospital_id + ', ' + doctor_id + ', \'' + day.date + ' ' + day.freeTimes[i] + '\')">' + day.freeTimes[i] + '</p></div>';
                    }
                }
                output += '</div>';
            });
            output += "</div>";
        } else { // nincs szabad időpont vagy nem sikeres a lekérdezés
            output = '<div class="day-times" id="day-times">';
            for(var i = 0; i < 5; i++){
                output += '<div class="day">';
                for(var j = 0; j < 3; j++){
                    output += '<div class="day-time"></div>';
                }
                output += '</div>'
            }
            output += "</div>";
        }
        output += '<div class="other-options"><button class="text-like" onclick="OpenMoreOptionsModal(' + hospital_id + ', ' + doctor_id + ')">Továbbiak</button></div>'
        calendarElement.innerHTML += output; // tartalom beillesztése
    }
}


function LoadAllFreeTimes() {
    const options = Array.from(document.getElementById("service-options").children);

    for (var element of options) {
       LoadFreeTime(service_id, element.dataset.hospital_id, element.dataset.doctor_id, element.dataset.from_date, element.dataset.to_date);
    };
}

function OpenMoreOptionsModal(hospitalId, doctorId) {

    // naptár betöltése
    GetFreeTimesForCalendar(currentYear, currentMonth, hospitalId, doctorId).then((response) => {
        selectedHospitalId = hospitalId;
        selectedDoctorId = doctorId;
        renderCalendar(currentMonth, currentYear, response);
        // felugró ablak megjelenítése
        OpenModal("more-options-modal");
    });
}


async function OpenReservationDetailModal(userId, serviceId, hospitalId, doctorId, datetime){
    if(userId != null){
        //adatok lekérdezése a felugró ablakhoz
        var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
            `${API_URL}/appointments/GetAppointmentTimeDetails?service_id=${serviceId}&hospital_id=${hospitalId}&doctor_id=${doctorId}&user_id=${userId}`
        )}`;
        const response = await fetch(requestURL, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const data = await response.json();

        if(data.success){
            // adatok felöltése a felugró ablakba
            $("#reservation-name").text(data.data.user.name);
            $("#reservation-email").text(data.data.user.email);
            $("#reservation-phonenumber").text(data.data.user.phone);

            $("#reservation-service-name").text(data.data.service.name);
            $("#reservation-datetime").text(datetime);
            $("#reservation-doctor-name").text(data.data.doctor.name);
            $("#reservation-location").text(data.data.service.address);
            $("#reservation-service-price").text(data.data.service.price + " Ft");

            // gomb kattintás eseményének feltöltése
            document.getElementById("reserve-appointment").onclick = function () {ReserveAppointment(hospitalId, serviceId, doctorId, datetime)};

            // ha meg van nyitva a másik ablak, akkor bezárja
            CloseModal("more-options-modal")
            // felugró ablak megjelenítése
            OpenModal("reservation-details-modal");
        } else {
            AddMessage("error", data.error.message); 
        }
    } else { // átirányítjuk a bejelentkezés oldalra
        //elmentjük sessionStorageba az aktuális állást, hogy innen folytathassa és url-be megadjuk a visszavezető url-t
        sessionStorage.setItem("reservationData", JSON.stringify({"hospitalId" : hospitalId, "doctorId" : doctorId, "datetime" : datetime}));

        //átirányítás
        window.location.href = BASE_URL + "/bejelentkezes?redirect_url=" + encodeURIComponent(window.location.href);
    }    
}

async function ReserveAppointment(hospitalId, serviceId, doctorId, datetime, type = "user"){
    //adatok lekérdezése a felugró ablakhoz
    var date = new Date(datetime);
    
    // Disable button and show spinner
    $("#reserve-appointment").prop("disabled", true);
    $("#reserve-appointment").find("span.btn-text").addClass("hidden") // Hide text
    $("#reserve-appointment").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        var requestURL = `${BASE_URL}/apiaccess.php`;
        var requestData = {
            "data": {
                "hospital_id": hospitalId,
                "service_id": serviceId,
                "doctor_id": doctorId,
                "date": date.toISOString().split('T')[0],
                "time": `${date.getHours()}:${date.getMinutes()}`,
                "type": type
            },
            "url": `${API_URL}/appointments/NewReservation`, 
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
            LoadFreeTime(serviceId, hospitalId, doctorId, $("#service-option-" + serviceId.toString() + hospitalId.toString() + doctorId.toString()).data("from_date"), $("#service-option-" + serviceId.toString() + hospitalId.toString() + doctorId.toString()).data("to_date"));
            
            AddMessage("success", data.message);
        } else {
            LoadFreeTime(serviceId, hospitalId, doctorId, $("#service-option-" + serviceId.toString() + hospitalId.toString() + doctorId.toString()).data("from_date"), $("#service-option-" + serviceId.toString() + hospitalId.toString() + doctorId.toString()).data("to_date"));
            
            throw Error(data.error.message);
        }
        CloseModal("reservation-details-modal");
    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#reserve-appointment").prop("disabled", false);
        $("#reserve-appointment").find("span.btn-text").removeClass("hidden") // Hide text
        $("#reserve-appointment").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
}



if(sessionStorage.getItem("reservationData") != null){
    const json_reservation_data = JSON.parse(sessionStorage.getItem("reservationData"));
    OpenReservationDetailModal(user_id, service_id, json_reservation_data.hospitalId, json_reservation_data.doctorId, json_reservation_data.datetime);
    sessionStorage.removeItem("reservationData");
}

if(sessionStorage.getItem("moreOptions") != null){
    const json_reservation_data = JSON.parse(sessionStorage.getItem("moreOptions"));
    OpenMoreOptionsModal(user_id, json_reservation_data.hospitalId, json_reservation_data.doctorId);
    sessionStorage.removeItem("moreOptions");
}

LoadAllFreeTimes();