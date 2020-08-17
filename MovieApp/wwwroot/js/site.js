// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    var url = window.location;
    $('ul.sidebarLink li.nav-active').removeClass('nav-active');
    $('ul.sidebarLink li.nav-item a').each(function () {
        if (this.href == url) {
            $(this).parent().addClass('nav-active');
        }
    });
});

