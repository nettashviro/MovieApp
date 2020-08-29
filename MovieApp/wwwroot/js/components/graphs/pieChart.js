// svg_id - id of target svg
// data - array of objects {label, value}
var drawPieChartSVG = function (svg_id, data) {
    var svg = $("#" + svg_id)[0];
    var svgText = $("#" + svg_id + "_txt")[0];
    svg.innerHTML = '';
    svgText.innerHTML = '';
    var width = svg.width.baseVal.value,
        height = svg.height.baseVal.value,
        radius = Math.min(width, height) / 2;
    var colors = d3.scaleOrdinal(d3.schemeSet2);
    var arc = d3.arc()
        .outerRadius(radius - 10)
        .innerRadius(0)
        .padAngle(0.03);

    var pie = d3.pie().value(function (d) { return d.count; });
    var root = d3.select("#" + svg_id)
        .datum(data)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform",
            "translate(" + width / 2.0 + "," + height / 2.0 + ")");

    var arcs = root
        .selectAll("g.slice")
        .data(pie)
        .enter()
        .append("g")
        .attr("class", "slice");

    arcs
        .append("path")
        .attr("fill", function (d, i) { return colors(i); })
        .attr("d", arc)

        // show details in the separate html element when mouse is over the specified sector
        .on("mouseover", function (d, i) {
            var txt = document.getElementById(svg_id + "_txt");
            if (txt) {
                txt.innerHTML = (d.data.key || "ללא") + " (" + d.data.count + ")";
            }
        });

    // place labels on the chart
    arcs.append("svg:text")
        .attr("transform", function (d) {
            d.innerRadius = radius / 2.0;
            d.outerRadius = radius;
            // text will be inserted in the center of the current section

            return "translate(" + arc.centroid(d) + ")";
        })
        .attr("text-anchor", "middle")

        // d.data - our data item, assigned to the current section. "label" is a part of our data object
        .text(function (d, i) { return d.data.key || "ללא"; });
};


$("#spliceByMoviesSelect").change(function (data) {
    var text = $('#spliceByMoviesSelect option[value="' + data.currentTarget.value + '"')[0].text;
    var currentTarget = $("#spliceTarget")[0].value; 
    d3.json("Count/?splice=" + text + "&context=" + currentTarget).then(function (data) {
        //Here you have data available, an object with the same structure 
        //as the JSON that was send by the server.
        if (data) {
            drawPieChartSVG("svg_panel", data);
        }
    });
});

$("#spliceByOfficialsSelect").change(function (data) {
    var text = $('#spliceByOfficialsSelect option[value="' + data.currentTarget.value + '"')[0].text;
    var currentTarget = $("#spliceTarget")[0].value; 
    d3.json("Count/?splice=" + text + "&context=" + currentTarget).then(function (data) {
        //Here you have data available, an object with the same structure 
        //as the JSON that was send by the server.
        if (data) {
            drawPieChartSVG("svg_panel", data);
        }
    });
});

$("#spliceTarget").change(function (data) {
    var splices = $(".splice-by");
    for (var splice of splices) {
        splice.style = "display: none;";
    }
    var currentSplice = $("#spliceBy" + data.currentTarget.value + "Select")[0];
    currentSplice.style = "display: block;";
    var svg = $("#svg_panel")[0];
    var svgText = $("#svg_panel_txt")[0];
    svg.innerHTML = '';
    svgText.innerHTML = '';
});