let searchTimeout;

document.getElementById("search").addEventListener("input", (e) => {
    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(() => {
        updateURL();
        filterResults();
    }, 800);
});

// URL frissítése a kereső és szűrők alapján
function updateURL() {
    let params = new URLSearchParams();
    let searchQuery = document.getElementById("search").value;
    if (searchQuery.trim() != "") params.set("kereses", searchQuery);

    document.querySelectorAll(".filterSetting").forEach(filter => {
        if (filter.value != "") params.set(filter.name, filter.value);
    });

    if (params.size != 0) {
        document.getElementsByClassName("filter-container")[0].classList.add("active");
    } else {
        document.getElementsByClassName("filter-container")[0].classList.remove("active");
    }

    history.pushState({}, "", "?" + params.toString());
}

// Szűrők és kereső betöltése URL-ből
function loadFiltersFromURL() {
    let params = new URLSearchParams(window.location.search);
    document.getElementById("search").value = params.get("kereses") || "";
    document.querySelectorAll(".filterSetting").forEach(filter => {
        filter.value = params.get(filter.name) || "";
    });
    filterResults();
}


function OpenFilters() {
    if (document.getElementById("filters-container").classList.contains("hidden")) {
        document.getElementById("filters-container").classList.remove("hidden");
    } else {
        document.getElementById("filters-container").classList.add("hidden");
    }
}


// Szűrők kiolvasása az űrlapból
function getFilters() {
    let filters = {};
    document.querySelectorAll(".filterSetting").forEach(filter => {
        filters[filter.name] = filter.value;
    });
    return filters;
}

loadFiltersFromURL();