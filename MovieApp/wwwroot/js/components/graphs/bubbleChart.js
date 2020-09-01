// svg_id - id of target svg
var drawBubbleChartSVG = function (svg_id, data) {
    var originalWidth = $("#" + svg_id)[0].width.baseVal.value;
    var originalHeight = $("#" + svg_id)[0].height.baseVal.value;
    // set the dimensions and margins of the graph
    var margin = { top: 100, right: 100, bottom: 100, left: 100 },
        width = originalWidth - margin.left - margin.right,
        height = originalHeight - margin.top - margin.bottom;

    // append the svg object to the body of the page
    var svg = d3.select("#" + svg_id)
        .attr("width", originalWidth)
        .attr("height", originalHeight)
        .append("g")
        .attr("width", width)
        .attr("height", height)
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")");

    // Add X axis
    var x = d3.scaleLinear()
        .domain([0, 220])
        .range([0, width]);
    svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x))
        .append("text")
        .attr("x", width + 50)
        .attr("fill", "currentColor")
        .attr("style", "font-weight: bold; font-size: 14px;")
        .text("אורך הסרט");;

    // Add Y axis
    var y = d3.scaleLinear()
        .domain([0, 5])
        .range([height, 0]);
    svg.append("g")
        .call(d3.axisLeft(y))
        .append("text")
        .attr("dx", "-50")
        .attr("dy", "-10")
        .attr("fill", "currentColor")
        .attr("style", "font-weight: bold; font-size: 14px;")
        .text("דירוג");

    // Add a scale for bubble size
    var z = d3.scaleLinear()
        .domain([0, 50])
        .range([1, 40]);

    // Add a scale for bubble color
    var myColor = d3.scaleOrdinal()
        .domain([0, 1, 2, 3, 4, 5])
        .range(d3.schemeSet2);

    // -1- Create a tooltip div that is hidden by default:
    var tooltip = d3.select("#svg_wrapper")
        .append("p")
        .style("opacity", 0)
        .attr("class", "tooltip")
        .style("background-color", "black")
        .style("border-radius", "5px")
        .style("padding", "10px")
        .style("color", "white")

    // -2- Create 3 functions to show / update (when mouse move but stay on same circle) / hide the tooltip
    var showTooltip = function (d) {
        tooltip
            .transition()
            .duration(200)
        tooltip
            .style("opacity", 1)
            .style("white-space", "pre-line")
            .html("שם: " + d.name + "\n" + "סוגה: " + convertGenreEnumValueToAttribute(d.genre) + "\n" + "כמות: " + d.count)
            .style("left", (d3.mouse(this)[0] + 70) + "px")
            .style("top", (d3.mouse(this)[1] + 300) + "px")

    }
    var hideTooltip = function (d) {
        tooltip
            .transition()
            .duration(200)
            .style("opacity", 0)
    }

    // Add dots
    svg.append('g')
        .selectAll("dot")
        .data(data)
        .enter()
        .append("circle")
        .attr("cx", function (d) { return x(d.duration); })
        .attr("cy", function (d) { return y(d.rating); })
        .attr("r", function (d) { return z(d.count); })
        .style("fill", "#69b3a2")
        .style("opacity", "0.7")
        .attr("stroke", "black")
        .style("fill", function (d) { return myColor(d.genre); })
        .on("mouseover", showTooltip)
        .on("mouseleave", hideTooltip)

    var yAxisTickTexts = $(".tick text");
    for (yAxisTickText of yAxisTickTexts) {
        if (yAxisTickText.getAttribute("x")) {
            yAxisTickText.setAttribute("x", "-25");
        }
    }
};

var convertGenreEnumValueToAttribute = function (enumValue) {
    const enumValueToAttribute = ["אימה","דרמה", "קומדייה","אקשן","רומנטיקה","אנימציה"]

    return enumValueToAttribute[enumValue];
}

$("#showGraph").click(function (data) {
    d3.json("GroupBy").then(function (data) {
        //Here you have data available, an object with the same structure 
        //as the JSON that was send by the server.
        if (data && data.length > 0) {
            drawBubbleChartSVG("svg_panel", data);
        }
    });
});