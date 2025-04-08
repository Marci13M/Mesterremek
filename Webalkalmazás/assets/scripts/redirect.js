// automatikus átirányítás
function Redirect(url = "") {
    if (url == "") {
        const urlParams = new URLSearchParams(window.location.search);

        if (urlParams.has("redirect_url")) {
            window.location.href = urlParams.get("redirect_url");
        } else {
            location.href = BASE_URL;
        }
    } else {
        window.location.href = url;
    }
}