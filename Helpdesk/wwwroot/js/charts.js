
window.onload = getDayly;

var culture; 
if (window.navigator.languages) {
    culture = document.cookie;
}
else {
    culture = window.navigator.userLanguage || window.navigator.language;
}
console.log(culture);
Array.prototype.remove = function () {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};
//*open AJAX request for all requests*//
var request = new XMLHttpRequest();
request.open('Get', '/HeadManager/Requests/GetRequests');
request.onload = function () {
    var ourData = JSON.parse(request.responseText);
    var d = new Date(ourData.data[0].rDateTime);
    // console.log(ourData.data[0].rMonth);
    console.log(d.getDay());

    filterData(ourData, 1);
}
request.send();
//*open AJAX request for all requests ends here*//

$.ajax({
    url: '/HeadManager/Requests/GetRejectedRequests',
    dataType: 'json',
    success: function (data) {
        if (data != null) {
            //toastr.success("blabal");
            //var date = new Date(data.data[0].rDateTime);
            //console.log("it is rejecteds " + date.getDay());
            filterData(data, 2);

        }
        else {
            toastr.error("Error");
        }
    }
});


var monthCounter = [0, 0, 0, 0, 0];
var monthCounterR = [0, 0, 0, 0, 0];
var dayCounter = [[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]];
var dayCounterR = [[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]];
var weekCounterR = [0, 0, 0, 0, 0];
var weekCounter = [0, 0, 0, 0, 0];

var statisticsByYear = [];

var today = new Date();
var dd = String(today.getDate()).padStart(2, '0');
//console.log("bugun " + dd + "day is " + typeof dd);

for (i = 6, j = parseInt(dd); i >= 0; i--, j--) {
    dayCounter[i][1] = j;
    dayCounterR[i][1] = j;
}

console.log("it is daycounter: " + dayCounter);

