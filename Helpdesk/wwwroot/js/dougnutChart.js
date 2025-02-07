 
$.ajax({
    url: '/HeadManager/Requests/GetRequests',
    dataType: 'json',
    success: function (data) {
        if (data != null) {
            //toastr.success("blabal");
            //var date = new Date(data.data[0].rDateTime);
            //console.log("it is rejecteds " + date.getDay());
            assign(data);
        }
        else {
            toastr.error("Error");
        }
    }
});
var dataa = [0, 0, 0, 0, 0];

function assign(datas) {
    for (i = 0; i < datas.data.length; i++) {
        if (datas.data[i].rStatus == "blue") {
            dataa[0]++;
        }
        if (datas.data[i].rStatus == "Taken") {
            dataa[1]++;
        }
        if (datas.data[i].rStatus == "yellow") {
            dataa[2]++;
        }
        if (datas.data[i].rStatus == "red") {
            dataa[3]++;
        }
        if (datas.data[i].rStatus == "green") {
            dataa[4]++;
        }

        console.log("data: " + datas.data[i].rStatus);
    }
    
    for (j = 0; j < 5; j++) {
        myDoughnutChart.data.datasets[0].data[j] = dataa[j];
       
    }
    myDoughnutChart.update();
}




var ctx_ = document.getElementById('meChart').getContext('2d');
var myDoughnutChart = new Chart(ctx_, {
    type: 'doughnut',
    data: {
        labels: [
            'Янгилари',
            'Бажарилаётган',
            'Тайинланган',
            'Қайтарилган',
            'Тугатилган'

        ],
        datasets: [{
            data: [10,20, 30, 40, 50],
            backgroundColor: ["rgba(17, 212, 205, 0.5)",
                              "rgba(186, 227, 36, 0.5)",
                              "rgba(235, 175, 47, 0.5)",
                              "rgba(135, 8, 8, 0.5)",
                              "rgba(42, 135, 8, 0.5)"],
           
            borderColor: ["rgba(17, 212, 205, 1)",
                          "rgba(186, 227, 36, 1)",
                          "rgba(235, 175, 47, 1)",
                          "rgba(135, 8, 8, 1)",
                          "rgba(42, 135, 8, 1)"],
            
        }],
        
        
    }
    

});