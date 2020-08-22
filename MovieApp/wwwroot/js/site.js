// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// NavBar
$(document).ready(function () {

    var url = window.location;
    $('ul.sidebarLink li.nav-active').removeClass('nav-active');
    $('ul.sidebarLink li.nav-item a').each(function () {
        if (this.href == url) {
            $(this).parent().addClass('nav-active');
        }
    });
});

$(function () {
    $('.upload-img').on('change', function (evt) {
        var file = evt.target.files[0];
        var _this = evt.target;
        var reader = new FileReader();
        $(this).parent('.upload-section').hide();
        reader.addEventListener("load", function (event) {
            var picFile = event.target;
            var span = `<img class="thumb" src=${picFile.result} title=${file.name} alt="your image" /><span class="remove_img_preview"></span>`;
            $(_this).parent('.upload-section').next().append($(span));
        });
        //Read the image
        reader.readAsDataURL(file)
    });

    $('.preview-section').on('click', '.remove_img_preview', function () {
        $('input.upload-img').val("");
        $('input.imgUpload').remove();


        $(this).parent('.preview-section').prev().show();
        $(this).parent('.preview-section').remove();
    });
});

