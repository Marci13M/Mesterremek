async function sendContactForm() { // profilszerkesztő felület betöltése
    // hibás mezők visszaállítása
    var inputs = $("#contact-form :input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].classList.remove("error");
    }


    // beírt adatok kigyűjtése
    const name = $("#name").val();

    const phone = $("#phone").val();

    const email = $("#email").val();

    const subject = $("#subject").val();

    const message = $("#message").val();

    // Disable button and show spinner
    $("#contact-form-btn").prop("disabled", true);
    $("#contact-form-btn").find("span.btn-text").addClass("hidden") // Hide text
    $("#contact-form-btn").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {

        // üzenet küldése
        var requestURL = `${BASE_URL}/apiaccess.php`;

        const response = await fetch(requestURL, {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ "url": `${API_URL}/contact`, "data": { "name": name, "email": email, "phonenumber": phone, "subject": subject, "message": message } })
        });

        const data = await response.json();

        if (data.success) {
            $("#name").val("");
            $("#phone").val("");
            $("#email").val("");
            $("#subject").val("");
            $("#message").val("");

            AddMessage("success", "Üzenetét megkaptuk!");
        } else {
            data.error.details.errorFields.forEach(field => {
                document.getElementById(field).classList.add("error");
            });
            throw Error(data.error.message);
        }
    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#contact-form-btn").prop("disabled", false);
        $("#contact-form-btn").find("span.btn-text").removeClass("hidden") // Hide text
        $("#contact-form-btn").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
}