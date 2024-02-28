function startClick() {
  var startPostTest = document.getElementById("startPostTest");
  var formQuestion = document.getElementById("quiz");

  startPostTest.classList.add("post-test-start");
  formQuestion.classList.remove("post-test-start");
  setTimeout(startPostTest.classList.add("d-none"), 0.2);
}

var currentQuestion = 1;
var totalQuestions = 2;

document.getElementById("question-" + currentQuestion).classList.add("active");

// Next button
var nextButton = document.getElementsByClassName("next-button");
for (var i = 0; i < nextButton.length; i++) {
  nextButton[i].addEventListener("click", function () {
    if (currentQuestion < totalQuestions) {
      document
        .getElementById("question-" + currentQuestion)
        .classList.remove("active");
      currentQuestion++;
      document
        .getElementById("question-" + currentQuestion)
        .classList.add("active");
    }
    // } else {
    //   // alert("Quiz selesai!");
    //   document
    //     .getElementById("question-" + currentQuestion)
    //     .classList.remove("active");
    //   currentQuestion++;
    //   document.getElementById("finish-question").classList.add("active");
    // }
  });
}

// Previous button
var prevButton = document.getElementsByClassName("prev-button");
for (var i = 0; i < prevButton.length; i++) {
  prevButton[i].addEventListener("click", function () {
    if (currentQuestion > 1) {
      document
        .getElementById("question-" + currentQuestion)
        .classList.remove("active");
      currentQuestion--;
      document
        .getElementById("question-" + currentQuestion)
        .classList.add("active");
    }
  });
}

// -----------------

var countDownDate = new Date().getTime() + 60000;

// Update the count down every 1 second
var x = setInterval(function () {
  // Get today's date and time
  var now = new Date().getTime();

  // Find the distance between now and the count down date
  var distance = countDownDate - now;

  // Time calculations for days, hours, minutes and seconds
  var days = Math.floor(distance / (1000 * 60 * 60 * 24));
  var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
  var seconds = Math.floor((distance % (1000 * 60)) / 1000);

  // Output the result in an element with id="timer"
  document.getElementById("timer").innerHTML =
    hours + "h " + minutes + "m " + seconds + "s ";

  // If the count down is over, write some text
  if (distance < 0) {
    timeOut();
  }
}, 1000);
