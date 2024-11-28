// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Bar Chart Example
var ctxHabitaciones = document.getElementById("habitacionesChart");
var myLineChart = new Chart(ctxHabitaciones, {
  type: 'bar',
  data: {
    labels: ["January", "February", "March", "April", "May", "June"],
    datasets: [{
        label: 'Habitaciones',
      backgroundColor: "rgba(2,117,216,1)",
      borderColor: "rgba(2,117,216,1)",
        data: habitacionesData.data,
    }],
  },
  options: {
    scales: {
      xAxes: [{
        time: {
          unit: 'month'
        },
        gridLines: {
          display: false
        },
        ticks: {
          maxTicksLimit: 6
        }
      }],
      yAxes: [{
        ticks: {
          min: 0,
          max: 15000,
          maxTicksLimit: 5
        },
        gridLines: {
          display: true
        }
      }],
    },
    legend: {
      display: false
    }
  }
});
// Gráfico de Habitaciones
//var ctxHabitaciones = document.getElementById('habitacionesChart').getContext('2d');
//new Chart(ctxHabitaciones, {
//    type: 'pie',
//    data: {
//        labels: habitacionesData.labels,
//        datasets: [{
//            label: 'Habitaciones',
//            data: habitacionesData.data,
//            backgroundColor: [
//                'rgba(255, 99, 132, 0.2)',
//                'rgba(54, 162, 235, 0.2)'
//            ],
//            borderColor: [
//                'rgba(255, 99, 132, 1)',
//                'rgba(54, 162, 235, 1)'
//            ],
//            borderWidth: 1
//        }]
//    },
//    options: {
//        responsive: true
//    }
//});