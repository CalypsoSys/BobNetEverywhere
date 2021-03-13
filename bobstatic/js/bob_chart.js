var vueChartDefinition = {
    template: null,
    props: {
        show: Boolean
    },
    data: function () {
        return {
            showChart: false,

            chartCTX: null,
            chart_type: "line",
            myChart: null,

            chart_options: {
                responsive: true,
                lineTension: 1,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            padding: 25,
                        }
                    }],
                    xAxes: [{
                        ticks: {
                            autoSkip: false
                        }
                    }]
                }
            },
            chartData: {}
        }
    },
    mounted() {
        this.chartCTX = document.getElementById("bob-quick-chart");
    },
    methods: {
        getRandomColor: function () {
            var letters = '0123456789ABCDEF'.split('');
            var color = '#';
            for (var i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
            return color;
        },
        onSubmitDoNothing: function () {
        },
        createChart: function () {
            axios.get('/api/chart/get_chart_data/', {
                params: {
                    id: 1,
                    chart_type: this.chart_type
                }
            })
            .then(response => {
                if (response.data && response.data.Success && response.data.ChartData ) {
                    this.showChart = true;
                    if (this.myChart) {
                        this.myChart.destroy();
                    }

                    this.chartData = response.data.ChartData;
                    this.myChart = new Chart(this.chartCTX, {
                        //type: this.chart_type,
                        type: response.data.ChartType,
                        data: response.data.ChartData,
                        options: this.chart_options,
                    });
                } else {
                    bobCalypso.showModalDialog("Unknown Error", "Bob: failure getting chart data.");
                }
            })
            .catch(error => {
                bobCalypso.showModalDialog("Unknown Error", "Bob: unknown error chart data.", error);
            });
        },
        openReportWindow: function () {
            var dataSets = [];
            for (var i = 0; i < this.chartData.datasets.length; i++) {
                dataSets.push({
                    backgroundColor: this.chartData.datasets[i].backgroundColor,
                    borderWidth: this.chartData.datasets[i].borderWidth,
                    data: this.chartData.datasets[i].data,
                    label: this.chartData.datasets[i].label,
                });
            }
            var bobcharting = customWindowOpen("/bob_charting.html", "_blank",
                { chartData:
                    {
                        chart_type: this.chart_type,
                        labels: this.chartData.labels,
                        datasets: dataSets,
                        chart_options: this.chart_options
                    }
            });
        },
        closeUp: function () {
            this.$emit('close');
        }
    }
};

function vueChartResolver(resolve, reject) {
    axios.get('/comp/modal_chart.html')
    .then(response => {
        vueChartDefinition.template = response.data;
        resolve(vueChartDefinition);
    });
}

Vue.component('modal_chart', vueChartResolver);
