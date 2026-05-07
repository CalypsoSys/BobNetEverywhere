var vueChartDefinition = Vue.defineAsyncComponent(function () {
    return axios.get('comp/modal_chart.html')
        .then(response => ({
            template: response.data,
            props: {
                show: Boolean
            },
            emits: ['close'],
            data: function () {
                return {
                    showChart: false,

                    chartCTX: null,
                    chart_type: "line",
                    myChart: null,
                    chartRequestId: 0,

                    chart_options: {
                        responsive: true,
                        animation: {
                            duration: 0
                        },
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
                };
            },
            mounted: function () {
            },
            watch: {
                show: function (isVisible) {
                    if (isVisible) {
                        this.loadChart();
                    }
                },
                chart_type: function () {
                    if (this.show) {
                        this.loadChart();
                    }
                }
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
                loadChart: function () {
                    var requestId = ++this.chartRequestId;
                    axios.get(bobApiUrl('/api/chart/get_chart_data/'), {
                        params: {
                            id: 1,
                            chart_type: this.chart_type
                        }
                    })
                    .then(response => {
                        if (requestId !== this.chartRequestId) {
                            return;
                        }

                        if (response.data && response.data.Success && response.data.ChartData) {
                            this.showChart = true;
                            this.chartData = response.data.ChartData;

                            this.$nextTick(() => {
                                var canvas = document.getElementById("bob-quick-chart");
                                if (!canvas) {
                                    return;
                                }

                                if (this.myChart) {
                                    this.myChart.stop();
                                    this.myChart.destroy();
                                    this.myChart = null;
                                }

                                this.myChart = new Chart(canvas, {
                                    type: response.data.ChartType,
                                    data: response.data.ChartData,
                                    options: this.chart_options,
                                });
                            });
                        } else {
                            this.$root.showModalDialog("Unknown Error", "Bob: failure getting chart data.");
                        }
                    })
                    .catch(error => {
                        this.$root.showModalDialog("Unknown Error", "Bob: unknown error chart data.", error);
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
                    customWindowOpen("bob_charting.html", "_blank",
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
                    if (this.myChart) {
                        this.myChart.stop();
                        this.myChart.destroy();
                        this.myChart = null;
                    }
                    this.showChart = false;
                    this.$emit('close');
                }
            }
        }));
});
