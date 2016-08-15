var legendData;
var seriesData;

var app = angular.module("myApp", []);

app.controller("myCtrl", function ($scope, $http) {
    $http
        .post("/Home/PieMap")
        .success(function (response) {
            legendData = response.result.LegendData;
            seriesData = response.result.SeriesData;
        })
        .error(function () {
            alert("系统发生异常");
        });

    // 路径配置
    require.config({
        paths: {
            echarts: 'http://echarts.baidu.com/build/dist'
        }
    });

    // 使用
    require(
        [
            "echarts",
            "echarts/chart/pie" // 使用饼状图就加载pie模块，按需加载
        ],
        function (ec) {
            // 基于准备好的dom，初始化echarts图表
            var myChart = ec.init($("#main")[0]);

            var option = getOption();

            // 为echarts对象加载数据
            myChart.setOption(option);
        }
    );

});

function getOption() {
    return {
        title: {
            text: '某站点用户访问来源',
            subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            x: 'left',
            data: legendData
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel'],
                    option: {
                        funnel: {
                            x: '25%',
                            width: '50%',
                            funnelAlign: 'left',
                            max: 1548
                        }
                    }
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        series: [
            {
                name: '访问来源',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: seriesData
            }
        ]
    };
}

