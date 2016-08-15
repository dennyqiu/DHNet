(function ($) {
    $.extend($.fn, {
        jDataChart: function (setting) {
            var options = $.extend({
                //需要展示的位置（哪个DOM元素内）
                renderTo: $(document.body),
                //图表类型：bar,line,spline,column,pie,area,areaspline,combine,bubble,scatter
                chartType: "",
                //图表大标题
                title: "",
                //图表小标题
                subtitle: "",
                //X轴名称
                xAxisName: "",
                //维度列-需要将数据集中的那一列当成维度展现（例如时间，网站，频道等）,相当于yAxisColumn中每个Y轴的Key
                xAxisColumn: "",
                //Y轴设置，可添加多条Y轴， key-Y轴名称，oppositeOption-true/false（true为right, false为left）
                yAxisSetting: [{ key: "", oppositeOption: false }],
                //指标列-需要将数据集中的那几列当成指标来展示（例如Impressiosn,Clicks,UV,CTR等）
                //key-指标列明，chartType-图形类型，yIndex-每个指标集需要对应的具体Y轴索引
                yAxisColumn: [{ key: "", chartType: "", yIndex: 0, color: null }],
                //图表数据源，是一个JSON的LIST对象，常用的一张二维表数据例如List<TModel>,
                dataSource: {},
                //Point MouseOver事件
                mouseOver: function (x, y, category) { },
                //Point MouseOver事件
                mouseOut: function (x, y, category) { },
                //Point MouseOver事件
                click: function (x, y, category) { },
                //是否开启导出按钮
                exportButton: true,
                //图标容器大小
                containerSize: { width: null, height: null },
                //图标标题
                legend: {}

            }, setting);

            options.renderTo = (typeof options.renderTo == "string" ? $(options.renderTo) : options.renderTo);

            var _renderTo = options.renderTo,
                _chartType = options.chartType.toString().toLowerCase(),
                _title = options.title.toString(),
                _subtitle = options.subtitle.toString(),
                _xAxisName = options.xAxisName.toString(),
                _dataSource = options.dataSource,
                _yAxisSettingSource = options.yAxisSetting,
                _yAxisColumnSource = options.yAxisColumn,
                _exportBtn = options.exportButton,
                _legend = options.legend,
                _xAxisCategoryVal = [],
                _xAxisSettingArr = {},
                _yAxisSettingArr = [],
                _yAxisColumnVal = [],
                _pointX = 0,
                _pointY = 0,
                _chartObj = {},
                _toolTipArr = {};


            var _mouseOverEvent = options.mouseOver,
                _mouseOutEvent = options.mouseOut,
                _clickEvent = options.click;

            //设置X轴（维度），Y轴指标列（根据不同图形类型转化成不同X轴，Y轴数据源）
            var getChartSetting = function () {

                _chartType = options.chartType.toString().toLowerCase();
                _title = options.title.toString()
                _subtitle = options.subtitle.toString();
                _xAxisName = options.xAxisName.toString();
                _yAxisSettingSource = options.yAxisSetting,
                _yAxisColumnSource = options.yAxisColumn,

                _toolTipArr = _chartType == "pie" ?
                {
                    useHTML: false,
                    formatter: function () {
                        var totalY = 0;
                        var s = "" + this.series.name + ": <b>" + Highcharts.numberFormat(this.y, 0) + "</b><br/>百分比：<b>" + this.percentage.toFixed(2) + "%</b>";
                        return s;
                    }
                } :
                {
                    crosshairs: true,
                    shared: true,
                    useHTML: false
                };

                var _xAxisCategoryArr = [],
                    _yAxisSettings = [],
                    _yAxisColumnArr = [];

                var _xAxisKey = options.xAxisColumn.toString();

                //设置X轴数据
                $(_dataSource).each(function (index, item) {
                    _xAxisCategoryArr.push(item[_xAxisKey] == undefined ? "" : item[_xAxisKey].toString());
                })
                _xAxisCategoryVal = _xAxisCategoryArr;

                //设置Y轴相关信息
                $(_yAxisSettingSource).each(function (index, item) {
                    var _tempObj;
                    if (_yAxisSettingSource.length > 1) {
                        _tempObj = { title: { text: item.key }, opposite: item.oppositeOption };
                    } else {
                        _tempObj = { title: { text: item.key } };
                    }
                    _yAxisSettings.push(_tempObj);
                })

                _yAxisSettingArr = _yAxisSettings;
                _xAxisSettingArr = _getXAxisSettingArr(_chartType);

                //设置Y轴数据
                var _tempObj;
                if (_chartType == "bubble" || _chartType == "scatter") {
                    var maxLength = _chartType == "bubble" ? 3 : 2;
                    $(_dataSource).each(function (idx, dataItem) {
                        var _tempName = dataItem[_xAxisKey] == undefined ? "" : dataItem[_xAxisKey].toString()
                        _tempObj = { name: _tempName, data: [] };
                        var tempArr = [];
                        $(_yAxisColumnSource).each(function (index, item) {
                            //根据是bubble还是scatter决定添加数据的个数（bubble-3个，scatter-2个）
                            if (index < maxLength) {
                                var pieItemVal = dataItem[item.key.toString()] == undefined ? 0 : parseFloat(dataItem[item.key.toString()]);
                                tempArr.push(pieItemVal);
                            }
                        })
                        _tempObj.data.push(tempArr);
                        _yAxisColumnArr.push(_tempObj);
                    });

                } else {

                    $(_yAxisColumnSource).each(function (index, item) {
                        if (_chartType == "pie") {
                            if (index < 1) {
                                _tempObj = { name: item.key, data: [], type: "pie", allowPointSelect: true, showInLegend: true };
                                var pieItemName, pieItemVal, pieModel;
                                $(_dataSource).each(function (idx, dataItem) {
                                    pieItemName = dataItem[_xAxisKey] == undefined ? "" : dataItem[_xAxisKey].toString();
                                    pieItemVal = dataItem[item.key.toString()] == undefined ? 0 : parseFloat(dataItem[item.key.toString()]);
                                    pieModel = { name: pieItemName, y: pieItemVal };
                                    _tempObj.data.push(pieModel);
                                });
                                _yAxisColumnArr.push(_tempObj);
                            }
                        } else {
                            if (_chartType == "combine" && _yAxisSettingSource.length > 1) {
                                _tempObj = { name: item.key, data: [], type: item.chartType, yAxis: item.yIndex, color: item.color };
                            } else {
                                _tempObj = { name: item.key, data: [], type: _chartType == "stack" ? "column" : item.chartType, color: item.color };
                            }
                            $(_dataSource).each(function (idx, dataItem) {
                                var dataVal = dataItem[item.key.toString()] == undefined ? 0 : parseFloat(dataItem[item.key.toString()]);
                                _tempObj.data.push(dataVal);
                            });

                            _yAxisColumnArr.push(_tempObj);
                        }
                    });
                }

                _yAxisColumnVal = _yAxisColumnArr
            }

            //根据类型获取相应设置
            var _getXAxisSettingArr = function (type) {
                var temp;
                switch (type.toString().toLowerCase()) {
                    case "bubble":
                        {
                            temp = {
                                title: { text: _xAxisName },
                                startOnTick: false,
                                endOnTick: false
                            };
                        }; break;
                    case "scatter":
                        {
                            temp = {
                                title: { text: _xAxisName },
                                startOnTick: false,
                                endOnTick: false
                            };
                        }; break;
                    default:
                        {
                            temp = {
                                categories: _xAxisCategoryVal,
                                title: { text: _xAxisName },
                                labels: {
                                    rotation: -45,
                                    align: 'right',
                                    style: {
                                        fontSize: '13px',
                                        fontFamily: 'Verdana, sans-serif'
                                    }
                                }
                            };
                        };
                        break;

                }
                return temp;
            }

            var draw = function () {

                //画图之前转化相应的XY信息
                getChartSetting();
                var chart = new Highcharts.Chart({
                    chart: {
                        renderTo: $(_renderTo).get(0),
                        type: _chartType,
                        width: options.containerSize.width,
                        height: options.containerSize.height,
                    },
                    title: {
                        text: _title
                    },
                    subtitle: {
                        text: _subtitle
                    },
                    tooltip: _toolTipArr,
                    xAxis: _xAxisSettingArr,
                    yAxis: _yAxisSettingArr,
                    legend: _legend,
                    exporting: {
                        enabled: _exportBtn
                    },
                    credits: {
                        enabled: false
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true
                            },
                            showInLegend: true
                        },
                        column: {
                            stacking: _chartType == "stack" ? "normal" : ""
                        },
                        scatter: {
                            marker: {
                                radius: 5,
                                symbol: "circle"
                            }
                        },
                        series: {
                            point: {
                                events: {
                                    mouseOver: function () {
                                        _mouseOverEvent(this.x, this.y, this.category);
                                    },
                                    mouseOut: function () {
                                        _mouseOutEvent(this.x, this.y, this.category);
                                    },
                                    click: function () {
                                        _clickEvent(this.x, this.y, this.category);
                                    }
                                }
                            }
                        }
                    },
                    series: _yAxisColumnVal
                });

                _chartObj = chart;
            }

            //获取图表控件基本设置属性
            this.getChartOptions = function () {
                return options;
            }

            //设置图标控件基本属性
            this.setChartOptions = function (settings) {
                options.title = settings.title;
                options.subtitle = settings.subtitle;
                options.xAxisName = settings.xAxisName;
                options.yAxisSetting = settings.yAxisSetting;
                options.yAxisColumn = settings.yAxisColumn;
            }

            //获取图表控件对象
            this.getChartObject = function () {
                return _chartObj;
            }

            //刷新图表控件
            this.refresh = function () {
                draw();
            }

            //控件初始化事件
            this.create = function () {
                draw();
            }

            return this;
        }
    });
})(jQuery)
