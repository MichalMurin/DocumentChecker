window.saveAsFile = (uri, filename) => {
    var link = document.createElement('a');
    link.href = uri;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.TriggerImport = (elemntId) => {
    document.getElementById(elemntId).click();
}
