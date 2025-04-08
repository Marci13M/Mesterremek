function OpenPasswordChangeModal() {
    // mezők tartalmának törlése
    $("#old-password").val("");
    $("#new-password").val("");
    $("#new-password-again").val("");

    OpenModal("change-password-modal");
}

async function ChangePassword() {
    const oldPassword = $("#old-password").val();
    const newPassword = $("#new-password").val();
    const newPasswordAgain = $("#new-password-again").val();

    // hibák törlése
    var inputs = $("#password-change-inputs > :input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].classList.remove("error");
    }

    // jelszavak alap ellenőrzése
    if (newPassword != newPasswordAgain) {
        AddMessage("warning", "Az új jelszavak nem egyeznek!");
        document.getElementById("new-password").classList.add("error");
        document.getElementById("new-password-again").classList.add("error");
        return;
    }

    if (oldPassword == newPassword) {
        AddMessage("error", "Nem lehet az új jelszó a régi!");
        document.getElementById("old-password").classList.add("error");
        document.getElementById("new-password").classList.add("error");
        return;
    }

    // Disable button and show spinner
    $("#change-password").prop("disabled", true);
    $("#change-password").find("span.btn-text").addClass("hidden") // Hide text
    $("#change-password").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        // kérés küldése az API-nak
        var requestURL = `${BASE_URL}/apiaccess.php`;

        var requestData = {
            "data": {
                "user_id": userId,
                "old_password": oldPassword,
                "new_password": newPassword,
                "new_password_again": newPasswordAgain
            },
            "url": `${API_URL}/users/UpdatePassword`
        }

        const response = await fetch(requestURL, {
            method: "POST",
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(requestData)
        });

        const data = await response.json();

        if (data.success) {
            AddMessage("success", data.message);
            CloseModal("change-password-modal");
        } else {
            if ("errorFields" in data.error.details) {
                data.error.details.errorFields.forEach(field => {
                    if (field == "password") {
                        field = "new-password";
                    }
                    document.getElementById(field).classList.add("error");
                });
            }
            throw Error(data.error.message);
        }
    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#change-password").prop("disabled", false);
        $("#change-password").find("span.btn-text").removeClass("hidden") // Hide text
        $("#change-password").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
}

async function Edit() { // profilszerkesztő felület betöltése
    // beírt adatok cseréje beviteli mezőre
    const name = $("#user-name").text();
    $("#user-name-input").val(name);

    const phone = $("#user-phone").text();
    $("#user-phone-input").val(phone);

    const address = $("#user-address").text();
    $("#user-address-input").val(address);

    const birthdate = $("#user-birthdate").text();
    $("#user-birthdate-input").val(birthdate);

    const taj = $("#user-taj").text();
    $("#user-taj-input").val(taj);

    const tb = ($("#user-tb").text() == "Igen") ? true : false;
    $("#user-hasTB-input").html("<option value='true' " + ((tb) ? "selected" : "") + ">Igen</option><option value='false' " + ((tb) ? "" : "selected") + ">Nem</option>")


    // nemek lekérdezése
    var requestURL = `${BASE_URL}/apiaccess.php?url=${encodeURIComponent(
            `${API_URL}/data/general/GetGenders`)}`;

    const response = await fetch(requestURL, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });

    const data = await response.json();

    if (data.success) {
        const gender = $("#user-gender").text();
        var generatedHTML = "";
        data.data.genders.forEach(element => {
            generatedHTML += "<option value='" + element.id + "' " + ((gender == element.name) ? "selected" : "") + ">" + element.name + "</option>";
        });
        $("#user-gender-input").html(generatedHTML);
    } else {
        AddMessage("error", data.error.message);
    }

    
    var inputs = $("#update :input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].classList.remove("error");
    }

}

async function UpdateUser() {
    const name = $("#user-name-input").val();
    const phone = $("#user-phone-input").val();
    const taj = $("#user-taj-input").val();
    const address = $("#user-address-input").val();
    const gender = $("#user-gender-input").val();
    const birthdate = $("#user-birthdate-input").val();
    const hasTB = $("#user-hasTB-input").val();

    // Disable button and show spinner
    $("#update-profile").prop("disabled", true);
    $("#update-profile").find("span.btn-text").addClass("hidden") // Hide text
    $("#update-profile").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        var requestURL = `${BASE_URL}/apiaccess.php`;

        const response = await fetch(requestURL, {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ "url": `${API_URL}/users/UpdateUser`, "data": { "name": name, "phone": phone, "taj": taj, "address": address, "gender": gender, "birthdate": birthdate, "hasTB": hasTB }, "setSession": "update", "includeUserId": true })
        });

        const data = await response.json();

        if (data.success) {
            CloseModal("update-user-modal");
            AddMessage("success", data.message);

            // menüben szereplő név frissítése
            $("#navigation-username").text(name);

            // adatlap frissítése a profil oldalon
            $("#user-name").text($("#user-name-input").val());
            $("#user-phone").text($("#user-phone-input").val());
            $("#user-taj").text($("#user-taj-input").val());
            $("#user-address").text($("#user-address-input").val());
            $("#user-gender").text($("#user-gender-input").find("option:selected").text());
            $("#user-birthdate").text($("#user-birthdate-input").val());
            $("#user-tb").text(($("#user-hasTB-input").find("option:selected").text()));
        } else {
            var inputs = $("#update :input");
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].classList.remove("error");
            }

            data.error.details.errorFields.forEach(field => {
                console.log("user-" + field + "-input");
                document.getElementById("user-" + field + "-input").classList.add("error");
            });
            throw Error(data.error.message);
        }

    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#update-profile").prop("disabled", false);
        $("#update-profile").find("span.btn-text").removeClass("hidden") // Hide text
        $("#update-profile").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
}