function openTab(tabName) {
    // oldalcímek deaktiválása
    var tabControls = document.getElementsByClassName("tab-control-btn");
    for (i = 0; i < tabControls.length; i++) {
        tabControls[i].classList.remove("tab-active");
    }
    // megfelelő oldalcím aktiválása
    document.getElementById(tabName + "-btn").classList.add("tab-active");

    // oldalak deaktiválása
    var tabs = document.getElementsByClassName("tab");
    for (i = 0; i < tabs.length; i++) {
        tabs[i].style.display = "none";
    }
    // megfelelő oldal aktiválása
    document.getElementById(tabName).style.display = "block";
}