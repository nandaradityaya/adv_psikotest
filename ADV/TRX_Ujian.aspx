<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TRX_Ujian.aspx.vb" Inherits="TRX_Ujian" %>

<!DOCTYPE html>
<html>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
    
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="./assets/img/index.ico" type="image/x-icon" />
    <link href="./assets/css/bootstrap-responsive.css" rel="stylesheet" />
    <link href="./assets/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" />
    <link href="./assets/css/fullcalendar.css" rel="stylesheet" />
    <link href="./assets/css/fullcalendar.print.css" rel="stylesheet" media="print" />

    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>
    <%--<meta name="viewport" content="width=1024">--%>
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0">--%>
    <!-- Bootstrap 4 -->
    <link href="./assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- For version bootstrap 4 -->
     
    <!-- jQuerry -->
    <script src="./assets/plugins/jQuery/jquery-3.1.1.min.js"></script>

    <!-- Ace Admin -->
    <link rel="stylesheet" href="assets/css/ace.min.css" type="text/css" />

    <!-- FontAwesome 4.3.0 -->
    <link href="./assets/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Ionicons 2.0.0 -->
    <link href="./assets/dist/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <%--<link href="./assets/dist/css/AdminLTE.min.css" rel="stylesheet" type="text/css" />--%>
    <!-- For version bootstrap 4, -->

    <!-- AdminLTE Skins. Choose a skin from the css/skins 
         folder instead of downloading all of them to reduce the load. -->
    <%--<link href="./assets/dist/css/skins/skin-blue.min.css" rel="stylesheet" type="text/css" />--%>

    <meta charset="UTF-8">
    <title>DCT - Psikotest</title>

    <!-- ganti semua file di folder skin menggunakan latest version -->
    <style type="text/css">
        .ReadOnly {
            background-image: none;
            background-color: #7FFFD4;
        }
        a {
            text-decoration: none;
        }
        button:focus {
            outline: none;
        }
        .blured {
            filter: blur(10px);
            -webkit-filter: blur(10px);
            background-color: rgba(255, 255, 255, 0.5);
            backdrop-filter: blur(1px);
            display: flex;
            width: 100%;
            height: 100vh;
        }
        #tes {
        }

        .prevent-select {
            -webkit-user-select: none; /* Safari */
            -ms-user-select: none; /* IE 10 and IE 11 */
            user-select: none; /* Standard syntax */
        }
    </style>
    <script type="text/javascript">
        var popupWindow = null;

        function centeredPopup(url, winName, w, h, scroll) {
            LeftPosition = (screen.width) ? (screen.width - w) / 2 : 0;
            TopPosition = (screen.height) ? (screen.height - h) / 2 : 0;
            settings =
                'height=' + h + ',width=' + w + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=' + scroll + ',resizable=no, fullscreen=no'
            popupWindow = window.open(url, winName, settings)
        }

        function preventBack() { window.history.forward(); }

        setTimeout("preventBack()", 0);

        window.onunload = function () { null };

    </script>

    <script type="text/javascript">
        
        window.addEventListener('focus', () => {
            console.log('focus');
            document.getElementById("tes").classList.remove("blured");
        });

        window.addEventListener('blur', () => {
            console.log('leave');
            document.getElementById("tes").classList.add("blured");
        });

        const AwalanID = "btn-navigasi-";
        let connectionLostCounter = 0;

        function connectionLost() {
            if (connectionLostCounter > 5) {
                Ext.toast('Koneksi terputus');
                connectionLostCounter = 0;
            } else {
                connectionLostCounter += 1;
            }
        }

        function nfChanged() {
            var nf = document.getElementById("nfJawabanKreplin-inputEl");
            //console.log(nf.value);

            var n = nf.value;
            if (!isNaN(parseFloat(n)) && isFinite(n)) { //cek numeric
                if (n.length > 1) {
                    n = n.substring(0, 1);
                }
            } else {
                n = n.substring(0, 1);
                if (!isNaN(parseFloat(n)) && isFinite(n)) { //cek kalo karakter pertamanya numeric
                } else {
                    n = '';
                }
            }
            nf.value = n;
        }

        function resize() {
            if (screen.width < 800) {
                document.getElementById("widgetMain").style.flexDirection = "column";
                let wBoxEl = document.getElementsByClassName('w-box-custom');
                for (var i = 0; i < wBoxEl.length; i++) {
                    wBoxEl[i].classList.add("auto-width");
                }

                document.getElementById("widgetMainChild1").classList.add('auto-width');
                document.getElementById("divContainerbtnNavBody1").classList.add('auto-width');
                document.getElementById("divContainerbtnNavBody1").classList.add('justify-content_center');
                // winVerifikasi
                document.getElementById("dtTglLahir").classList.add('width-300');
                document.getElementById("btnwinVerifikasiPeserta_Submit").classList.add('width-300');
                // winPetunjuk
                document.getElementById("toolbar-1014-innerCt").classList.add('overflow-scroll');

                App.PanelWest.setWidth(0);
                App.PanelEast.setWidth(0);
                
                App.winVerifikasiPeserta_lblIdentifikasiData.addCls('font-30');
                App.winVerifikasiPeserta_lblPerintah.addCls('font-15');

                App.winVerifikasiPeserta_lblIdentifikasiData.setWidth(300);
                App.winVerifikasiPeserta_lblPerintah.setWidth(300);
                App.winVerifikasiPeserta_lblWarning.setWidth(300);
                App.nfNoKTP.setWidth(300);

                App.lblSelesaiUjian1.addCls('font-15');
                App.lblSelesaiUjian2.addCls('font-15');
                App.lblSelesaiUjian3.addCls('font-15');
            } else {
                document.getElementById("widgetMain").style.flexDirection = "row";
                let wBoxEl = document.getElementsByClassName('w-box-custom');
                for (var i = 0; i < wBoxEl.length; i++) {
                    wBoxEl[i].classList.remove("auto-width");
                }

                document.getElementById("widgetMainChild1").classList.remove('auto-width');
                document.getElementById("divContainerbtnNavBody1").classList.remove('auto-width');
                document.getElementById("divContainerbtnNavBody1").classList.remove('justify-content_center');
                // winVerifikasi
                document.getElementById("dtTglLahir").classList.remove('width-300');
                document.getElementById("btnwinVerifikasiPeserta_Submit").classList.remove('width-300');
                // winPetunjuk
                document.getElementById("toolbar-1014-innerCt").classList.remove('overflow-scroll');

                App.PanelWest.setWidth(100);
                App.PanelEast.setWidth(100);
                
                App.winVerifikasiPeserta_lblIdentifikasiData.removeCls('font-30');
                App.winVerifikasiPeserta_lblPerintah.removeCls('font-15');

                App.winVerifikasiPeserta_lblIdentifikasiData.setWidth(400);
                App.winVerifikasiPeserta_lblPerintah.setWidth(400);
                App.winVerifikasiPeserta_lblWarning.setWidth(400);
                App.nfNoKTP.setWidth(400);

                App.lblSelesaiUjian1.removeCls('font-15');
                App.lblSelesaiUjian2.removeCls('font-15');
                App.lblSelesaiUjian3.removeCls('font-15');
            }
        }

        function sleep(ms) {
            return new Promise(resolve => setTimeout(resolve, ms));
        }

        var loadedCounter = 0;
        var OldViewSize;

        function loaded() {
            webcam = document.getElementById('webcam');
            snapshotCanvas = document.getElementById('snapshotCanvas');
            if (loadedCounter > 2) {
                loadedCounter = 0;
                return;
            }
            loadedCounter += 1;

            var ViewSize = Ext.getBody().getViewSize();
            if (!(OldViewSize)) {
                if (ViewSize == OldViewSize) {
                    loaded();
                    return;
                }
            }
            OldViewSize = ViewSize;

            var counter = 0;
            while (App.winVerifikasiPeserta == null) {
                if (counter == 100) {
                    console.log("Ga keburu");
                    location.reload();
                    return;
                }
                sleep(100);
                counter += 1;
            }
            document.getElementById("winVerifikasiPeserta_lblIdentifikasiData-textEl").classList.add('dispFlex');
            document.getElementById("winVerifikasiPeserta_lblIdentifikasiData-textEl").classList.add('justify-content_center');

            var ViewSize = Ext.getBody().getViewSize();

            var w_num = 0.87;
            var h_num = 0.78;
            //console.log('winverifikasipeserta');
            App.winVerifikasiPeserta_Form.setSize(ViewSize.width * w_num, ViewSize.height * h_num);
            //App.winVerifikasiPeserta.setSize(ViewSize.width * w_num, ViewSize.height * h_num);
            //App.winVerifikasiPeserta.center();
            
            var num = 0.93;
            //console.log('winpetunjuk');
            App.winPetunjuk_Form.setSize(ViewSize.width * num, ViewSize.height * num);
            //App.winPetunjuk.setSize(ViewSize.width * num, ViewSize.height * num);
            //App.winPetunjuk.center();

            //console.log('winselesaiujian');
            App.winSelesaiUjian_Form.setSize(ViewSize.width * w_num, ViewSize.height * h_num);
            //App.winSelesaiUjian.setSize(ViewSize.width * w_num, ViewSize.height * h_num);
            //App.winSelesaiUjian.center();
            
            var nf = document.getElementById("nfJawabanKreplin-inputEl");
            nf.addEventListener("input", nfChanged);
            nf.value = '';
            
            resize(); // Buat Resolusi mobile

            //Buat cegah spasi di mobile
            document.getElementById('nfNoKTP-inputEl').addEventListener('keyup', nfNoKTP_Change);

            setTimeout(args => {
                //console.log('timeouted 200');
                var h_logo = 0.1;
                App.DCT_Logo.setSize((App.DCT_Logo.getWidth() / App.DCT_Logo.getHeight()) * ViewSize.height * h_logo, ViewSize.height * h_logo);
                App.DCT_Logo.setPosition(App.winVerifikasiPeserta_Form.getWidth() - App.DCT_Logo.getWidth() - 30, App.winVerifikasiPeserta_Form.getHeight() - App.DCT_Logo.getHeight() - 30);

                if (!App.winVerifikasiPeserta.hidden) {
                    App.winVerifikasiPeserta.hide();
                    App.winVerifikasiPeserta.show();
                }

                let ScrollToCenter = parseInt(document.getElementById('winVerifikasiPeserta_Form-innerCt').style.width.slice(0, -2)) - parseInt(document.getElementById('winVerifikasiPeserta_Form-body').style.width.slice(0, -2));
                ScrollToCenter = (ScrollToCenter > 25 ? ScrollToCenter / 2 : ScrollToCenter);
                //console.log(ScrollToCenter);
                App.winVerifikasiPeserta_Form.setScrollX(ScrollToCenter);

                loaded();
            }, 200);
        }

        window.onload = loaded;
        window.onresize = loaded;

        var hiddenCamTimer = false;

        var webcam;
        var snapshotCanvas;

        let mediaStream;
        let snapshotInterval;

        function winVerifikasiPeserta_Submit_Click() {
            App.direct.winVerifikasiPeserta_Submit_Click();
        }
        function winVerifikasiPeserta_Submit_Click_Berhasil() {
            if (!hiddenCamTimer) {
                navigator.mediaDevices.getUserMedia({ video: true })
                    .then(stream => {
                        mediaStream = stream;
                        webcam.srcObject = stream;
                        startSnapshotInterval();
                    })
                    .catch(error => {
                        console.error('Gagal mengakses webcam:', error);
                    });
                hiddenCamTimer = true;
            }
        }
        function startSnapshotInterval() {
            var intervalWaktuFoto = App.hdnIntervalWaktuFoto.getValue();
            var realInterval = 10;
            if (!isNaN(intervalWaktuFoto)) {
                realInterval = intervalWaktuFoto;
            }


            startSnapshotInit = setTimeout(() => {
                takeSnapshot();
            }, 1 * 60 * 1000);
            snapshotInterval = setInterval(() => {
                takeSnapshot();
            }, realInterval * 60 * 1000);
        }
        function takeSnapshot() {
            const context = snapshotCanvas.getContext('2d');
            context.drawImage(webcam, 0, 0, snapshotCanvas.width, snapshotCanvas.height);
            const snapshotDataURL = snapshotCanvas.toDataURL('image/png');
            
            const formData = new FormData();
            formData.append('imageData', snapshotDataURL);

            $.ajax({
                    type: "POST",
                    url: "./SaveSnapshot.ashx",
                    data: formData,
                    contentType: false,
                    processData:false,
                });
        }

        function winVerifikasiPeserta_lblWarning_Reload(num) {
            //console.log('reload winverikasi num -> ' + num.toString());
            App.winVerifikasiPeserta_lblPerintah.setHtml('Jika 3x identifikasi salah, maka <br/>Anda tidak dapat mengerjakan ujian');

            App.winVerifikasiPeserta_lblWarning.removeCls('visibilityHidden');
            App.winVerifikasiPeserta_lblWarning.setText('Data yang Anda masukkan salah, kesempatan Anda tinggal ' + num.toString() + 'x lagi.');
            loaded();
        }
        var GoToOnGoing = 0;
        function setGoToOnGoing(num) {
            GoToOnGoing = num;
        }

        function navGoTo(sender, value) {
            //console.log(sender);
            if (GoToOnGoing == 0) {
                var navNo = document.getElementById(sender.target.id).innerText.toString();
                //console.log('navGoTo >> ', navNo);
                GoToOnGoing = 1;
                App.direct.navTo(navNo);
            }
        }

        var CurrentNumber = 0;
        function setCurrentNumber(i) {
            CurrentNumber = i;
            //console.log("Set CurrentNumber: " + AwalanID + CurrentNumber);
            var nav_btn = document.getElementById(AwalanID + CurrentNumber);
            nav_btn.classList.add("soal_aktif");
            nav_btn.classList.remove("soal_kosong");
            nav_btn.classList.remove("soal_terjawab");
        }

        var UrutanNoGroup = 0;
        function setUrutanNoGroup(i) {
            UrutanNoGroup = i;
        }

        var SoalTerjawab = [];
        function setSoalTerjawab(i) {
            SoalTerjawab = [];
            SoalTerjawab = SoalTerjawab.concat(JSON.parse(i));
            //console.log("SoalTerjawab: " + SoalTerjawab);
            reloadNavBtn();
        }

        var TotalSoal = 0;
        function setTotalSoal(num) {
            TotalSoal = num;
            setupNavigasi(num);
        }

        function resetColor(_id) {
            var nav_btn = document.getElementById(_id);
            var nomor = nav_btn.innerText;
            if (nav_btn == null) {
                return;
            }
            nav_btn.blur();
            //console.log("Reset color: " + nomor);

            if (nomor == CurrentNumber.toString()) {
                //console.log("Masuk aktif");
                nav_btn.classList.add("soal_aktif");
                nav_btn.classList.remove("soal_terjawab");
                nav_btn.classList.remove("soal_kosong");
            } else if (SoalTerjawab.includes(nomor.toString())) {
                //console.log("Masuk terjawab");
                nav_btn.classList.add("soal_terjawab");
                nav_btn.classList.remove("soal_aktif");
                nav_btn.classList.remove("soal_kosong");
            } else {
                //console.log("Masuk kosong");
                nav_btn.classList.add("soal_kosong");
                nav_btn.classList.remove("soal_aktif");
                nav_btn.classList.remove("soal_terjawab");
            }
        }

        function reloadNavBtn() {
            for (var i = 1; i <= TotalSoal; i++) {
                resetColor(AwalanID + i);
            }
        }

        function ShowThis(_id) {
            var el = document.getElementById(_id);
            if (el == null) {
                return;
            }
            el.classList.remove("dispNone");
            el.classList.add("dispBlock");
        }
        function HideThis(_id) {
            var el = document.getElementById(_id);
            if (el == null) {
                return;
            }
            el.classList.remove("dispBlock");
            el.classList.remove("dispFlex");
            el.classList.add("dispNone");
        }
        function FlexThis(_id) {
            var el = document.getElementById(_id);
            if (el == null) {
                return;
            }
            el.classList.remove("dispNone");
            el.classList.add("dispFlex");
        }

        function nfJawabanKreplin_Down() {
            if (App.nfJawabanKreplin.getValue() == " ") {
                if (App.nfJawabanKreplin.getValue() !== 0) {
                document.getElementById("LblKreplinPrompt1").classList.remove("transparent");
                document.getElementById("LblKreplinPrompt2").classList.remove("transparent");
                    Ext.toast('Jawaban harus diisi');
                    return;
                }
            }
            document.getElementById("LblKreplinPrompt1").classList.add("transparent");
            document.getElementById("LblKreplinPrompt2").classList.add("transparent");
            App.direct.nfKreplin_Down();
        }

        function setupNavigasi(num) {
            var container = document.getElementById("divContainerbtnNavBody1");
            while (container.firstChild) {
                container.removeChild(container.lastChild);
            }
            for (var i = 1; i <= num; i++) {
                var span = document.createElement("span");
                span.id = "span-btn-navigasi-" + i;
                span.classList.add("inner-tombol_navigasi");
                span.unselectable = "on";
                span.innerHTML = i;

                var button = document.createElement("button");
                button.id = "btn-navigasi-" + i;
                button.classList.add("tombol_navigasi");
                button.classList.add("soal_kosong");
                button.type = "button";
                button.addEventListener("click", navGoTo);
                button.appendChild(span);

                container.appendChild(button);
            }
        }

        function setPilihanRadio(s) {
            var data = JSON.parse(s);

            var container = document.getElementById("divContainerRadio");
            while (container.firstChild) {
                container.removeChild(container.lastChild);
            }
            for (var i = 0; i < data.length; i++) {
                var input = document.createElement("input");
                input.id = "R" + data[i].i;
                input.name = "RGroup1";
                input.type = "radio";
                input.classList.add("ace");

                var span = document.createElement("span");
                span.classList.add("lbl");
                span.innerHTML = "   " + data[i].Jwb;

                var label = document.createElement("label");
                label.id = "lbl" + data[i].i;
                label.classList.add("clsRadio");
                label.appendChild(input);
                label.appendChild(span);
                label.addEventListener("change", radioChanged);

                var div = document.createElement("div");
                div.classList.add("radio");
                div.appendChild(label);

                if ("link" in data[i]) {
                    var iframe = document.createElement("iframe");
                    iframe.width = 560;
                    iframe.height = 315;
                    iframe.frameBorder = 0;
                    iframe.src = data[i].link;
                    label.appendChild(iframe);
                } else if ("img" in data[i]) {
                    var img = document.createElement("img");
                    img.style = "padding: 15px;";
                    img.src = data[i].img;
                    img.alt = span.innerHTML;
                    label.appendChild(img);

                    span.innerHTML = "";
                    label.style = "margin: 6px 3px;";
                    div.style = "width: max-content;";
                }

                container.appendChild(div);
            }
        }
        function radioChanged(e) {
            //console.log(e);
            var container = document.getElementById("divContainerRadio");
            for (let child of container.children) {
                child.lastChild.classList.remove("soal_terjawab");
            }

            //var el = document.getElementById(e.path[1].id);
            var el = document.getElementById(e.target.id).parentElement;
            el.classList.add("soal_terjawab");

            App.direct.JawabSoal(el.id.substring(3));
        }

        function setPilihanDiJawab(num) {
            var radio = document.getElementById("R" + num);
            radio.checked = true;

            var container = document.getElementById("divContainerRadio");
            for (let child of container.children) {
                child.lastChild.classList.remove("soal_terjawab");
            }
            
            var el = document.getElementById("lbl" + num);
            el.classList.add("soal_terjawab");
        }

        function ModifyBtnSelesai(state) {
            if (state == 1) {
                document.getElementById("btnSelesai").disabled = false;
                document.getElementById("btnSelesai").classList.remove("disabled");
            } else {
                document.getElementById("btnSelesai").disabled = true;
            }
        }
        function showWinPetunjuk() {
            App.winPetunjuk.show();
        }
        function setInner(ID, s) {
            document.getElementById(ID).innerHTML = s;
        }
        function setHeightBodyPanel(s) {
            if (s == 0) {
                document.getElementById("PanelCenter-body").classList.add("heightFitContent");
            } else {
                document.getElementById("PanelCenter-body").classList.remove("heightFitContent");
            }
        }

        var lastNoKTP = ""
        function nfNoKTP_Change() { //Cegah spasi di hp
            let newInput = lastNoKTP;

            let newNoKTP = App.nfNoKTP.value.replaceAll(' ', '');
            let inputBenar = true;
            for (let i = 0; i < newNoKTP.length; i++) {
                let chCode = newNoKTP.charCodeAt(i);
                if (chCode < 48 || chCode > 57) {
                    inputBenar = false;
                    break;
                }
            }

            if (inputBenar)
                newInput = newNoKTP;

            App.nfNoKTP.value = newInput;
            document.getElementById('nfNoKTP-inputEl').value = newInput;

            lastNoKTP = newInput;
        }
    </script>
    <style type="text/css">
        .transparent {
            color: transparent;
        }
        .dispBlock {
            display: block !important;
        }
        .dispNone {
            display: none !important;
        }
        .dispFlex {
            display: flex !important;
        }
        .heightFitContent {
            /*height: auto !important;*/
            /*overflow-y: hidden;*/
            /*overflow: auto;
            display: block;*/
            /*height: auto !important;
            overflow:scroll;*/
        }
        .PlaceholderPanel { /* Isi Panel East, West */
            background-color: transparent;
            border-color: transparent;
            width: 100px !important;
        }
        .clsHeaderUjian { /* Isi Panel North */
            margin-bottom: 0;
            padding-left: 230px;
            border: none;
            min-height: 50px;
            border-radius: 0;
            background-color: #3c8dbc;
        }
        .clsFooter { /* Isi Panel South */
            padding: 15px;
            font-size: 0.85rem;
        }
        .clsLblVersion {
            vertical-align: baseline;
        }

        .clsWinVerifikasiPeserta_Form { /* Buat window verifikasi peserta */
            background-color: #fdf1e7;
            border-radius: 9px;
        }
        #winVerifikasiPeserta {
            background-color: rgba(0,0,0,0.4) !important;
        }
        #winVerifikasiPeserta-bodyWrap, #winVerifikasiPeserta-body {
            background-color: transparent;
        }
        #nfNoKTP-inputEl {
            text-align: center;
        }
        #nfNoKTP-inputWrap {
            border: none;
        }
        #dtTglLahir {
            background-image: url(/extjs/packages/theme_gray/build/resources/images/form/text-bg-gif/ext.axd);
            background-size: contain;
        }
        #DCT_Logo {
            opacity: 0.5;
        }
        .cls_btnwinVerifikasiPeserta {
            width: 400px;
            border-radius: 9px;
            background-color: white;
            color: black;
            font-size: 30px;
            -webkit-transition: ease-out 0.4s;
            -moz-transition: ease-out 0.4s;
        }
        .cls_btnwinVerifikasiPeserta:hover {
            box-shadow: inset 500px 0 0 0 lime;
        }
        .visibilityHidden {
            visibility: hidden;
        }
        .font-30 { /* Buat font mobile */
            font-size: 30px !important;
        }
        .font-15 {
            font-size: 15px !important;
        }
        .width-300 { /* Buat width mobile */
            width: 300px !important;
        }
        .content_left-50 {
            left: 50px !important;
        }

        .clsUrutanNoGroup { /* Buat label urutan group soal */
            line-height: 1.15;
            padding: 10px;
            margin: 6px;
            font-size: 15px;
            background-color: #32CD32;
            display: inline-block;
        }

        fieldset { /* Container divKreplin, divRadio */
            margin: 2px 2px;
            padding: 18px 10px 10px 19px;
            width: 100%;
            height: 100%;
        }

        /* Kreplin */
        .clsLblKreplin { /* Buat label prompt Kreplin */
            font-size: 70px;
            font-weight: bold;
            padding: 5px 20px;
            margin: 10px 15px;
        }
        input[type=text] {
            border-radius: 20px !important;
        }
        #nfJawabanKreplin-inputWrap { /* Buat matiin border ext:net */
            border-style: none;
        }
        .clsNfJawabanKreplinText { /* Buat NumberField Jawaban Kreplin */
            text-align: center;
            font-size: 1rem;
            font: bold 70px tahoma, arial, verdana, sans-serif !important;
            color: black !important;
            height: 87px;
            width: 98px;
            margin: 0;
            border: 2px solid #D5D5D5 !important;
        }
        .clsNfJawabanKreplinText:focus {
            border: 3px solid deepskyblue !important;
        }
        
        /* Navigasi */
        .lblNavigasi { /* Buat label navigasi */
            text-align: center; 
            color: white;
            background-color: #307ECC; 
            background-image: none;
            height: 100%;
            width: 100%;
            padding: 5px 0px;
            font-size: 21px;
        }
        .tombol_navigasi { /* Buat btn navigasi */
            border-radius: 15px;
            margin: 4px;
            border-style: none;
            background-image: none;
            width: 30px;
            height: 31px;
        }
        .tombol_navigasi:hover {
            background-color: rgba(166, 0, 0, 0.03);
        }
        .inner-tombol_navigasi { /* Buat text dalam btn navigasi */
            font-size: 14px;
            color: white;
        }
        .contoh_tombol_navigasi { /* Buat btn sample navigasi */
            border-radius: 15px;
            margin: 5px;
            border-style: none;
            background-image: none;
            width: 25px;
            height: 25px;
        }
        .justify-content_center { /* Buat tombol navigasi mobile */
            justify-content: center;
        }

        .soal_kosong {
            background-color: lightgrey !important;
            border-color: lightgrey !important;
        }
        .soal_aktif {
            background-color: gold !important;
            border-color: gold !important;
        }
        .soal_terjawab {
            background-color: deepskyblue !important;
            border-color: deepskyblue !important;
        }

        /* window Petunjuk*/
        #winPetunjuk {
            background-color: rgba(0,0,0,0.4) !important;
        }
        #winPetunjuk-bodyWrap, #winPetunjuk-body {
            background-color: transparent;
        }
        #winPetunjuk_Form {
            border-radius: 4px;
            background-color: gainsboro;
        }
        .JdlPetunjuk { /* Buat label judul petunjuk */
            font-size: 48px;
            margin-left: 12px !important;
        }
        #btnwinPetunjuk_Close-btnIconEl {
            line-height: 48px;
            background-size: 48px;
            height: 48px;
            width: 48px;
        }
        .overflow-scroll {
            overflow: scroll !important;
        }
        .KternganPetunjuk { /* Buat text keterangan petunjuk */
            margin-top: 12px;
            margin-left: 12px !important;
        }
        
        .form-actions { /* Buat ContainerPindahSoalPG */
            margin-bottom: 0px !important;
            display: flex;
            border: none;
            background: #FFF;
        }

        .clsBtns { /* Buat btn 'Akhiri Sesi', 'Sebelumnya', 'Berikutnya' */
            font-size: 16px;
            max-width: 520px;
            height: 50px;
            margin: 0px 14px;
            flex: 1;
            border-radius: 50px;
        }
        .clsSuccess { /* set warna btn 'Sebelumnya' */
            background-color: #32CD32 !important;
            border-color: #32CD32;
        }

        .lbl { /* Ini buat radio btn Pilihan */
            margin: 3px 10px 5px 5px !important;
        }
        input[type=checkbox].ace + .lbl::before, input[type=radio].ace + .lbl::before { /* Hitamkan border radio btn */
            border: 1px solid #808080;
        }
        .clsRadio { /* Set div pilihan */
            /*width: 100%;*/
            border: 1px solid grey;
            margin: 3px 3px;
            border-radius: 15px;
            cursor: pointer;
        }
        .clsRadio:hover {
            background-color: #EDEDED;
        }
        .clsRadio:active {
            background-color: deepskyblue;
            color: white;
        }
        .clsRadio.soal_terjawab {
            color: white;
        }

        .widget-body ul {/* Ini 2 buat hilangkan bullet element ul li di bagian navigasi */
            margin: 25px 0px 0px;
        }
        .widget-body ul li {
            list-style: none;
        }

        .w-box-custom {/* Ini buat width box Soal - Navigasi */
            width: 156px;
            margin: 5px 15px;
            border-radius: 15px;
            overflow: hidden;
            border: 2px solid #307ecc;
            background: #FFF;
        }
        .auto-width {/* Ini buat width mobile */
            width: auto !important;
        }

        .x-autocontainer-outerCt {/* Buat mentokin height container halaman soal */
            height: 100%;
        }
        
        #winSelesaiUjian {
            background-color: rgba(0,0,0,0.4) !important;
        }
        #winSelesaiUjian-bodyWrap, #winSelesaiUjian-body {
            background-color: transparent;
        }
        #winSelesaiUjian_Form {
            border-radius: 4px;
            background-color: gainsboro;
        }
    </style>
