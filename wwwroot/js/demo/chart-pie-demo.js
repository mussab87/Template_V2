//// Set new default font family and font color to mimic Bootstrap's default styling
//Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
//Chart.defaults.global.defaultFontColor = '#858796';

var Present = document.getElementById('Present').value;
var Late = document.getElementById('Late').value;
var NotCome = document.getElementById('NotCome').value;
var Authorized = document.getElementById('Authorized').value;
var Vacation = document.getElementById('Vacation').value;

// Pie Chart Example
var ctx = document.getElementById("myPieChart");


var myPieChart = new Chart(ctx, {
  type: 'doughnut',
  data: {
      labels: ["غائب", "اجازة", "مستأذن", "متأخر", "حاضر"],
    datasets: [{
        data: [NotCome, Vacation, Authorized, Late, Present],
        backgroundColor: ["#77672e", '#bda87d', '#8e7c27', "#9d4124", "#b17e14"],
      //hoverBackgroundColor: ['#9d7d24', '#17a673', '#2c9faf'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: true,
      caretPadding: 10,
    },
    legend: {
        display: true
    },
    cutoutPercentage: 80,
  },
});
