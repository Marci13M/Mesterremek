function OpenModal(modal) {
    // felugró ablak megjelenítése
    if (!$("#" + modal).data("showing")) {
        $("#" + modal).fadeIn(150);
        $("#" + modal).data("showing", true);
        $("body").css("overflow", "hidden");
    }
}

function CloseModal(modal) {
    // felugró ablak bezárása
    if ($("#" + modal).data("showing")) {
        $("#" + modal).data("showing", false);
        $("#" + modal).fadeOut(150);
        if ($("body").data("onepage")) {
            $("body").css("overflow", "hidden");
        } else {
            $("body").css("overflow", "unset");
        }
    }
}


$("[name='open-modal-btn']").click((e) => {
    const modal = e.target.dataset.modal;
    OpenModal(modal);
});



$("[name='close-open-modal-btn']").click((e) => {
    const modal = e.target.dataset.modal;
    CloseModal(modal);
});