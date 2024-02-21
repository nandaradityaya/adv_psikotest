function toggleBtn(evt, menuName) {
  // Declare all variables
  var i, tabcontent, tablinks;

  // Get all elements with class="tabcontent" and hide them
  tabcontent = document.getElementsByClassName("invitation-body");
  for (i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.transform = "scaleY(0.2)";
    tabcontent[i].style.display = "none";
  }

  // Get all elements with class="tablinks" and remove the class "active"
  tablinks = document.getElementsByClassName("tablinks");
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" selected", "");
  }

  // Show the current tab, and add an "active" class to the button that opened the tab
  document.getElementById(menuName).style.display = "block";
  evt.currentTarget.className += " selected";
  setTimeout(() => {
    document.getElementById(menuName).style.transform = "scaleY(1)";
  }, 50);
}

function btnPsikotestResult(evt, menuName) {
  // Declare all variables
  var i, tabcontent, tablinks;

  // Get all elements with class="tabcontent" and hide them
  tabcontent = document.getElementsByClassName("psikotest-result");
  for (i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.transform = "scaleY(0.2)";
    tabcontent[i].style.display = "none";
  }

  // Get all elements with class="tablinks" and remove the class "active"
  tablinks = document.getElementsByClassName("tab-result");
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" selected", "");
  }

  // Show the current tab, and add an "active" class to the button that opened the tab
  document.getElementById(menuName).style.display = "block";
  evt.currentTarget.className += " selected";
  setTimeout(() => {
    document.getElementById(menuName).style.transform = "scaleY(1)";
  }, 50);
}
