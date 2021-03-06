﻿$(function () {

    $('#TMDBButton').click(function () {       
        $.ajax({
            url: "/Movies/FindMovieId",
            data: "name=" + $('#movieNameHebrew').val().toString(),
            success: function (result) {
                if (result != null) {
                    if (result.length > 0) {

                        resultList = result;

                        var tableBody = "";

                        for (var i = 0; i < result.length; i++) {
                            tableBody += "<tr onclick='chooseTMDBInfo(" + i + "); getVideoTrailer(" + result[i].id + ");'><td>" + result[i].id + "</td><td>" + result[i].title + "</td><td>" + result[i].release_date + "</td></tr>";
                        }
                        $('#resultTable tbody').html(tableBody);
                        $('#resultMessage').text("");                     
                    }
                    else {
                        $('#resultTable tbody').html("");
                        $('#resultMessage').text("לא נמצאנו נתונים, אנא בדוק את שם הסרט");
                    }                 
                }
                else {
                    $('#resultTable tbody').html("");
                    $('#resultMessage').text("משהו השתבש בתהליך אחזור הנתונים");
                }     
            }
        }); 
    });   

    $('#loaderDiv').hide();
    $(document).ajaxStart(function () {
        $('#resultTable tbody').empty();
        $('#loaderDiv').show();
    }).ajaxStop(function () {
        $('#loaderDiv').hide();
    });
});

var resultList;

function getVideoTrailer(id) {
    $.ajax({
        url: "/Movies/FindMovieVideos",
        data: { id: id },
        success: function (result) {
            if (result != null) {
                $('#movieTrailerURL').val(result);
            }
            else {
                $('#movieTrailerURL').val(null);
            }
        }
    }); 
}

function chooseTMDBInfo(i) {
    $('#TmdbId').val(resultList[i].id);
    $('#movieNameHebrew').val(resultList[i].title);   
    $('#TmdbRating').val(resultList[i].vote_average);
    $('#movieImgURL').val(resultList[i].poster_path);
}

