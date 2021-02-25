(function ($) {

    //Humberger Menu
    $(".humberger__open").on('click', function () {
        $(".humberger__menu__wrapper").addClass("show__humberger__menu__wrapper");
        $(".navbar-toggler-awesome").removeClass("fa-bars");
        $(".navbar-toggler-awesome").addClass("fa-times");
        $(".humberger__menu__overlay").addClass("active");

        $("body").addClass("over_hid");
    });

    $(".humberger__menu__overlay").on('click', function () {
        $(".humberger__menu__wrapper").removeClass("show__humberger__menu__wrapper");
        $(".navbar-toggler-awesome").removeClass("fa-times");
        $(".navbar-toggler-awesome").addClass("fa-bars");
        $(".humberger__menu__overlay").removeClass("active");
        $("body").removeClass("over_hid");
    });

})(jQuery)