var HighChart = {
    ChartDataFormate: {
        FormatGroupData: function (data) {//处理分组数据，数据格式：name：XXX，group：XXX，value：XXX用于折线图、柱形图（分组，堆积）
            var names = new Array();
            var groups = new Array();
            var series = new Array();
            for (var i = 0; i < data.length; i++) {
                // if (!names.contains(data[i].name)) {//括号内的方法有错误
                if (jQuery.inArray(data[i].hours, names) == -1)
                    names.push(data[i].hours);
                if (jQuery.inArray(data[i].batchNo, groups) == -1)
                    groups.push(data[i].batchNo);
            }
            for (var i = 0; i < groups.length; i++) {
                var temp_series = {};
                var temp_data = new Array();
                for (var j = 0; j < data.length; j++) {
                    for (var k = 0; k < names.length; k++)
                        if (groups[i] == data[j].batchNo && data[j].hours == names[k]) {
                            temp_data.push(data[j].value);
                        }
                }
                temp_series = { name: groups[i], data: temp_data };
                series.push(temp_series);
            }
            return { category: names, series: series };
        }
    },
    ChartOptionTemplates: {
        Line: function (data, name, title) {
            var line_datas = HighChart.ChartDataFormate.FormatGroupData(data);
            var option = {
                title: {
                    text: title || '',
                    x: -20
                },
                subtitle: {
                    text: '',
                    x: -20
                },
                xAxis: {
                    categories: line_datas.category
                },
                yAxis: {
                    title: {
                        text: name || ''
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: ''
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                },
                series: line_datas.series
            };
            return option;
        }
    },
    RenderChart: function (option, container) {
        container.highcharts(option);
    }
};