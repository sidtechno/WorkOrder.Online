var row;

function start() {
    row = event.target;
}
function dragover() {
    var e = event;
    e.preventDefault();

    let children = Array.from(e.target.parentNode.parentNode.children);

    if (children.indexOf(e.target.parentNode) > children.indexOf(row))
        e.target.parentNode.after(row);
    else
        e.target.parentNode.before(row);

    updateIndex();
    updateIndexEdit();
}

updateIndex = function (e, ui) {
    var idx = 0;

    $('td.index').each(function (i) {
        idx++;
        $(this).html(idx);
    });
};

updateIndexEdit = function (e, ui) {
    var idx = 0;
    $('td.indexEdit').each(function (i) {
        idx++;
        $(this).html(idx);
    });
};