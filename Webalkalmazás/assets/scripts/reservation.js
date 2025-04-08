async function DeleteReservation(reservation_id) {
    // Disable button and show spinner
    $("#delete-reservation").prop("disabled", true);
    $("#delete-reservation").find("span.btn-text").addClass("hidden") // Hide text
    $("#delete-reservation").find("div.dot-spinner").removeClass("hidden"); // Show spinner

    try {
        var requestURL = `${BASE_URL}/apiaccess.php`;

        const response = await fetch(requestURL, {
            method: 'DELETE',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ "url": `${API_URL}/appointments/CancelReservation`, "data": { "reservation_id": reservation_id }, "includeUserId": true })
        });
        const data = await response.json();

        if (data.success) {
            AddMessage("success", data.message);

            Redirect("../profil");
        } else {
            throw Error(data.error.message);
        }

    } catch (error) {
        AddMessage("error", error.message);
    } finally {
        // Disable button and show spinner
        $("#delete-reservation").prop("disabled", false);
        $("#delete-reservation").find("span.btn-text").removeClass("hidden") // Hide text
        $("#delete-reservation").find("div.dot-spinner").addClass("hidden"); // Show spinner
    }
}