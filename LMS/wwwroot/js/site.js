$(document).ready(function () {

    // Auto-hide notification
    $(".notification").delay(3000).fadeOut("slow");

    // Price & Quantity: right aligned + clear 0 on focus
    $('input[name="Price"], input[name="Quantity"], input[name="MenuRolePermissionID"], input[name="MenuId"], input[name="MenuLevel"], input[name="DisplayOrder"] ')
        .addClass('text-end')
        .on('focus', function () {
            if ($(this).val() === '0') {
                $(this).val('');
            }
        });

});
