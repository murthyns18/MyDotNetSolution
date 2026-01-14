
$(document).ready(function () {
    $.ajax({
        url: APIURL + '/Book/BookList?BookId=0',
        type: 'GET',
        dataType: 'JSON',
        success: function (result) {
            console.log("result", result);
        },
        error: function (err) {
            console.error("ajax error", err);
        }
    });
});
