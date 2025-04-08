// Menü megjelenítése
$("#phone-nav-opener").click(function() {
    if (window.screen.availWidth <= 800) {
        $(".nav-elements-container.phone").fadeToggle(150);
        if ($(".nav-container")[0].dataset.open == "true") {
            setTimeout(() => {
                $(".nav-container")[0].dataset.open = false;
            }, 150);
        } else {
            $(".nav-container")[0].dataset.open = true;
        }
    }
});


// lenyíló menü megjelenítése
$(".dropdown-opener").click(function(e) {
    if (window.screen.availWidth <= 800) {
        $("#" + e.target.attributes.id.nodeValue + "-drop").slideToggle();
    }
});

// menü visszaálítása nagyítás után
$(window).resize(function() {
    if (window.screen.availWidth > 800) {
        $(".nav-elements-container.phone").show();
        $(".nav-container")[0].dataset.open = false;
    } else {
        $(".nav-elements-container.phone").hide();
        $("[id^='drop-'][id$='-drop']").removeAttr("style");
    }
});