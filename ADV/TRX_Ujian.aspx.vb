Imports System.Data
Imports System.IO
Imports Ext.Net
Imports Newtonsoft.Json
Imports System.Threading

Partial Class TRX_Ujian
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Session("UserIDPsikotest") = ""
            Session("UserNamePsikotest") = ""

            Session("CountM") = 0
            Session("CountS") = 0
            Session("Counting") = False
            Session("TimeOutAll") = Nothing

            Session("IsBaruLogin") = True
            Session("UrutanNoGroup") = 1
            Session("CurrentNumber") = 1

            Session("TmrTampilwinGantiKreplin") = -1

            LblVersion.Text = ConfigurationManager.AppSettings("AppVer").ToString

            hdnIntervalWaktuFoto.Value = clsTrxTest.getIntervalWaktuFoto()

            Dim T As Object = Request.QueryString("Token")
            If (T Is Nothing) Then
                'Link tidak ada querystring Token
                Response.Write("1-Link yang digunakan salah, mohon hubungi admin anda.")
                Response.End()
            End If

            Dim E64 = New Encryption64()
            Dim s = E64.Decrypt(T.ToString, ConfigurationManager.AppSettings("EncryptionKey").ToString)
            If s.Equals("Invalid length for a Base-64 char array or string.") Or s.Equals("Length of the data to decrypt is invalid.") Then
                'Link hasil decrypt error(link tidak lengkap)
                Response.Write("2-Link yang digunakan salah, mohon hubungi admin anda.")
                Response.End()
            End If

            Dim UserID = Mid(s, 3, InStr(s, "&P=") - 3)
            Session("UserIDPsikotest") = UserID
            Dim Password = Mid(s, InStr(s, "&P=") + 3)
            Dim e_Password = E64.Encrypt(Password, ConfigurationManager.AppSettings("EncryptionKey").ToString)
            Dim dt_Login = clsTrxTest.loginUjian(UserID, e_Password, 1, If(IsNothing(Session("isLoggedIn")), 0, 1))
            If dt_Login.Rows(0)("msg") <> "" Then
                Response.Write(dt_Login.Rows(0)("msg").ToString)
                Response.End()
            End If

            Session("UserNamePsikotest") = dt_Login.Rows(0)("NamaPeserta")
            Session("NoPeserta") = dt_Login.Rows(0)("NoPeserta").ToString
            Session("NoPaket") = dt_Login.Rows(0)("NoPaket")

            If IsDBNull(dt_Login.Rows(0)("LastNoGroup")) Then
                mulaiGroupSoal("", "")
            Else
                'Temukan urutan group soal setelah login ulang
                Dim dt_PaketSoalGroup = getPaketSoalGroup()
                Dim rows = dt_PaketSoalGroup.Select("NoGroup = " + dt_Login.Rows(0)("LastNoGroup").ToString)
                If rows.Count > 0 Then
                    Session("UrutanNoGroup") = dt_PaketSoalGroup.Rows.IndexOf(rows(0)) + 1
                    Ext.Net.X.AddScript("setInner(""lblUrutanNoGroup"", " + Session("UrutanNoGroup").ToString + ");")
                End If

                mulaiGroupSoal(dt_Login.Rows(0)("LastNoGroup").ToString, dt_Login.Rows(0)("LastNoUrut"))
            End If
            Ext.Net.X.AddScript("document.getElementById('dtTglLahir').valueAsDate = new Date();")
            winVerifikasiPeserta.Show()
        End If

    End Sub
    Protected Sub mulaiGroupSoal(LastNoGroup As String, LastNoUrut As String)
        Dim dt_PaketSoalGroup = getPaketSoalGroup()
        Session("JmlSoal") = CInt(dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("MinimumJmlSoal"))

        If Not LastNoGroup.Equals("") Then
            Dim dr = dt_PaketSoalGroup.Select("[NoGroup] = " + LastNoGroup)(0)
            Session("UrutanNoGroup") = dt_PaketSoalGroup.Rows.IndexOf(dr) + 1
            Ext.Net.X.AddScript("setInner(""lblUrutanNoGroup"", " + Session("UrutanNoGroup").ToString + ");")
            Session("CurrentNoGroup") = LastNoGroup
        End If
        getPaketSoalGroupDtl()

        setupNomorSoal(dt_PaketSoalGroup)

        If Not LastNoGroup.Equals("") And Not LastNoUrut.Equals("-1") Then
            Dim UrutanSoal = Split(Session("UrutanSoal"), ",")
            Dim a = Array.IndexOf(UrutanSoal, (CInt(LastNoUrut) - 1).ToString)
            Session("CurrentNumber") = a + 1
        End If
        Ext.Net.X.AddScript("setTotalSoal(" + Session("JmlSoal").ToString + ");")

        setupWinPetujuk(dt_PaketSoalGroup)
        If Trim(dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("Tipe")).Equals("Kreplin") Then
            Ext.Net.X.AddScript("FlexThis(""divKreplin"");" +
                                "setInner(""divContainerbtnNavTitle"", ""Kolom 1 dari " + Session("JmlSoal").ToString + """);" +
                                "setHeightBodyPanel(1);" +
                                "HideThis(""divRadio"");" +
                                "HideThis(""ContainerPindahSoalPG"");" +
                                "HideThis(""divContainerbtnNavBody1"");" +
                                "HideThis(""divContainerbtnNavBody2"");")
            nextKolomKreplin()
        Else
            Ext.Net.X.AddScript("HideThis(""divKreplin"");" +
                                "setInner(""divContainerbtnNavTitle"", ""Navigasi"");" +
                                "setHeightBodyPanel(0);" +
                                "ShowThis(""divRadio"");" +
                                "FlexThis(""ContainerPindahSoalPG"");" +
                                "FlexThis(""divContainerbtnNavBody1"");" +
                                "FlexThis(""divContainerbtnNavBody2"");")
            getSoalRadio()
        End If
    End Sub
    Protected Function getPaketSoalGroup() As DataTable
        Dim dt = clsTrxTest.getPaketSoalGroup(Session("NoPaket").ToString)

        Session("JmlGroup") = dt.Rows.Count
        Ext.Net.X.AddScript("setInner(""lblJmlGroup"", " + Session("JmlGroup").ToString + ");")
        Session("CurrentNoGroup") = dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("NoGroup").ToString
        Return dt
    End Function
    Protected Sub setupWinPetujuk(dt As DataTable)
        lblJudulPetunjuk.Text = "Petunjuk " + dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("NamaGroup").ToString
        lblKeteranganPetunjuk.Html = dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("Keterangan").ToString

        If Convert.ToBoolean(Session("IsBaruLogin")) Then
            Session("IsBaruLogin") = False
        Else
            winPetunjuk.Show()
        End If

        Ext.Net.X.AddScript("Ext.getBody().unmask();setInner(""lblNmGroupSoal"", """ + removeEndLine(dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("NamaGroup").ToString) + """);")
    End Sub
    Protected Function setupNomorSoal(dt As DataTable) As Int16()
        Dim UrutanNoSoal(getPaketSoalGroupDtl().Rows.Count - 1) As Int16

        For i As Int16 = 0 To UrutanNoSoal.Length - 1
            UrutanNoSoal(i) = i
        Next

        If Trim(dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("bRandom").ToString).Equals("True") Then
            Dim rnd As New Random(Session("NoPeserta"))
            Shuffle(UrutanNoSoal, rnd)
        End If

        Array.Resize(UrutanNoSoal, CInt(dt.Rows(CInt(Session("UrutanNoGroup")) - 1)("MinimumJmlSoal")))
        Session("UrutanSoal") = String.Join(",", UrutanNoSoal)

        Ext.Net.X.AddScript("setUrutanNoGroup(" + Session("UrutanNoGroup").ToString + ");")
        Return UrutanNoSoal
    End Function
    Protected Function getPaketSoalGroupDtl() As DataTable
        Dim dt = clsTrxTest.getPaketSoalGroupDtl(Session("NoPaket").ToString, Session("CurrentNoGroup").ToString)
        Return dt
    End Function

#Region "setupKreplin"
    Protected Sub nextKolomKreplin()
        If Not Session("CurrentNumber").ToString.Equals("1") Then
            Dim dt_PaketSoalGroup = getPaketSoalGroup()
            setupTimer(CInt(dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("WaktuPengerjaan")), 0)
        End If

        Session("JumlahJawabBenar") = 0
        Session("JumlahJawabSalah") = 0

        Session("NoSoal") = Split(Session("UrutanSoal"), ",")(CInt(Session("CurrentNumber")) - 1)
        getSoalKreplin(New Random)
    End Sub
    Protected Sub getSoalKreplin(rnd As Random)
        Dim dt_PaketSoalGroupDtl = getPaketSoalGroupDtl()
        Dim P1 = rnd.Next(0, 9)
        Dim P2 = rnd.Next(0, 9)

        Ext.Net.X.AddScript("document.getElementById(""LblKreplinPrompt1"").innerHTML=" + P1.ToString + ";" +
                            "document.getElementById(""LblKreplinPrompt1"").classList.remove(""transparent"")" + ";" +
                            "document.getElementById(""LblKreplinPrompt2"").innerHTML=" + P2.ToString + ";" +
                            "document.getElementById(""LblKreplinPrompt2"").classList.remove(""transparent"")" + ";")

        Session("KreplinPrompt1") = P1
        Session("KreplinPrompt2") = P2
    End Sub
    <DirectMethod()>
    Public Sub nfKreplin_Down()
        If nfJawabanKreplin.Text.Equals("") Then
            Ext.Net.X.AddScript("document.getElementById(""LblKreplinPrompt1"").classList.remove(""transparent"")" + ";" +
                                "document.getElementById(""LblKreplinPrompt2"").classList.remove(""transparent"")" + ";")
            Ext.Net.X.Toast("Jawaban harus diisi")
            Exit Sub
        End If

        Dim JawabanBenar = (CInt(Session("KreplinPrompt1")) + CInt(Session("KreplinPrompt2"))) Mod 10
        If JawabanBenar = CInt(nfJawabanKreplin.Text) Then
            Session("JumlahJawabBenar") = CInt(Session("JumlahJawabBenar")) + 1
            'Ext.Net.X.Toast("Benar")
        Else
            Session("JumlahJawabSalah") = CInt(Session("JumlahJawabSalah")) + 1
            'Ext.Net.X.Toast("Salah")
        End If
        Ext.Net.X.AddScript("document.getElementById(""nfJawabanKreplin-inputEl"").value='';")
        getSoalKreplin(New Random)
    End Sub
#End Region

#Region "setupRadio"
    Protected Function getSoalTerjawab() As String()
        Dim SoalTerjawab = clsTrxTest.getAnswered(Session("UserIDPsikotest").ToString, Session("NoPaket").ToString, Session("CurrentNoGroup").ToString).Split(","c)

        Dim newSoalTerjawab As New List(Of String)
        Dim UrutanSoal = Split(Session("UrutanSoal"), ",")
        If Not SoalTerjawab(0).Equals("") Then
            For Each i In SoalTerjawab
                i -= 1
                newSoalTerjawab.Add(Array.IndexOf(UrutanSoal, i) + 1.ToString)
            Next
        End If

        Ext.Net.X.AddScript("setSoalTerjawab('" + JSON.Serialize(newSoalTerjawab) + "');")
        Dim a = JSON.Serialize(newSoalTerjawab)
        Return SoalTerjawab
    End Function
    Protected Sub getSoalRadio()
        Dim dt_PaketSoalGroupDtl = getPaketSoalGroupDtl()

        Ext.Net.X.AddScript("setInner(""lblNoSoal"", ""Soal No. " + Session("CurrentNumber").ToString + """);")

        Session("NoSoal") = Split(Session("UrutanSoal"), ",")(CInt(Session("CurrentNumber")) - 1)
        Dim TipeMediaCurrSoal = Trim(dt_PaketSoalGroupDtl.Rows(CInt(Session("NoSoal")))("TipeMedia").ToString)
        If TipeMediaCurrSoal.Equals("YOUTUBE") Then
            Ext.Net.X.AddScript("document.getElementById(""PanelSoal"").src=""" + dt_PaketSoalGroupDtl.Rows(CInt(Session("NoSoal")))("UrlMedia").ToString + """;" +
                                "document.getElementById(""PanelSoal"").classList.remove(""dispNone"");" +
                                "document.getElementById(""ImgSoal"").classList.add(""dispNone"");")
        ElseIf TipeMediaCurrSoal.Equals("JPG") Or TipeMediaCurrSoal.Equals("PNG") Then
            Dim DirFile = Session("NoPaket").ToString & "/" & Session("CurrentNoGroup").ToString & "/" &
                          dt_PaketSoalGroupDtl.Rows(CInt(Session("NoSoal")))("MediaFileName").ToString

            Ext.Net.X.AddScript("document.getElementById(""ImgSoal"").src=""" + "ViewAttachmentTRX.ashx?MaxHeight=200&MaxWidth=200&FileName=" + DirFile + """;" +
                                "document.getElementById(""ImgSoal"").classList.remove(""dispNone"");" +
                                "document.getElementById(""PanelSoal"").classList.add(""dispNone"");")
        Else
            Ext.Net.X.AddScript("document.getElementById(""PanelSoal"").classList.add(""dispNone"");" +
                                "document.getElementById(""ImgSoal"").classList.add(""dispNone"");")
        End If


        Ext.Net.X.AddScript("setInner(""lblSoal"", '" + removeEndLine(dt_PaketSoalGroupDtl.Rows(CInt(Session("NoSoal")))("Deskripsi").ToString) + "');")
        reloadPilihan()

        getSoalTerjawab()

        Ext.Net.X.AddScript("setCurrentNumber(" + Session("CurrentNumber").ToString + ");")
        Ext.Net.X.AddScript("reloadNavBtn();")
    End Sub
    <DirectMethod()>
    Public Sub prevQ()
        If CInt(Session("CurrentNumber")) - 1 >= 1 Then
            Session("CurrentNumber") = CInt(Session("CurrentNumber")) - 1
            getSoalRadio()

            Ext.Net.X.AddScript("setCurrentNumber(" + Session("CurrentNumber").ToString + ");")
        Else
            Ext.Net.X.Msg.Alert("Alert", "Anda sudah mencapai nomor pertama").Show()
        End If
    End Sub
    <DirectMethod()>
    Public Sub nextQ()
        If CInt(Session("CurrentNumber")) + 1 <= CInt(Session("JmlSoal")) Then
            Session("CurrentNumber") = CInt(Session("CurrentNumber")) + 1
            getSoalRadio()

            Ext.Net.X.AddScript("setCurrentNumber(" + Session("CurrentNumber").ToString + ");")
        Else
            Ext.Net.X.Msg.Alert("Alert", "Anda sudah mencapai nomor terakhir").Show()
        End If
    End Sub
    <DirectMethod()>
    Public Sub JawabSoal(ans As String)
        clsTrxTest.submitJawabanRadio(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString,
                                      Session("CurrentNoGroup").ToString, (CInt(Session("NoSoal")) + 1).ToString, ans)

        If CInt(Session("CurrentNumber")) <> CInt(Session("JmlSoal")) Then
            Session("CurrentNumber") = CInt(Session("CurrentNumber")) + 1
            getSoalRadio()
        End If
    End Sub
    Protected Sub reloadPilihan()
        Dim dt_Pilihan = clsTrxTest.getPaketSoalGroupDtlJawaban(Session("NoPaket").ToString, Session("CurrentNoGroup").ToString, (CInt(Session("NoSoal")) + 1).ToString)

        setPilihanRadio(dt_Pilihan)
        Dim getJawaban = clsTrxTest.getJawaban(Session("UserIDPsikotest").ToString, Session("NoPaket").ToString, Session("CurrentNoGroup").ToString, (Session("NoSoal") + 1).ToString)
        If Not IsNothing(getJawaban) Then
            If Not getJawaban.Equals("") Then
                setPilihanDiJawab(getJawaban)
            End If
        End If
    End Sub
#End Region
    <DirectMethod()>
    Public Sub SelesaiGroupSoal(arg As String)

        Dim SoalTerjawab = getSoalTerjawab()
        If arg = "U" And SoalTerjawab.Length <> CInt(Session("JmlSoal")) Then

            Dim yes As New MessageBoxButtonConfig
            yes.Handler = "App.direct.SelesaiGroupSoal('C');"
            yes.Text = "YA"
            Dim no As New MessageBoxButtonConfig
            no.Handler = "Ext.getBody().unmask();"
            no.Text = "TIDAK"

            Dim btnConfig As MessageBoxButtonsConfig = New MessageBoxButtonsConfig()
            btnConfig.Yes = yes : btnConfig.No = no

            Dim msgBoxConfig As New MessageBoxConfig
            msgBoxConfig.Closable = False
            msgBoxConfig.Icon = MessageBox.Icon.QUESTION
            msgBoxConfig.Buttons = MessageBox.Button.YESNO
            msgBoxConfig.MessageBoxButtonsConfig = btnConfig

            Dim soalTersisa = CInt(Session("JmlSoal"))
            If Not (SoalTerjawab.Length = 1 And SoalTerjawab(0).Equals("")) Then
                soalTersisa -= SoalTerjawab.Length
            End If

            msgBoxConfig.Title = "Alert"
            msgBoxConfig.Message = "Terdapat " + soalTersisa.ToString + " soal yang belum dikerjakan, lanjutkan?"
            Ext.Net.X.Msg.Show(msgBoxConfig)

            'Ext.Net.X.Msg.Confirm("Alert", "Terdapat " + soalTersisa.ToString + " soal yang belum dikerjakan, lanjutkan?", config).Show()
        Else
            Session("Counting") = False
            clsTrxTest.timeoutSoalRadio(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString, Session("CurrentNoGroup").ToString)
            If Session("UrutanNoGroup") = Session("JmlGroup") Then
                clsTrxTest.selesaiUjian(Session("UserIDPsikotest").ToString)
                TaskManager1.StopAll()
                winSelesaiUjian.Show()
            Else
                Session("UrutanNoGroup") = Session("UrutanNoGroup") + 1
                Ext.Net.X.AddScript("setInner(""lblUrutanNoGroup"", " + Session("UrutanNoGroup").ToString + ");")
                Session("CurrentNumber") = 1
                mulaiGroupSoal("", "")
            End If
        End If
    End Sub

#Region "Window Verifikasi Peserta"
    <DirectMethod()>
    Public Sub winVerifikasiPeserta_Submit_Click()
        If nfNoKTP.Value = Nothing Then
            Ext.Net.X.Msg.Alert("Alert", "Mohon isi No KTP").Show()
            Return
        End If
        If dtTglLahir.Value = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Mohon isi tanggal lahir").Show()
            Return
        End If
        If Not IsDate(dtTglLahir.Value) Then
            Ext.Net.X.Msg.Alert("Alert", "Tanggal lahir tidak valid").Show()
            Return
        End If

        Dim dt = clsTrxTest.CekNoKTP(Session("UserIDPsikotest"), CDate(dtTglLahir.Value), nfNoKTP.Value.ToString, If(IsNothing(Session("isLoggedIn")), 0, 1))
        If dt.Rows(0)("msg") = "" Then 'NIK dan tgl lahir benar
            Session("isLoggedIn") = True

            Ext.Net.X.AddScript("winVerifikasiPeserta_Submit_Click_Berhasil();")
            winPetunjuk.Show()
            winVerifikasiPeserta.Hide()
        ElseIf CInt(dt.Rows(0)("VerifikasiKe")) = -1 Then 'Udah login (pas load barengan tapi kalo login KTP bisa dicut juga)
            Ext.Net.X.AddScript("window.alert('" + dt.Rows(0)("msg").ToString + "');window.location.href='Default.aspx';")
        ElseIf CInt(dt.Rows(0)("VerifikasiKe")) < 3 Then 'NIK dan tgl lahir salah
            Ext.Net.X.AddScript("winVerifikasiPeserta_lblWarning_Reload(" + (3 - CInt(dt.Rows(0)("VerifikasiKe"))).ToString() + ");")
        Else
            Ext.Net.X.AddScript("window.alert('Peserta gagal melakukan verifikasi sebanyak 3x');window.location.href='Default.aspx';")
        End If
    End Sub
#End Region

#Region "Window Petunjuk"
    Protected Sub btnwinPetunjuk_Close_Click()
        Dim dr As DataRow = clsTrxTest.loginUjian(Session("UserIDPsikotest").ToString, "2", CInt(Session("CurrentNoGroup").ToString))

        Session("TimeOutAll") = CDate(dr("TimeOutAll"))

        Dim dt_PaketSoalGroup = getPaketSoalGroup()
        If Not Session("Counting").Equals("") Then
            If Not Session("Counting") Then
                If IsDBNull(dr("TimeLeftGroupSoalM")) OrElse IsDBNull(dr("TimeLeftGroupSoalS")) OrElse Not IsNumeric(dr("TimeLeftGroupSoalM")) OrElse Not IsNumeric(dr("TimeLeftGroupSoalS")) Then
                    setupTimer(CInt(dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("WaktuPengerjaan")), 0)
                Else
                    If CInt(dr("TimeLeftGroupSoalM")) < 0 OrElse CInt(dr("TimeLeftGroupSoalS")) < 0 Then
                        timeUp()
                        Return
                    End If
                    setupTimer(Math.Floor(CInt(dr("TimeLeftGroupSoalS")) / 60), CInt(dr("TimeLeftGroupSoalS")) Mod 60)
                End If
            End If
        Else
            If IsDBNull(dr("TimeLeftGroupSoalM")) OrElse IsDBNull(dr("TimeLeftGroupSoalS")) OrElse Not IsNumeric(dr("TimeLeftGroupSoalM")) OrElse Not IsNumeric(dr("TimeLeftGroupSoalS")) Then
                setupTimer(CInt(dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("WaktuPengerjaan")), 0)
            Else
                If CInt(dr("TimeLeftGroupSoalM")) < 0 OrElse CInt(dr("TimeLeftGroupSoalS")) < 0 Then
                    timeUp()
                    Return
                End If
                setupTimer(Math.Floor(CInt(dr("TimeLeftGroupSoalM")) / 60), CInt(dr("TimeLeftGroupSoalS")) Mod 60)
            End If
        End If

        Ext.Net.X.AddScript("document.getElementById(""nfJawabanKreplin-inputEl"").value='';")
        nfJawabanKreplin.Focus()
        winPetunjuk.Hide()
        Ext.Net.X.AddScript("reloadNavBtn();")
        Ext.Net.X.AddScript("Ext.getCmp('PanelCenter').body.scrollTo('top', 0);")
    End Sub
#End Region

#Region "Window SelesaiUjian"
    Protected Sub btnwinSelesaiUjian_Close_Click()
        Response.Redirect("logout.aspx")
    End Sub
#End Region

#Region "Navigasi"
    Protected Sub setupNavigasi()
        Dim arr As String
        arr = "["
        For i As Int32 = 1 To CInt(Session("JmlSoal"))
            If i <> 1 Then
                arr += ","
            End If
            arr += i.ToString
        Next
        arr += "]"
        Ext.Net.X.AddScript("setupNavigasi('" + arr + "');")
    End Sub
    <DirectMethod()>
    Public Sub navTo(go As String)
        If Not go.Equals("") Then
            Dim destination As Int16 = CInt(go)

            If destination <> -1 Then
                Session("CurrentNumber") = destination
                getSoalRadio()
            End If
        End If
        Ext.Net.X.AddScript("setGoToOnGoing(0);")
        Ext.Net.X.AddScript("reloadNavBtn();")
    End Sub
#End Region

#Region "Timer"
    Protected Sub setupTimer(m As Int32, s As Int32)
        Session("CountM") = m
        Session("CountS") = s
        Session("Counting") = True
        hdnCountM.Value = m
        hdnCountS.Value = s
        Ext.Net.X.AddScript("setInner(""lblTmr"", """ + timerDisplay() + """);")
    End Sub
    <DirectMethod>
    Public Sub timeCounter()
        If Not IsNothing(Session("TimeOutAll")) Then
            If Session("TimeOutAll") < Date.Now Then
                timeUpAll()
                Ext.Net.X.AddScript("window.alert('Waktu peserta mengerjakan soal sudah habis');window.location.href='Default.aspx';")
            End If
        End If

        Dim t = countTime()
        Ext.Net.X.AddScript("setInner(""lblTmr"", """ + t + """);")
    End Sub
    Protected Function countTime() As String
        If Not Session("Counting").Equals("") Then
            If CType(Session("Counting"), Boolean) Then
                If CInt(Session("CountS")) = 0 Then
                    If CInt(Session("CountM")) = 0 Then
                        Session("Counting") = False
                        Ext.Net.X.Toast("Waktu Habis!!")
                        timeUp()
                        Return "00:00"
                    End If
                    Session("CountM") = CInt(Session("CountM")) - 1
                    Session("CountS") = 60
                    hdnCountM.Value = Session("CountM")
                    hdnCountS.Value = Session("CountS")
                End If
                Session("CountS") = CInt(Session("CountS")) - 1
                hdnCountS.Value = Session("CountS")
                Return timerDisplay()
            End If
        End If
        If CInt(Session("TmrTampilwinGantiKreplin")) = 0 Then
            Session("TmrTampilwinGantiKreplin") = -1
            GantiKolomKreplin_Up()
        End If
        If CInt(Session("TmrTampilwinGantiKreplin")) > 0 Then
            Session("TmrTampilwinGantiKreplin") = CInt(Session("TmrTampilwinGantiKreplin")) - 1
        End If
        Return "00:00"
    End Function
    Protected Function timerDisplay() As String
        Dim Text As String = ""
        If CInt(Session("CountM")) > 9 Then
            Text += Session("CountM").ToString + ":"
        Else
            Text += "0" + Session("CountM").ToString + ":"
        End If
        If CInt(Session("CountS")) > 9 Then
            Text += Session("CountS").ToString
        Else
            Text += "0" + Session("CountS").ToString
        End If
        Return Text
    End Function
    Protected Sub timeUp()
        Dim dt_PaketSoalGroup = getPaketSoalGroup()
        Dim Tipe = dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("Tipe")
        If Tipe.Equals("Kreplin") Then
            clsTrxTest.submitJawabanKreplin(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString,
                                            Session("CurrentNoGroup").ToString, (CInt(Session("NoSoal")) + 1).ToString,
                                            Session("JumlahJawabBenar").ToString, Session("JumlahJawabSalah").ToString)

            If CInt(Session("CurrentNumber")) <> CInt(Session("JmlSoal")) Then
                Session("CurrentNumber") = CInt(Session("CurrentNumber")) + 1
                Session("TmrTampilwinGantiKreplin") = 2

                winGantiKolomKreplin.Show()
                Exit Sub
            End If
        Else
            clsTrxTest.timeoutSoalRadio(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString, Session("CurrentNoGroup").ToString)
        End If
        Ext.Net.X.Toast("Selesai Group Soal")
        SelesaiGroupSoal("T")
    End Sub
    Protected Sub timeUpAll()
        Dim dt_PaketSoalGroup = getPaketSoalGroup()
        While dt_PaketSoalGroup.Rows.Count > Session("UrutanNoGroup")
            Dim Tipe = dt_PaketSoalGroup.Rows(CInt(Session("UrutanNoGroup")) - 1)("Tipe")
            If Tipe.Equals("Kreplin") Then
                clsTrxTest.submitJawabanKreplin(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString,
                                            Session("CurrentNoGroup").ToString, (CInt(Session("NoSoal")) + 1).ToString,
                                            Session("JumlahJawabBenar").ToString, Session("JumlahJawabSalah").ToString)

                If CInt(Session("CurrentNumber")) <> CInt(Session("JmlSoal")) Then
                    Session("CurrentNumber") = CInt(Session("CurrentNumber")) + 1
                    Session("TmrTampilwinGantiKreplin") = 2

                    winGantiKolomKreplin.Show()
                    Exit Sub
                End If
            Else
                clsTrxTest.timeoutSoalRadio(Session("NoPeserta").ToString, Session("UserIDPsikotest").ToString, Session("NoPaket").ToString, Session("CurrentNoGroup").ToString)
            End If
            Session("UrutanNoGroup") = CInt(Session("UrutanNoGroup")) + 1
        End While
        Ext.Net.X.Toast("Selesai Group Soal")
        SelesaiGroupSoal("T")
    End Sub
    Public Sub GantiKolomKreplin_Up()
        winGantiKolomKreplin.Hide()
        nextKolomKreplin()
        nfJawabanKreplin.Focus()
        Ext.Net.X.AddScript("setInner(""divContainerbtnNavTitle"", ""Kolom " + (CInt(Session("NoSoal")) + 1).ToString + " dari " + Session("JmlSoal").ToString + """);")
    End Sub