function filterData(dataRequest, filter) {

    for (i = 0; i < dataRequest.data.length; i++) {
        var date = new Date(dataRequest.data[i].rDateTime);
        var day = String(date.getDate()).padStart(2, '0');

        
        if (date.getMonth() == today.getMonth() && date.getFullYear() == today.getFullYear()) {
            if (filter == 1) {
                monthCounter[4]++;
            }
            if (filter == 2) {
                monthCounterR[4]++;
            }
           
        }
        else if (today.getMonth() > 3 && date.getFullYear() == today.getFullYear()) {
            if (date.getMonth() == today.getMonth() - 1) {
                if (filter == 1) {
                    monthCounter[3]++;
                }
                if (filter == 2) {
                    monthCounterR[3]++;
                }
               
            }
            if (date.getMonth() == today.getMonth() - 2) {
                if (filter == 1) {
                    monthCounter[2]++;
                }
                if (filter == 2) {
                    monthCounterR[2]++;
                }
            }
                
            if (date.getMonth() == today.getMonth() - 3) {
                if (filter == 1) {
                    monthCounter[1]++;
                }
                if (filter == 2) {
                    monthCounterR[1]++;
                }
            }
                
            if (date.getMonth() == today.getMonth() - 4) {
                if (filter == 1) {
                    monthCounter[0]++;
                }
                if (filter == 2) {
                    monthCounterR[0]++;
                }
            }
                
        }
        else if (today.getMonth() <= 3) {
            var diff = 4 - today.getMonth();
            if (diff == 1) {
                if (date.getMonth() == today.getMonth() - 1) {
                    if (filter == 1) {
                        monthCounter[3]++;
                    }
                    if (filter == 2) {
                        monthCounterR[3]++;
                    }
                    
                }
                if (date.getMonth() == today.getMonth() - 2) {
                    
                    if (filter == 1) {
                        monthCounter[2]++;
                    }
                    if (filter == 2) {
                        monthCounterR[2]++;
                    }
                }
                if (date.getMonth() == today.getMonth() - 3) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
                if (date.getMonth() == 11 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
            }
            if (diff == 1) {
                if (date.getMonth() == today.getMonth() - 1) {
                    if (filter == 1) {
                        monthCounter[3]++;
                    }
                    if (filter == 2) {
                        monthCounterR[3]++;
                    }
                }
                if (date.getMonth() == today.getMonth() - 2) {
                    if (filter == 1) {
                        monthCounter[2]++;
                    }
                    if (filter == 2) {
                        monthCounterR[2]++;
                    }
                }
                if (date.getMonth() == today.getMonth() - 3) {
                    if (filter == 1) {
                        monthCounter[1]++;
                    }
                    if (filter == 2) {
                        monthCounterR[1]++;
                    }
                }
                if (date.getMonth() == 11 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
            }
            if (diff == 2) {
                if (date.getMonth() == today.getMonth() - 1) {
                    if (filter == 1) {
                        monthCounter[3]++;
                    }
                    if (filter == 2) {
                        monthCounterR[3]++;
                    }
                }
                if (date.getMonth() == today.getMonth() - 2) {
                    if (filter == 1) {
                        monthCounter[2]++;
                    }
                    if (filter == 2) {
                        monthCounterR[2]++;
                    }
                 
                }
                if (date.getMonth() == 11 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[1]++;
                    }
                    if (filter == 2) {
                        monthCounterR[1]++;
                    }
                }
                if (date.getMonth() == 10 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
            }
            if (diff == 3) {
                if (date.getMonth() == today.getMonth() - 1) {
                    if (filter == 1) {
                        monthCounter[3]++;
                    }
                    if (filter == 2) {
                        monthCounterR[3]++;
                    }
                }
                if (date.getMonth() == 11 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[2]++;
                    }
                    if (filter == 2) {
                        monthCounterR[2]++;
                    }
                }
                if (date.getMonth() == 10 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[1]++;
                    }
                    if (filter == 2) {
                        monthCounterR[1]++;
                    }
                }
                if (date.getMonth() == 9 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
            }
            if (diff == 4) {
                if (date.getMonth() == 11 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[3]++;
                    }
                    if (filter == 2) {
                        monthCounterR[3]++;
                    }
                }
                if (date.getMonth() == 10 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[2]++;
                    }
                    if (filter == 2) {
                        monthCounterR[2]++;
                    }
                }
                if (date.getMonth() == 9 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[1]++;
                    }
                    if (filter == 2) {
                        monthCounterR[1]++;
                    }
                }
                if (date.getMonth() == 8 && date.getFullYear() == today.getFullYear() - 1) {
                    if (filter == 1) {
                        monthCounter[0]++;
                    }
                    if (filter == 2) {
                        monthCounterR[0]++;
                    }
                }
            }
        }
        
        if (day == dd && date.getMonth() == today.getMonth() && today.getFullYear() == date.getFullYear()) {
            if (filter == 1) {
                dayCounter[6][0]++;
                console.log("daycunter: " + dayCounter);
            }
            if (filter == 2) {
                dayCounterR[6][0]++;
            }
           

        }
        if (parseInt(day) > dd - 7 && date.getMonth() == today.getMonth()) {
            if (filter == 1) {
                for (m = 0; m < 6; m++) {
                    if (dayCounter[m][1] == parseInt(day)) {
                        dayCounter[m][0]++;
                    }

                }
            }

            if (filter == 2) {
                for (m = 0; m < 6; m++) {
                    if (dayCounterR[m][1] == parseInt(day)) {
                        dayCounterR[m][0]++;
                    }

                }
            }
            

            //console.log("it is daycounter: " + dayCounter);
            //console.log("it is daycounterR: " + dayCounterR);

        }


    }
    //console.log("it is pre daycounter: " + dayCounter);
    //console.log("it is pre daycounterR: " + dayCounterR);
    for (i = 6; i > 0; i--) {
        for (j = 0; j < i; j++) {
            if (filter == 1) {
                if (dayCounter[j][1] > dayCounter[j + 1][1]) {
                    var temp = dayCounter[j + 1];
                    dayCounter[j + 1] = dayCounter[j];
                    dayCounter[j] = temp;
                }

            }
            if (filter == 2) {
                if (dayCounterR[j][1] > dayCounterR[j + 1][1]) {
                    var temp = dayCounterR[j + 1];
                    dayCounterR[j + 1] = dayCounterR[j];
                    dayCounterR[j] = temp;
                }

            }
            

        }
    }



}





