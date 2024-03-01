$(function () {
    "use strict";
    var options = {
        series: [
            {
                name: "Nilai",
                data: [80, 50, 30, 40],
            },
            {
                name: "Norma",
                data: [20, 30, 40, 80],
            },
        ],
        chart: {
            foreColor: "#9ba7b2",
            height: 350,
            type: "radar",
            dropShadow: {
                enabled: true,
                blur: 1,
                left: 1,
                top: 1,
            },
            toolbar: {
                show: false
            }
        },
        dataLabels: {
            enabled: true,
        },
        colors: ["#8833ff", "#29cc39"],
        plotOptions: {
            radar: {
                size: 140,
                polygons: {
                    strokeColors: "#e9e9e9",
                    fill: {
                        colors: ["#f8f8f8", "#fff"],
                    },
                },
            },
        },
        // title: {
        //   text: "Radar Chart - Multi Series",
        // },
        stroke: {
            width: 2,
        },
        fill: {
            opacity: 0.1,
        },
        markers: {
            size: 4,
            colors: ["#fff"],
            strokeColor: "#FF4560",
            strokeWidth: 2,
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val;
                },
            },
        },
        xaxis: {
            categories: ["AT-IPP-01", "AT-IPP-02", "AT-IPP-03", "I-PT-07"],
        },
    };
    var chart = new ApexCharts(document.querySelector("#chartScore"), options);
    chart.render();
});
