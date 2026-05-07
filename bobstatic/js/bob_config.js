window.bobApiBaseUrl = window.bobApiBaseUrl || "";

function bobApiUrl(path) {
    return `${window.bobApiBaseUrl}${path}`;
}
