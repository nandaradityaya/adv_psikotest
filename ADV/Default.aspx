<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=1,initial-scale=1,user-scalable=1" />
    <title>DCT Recruitment | Login</title>
    <!-- FontAwesome 4.3.0 -->
    <link href="./assets/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="assets/img/index.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/styles-login.css" />

    <style>
        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        } 
    </style>
    <script>
        function myFunction() {
            var x = document.getElementById("passwd");
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
        function showPass() {
            var eye = document.getElementById("eye");
            var pass = document.getElementById("passwd");
            var value = document.getElementById('passwd').value;

            if (value.length > 0) {
                eye.style.display = "block";
            }
            if (pass.type === "password") {
                pass.type = "text";
                eye.classList.toggle('fa-eye');
                eye.classList.toggle('fa-eye-slash');
            } else {
                pass.type = "password";
                eye.classList.toggle('fa-eye');
                eye.classList.toggle('fa-eye-slash');
            }
        }
    </script>
</head>
<body>

    <section class="container login-form">
        <section>
            <form id="form1" runat="server" method="post" autocomplete="off">
                <img src="assets/images/logo.png" alt="" class="img-responsive"  />
                <span class="login-text">
                    Login
                </span>
                <div class="form-group">
                    <span class="login-input-hint">
                        User ID / Email / No Hp
                    </span>
                    <input type="text" name="useid" class="form-control" autocomplete="off" required />
                </div>
                <div class="form-group">
                    <span class="login-input-hint">
                        Password
                        <span class="login-input-hint" style="float: right;"><i class="fa fa-eye" id="eye" onclick="showPass();"></i>Hide</span>
                    </span>
                    <br />
                    <input type="Password" id="passwd" name="passwd" class="form-control" autocomplete="off" required />
                </div>
                <asp:Button ID="btnlogin" runat="server" Text="Sign in" CssClass="btn btn-primary btn-block btn-login" />
            </form> 
        </section>
    </section>
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>

</body>
</html>
