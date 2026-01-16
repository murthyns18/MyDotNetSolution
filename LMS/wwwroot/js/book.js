
$(document).ready(function () {

    $('input[name="Price"], input[name="Quantity"]').on('focus', function () {
        if ($(this).val() === '0') {
            $(this).val('');
        }
    });
});
