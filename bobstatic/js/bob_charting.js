var router = new VueRouter({
    mode: 'history',
    routes: []
});


var bobCharting = new Vue({
    router,
    el: '#bob_charting',
    data: {
        chartName: "",
    },

    mounted: function () {
        document.onreadystatechange = () => {
            if (document.readyState == "complete") {
                renderWindowData(this.$route.query.id, this.renderChart);
            }
        }
    },
    methods: {
        renderChart: function (data) {
            var myChart = new Chart(document.getElementById("bob-charting"), {
                type: data.chartData.chart_type,
                data: { labels: data.chartData.labels, datasets: data.chartData.datasets },
                options: data.chartData.chart_options,
            });
        }
    }
})
