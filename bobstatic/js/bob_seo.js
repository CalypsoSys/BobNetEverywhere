function isBobLocalHost(hostname) {
    return hostname === "localhost" || hostname === "127.0.0.1";
}

function normalizeBobCanonicalPath(pathname) {
    if (!pathname || pathname === "/index.html") {
        return "/";
    }

    return pathname.endsWith("/") ? pathname.slice(0, -1) : pathname;
}

function ensureMetaElement(name, propertyName) {
    var selector = name ? 'meta[name="' + name + '"]' : 'meta[property="' + propertyName + '"]';
    var element = document.head.querySelector(selector);

    if (!element) {
        element = document.createElement("meta");

        if (name) {
            element.setAttribute("name", name);
        } else {
            element.setAttribute("property", propertyName);
        }

        document.head.appendChild(element);
    }

    return element;
}

function ensureCanonicalLink() {
    var element = document.head.querySelector('link[rel="canonical"]');

    if (!element) {
        element = document.createElement("link");
        element.setAttribute("rel", "canonical");
        document.head.appendChild(element);
    }

    return element;
}

function buildBobCanonicalUrl() {
    return "https://bobcalypso.com" + normalizeBobCanonicalPath(window.location.pathname);
}

function applyBobSeo() {
    var robotsMeta = ensureMetaElement("robots", null);

    if (isBobLocalHost(window.location.hostname)) {
        robotsMeta.setAttribute("content", "noindex, nofollow");
        return;
    }

    var canonicalUrl = buildBobCanonicalUrl();
    robotsMeta.setAttribute("content", "index, follow");
    ensureCanonicalLink().setAttribute("href", canonicalUrl);
    ensureMetaElement(null, "og:type").setAttribute("content", "website");
    ensureMetaElement(null, "og:url").setAttribute("content", canonicalUrl);
}

applyBobSeo();