</head>
<body>
    <div id="tes"></div>
    <form runat="server" onkeydown="return (event.keyCode!=13);" id="FormMaster">
        <ext:ResourceManager ID="ResourceManager1" runat="server" ShowWarningOnAjaxFailure="false">
            <Listeners>
                <AjaxRequestException Handler="connectionLost();" />
            </Listeners>
        </ext:ResourceManager>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
        <ext:Viewport runat="server" Layout="BorderLayout" AutoScroll="true">
            <Items>
                <ext:Panel runat="server" ID="PanelNorth" Region="North" Height="75" BaseCls="clsHeaderUjian">
<%--                    <Items>
                        <ext:Container runat="server" BaseCls="navbar" />
                    </Items>--%>
                </ext:Panel>
                <ext:Panel runat="server" ID="PanelEast" Region="East" BaseCls="PlaceholderPanel" Width="100"/>
                <ext:Panel runat="server" ID="PanelWest" Region="West" BaseCls="PlaceholderPanel" Width="100"/>
                <ext:Panel runat="server" ID="PanelSouth" Region="South" StyleSpec="flex: 1;">
                    <Content>
                        <div class="clsFooter dispNone">
                            <div class="pull-right hidden-xs">
                                <b>Version</b>
                                <ext:Label runat="server" ID="LblVersion" Text="Version" BaseCls="clsLblVersion" />

                            </div>
                            <strong>Copyright &copy; 2018 - IT Division PT. Advantage SCM. All rights reserved.</strong>
                        </div>
                    </Content>
                </ext:Panel>
                <ext:Panel runat="server" ID="PanelCenter" Region="Center" AutoScroll="true">
                    <Items>
                        <ext:Hidden ID="hdnIsBaruLogin" runat="server" />
                        <ext:Hidden ID="hdnIntervalWaktuFoto" runat="server" />

                        <ext:Hidden ID="hdnNoPeserta" runat="server" />
                        <ext:Hidden ID="hdnNoPaket" runat="server" />
                        
                        <ext:Hidden ID="hdnUrutanSoal" runat="server" />
                        <ext:Hidden ID="hdnNoSoal" runat="server" />

                        <ext:Hidden ID="hdnUrutanNoGroup" runat="server" />
                        <ext:Hidden ID="hdnCurrentNoGroup" runat="server" />
                        <ext:Hidden ID="hdnCurrentNoGroupDtl" runat="server" />
                        <ext:Hidden ID="hdnCurrentNumber" runat="server" />

                        <ext:Hidden ID="hdnJmlGroup" runat="server" />
                        <ext:Hidden ID="hdnJmlSoal" runat="server" />

                        <ext:Hidden ID="hdnTmrCounting" runat="server" />
                        <ext:Hidden ID="hdnCountH" runat="server" />
                        <ext:Hidden ID="hdnCountM" runat="server" />
                        <ext:Hidden ID="hdnCountS" runat="server" />
                        
                        <ext:Container ID="ContainerHalamanSoal" runat="server" StyleSpec="height: 99%; y: -3px;">
                            <CustomConfig>
                                <ext:ConfigItem Name="contentEl" Value="div_height_100" Mode="Value" />
                            </CustomConfig>
                            <Content>
                                <div id="div_height_100" class="x-hidden" style="height: 100%; margin: 5px 0px;" />
                                <div class="widget-box widget-color-blue no-border" style="height: 100%; margin: 0px; display: flex; flex-direction: column; align-items: stretch;">
                                    <div id="widgetHeadGroupSoal" class="widget-header widget-header-large" style="border-radius: 15px; overflow: hidden; border: 2px solid #307ecc; margin: 0px 15px;">
                                        <h3 id="lblNmGroupSoal" class="widget-title"></h3>
                                        <div class="widget-toolbar no-border">
                                            <span id="lblTmr"></span>
                                            <div class="clsUrutanNoGroup">
                                                Ujian ke-<span id="lblUrutanNoGroup">1</span> dari <span id="lblJmlGroup"></span>
                                            </div>
                                            <a class="btn btn-light" style="border-radius: 50%; width: 32px; height: 32px;" onclick="showWinPetunjuk();">
                                                <i class="ace-icon icon-only fa fa-question bigger-125" style="padding-right: 1px;"></i>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="widget-body" style="display: flex; height: 100%;">
                                        <div id="widgetMain" class="widget-main no-padding" style="width: 100%; height: 100%; display: flex; flex-direction: row; align-items: stretch;">
                                            <div id="widgetMainChild1" style="width: 100%; margin: 0px 5px;">
                                                <div class="widget-box w-box-custom" style="width: 100%;">
                                                    <fieldset>
                                                        <div id="divKreplin" style="align-items: center; flex-direction: column; height: 100%; justify-content: center;">
                                                            <div class="dispFlex" style="align-items: center; margin-bottom: 40px;">
                                                                <div style="display: flex; flex-direction: column;">
                                                                    <label id="LblKreplinPrompt1" class="clsLblKreplin">
                                                                    </label>
                                                                    <label id="LblKreplinPrompt2" class="clsLblKreplin">
                                                                    </label>
                                                                </div>
                                                                <ext:NumberField ID="nfJawabanKreplin" runat="server" BaseCls="clsNfJawabanKreplin" Height="70" Width="100" 
                                                                FieldCls="clsNfJawabanKreplinText" hideTrigger="true" keyNavEnabled="false" mouseWheelEnabled="false">
                                                                    <KeyMap runat="server">
                                                                        <ext:KeyBindItem Handler="nfJawabanKreplin_Down()" Key="13" />
                                                                    </KeyMap>
                                                                </ext:NumberField>
                                                            </div>
                                                            <label style="font-size: 30px; text-align: center;">Tekan 'Enter' untuk menyimpan jawaban</label>
                                                        </div>
                                                        <div id="divRadio">
                                                            <h3 id="lblNoSoal" style="font-size: 26px;"></h3>
                                                            <p>
                                                                <iframe id="PanelSoal" width="560" height="315" frameborder="0" allowfullscreen="true"></iframe>
                                                                <img id="ImgSoal" src="#"></img>
                                                            </p>
                                                            <label id="lblSoal" class="prevent-select" style="font-size: 20px;"></label>
                                                            <div id="divContainerRadio" class="control-group">
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                            <div id="widgetMainChild2" style="margin: 0px 5px;">
                                                <div class="widget-box w-box-custom widget-color-blue">
                                                    <div id="divContainerbtnNavTitle" class="widget-title lblNavigasi">Navigasi</div>
                                                    <div id="divContainerbtnNavBody1" class="widget-body" style="flex-wrap: wrap; width: 152px; margin: auto;">
                                                    </div>
                                                    <div id="divContainerbtnNavBody2" class="widget-body" style="flex-wrap: wrap;">
                                                        <ul>
                                                            <li>
                                                                <button class="contoh_tombol_navigasi soal_kosong" style="cursor: default;" type="button">
                                                                    <span class="inner-tombol_navigasi" unselectable="on"></span>
                                                                </button>
                                                                Belum Terjawab
                                                            </li>
                                                            <li>
                                                                <button class="contoh_tombol_navigasi soal_aktif" style="cursor: default;" type="button">
                                                                    <span class="inner-tombol_navigasi" unselectable="on"></span>
                                                                </button>
                                                                Sedang dikerjakan
                                                            </li>
                                                            <li>
                                                                <button class="contoh_tombol_navigasi soal_terjawab" style="cursor: default;" type="button">
                                                                    <span class="inner-tombol_navigasi" unselectable="on"></span>
                                                                </button>
                                                                Terjawab
                                                            </li>
                                                        </ul>
                                                        <div class="center" style="width: 100%; height: 30px; margin: 5px; padding: 0px;">
                                                            <button id="btnSelesai" class="clsBtns btn btn-danger" type="button" onclick="Ext.getBody().mask('Mohon tunggu!!');App.direct.SelesaiGroupSoal('U');" style="width: 100%; height: 100%; padding: 0px; margin: 0px;">
                                                                Akhiri Sesi
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="flex: 1"></div>
                                    <div id="ContainerPindahSoalPG" class="form-actions center" style="justify-content: space-evenly; margin-top: 0px;">
                                        <button id="btnPrev" type="button" class="btn btn-success clsBtns clsSuccess" onclick="App.direct.prevQ()">
                                            <i class="ace-icon fa fa-arrow-left icon-on-right bigger-110">
                                            </i>
                                            Sebelumnya
                                        </button>
                                        <button id="btnNext" type="button" class="btn btn-primary clsBtns" onclick="App.direct.nextQ()">
                                            Berikutnya
                                            <i class="ace-icon fa fa-arrow-right icon-on-right bigger-110">
                                            </i>
                                        </button>
                                    </div>
                                </div>
                            </Content>
                        </ext:Container>
                        <ext:Panel runat="server" BaseCls="x-plain" Flex="1" />
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="QuestionTimer" Interval="1000">
                    <DirectEvents>
                        <Update OnEvent="timeCounter" />
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>


        <ext:Window runat="server" ID="winVerifikasiPeserta" WidthSpec="100%" HeightSpec="100%" Layout="CenterLayout" PaddingSpec="0" BorderSpec="0" ClientIDMode="Static"
             Header="false" BodyStyle="border: none" Draggable="false" Resizable="false" Closable="false" Hidden="true">
            <Items>
                <ext:FormPanel runat="server" ID="winVerifikasiPeserta_Form" Frame="false" Header="false" Layout="VBoxLayout" BodyPadding="5" Width="1200" Height="420"
                    BodyStyle="border: none;" AnchorHorizontal="100%" Scrollable="Both" BaseCls="clsWinVerifikasiPeserta_Form">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Center" />
                    </LayoutConfig>
                    <Items>
                        <ext:Label runat="server" ID="winVerifikasiPeserta_lblIdentifikasiData" Html="Identifikasi Data" StyleSpec="font-size: 40px; font-weight: bold;" MarginSpec="20" />
                        <ext:Label runat="server" ID="winVerifikasiPeserta_lblPerintah" Html="Silahkan isi data KTP & Tgl lahir saudara/i sesuai dengan data diri anda" StyleSpec="font-size: 20px; color: red; width: 400px; text-align: center; background-color: #ffe5cf; border-radius: 15px; height: auto;" MarginSpec="20" />
                        <ext:TextField runat="server" ID="nfNoKTP" EmptyText="No KTP" Width="400"  MaskRe="/[0-9]/">
                        </ext:TextField>
                        <ext:Container runat="server">
                            <Content>
                                <input type="date" runat="server" id="dtTglLahir" style="border-radius: 15px !important; text-align: center; width: 400px;" />
                            </Content>
                        </ext:Container>
                        <ext:Label runat="server" ID="winVerifikasiPeserta_lblWarning" Margin="0" Text="Data yang Anda masukkan salah, kesempatan Anda tinggal 2x lagi." StyleSpec="font-size: 15px; color: red; width: 300px; text-align: center; height: auto;" BaseCls="visibilityHidden" MarginSpec="5" />
                        <ext:Container runat="server" MarginSpec="auto">
                            <Content>
                                <button runat="server" id="btnwinVerifikasiPeserta_Submit" type="button" class="cls_btnwinVerifikasiPeserta" onclick="winVerifikasiPeserta_Submit_Click()">
                                    SUBMIT
                                </button>
                            </Content>
                        </ext:Container>
                        <ext:Image Width="150" Height="43" runat="server" ID="DCT_Logo" Src="assets/images/DCT_Logo.png" />
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="winPetunjuk" WidthSpec="100%" HeightSpec="100%" Hidden="true" Layout="CenterLayout" Draggable="false" Resizable="false" Closable="false" PaddingSpec="0" BorderSpec="0">
            <Items>
                <ext:FormPanel runat="server" ID="winPetunjuk_Form" Frame="false" Header="false" BodyPadding="5" AnchorHorizontal="100%" Scrollable="Both" ButtonAlign="Center">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Label runat="server" ID="lblJudulPetunjuk" Text="Petunjuk *Nama Paket Soal*" BaseCls="JdlPetunjuk" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Label runat="server" ID="lblKeteranganPetunjuk" Html="*Keterangan Petunjuk*" BaseCls="KternganPetunjuk" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="btnwinPetunjuk_Close" Height="45" Width="100" UI="Success" StandOut="true" Text="<h3 style='margin-bottom: -2px;'>Mulai</h3>">
                            <DirectEvents>
                                <Click OnEvent="btnwinPetunjuk_Close_Click" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="winGantiKolomKreplin" Modal="true" Width="1200" Height="220" Layout="CenterLayout"
             Header="false" BodyStyle="border: none" hidden="true" Draggable="false" Resizable="false" Closable="false">
            <Items>
                <ext:Container runat="server" Layout="VBoxLayout" Height="200">
                    <Items>
                        <ext:Label runat="server" ID="lblGantiKolomKreplin1" Text="Pergantian Kolom Kreplin" StyleSpec="font-size: 40px;" MarginSpec="20" />
                        <ext:Label runat="server" ID="lblGantiKolomKreplin2" Text="STOP! Pindah menuju kolom berikutnya..." StyleSpec="font-size: 40px;" MarginSpec="20" />
                    </Items>
                </ext:Container>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="winSelesaiUjian" WidthSpec="100%" HeightSpec="100%" Layout="CenterLayout"
             Header="false" BodyStyle="border: none" hidden="true" Draggable="false" Resizable="false" Closable="false">
            <Items>
                <ext:FormPanel runat="server" ID="winSelesaiUjian_Form" Frame="false" Header="false" Layout="VBoxLayout" BodyPadding="5" AnchorHorizontal="100%" Scrollable="Both">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Center" />
                    </LayoutConfig>
                    <Items>
                        <ext:Label runat="server" ID="lblSelesaiUjian1" Text="Selamat anda sudah menyelesaikan tahap ujian." StyleSpec="font-size: 40px;" MarginSpec="20" />
                        <ext:Label runat="server" ID="lblSelesaiUjian2" Text="HRD akan menghubungi jika anda lulus." StyleSpec="font-size: 40px;" MarginSpec="20" />
                        <ext:Label runat="server" ID="lblSelesaiUjian3" Text="Atas partisipasinya kami ucapkan terima kasih." StyleSpec="font-size: 40px;" MarginSpec="20" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" ID="btnwinSelesaiUjian_Close" Height="30" UI="Success" StandOut="true" Text="Selesai" Icon="Accept">
                            <DirectEvents>
                                <Click OnEvent="btnwinSelesaiUjian_Close_Click" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window runat="server" ID="winHiddenCam" WidthSpec="100%" HeightSpec="100%" Layout="CenterLayout"
             Header="false" BodyStyle="border: none" hidden="true" Draggable="false" Resizable="false" Closable="false">
            <Items>
                <ext:Container runat="server">
                    <Content>
                        <video id="webcam" width="640" height="480" autoplay></video>
                        <canvas id="snapshotCanvas" width="640" height="480" style="display: none;"></canvas>
                    </Content>
                </ext:Container>
            </Items>
        </ext:Window>
    </form>
</body>
</html>