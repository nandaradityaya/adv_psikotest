<!DOCTYPE html>
<html lang="en" class="color-sidebar sidebarcolor3">
  <head>
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!--favicon-->
    <link rel="icon" href="../assets/newAssets/images/index.ico" type="image/png" />
    <!--plugins-->
    <link href="../assets/newAssets/plugins/simplebar/css/simplebar.css" rel="stylesheet" />
    <link
      href="../assets/newAssets/plugins/fullcalendar/css/main.min.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/perfect-scrollbar/css/perfect-scrollbar.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/metismenu/css/metisMenu.min.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/highcharts/css/highcharts.css"
      rel="stylesheet"
    />
    <link rel="stylesheet" href="../assets/newAssets/libraries/gijgo/css/gijgo.min.css" />
    <link href="../assets/newAssets/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link
      href="../assets/newAssets/plugins/select2/css/select2-bootstrap4.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/datatable/css/dataTables.bootstrap5.min.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/datetimepicker/css/classic.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/datetimepicker/css/classic.time.css"
      rel="stylesheet"
    />
    <link
      href="../assets/newAssets/plugins/datetimepicker/css/classic.date.css"
      rel="stylesheet"
    />
    <link
      rel="stylesheet"
      href="../assets/newAssets/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.min.css"
    />
    <link
      rel="stylesheet"
      href="../assets/newAssets/plugins/richtexteditor/rte_theme_default.css"
    />
    <!-- loader-->
    <link href="../assets/newAssets/css/pace.min.css" rel="stylesheet" />
    <script src="../assets/newAssets/js/pace.min.js"></script>
    <!-- Bootstrap CSS -->
    <link href="../assets/newAssets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../assets/newAssets/css/bootstrap-extended.css" rel="stylesheet" />
    <link
      href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500&display=swap"
      rel="stylesheet"
    />
    <link href="../assets/newAssets/css/app.css" rel="stylesheet" />
    <link href="../assets/newAssets/css/icons.css" rel="stylesheet" />
    <!-- Theme Style CSS -->
    <!-- <link rel="stylesheet" href="../assets/newAssets/css/dark-theme.css" /> -->
    <link rel="stylesheet" href="../assets/newAssets/css/semi-dark.css" />
    <link rel="stylesheet" href="../assets/newAssets/css/header-colors.css" />
    <title>DCT WEB</title>
  </head>

  <body
    style="
      background-image: url(../assets/newAssets/images/gallery/bg-psikotest-v3.jpg);
      background-repeat: no-repeat;
      background-attachment: fixed;
      background-size: cover;
    "
  >
    <!--wrapper-->
    <div class="wrapper">
      <img
        src="../assets/newAssets/images/gallery/plus.svg"
        alt=""
        class="icon-plus-validation"
        width="20"
      />
      <img
        src="../assets/newAssets/images/gallery/round.svg"
        alt=""
        class="icon-round-validation"
        width="20"
      />
      <div
        class="section-authentication-signin d-flex align-items-center justify-content-center my-5 my-lg-0"
      >
        <div class="container-fluid">
          <div class="row row-cols-1 row-cols-lg-2 row-cols-xl-2">
            <div class="col mx-auto">
                <div class="text-center mb-4">
                  <a href="#" class="text-primary" style="font-family: 'Segoe UI Black'">
                    <span style="font-size: 24px; font-style: italic">DCT</span>
                    <span style="font-size: 20px; text-decoration: overline"
                      >Recruitment</span
                    >
                  </a>
                </div>

              <!-- SHOW THIS IF PSIKOTEST NOT YET STARTED -->
             <div class="card radius-10">
                <div class="card-body text-center">
                  <div class="d-flex justify-content-center">
                    <dotlottie-player
                      src="https://lottie.host/6d3fde4b-0f21-456e-b10e-a375ef723656/cZshQ0jxxk.json"
                      background="transparent"
                      speed="1"
                      style="width: 300px; height: 300px"
                      loop
                      autoplay
                    ></dotlottie-player>
                  </div>
                  <div class="">
                    <p class="mb-2">
                      Psikotest untuk saat ini belum tersedia. Harap kembali
                      pada waktu yang telah ditentukan untuk memulai sesi
                      psikotest.
                    </p>
                    <h6 class="mb-2">29 Februari 2024 13.00</h6>
                    <p class="mb-0">
                      Terima kasih atas kesabaran dan pengertian Anda.
                    </p>
                  </div>
                </div>
              </div>
              <!-- END -->

              <!-- ------------------------------------ -->

              <!-- SHOW THIS IF PSIKOTEST EXPIRED -->
             <div class="card radius-10">
                <div class="card-body text-center">
                  <div class="d-flex justify-content-center">
                    <dotlottie-player
                      src="https://lottie.host/45dc3c14-6965-4811-b636-21738451cd92/1n6R8v9Qqn.json"
                      background="transparent"
                      speed="1"
                      style="width: 300px; height: 300px"
                      loop
                      autoplay
                    ></dotlottie-player>
                  </div>
                  <div class="">
                    <h6 class="mb-2">
                      Mohon maaf, Anda telah melewatkan batas waktu untuk
                      mengerjakan psikotest ini.
                    </h6>
                    <h6 class="mb-0">Terima kasih atas perhatiannya.</h6>
                  </div>
                </div>
              </div>
              <!-- END -->

              <!-- ------------------------------------------ -->

              <!-- SHOW THIS IF PSIKOTEST HAS BEEN DONE -->
              <div class="card radius-10">
                <div class="card-body text-center">
                  <div class="d-flex justify-content-center">
                    <dotlottie-player
                      src="https://lottie.host/8ff372ae-2af5-477b-a92c-1435ee981820/MzsSV7e2M7.json"
                      background="transparent"
                      speed="1"
                      style="width: 300px; height: 300px"
                      loop
                      autoplay
                    ></dotlottie-player>
                  </div>
                  <h6 class="mb-4">
                    Selamat! Anda telah menyelesaikan psikotest. Terima kasih
                    atas partisipasi Anda.
                  </h6>
                </div>
              </div>
              <!-- END -->
            </div>
          </div>
          <!--end row-->
        </div>
      </div>
      <div class="container"></div>
    </div>
    <!--end wrapper-->
    <!-- Bootstrap JS -->
    <script src="../assets/newAssets/js/bootstrap.bundle.min.js"></script>
    <!--plugins-->
    <script src="../assets/newAssets/js/jquery.min.js"></script>
    <script src="../assets/newAssets/plugins/simplebar/js/simplebar.min.js"></script>
    <script src="../assets/newAssets/plugins/fullcalendar/js/main.min.js"></script>
    <script src="../assets/newAssets/plugins/metismenu/js/metisMenu.min.js"></script>
    <script src="../assets/newAssets/plugins/perfect-scrollbar/js/perfect-scrollbar.js"></script>
    <script src="../assets/newAssets/libraries/gijgo/js/gijgo.min.js"></script>
    <script src="../assets/newAssets/plugins/select2/js/select2.min.js"></script>
    <script src="../assets/newAssets/plugins/datatable/js/jquery.dataTables.min.js"></script>
    <script src="../assets/newAssets/plugins/datatable/js/dataTables.bootstrap5.min.js"></script>
    <script src="../assets/newAssets/plugins/datetimepicker/js/legacy.js"></script>
    <script src="../assets/newAssets/plugins/datetimepicker/js/picker.js"></script>
    <script src="../assets/newAssets/plugins/datetimepicker/js/picker.time.js"></script>
    <script src="../assets/newAssets/plugins/datetimepicker/js/picker.date.js"></script>
    <script src="../assets/newAssets/plugins/bootstrap-material-datetimepicker/js/moment.min.js"></script>
    <script src="../assets/newAssets/plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.min.js"></script>
    <script
      type="text/javascript"
      src="../assets/newAssets/plugins/richtexteditor/rte.js"
    ></script>
    <script
      type="text/javascript"
      src="../assets/newAssets/plugins/richtexteditor/plugins/all_plugins.js"
    ></script>

    <script src="../assets/newAssets/js/index4.js"></script>
    <script src="../assets/newAssets/js/toggle-button.js"></script>
    <script src="../assets/newAssets/js/psikotest.js"></script>
    <script
      src="https://unpkg.com/@dotlottie/player-component@latest/dist/dotlottie-player.mjs"
      type="module"
    ></script>

    <!--app JS-->
    <script src="../assets/newAssets/js/app.js"></script>
  </body>
</html>
