    function Encrypt(str) {
    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

    var encrypted = CryptoJS.AES.encrypt(
        CryptoJS.enc.Utf8.parse(str),
        key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }
    );

    return encrypted.toString();
}

// ===============================
// GLOBAL COMMON JS (layout.js)
// ===============================
var App = App || {};

// ---------- ALERT & CONFIRM ----------
App.alert = function (message) {
    $("#alertpopup").modal('show');
    $('#popupmesssage').html(message);
};

window.confirm = function (msg, func) {
    $('#confirmpopupmesssage').empty().html(msg);
    $('#confirmpopup').modal('show');
    if (func) {
        $("#ConfirmOKbutton").off("click").on("click", func);
    }
};

// ---------- GLOBAL VARIABLES ----------
App.outputArrayMultiselect = [];

// ---------- AJAX HELPERS ----------
App.CallAjaxGET = function (URL, params, async) {
    $.ajax({
        type: "GET",
        url: ROOT + URL,
        data: params,
        async: async,
        dataType: "json",
        contentType: 'application/json',
        error: function () {
            App.alert("An error occurred.");
        }
    });
};

App.CallAjaxPOST = function (URL, params, async) {
    $.ajax({
        type: "POST",
        url: ROOT + URL,
        data: params,
        async: async,
        dataType: "json",
        contentType: 'application/json',
        error: function () {
            App.alert("An error occurred.");
        }
    });
};

// ---------- JQGRID (VIRTUAL SCROLL + MULTISELECT) ----------
App.CreateJQGrid = function (
    selector,
    url,
    dataType,
    params,
    colmodels,
    token,
    enableAuthorization = false,
    enableMultiselect = false,
    maxheight = "45vh",
    mtype = "GET"
) {

    $.jgrid.gridUnload(selector);

    $(selector).jqGrid({
        url: url,
        datatype: dataType,
        mtype: mtype,
        data: params,

        loadBeforeSend: function (jqXHR) {
            if (enableAuthorization && token) {
                jqXHR.setRequestHeader("Authorization", "Bearer " + token);
            }
        },

        colModel: colmodels,
        loadonce: true,
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        rowNum: enableMultiselect ? params.length : 1000,
        scroll: 1,
        pager: selector + '_pager',
        multiselect: enableMultiselect,

        beforeSelectRow: function (rowId, e) {
            return $(e.target).is("input:checkbox");
        },

        onSelectRow: enableMultiselect ? selectGridRow : null,
        onSelectAll: enableMultiselect ? selectAllGrid : null,

        jsonReader: {
            root: function (obj) {
                return typeof obj === "string" ? $.parseJSON(obj) : obj;
            },
            repeatitems: false
        },

        search: true,
        searchClearButton: true
    });

    $(selector).jqGrid('filterToolbar', {
        autosearch: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

    $('.ui-jqgrid-bdiv').css({ 'max-height': maxheight });
};

