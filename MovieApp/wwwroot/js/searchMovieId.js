$(function () {

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
                            tableBody += "<tr onclick='chooseTMDBInfo(" + i + ")'><td>" + result[i].id + "</td><td>" + result[i].title + "</td><td>" + result[i].release_date + "</td></tr>";
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
});

var resultList;

function chooseTMDBInfo(i) {
    $('#TmdbId').val(resultList[i].id);
    $('#movieNameHebrew').val(resultList[i].title);   
    $('#TmdbRating').val(resultList[i].vote_average);
}