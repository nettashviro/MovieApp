// svg_id - id of target svg
// data - array of objects {label, value}
var drawBarChartSVG = function (svg_id, data, averageOf, averageBy) {
    var svg = d3.select("#" + svg_id),
        margin = 200,
        width = svg.attr("width") - margin,
        height = svg.attr("height") - margin
    $("#" + svg_id)[0].innerHTML = '';
    svg.append("text")
        .attr("transform", "translate(100,0)")
        .attr("x", 50)
        .attr("y", 50)
        .attr("font-size", "24px")

    var xScale = d3.scaleBand().range([0, width]).padding(0.4),
        yScale = d3.scaleLinear().range([height, 0]);

    var g = svg.append("g")
        .attr("transform", "translate(" + 100 + "," + 100 + ")");

    xScale.domain(data.map(function (d) { return d.key || "ללא"; }));
    yScale.domain([0, d3.max(data, function (d) { return d.count; })]);

    g.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(xScale))
        .append("text")
        .attr("y", height - 250)
        .attr("x", width)
        .attr("fill", "currentColor")
        .attr("style", "font-weight: bold; font-size: 14px;")
        .text(averageBy);

    g.append("g")
        .call(d3.axisLeft(yScale).tickFormat(function (d) {
            return d;
        })
            .ticks(10))
        .append("text")
        .attr("dy", "-10")
        .attr("dx", "-50")
        .attr("fill", "currentColor")
        .attr("style", "font-weight: bold; font-size: 14px;")
        .text(averageOf);

    g.selectAll(".bar")
        .data(data)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return xScale(d.key || "ללא"); })
        .attr("y", function (d) { return yScale(d.count); })
        .attr("width", xScale.bandwidth())
        .attr("height", function (d) { return height - yScale(d.count); });

    var yAxisTickTexts = $(".tick text");
    for (yAxisTickText of yAxisTickTexts) {
        if (yAxisTickText.getAttribute("x")) {
            yAxisTickText.setAttribute("x","-25");
        }
    }
}

var convertEnumTextToAttribute = function (enumText) {
    const enumValueToAttribute = {
        "משך בדקות": "Duration",
        "דירוג": "Rating",

        "מדינה": "Country",
        "סוגה": "Genre",
        "שנת יציאה": "Year",
        "שפת מקור": "Language",

        "גיל": "Age",
        "תפקיד": "Role",
        "מדינת מוצא": "OriginCountry"
    }

    return enumValueToAttribute[enumText];
}

$("#averageOfMoviesSelect").change(function (data) {
    var ofText = $('#averageOfMoviesSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var bySelectValue = $("#averageByMoviesSelect")[0].value;
    if (bySelectValue != "") {
        var byText = $('#averageByMoviesSelect option[value="' + bySelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    } 
});

$("#averageOfOfficialsSelect").change(function (data) {
    var ofText = $('#averageOfOfficialsSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var bySelectValue = $("#averageByOfficialsSelect")[0].value;
    if (bySelectValue != "") {
        var byText = $('#averageByOfficialsSelect option[value="' + bySelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    }
});

$("#averageByMoviesSelect").change(function (data) {
    var byText = $('#averageByMoviesSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var ofSelectValue = $("#averageOfMoviesSelect")[0].value;
    if (ofSelectValue != "") {
        var ofText = $('#averageOfMoviesSelect option[value="' + ofSelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    }
});

$("#averageByOfficialsSelect").change(function (data) {
    var byText = $('#averageByOfficialsSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var ofSelectValue = $("#averageOfOfficialsSelect")[0].value;
    if (ofSelectValue != "") {
        var ofText = $('#averageOfOfficialsSelect option[value="' + ofSelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    }
});

$("#averageOfSoundtracksSelect").change(function (data) {
    var ofText = $('#averageOfSoundtracksSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var bySelectValue = $("#averageBySoundtracksSelect")[0].value;
    if (bySelectValue != "") {
        var byText = $('#averageBySoundtracksSelect option[value="' + bySelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    }
});

$("#averageBySoundtracksSelect").change(function (data) {
    var byText = $('#averageBySoundtracksSelect option[value="' + data.currentTarget.value + '"')[0].text;

    var ofSelectValue = $("#averageOfSoundtracksSelect")[0].value;
    if (ofSelectValue != "") {
        var ofText = $('#averageOfSoundtracksSelect option[value="' + ofSelectValue + '"')[0].text;
        var currentTarget = $("#spliceTarget")[0].value;
        d3.json("Average/?avgOf=" + convertEnumTextToAttribute(ofText) + "&avgBy=" + convertEnumTextToAttribute(byText) + "&context=" + currentTarget).then(function (data) {
            //Here you have data available, an object with the same structure 
            //as the JSON that was send by the server.
            if (data) {
                drawBarChartSVG("svg_panel", data, ofText, byText);
            }
        });
    }
});


$("#spliceTarget").change(function (data) {
    var splicesBy = $(".average-by");
    var splicesOf = $(".average-of");

    for (var splice of splicesOf) {
        splice.style = "display: none;";
    }

    for (var splice of splicesBy) {
        splice.style = "display: none;";
    }

    var currentSpliceOf = $("#averageOf" + data.currentTarget.value + "Select")[0];
    var currentSpliceBy = $("#averageBy" + data.currentTarget.value + "Select")[0];
    currentSpliceOf.style = "display: block;";
    currentSpliceBy.style = "display: block;";
    var svg = $("#svg_panel")[0];
    svg.innerHTML = '';
});
