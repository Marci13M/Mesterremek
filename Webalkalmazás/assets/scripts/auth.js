const container = document.getElementById('container');
const registerBtn = document.getElementById('registerBtn');
const loginBtn = document.getElementById('loginBtn');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});


var pathName = window.location.pathname.split('/');
if (pathName[pathName.length - 1] == "regisztracio") {
    container.classList.add("active");
}

// Bejelentkezés kezelése
$("#login").bind("submit", async(event) => {
    event.preventDefault();

    const email = $("#login-email").val();
    const password = $("#login-password").val();

    // Disable button and show spinner
    $("#login-btn").prop("disabled", true);
    $("#login-btn").find("span.btn-text").addClass("hidden") // Hide text
    $("#login-btn").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        var requestURL = `${BASE_URL}/apiaccess.php`;

        const response = await fetch(requestURL, {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ "url": `${API_URL}/auth/login`, "data": { "email": email, "password": password }, "setSession": "login" })
        });
        const data = await response.json();

        if (data.success) {
            AddMessage("success", data.message);

            Redirect();
        } else {
            throw Error(data.error.message);
        }

    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Enable button and hide spinner
        $("#login-btn").prop("disabled", false);
        $("#login-btn").find("span.btn-text").removeClass("hidden"); // Hide text
        $("#login-btn").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
});

$("#registration").bind("submit", async(event) => {
    event.preventDefault();

    const name = $("#registration-name").val();
    const email = $("#registration-email").val();
    const password = $("#registration-password").val();
    const phone = $("#registration-phone").val();
    const taj = $("#registration-taj").val();
    const address = $("#registration-address").val();
    const gender = $("#registration-gender").val();
    const birthdate = $("#registration-birthdate").val();
    const hasTB = $("#registration-hasTB").val();

    // Disable button and show spinner
    $("#registration-btn").prop("disabled", true);
    $("#registration-btn").find("span.btn-text").addClass("hidden") // Hide text
    $("#registration-btn").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        var requestURL = `${BASE_URL}/apiaccess.php`;

        const response = await fetch(requestURL, {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ "url": `${API_URL}/auth/register`, "data": { "name": name, "email": email, "password": password, "phone": phone, "taj": taj, "address": address, "gender": gender, "birthdate": birthdate, "hasTB": hasTB }, "setSession": "register" })
        });

        const data = await response.json();

        if (data.success) {
            AddMessage("success", data.message);

            Redirect();
        } else {
            var inputs = $("#registration > div > :input");
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].classList.remove("error");
            }

            if ("errorFields" in data.error.details) {
                data.error.details.errorFields.forEach(field => {
                    document.getElementById("registration-" + field).classList.add("error");
                });
            }
            throw Error(data.error.message);
        }

    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#registration-btn").prop("disabled", false);
        $("#registration-btn").find("span.btn-text").removeClass("hidden") // Hide text
        $("#registration-btn").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
});