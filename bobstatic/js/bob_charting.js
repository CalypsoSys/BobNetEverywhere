Vue.createApp({
    data: function () {
        return {
            chartName: "",
        };
    },
    mounted: function () {
        var id = new URLSearchParams(window.location.search).get("id");
        renderWindowData(id, this.renderChart);
    },
    methods: {
        renderChart: function (data) {
            new Chart(document.getElementById("bob-charting"), {
                type: data.chartData.chart_type,
                data: { labels: data.chartData.labels, datasets: data.chartData.datasets },
                options: data.chartData.chart_options,
            });
        }
    }
}).mount('#bob_charting');
