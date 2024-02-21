$(function () {
  "use strict";

  var options = {
    series: [74.84, 25.16],
    chart: {
      foreColor: "#9ba7b2",
      height: 330,
      type: "pie",
    },
    colors: ["#0727D7", "#FD3550"],
    labels: ["Success Achieved", "No Success Yet"],
    legend: {
      position: "bottom",
    },
    tooltip: {
      enabled: true,
      y: {
        formatter: function (val) {
          return val + "%";
        },
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            height: 360,
          },
          legend: {
            position: "bottom",
          },
        },
      },
    ],
  };
  var chart = new ApexCharts(document.querySelector("#pencapaianSLA"), options);
  chart.render();

  var options = {
    series: [91.63, 6.08, 2.28],
    chart: {
      foreColor: "#9ba7b2",
      height: 330,
      type: "pie",
    },
    colors: ["#0727D7", "#FD3550", "#FFC107"],
    labels: ["Realization", "Cancel", "Deleted"],
    legend: {
      position: "bottom",
    },
    tooltip: {
      enabled: true,
      y: {
        formatter: function (val) {
          return val + "%";
        },
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            height: 360,
          },
          legend: {
            position: "bottom",
          },
        },
      },
    ],
  };
  var chart = new ApexCharts(
    document.querySelector("#transactionSummary"),
    options
  );
  chart.render();

  var optionsLine = {
    chart: {
      foreColor: "#9ba7b2",
      height: 360,
      type: "line",
      zoom: {
        enabled: false,
      },
      dropShadow: {
        enabled: true,
        top: 3,
        left: 2,
        blur: 4,
        opacity: 0.1,
      },
    },
    stroke: {
      curve: "smooth",
      width: 5,
    },
    colors: ["#8833ff", "#29cc39", "#0727D7", "#FFC107"],
    series: [
      {
        name: "Week 1",
        data: [12, 15, 45, 20, 33, 27, 12].map((value) => (value / 100) * 100),
      },
      {
        name: "Week 2",
        data: [30, 33, 21, 42, 19, 32, 24].map((value) => (value / 100) * 100),
      },
      {
        name: "Week 3",
        data: [21, 23, 12, 54, 15, 29, 32].map((value) => (value / 100) * 100),
      },
      {
        name: "Week 4",
        data: [32, 43, 65, 32, 43, 23, 12].map((value) => (value / 100) * 100),
      },
    ],
    markers: {
      size: 4,
      strokeWidth: 0,
      hover: {
        size: 7,
      },
    },
    grid: {
      show: true,
      padding: {
        bottom: 0,
      },
    },
    labels: [
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday",
    ],
    xaxis: {
      tooltip: {
        enabled: false,
      },
    },
    yaxis: {
      labels: {
        formatter: function (value) {
          return value.toFixed(0) + "%";
        },
      },
    },
    legend: {
      position: "top",
      horizontalAlign: "right",
      offsetY: -20,
    },
  };

  var chartLine = new ApexCharts(
    document.querySelector("#rasioEfesiensiCPC"),
    optionsLine
  );
  chartLine.render();

  // Chart Selisih Kurang
  var options = {
    series: [
      {
        data: [-100000, -800000],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      type: "bar",
      height: 350,
    },
    colors: ["#8833ff"],
    plotOptions: {
      bar: {
        horizontal: true,
        columnWidth: "35%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: true,
    },
    xaxis: {
      categories: ["Singkawang", "Medan"],
    },
    tooltip: {
      enabled: true,
      y: {
        formatter: function (value) {
          return "Rp " + Math.abs(value).toLocaleString("id-ID");
        },
      },
    },
  };
  var chart = new ApexCharts(
    document.querySelector("#chartLessDifference"),
    options
  );
  chart.render();

  var options = {
    series: [
      {
        name: "Week 1",
        data: [44, 55, 57, 56, 61, 58, 63],
      },
      {
        name: "Week 2",
        data: [76, 85, 101, 98, 87, 105, 91],
      },
      {
        name: "Week 3",
        data: [35, 41, 36, 26, 45, 48, 52],
      },
      {
        name: "Week 4",
        data: [35, 41, 36, 26, 45, 48, 52],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      type: "bar",
      height: 360,
    },
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: "55%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      show: true,
      width: 2,
      colors: ["transparent"],
    },
    // title: {
    //   text: "C",
    //   align: "left",
    //   style: {
    //     fontSize: "14px",
    //   },
    // },
    colors: ["#29cc39", "#8833ff", "#e62e2e", "#0727D7"],
    xaxis: {
      categories: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
    },

    fill: {
      opacity: 1,
    },
    tooltip: {
      // y: {
      //   formatter: function (val) {
      //     return "$ " + val + " thousands";
      //   },
      // },
    },
  };
  var chart = new ApexCharts(document.querySelector("#cancelByBank"), options);
  chart.render();

  var optionsLine = {
    chart: {
      foreColor: "#9ba7b2",
      height: 360,
      type: "line",
      zoom: {
        enabled: false,
      },
      dropShadow: {
        enabled: true,
        top: 3,
        left: 2,
        blur: 4,
        opacity: 0.1,
      },
    },
    stroke: {
      curve: "smooth",
      width: 5,
    },
    colors: ["#8833ff", "#29cc39", "#0727D7", "#FFC107"],
    series: [
      {
        name: "Week 1",
        data: [12, 15, 45, 20, 33, 27, 12],
      },
      {
        name: "Week 2",
        data: [30, 33, 21, 42, 19, 32, 24],
      },
      {
        name: "Week 3",
        data: [21, 23, 12, 54, 15, 29, 32],
      },
      {
        name: "Week 4",
        data: [32, 43, 65, 32, 43, 23, 12],
      },
    ],
    markers: {
      size: 4,
      strokeWidth: 0,
      hover: {
        size: 7,
      },
    },
    grid: {
      show: true,
      padding: {
        bottom: 0,
      },
    },
    labels: [
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday",
    ],
    xaxis: {
      tooltip: {
        enabled: false,
      },
    },

    legend: {
      position: "top",
      horizontalAlign: "right",
      offsetY: -20,
    },
  };

  var chartLine = new ApexCharts(
    document.querySelector("#cancelBasedFactor"),
    optionsLine
  );
  chartLine.render();

  var optionsLine = {
    chart: {
      foreColor: "#9ba7b2",
      height: 360,
      type: "line",
      zoom: {
        enabled: false,
      },
      dropShadow: {
        enabled: true,
        top: 3,
        left: 2,
        blur: 4,
        opacity: 0.1,
      },
    },
    stroke: {
      curve: "smooth",
      width: 5,
    },
    colors: ["#8833ff", "#29cc39", "#0727D7", "#FFC107"],
    series: [
      {
        name: "Week 1",
        data: [12, 15, 45, 20, 33, 27, 12],
      },
      {
        name: "Week 2",
        data: [30, 33, 21, 42, 19, 32, 24],
      },
      {
        name: "Week 3",
        data: [21, 23, 12, 54, 15, 29, 32],
      },
      {
        name: "Week 4",
        data: [32, 43, 65, 32, 43, 23, 12],
      },
    ],
    markers: {
      size: 4,
      strokeWidth: 0,
      hover: {
        size: 7,
      },
    },
    grid: {
      show: true,
      padding: {
        bottom: 0,
      },
    },
    labels: [
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday",
    ],
    xaxis: {
      tooltip: {
        enabled: false,
      },
    },

    legend: {
      position: "top",
      horizontalAlign: "right",
      offsetY: -20,
    },
  };

  var chartLine = new ApexCharts(
    document.querySelector("#cancelDataReal"),
    optionsLine
  );
  chartLine.render();

  var options = {
    series: [60.83, 39.17],
    chart: {
      foreColor: "#9ba7b2",
      height: 330,
      type: "pie",
    },
    colors: ["#0727D7", "#0dcaf0"],
    labels: ["Index Up", "Index Bottom"],
    legend: {
      position: "bottom",
    },
    tooltip: {
      enabled: true,
      y: {
        formatter: function (val) {
          return val + "%";
        },
      },
    },
    dataLabels: {
      enabled: true,
      formatter: function (val, opts) {
        return (
          opts.w.globals.labels[opts.seriesIndex] + ": " + val.toFixed(2) + "%"
        );
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            height: 360,
          },
          legend: {
            position: "bottom",
          },
        },
      },
    ],
  };
  var chart = new ApexCharts(document.querySelector("#indexOvertime"), options);
  chart.render();

  var options = {
    series: [
      {
        data: [400, 430, 448, 470, 540, 580, 690, 610, 800, 980],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      type: "bar",
      height: 350,
    },
    colors: ["#0727D7"],
    plotOptions: {
      bar: {
        horizontal: true,
        columnWidth: "35%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: false,
    },
    xaxis: {
      categories: [
        "Septiawan",
        "Satrio Adi Wicaksono",
        "Ardianto",
        "Tegar Prayogi",
        "Budhiman",
        "Syaifullah Maarif",
        "Mahfud Dzuhri",
        "Dwi Irfanto",
        "Ahmad Zikri",
        "Ahmad Puji",
      ],
    },
  };
  var chart = new ApexCharts(
    document.querySelector("#highestOvertimeHours"),
    options
  );
  chart.render();

  var options = {
    series: [1.05, 17.64, 4.36, 39.29, 1.27, 36.39],
    chart: {
      foreColor: "#9ba7b2",
      height: 330,
      type: "pie",
    },
    colors: ["#0727D7", "#FFC107", "#0dcaf0", "#15CA20", "#BA07D7", "#1D1D1D"],
    labels: [
      "EJ Sistem Bank",
      "OS EJ CR",
      "DONE BA CR",
      "EJ DONE",
      "OS EJ CR",
      "DONE BA CR",
    ],
    legend: {
      position: "bottom",
    },
    tooltip: {
      enabled: true,
      y: {
        formatter: function (val) {
          return val + "%";
        },
      },
    },
    dataLabels: {
      enabled: true,
      formatter: function (val, opts) {
        return (
          opts.w.globals.labels[opts.seriesIndex] + ": " + val.toFixed(2) + "%"
        );
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            height: 360,
          },
          legend: {
            position: "bottom",
          },
        },
      },
    ],
  };
  var chart = new ApexCharts(document.querySelector("#outstandingEJ"), options);
  chart.render();

  var options = {
    chart: {
      height: 300,
      type: "radialBar",
      toolbar: {
        show: false,
      },
    },
    plotOptions: {
      radialBar: {
        //startAngle: -135,
        //endAngle: 225,
        hollow: {
          margin: 0,
          size: "78%",
          //background: '#fff',
          image: undefined,
          imageOffsetX: 0,
          imageOffsetY: 0,
          position: "front",
          dropShadow: {
            enabled: false,
            top: 3,
            left: 0,
            blur: 4,
            color: "rgba(7, 39, 215, 0.25)",
            opacity: 0.65,
          },
        },
        track: {
          background: "#f0e6ff",
          //strokeWidth: '67%',
          margin: 0, // margin is in pixels
          dropShadow: {
            enabled: false,
            top: -3,
            left: 0,
            blur: 4,
            color: "rgba(7, 39, 215, 0.85)",
            opacity: 0.65,
          },
        },
        dataLabels: {
          showOn: "always",
          name: {
            offsetY: -25,
            show: true,
            color: "#6c757d",
            fontSize: "16px",
          },
          value: {
            formatter: function (val) {
              return val + "%";
            },
            color: "#000",
            fontSize: "45px",
            show: true,
            offsetY: 10,
          },
        },
      },
    },
    fill: {
      type: "gradient",
      gradient: {
        shade: "light",
        type: "horizontal",
        shadeIntensity: 0.5,
        gradientToColors: ["#0727D7"],
        inverseColors: false,
        opacityFrom: 1,
        opacityTo: 1,
        stops: [0, 100],
      },
    },
    colors: ["#0727D7"],
    series: [25.14],
    stroke: {
      lineCap: "round",
      //dashArray: 4
    },
    labels: ["Average"],
  };
  var chart = new ApexCharts(
    document.querySelector("#chartRatioOvertime"),
    options
  );
  chart.render();

  var options = {
    series: [
      {
        name: "EJ Tidak Selisih",
        data: [14, 3, 10],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      height: 360,
      type: "line",
      zoom: {
        enabled: false,
      },
      toolbar: {
        show: true,
      },
      dropShadow: {
        enabled: true,
        top: 3,
        left: 14,
        blur: 4,
        opacity: 0.1,
      },
    },
    stroke: {
      width: 5,
      curve: "smooth",
    },
    xaxis: {
      type: "month",
      categories: ["November", "December", "January"],
    },
    // title: {
    //   text: "Line Chart",
    //   align: "left",
    //   style: {
    //     fontSize: "16px",
    //     color: "#666",
    //   },
    // },
    fill: {
      type: "gradient",
      gradient: {
        shade: "light",
        gradientToColors: ["#8833ff"],
        shadeIntensity: 1,
        type: "horizontal",
        opacityFrom: 1,
        opacityTo: 1,
        stops: [0, 100, 100, 100],
      },
    },
    markers: {
      size: 4,
      colors: ["#8833ff"],
      strokeColors: "#fff",
      strokeWidth: 2,
      hover: {
        size: 7,
      },
    },
    colors: ["#8833ff"],
    // yaxis: {
    //   title: {
    //     text: "Engagement",
    //   },
    // },
  };
  var chart = new ApexCharts(
    document.querySelector("#summaryEJPendingNoDifference"),
    options
  );
  chart.render();

  var options = {
    series: [
      {
        name: "EJ Tidak Selisih",
        data: [54, 65, 43],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      height: 360,
      type: "line",
      zoom: {
        enabled: false,
      },
      toolbar: {
        show: true,
      },
      dropShadow: {
        enabled: true,
        top: 3,
        left: 14,
        blur: 4,
        opacity: 0.1,
      },
    },
    stroke: {
      width: 5,
      curve: "smooth",
    },
    xaxis: {
      type: "month",
      categories: ["November", "December", "January"],
    },
    // title: {
    //   text: "Line Chart",
    //   align: "left",
    //   style: {
    //     fontSize: "16px",
    //     color: "#666",
    //   },
    // },
    fill: {
      type: "gradient",
      gradient: {
        shade: "light",
        gradientToColors: ["#8833ff"],
        shadeIntensity: 1,
        type: "horizontal",
        opacityFrom: 1,
        opacityTo: 1,
        stops: [0, 100, 100, 100],
      },
    },
    markers: {
      size: 4,
      colors: ["#8833ff"],
      strokeColors: "#fff",
      strokeWidth: 2,
      hover: {
        size: 7,
      },
    },
    colors: ["#8833ff"],
    yaxis: {
      title: {
        text: "Engagement",
      },
    },
  };
  var chart = new ApexCharts(
    document.querySelector("#summaryEJPendingDifference"),
    options
  );
  chart.render();

  var options = {
    series: [
      {
        name: "October",
        data: [44, 55, 57, 56, 61, 58, 63, 60, 66],
      },
      {
        name: "November",
        data: [76, 85, 101, 98, 87, 105, 91, 114, 94],
      },
      {
        name: "December",
        data: [35, 41, 36, 26, 45, 48, 52, 53, 41],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      type: "bar",
      height: 360,
    },
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: "55%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      show: true,
      width: 2,
      colors: ["transparent"],
    },
    // title: {
    //   text: "Column Chart",
    //   align: "left",
    //   style: {
    //     fontSize: "14px",
    //   },
    // },
    colors: ["#29cc39", "#8833ff", "#FFC107"],
    xaxis: {
      categories: [
        "Serang",
        "Kudus",
        "Bandung",
        "Purwokerto",
        "Meruya",
        "Denpasar",
        "Balikpapan",
        "Kediri",
        "Surabaya",
      ],
    },
    yaxis: {
      title: {
        text: "Duration (Hours)",
      },
    },
    fill: {
      opacity: 1,
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return val + " hours";
        },
      },
    },
  };
  var chart = new ApexCharts(
    document.querySelector("#peningkatanDurasiLemburTigaBulan"),
    options
  );
  chart.render();

  var options = {
    series: [
      {
        name: "October",
        data: [44, 55, 57, 56, 61, 58, 63, 60, 66],
      },
      {
        name: "November",
        data: [76, 85, 101, 98, 87, 105, 91, 114, 94],
      },
      {
        name: "December",
        data: [35, 41, 36, 26, 45, 48, 52, 53, 41],
      },
    ],
    chart: {
      foreColor: "#9ba7b2",
      type: "bar",
      height: 360,
    },
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: "55%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      show: true,
      width: 2,
      colors: ["transparent"],
    },
    // title: {
    //   text: "Column Chart",
    //   align: "left",
    //   style: {
    //     fontSize: "14px",
    //   },
    // },
    colors: ["#0727D7", "#FD3550", "#0DCAF0"],
    xaxis: {
      categories: [
        "Joko",
        "Reza",
        "Andre",
        "Ardy",
        "Rifki",
        "Leonardo",
        "Amin",
        "Hana",
        "Caca",
      ],
    },
    yaxis: {
      title: {
        text: "Duration (Hours)",
      },
    },
    fill: {
      opacity: 1,
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return val + " hours";
        },
      },
    },
  };
  var chart = new ApexCharts(
    document.querySelector("#topKaryawanLembur"),
    options
  );
  chart.render();
});