function addData(chart, label, data) {
    chart.data.labels.push(label);
    chart.data.datasets.forEach((dataset) => {
        dataset.data.push(data);
    });
    chart.update();
}

function removeData(chart) {
    chart.data.labels.pop();
    chart.data.datasets.forEach((dataset) => {
        dataset.data.pop();
    });
    chart.update();
}


var labels = [];

//to get last 7 days report
function getDayly() {
    var today_ = today.getDay();
    var diff = 6 - today_;
    for (i = 0, j = today_ + 1; j <= 6; i++, j++) {
        labels[i] = DAYS[j];
    }
    for (j = 0; j <= today_; i++, j++) {
        labels[i] = DAYS[j];
    }
    for (i = 0; i < 7; i++) {
        myChart.data.datasets[0].data[i] = dayCounter[i][0];
       // myLineChart.data.datasets[1].data[i] = dayCounter[i][0];
    }
    for (i = 0; i < 7; i++) {
        myChart.data.datasets[1].data[i] = dayCounterR[i][0];
        //myLineChart.data.datasets[1].data[i] = dayCounter[i][0];
    }
   // console.log("it is daycounter: " + dayCounter);
    myChart.update();
    //myLineChart.update();
}

//to get last 5 weeks report
function getWeekly() {
    labels.remove(labels[6]);
    labels.remove(labels[5]);
    for (i = 0; i <= 4; i++) {
        labels[i] = WEEKS[i];
    }
    myChart.update();
}

//to get last 5 months report
function getPreviousMonths(number) {
    labels.remove(labels[6]);
    labels.remove(labels[5]);
    var currentMonth = today.getMonth();

    if (number < 12 && number > 0) {
        if (currentMonth >= number) {
            for (i = 0; i < number; i++) {
                labels[i] = MONTHS[currentMonth - number + i];
            }

        }
        else {
            var diff = number - currentMonth;
            var i = 0;
            for (; i < currentMonth; i++) {
                labels[i] = MONTHS[i];
            }
            for (j = 0; j < diff; i++, j++) {
                labels[i] = MONTHS[11 - j];
            }

        }

        labels[i] = MONTHS[currentMonth];
        // return prMonth;
    }
    else {
        number = 11;
        return getPreviousMonths(number);
    }

    for (i = 0; i < 5; i++) {
        myChart.data.datasets[0].data[i] = monthCounter[i];
    }
    for (i = 0; i < 5; i++) {
        myChart.data.datasets[1].data[i] = monthCounterR[i];
    }
    //console.log("it is monthcounter: " + monthCounter);
    myChart.update();
}

var DAYS = ['Якшанба', 'Душанба', 'Сешанба', 'Чоршанба', 'Пайшанба', 'Жума', 'Шанба'];
var WEEKS = ['First Week', 'Second Week', 'Third Week', 'Last Week', 'This Week'];
var MONTHS = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];

var color = Chart.helpers.color;


/*----Bar chart for request statistics ----*/

var ctx = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: labels,
        datasets: [{
            label: 'Юборилган',
            backgroundColor: color(window.chartColors.green).alpha(0.2).rgbString(),
            borderColor: window.chartColors.green,
            borderWidth: 1,
            data: [
                1,
                2,
                3,
                4,
                5
            ]
        }, {
             label: 'Бажарила олинмаган',
            backgroundColor: color(window.chartColors.red).alpha(0.2).rgbString(),
            borderColor: window.chartColors.red,
            borderWidth: 1,
            data: [
                1,
                2,
                3,
                2,
                1,
                2,
                3
            ]
        }
        ]

    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        responsive: true,
        legend: {
            position: 'top',
        }
    }
});