#End Region

#Region "Radio"
    Protected Sub setPilihanRadio(dt_Pilihan As DataTable)
        Dim i As Int16 = 0, arr As String
        arr = "["
        For Each dr As DataRow In dt_Pilihan.Rows
            If i <> 0 Then
                arr += ","
            End If
            i += 1
            arr += "{""i"":""" + i.ToString + """,""Jwb"":""" + removeEndLine(dr("Jawaban").ToString) + """"

            If dr("TipeMedia").Equals("YOUTUBE") Then
                arr += """link"":""" + removeEndLine(dr("UrlMedia")) + """"
            ElseIf dr("TipeMedia").Equals("JPG") Or dr("TipeMedia").Equals("PNG") Then
                arr += ",""img"":""ViewAttachmentTRX.ashx?MaxHeight=200&MaxWidth=200&FileName=" +
                                   removeEndLine(Session("NoPaket").ToString + "/" +
                                   Session("CurrentNoGroup").ToString + "/" + dr("MediaFileName").ToString) + """"
            End If
            arr += "}"
        Next
        arr += "]"
        Ext.Net.X.AddScript("setPilihanRadio('" + arr + "');")
    End Sub
    Protected Sub setPilihanDiJawab(Jawab As String)
        Ext.Net.X.AddScript("setPilihanDiJawab('" + Jawab + "');")
    End Sub
#End Region

#Region "Private Method"
    Public Sub Shuffle(Of T)(items As T(), rng As Random)
        Dim temp As T
        Dim j As Int32

        For i As Int32 = items.Count - 1 To 0 Step -1
            ' Pick an item for position i.
            j = rng.Next(i + 1)
            ' Swap 
            temp = items(i)
            items(i) = items(j)
            items(j) = temp
        Next i
    End Sub
    Public Function removeEndLine(s As String) As String
        Return s.Replace(vbLf, "<br>") _
                .Replace(vbCrLf, "<br>") _
                .Replace(vbCr, "<br>")
    End Function
    Public Function escape(s As String) As String
        Return s.Replace("&", "&amp;") _
                .Replace("<", "&lt;") _
                .Replace(">", "&gt;") _
                .Replace("""", "&quot;") _
                .Replace("'", "&#39;")
    End Function
#End Region
End Class
