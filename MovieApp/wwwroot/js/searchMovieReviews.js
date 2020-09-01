$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#loaderDiv').show();
    }).ajaxStop(function () {
        $('#loaderDiv').hide();
    });

    $.ajax({
        url: "/Movies/FindMovieReviews",
        data: "id=" + $('#movieTMDBId').text(),
        success: function (result) {
            if (result != null) {

                if (result.length > 0) {
                    var tableBody = "";


                    for (i = 0; i < result.length; i++) {
                        tableBody += "<div class='card shadow'><div class='card-header'><h5 class='card-title'>" + result[i].author + "</h5></div>";
                        tableBody += "<div class='card-body'><p class='card-text splitcol'>" + result[i].content + "</p>"
                        tableBody += "<a target='_blank' href ='" + result[i].url + "' class='btn btn-primary'> קישור לביקורת</a></div></div> ";
                    }

                    $('#card-result').html(tableBody);
                    $('#resultMessage').text("");
                }
                else {
                    $('#card-result').html("");
                    $('#resultMessage').text("לא נמצאנו ביקורות");
                }
            }
            else {
                $('#card-result').html("");
                $('#resultMessage').text("לא נמצאנו ביקורות");
            }
        }
    });   
});
