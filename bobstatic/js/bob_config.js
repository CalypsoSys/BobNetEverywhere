function detectBobApiBaseUrl() {
    if (window.bobApiBaseUrl) {
        return window.bobApiBaseUrl;
    }

    if (window.location.hostname === "bobcalypso.com") {
        return "https://bobcalypso.com";
    }

    if (window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1") {
        if (window.location.port === "5500") {
            return "http://localhost:5080";
        }
    }

    return "";
}

window.bobApiBaseUrl = detectBobApiBaseUrl();

function bobApiUrl(path) {
    return `${window.bobApiBaseUrl}${path}`;
}
