
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $(".navbar a, footer a[href='#page-header']").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 900, function () {
                window.location.hash = hash;
            });
        }
    });
})