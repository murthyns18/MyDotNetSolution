$(document).ready(function () {
    $("#notification").delay(3000).fadeOut("slow");
});


$(document).ready(function () {
    $("#search-input").on("keyup", function () {
        var input = $(this).val().toLowerCase();

        var found = false;

        $(".tbody tr").filter(function () {
            var match = $(this).text().toLocaleLowerCase().indexOf(input) > -1;

            if (match) {
                $(this).show();
                found = true;
            }
            else {
                $(this).hide();
            }

            if (!found) {
                $("#not-found").show();
            }
            else {

                $("#not-found").hide();
            }
        })
    })
})