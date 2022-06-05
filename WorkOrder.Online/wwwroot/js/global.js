(function () {

    function getCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    /* Dark mode / Light Mode switch */
    $('#layoutModeCheckbox').on('click', function () {
        if ($(this).is(':checked')) {
            $('body').attr('data-layout-mode', 'dark');
            $('#layoutModeDesc').html($('#hidDarkMode').val());
        }
        else {
            $('body').attr('data-layout-mode', 'light');
            $('#layoutModeDesc').html($('#hidLightMode').val());
        }

    });
})();
