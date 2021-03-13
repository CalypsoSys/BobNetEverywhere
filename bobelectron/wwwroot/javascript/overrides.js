function customWindowOpen(url, target, data) {
    var epoch = Date.now();
    var x = require('electron').ipcRenderer.send("open_window", ["http://localhost:63239" + url + "?id=" + epoch, epoch, data])
}

function renderWindowData(id, callback) {
    var ipcRenderer = require('electron').ipcRenderer;
    ipcRenderer.on('send_data', function (event, store) {
        callback(store);
    });

    ipcRenderer.send("opened_window", id);
}
