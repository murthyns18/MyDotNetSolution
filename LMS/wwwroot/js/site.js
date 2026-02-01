$(document).ready(function () {

    const welcomeKey = "welcomeAlertShown";

    if (!sessionStorage.getItem(welcomeKey)) {

        const $welcome = $("#welcomeAlert");

        $welcome.hide();
      
        setTimeout(function () {
            $welcome.fadeIn("slow");
        }, 500);

        setTimeout(function () {
            $welcome.fadeOut("slow");
        }, 3000);

     
        sessionStorage.setItem(welcomeKey, "true");

    } else {
       
        $("#welcomeAlert").hide();
    }

    $(".notification").delay(3000).fadeOut("slow");

    
    $('input[name="Price"], input[name="Quantity"], input[name="MenuRolePermissionID"], input[name="MenuId"], input[name="MenuLevel"], input[name="DisplayOrder"] ')
        .addClass('text-end')
        .on('focus', function () {
            if ($(this).val() === '0') {
                $(this).val('');
            }
        });

});
