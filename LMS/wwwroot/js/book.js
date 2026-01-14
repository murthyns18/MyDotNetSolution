$(document).ready(function () {
    $.ajax({
        url: APIURL + '/Book/BookList?BookId=0',
        type: 'GET',
        dataType: 'JSON',
        async: false,
        cache: false,
        success: function (result) {
            console.log("result", result)
        }
    })
})