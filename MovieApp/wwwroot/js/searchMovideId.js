$(function () {

    $('#OMDBButton').click(function () {       
        $.ajax({
            url: "/Movies/FindMovieId",
            data: "name=" + $('#movieNameHebrew').val().toString(),
            success: function (result) {
                if (result != null) {

                    if (result.length > 0) {
                        var tableBody = "";


                        for (i = 0; i < result.length; i++) {
                            tableBody += "<tr onclick='chooseOMDBID(" +result[i].id+")'><td>" + result[i].id + "</td><td>" + result[i].title + "</td><td>" + result[i].release_date + "</td></tr>";
                        }

                        $('#resultTable tbody').html(tableBody);
                        ('#resultMessage').innerHTML = "";
                    }                                 
                }
                else {
                    $('#resultTable tbody').html("");
                    $('#resultMessage').innerHTML = "לא נמצאנו נתונים, אנא בדוק את שם הסרט";
                }
            }
        });
        
    });

    
});

function chooseOMDBID(id) {
    $('#OmdbId').val(id); ;   
}