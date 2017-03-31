Imports InTheHand.Net
Imports InTheHand.Net.Sockets
Imports System.Data.SqlServerCe
Imports System.IO
Imports System.Threading
Imports System.Xml
Imports Microsoft.Win32
Imports System.Management
Imports System.Security.Principal

Public Class form_MAIN
    Public numer_sesji As Int32
    Public plik As String
    Public pliczek As String
    Public data_od As String
    Public data_do As String
    Public godz_od As String
    Public godz_do As String
    Public rozsylka As String

    Private Shared Function GetConn() As SqlCeConnection
        Dim conn As New SqlCeConnection()
        conn.ConnectionString = "Persist Security Info=False; Data Source = dbf_offline.sdf; Password = 123abc;"
        Return conn
    End Function

    Private Sub form_MAIN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "btConnect"
        NotifyIcon1.Visible = False
        de_data_od.DateTime = Today()
        de_data_do.DateTime = DateTime.Now.AddDays(1)
        de_rap_od.DateTime = Today()
        de_rap_do.DateTime = Today()

        Dim br As Bluetooth.BluetoothRadio = Bluetooth.BluetoothRadio.PrimaryRadio
        If Not br Is Nothing Then
            get_local_content()
            txt_NAZWA_NADAWCY.Text = br.Name.ToString
            lbl_ADRES.Text = br.LocalAddress.ToString
            lbl_PRODUCENT.Text = br.Manufacturer.ToString
            lbl_WAZNOSC_PRZESYLKI.Text = data_od & " do " & data_do
            lbl_ROZSYLKA.Text = rozsylka & " min."
            tbc_ROZSYLKA.Value = rozsylka * 2
            Timer1.Interval = rozsylka * 60000

            If br.Mode = Bluetooth.RadioMode.Discoverable Then
                lbl_STATUS.Text = "Nie wykryte"
            ElseIf br.Mode = Bluetooth.RadioMode.Connectable Then
                lbl_STATUS.Text = "Podłączone"
            ElseIf br.Mode = Bluetooth.RadioMode.PowerOff Then
                lbl_STATUS.Text = "Wyłączone"
            End If
        Else
            MsgBox("W systemie nie wykryto radia Bluetooth." & vbNewLine & "Podłącz adapter Bluetooth i uruchom program ponownie.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
            End
        End If
    End Sub

    Public Function czy_admin() As Boolean
        Dim id As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim p As WindowsPrincipal = New WindowsPrincipal(id)
        Return p.IsInRole(WindowsBuiltInRole.Administrator)
    End Function

    Public Function DiffDate(ByVal StartYear As String, ByVal StartMonth As String, ByVal StartDay As String, ByVal EndYear As String, ByVal EndMonth As String, ByVal EndDay As String) As Integer
        Try
            Dim DateStart As Date = New Date(Convert.ToInt32(StartYear), Convert.ToInt32(StartMonth), Convert.ToInt32(StartDay))
            Dim DateEnd As Date = New Date(Convert.ToInt32(EndYear), Convert.ToInt32(EndMonth), Convert.ToInt32(EndDay))
            Return Convert.ToInt32(DateDiff(DateInterval.Day, DateStart, DateEnd))
        Catch
            Return 0
        End Try
    End Function

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    Private Sub form_MAIN_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Minimized
            NotifyIcon1.Visible = True
            Me.Hide()
        End If
    End Sub

    Private Sub form_MAIN_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Result As MsgBoxResult
        Result = MsgBox("Napewno zamknąć program?", vbOKCancel + MsgBoxStyle.Question, "Info")
        If Result = MsgBoxResult.Ok Then
            If czy_admin() = True Then
            Else
                form_HASLO.ShowDialog()
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub NameOfEventTick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Now.Date >= data_od And Now.Date <= data_do Then
            lbl_STATUS_ROZSYLKI.Text = "Wysyłanie   "
            lbl_STATUS_ROZSYLKI.Refresh()
            check_radio()
            lbl_STATUS_ROZSYLKI.Text = "Oczekiwanie"
            lbl_STATUS_ROZSYLKI.Refresh()
        End If
    End Sub

    Private Sub get_local_content()
        '********************************************** procedura pobierajaca przesylke z lokalnego pliku
        Dim objFile As StreamReader
        Dim objXml As XmlDocument
        objFile = File.OpenText("app.config.xml")
        objXml = New XmlDocument()
        objXml.LoadXml(objFile.ReadToEnd())
        lbl_NAZWA_NADAWCY.Text = objXml.SelectSingleNode("//nadawca").InnerText
        lbl_NAZWA_PRZESYLKI.Text = objXml.SelectSingleNode("//przesylka").InnerText
        plik = objXml.SelectSingleNode("//przesylka").InnerText & ".gif"
        data_od = objXml.SelectSingleNode("//data_od").InnerText
        data_do = objXml.SelectSingleNode("//data_do").InnerText
        godz_od = objXml.SelectSingleNode("//godz_od").InnerText
        godz_do = objXml.SelectSingleNode("//godz_do").InnerText
        rozsylka = objXml.SelectSingleNode("//rozsylka").InnerText
        objFile.Close()
    End Sub

    Private Sub btn_RUN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RUN.Click
        lbl_STATUS_ROZSYLKI.Text = "Oczekiwanie"
        If btn_RUN.Text = "Run" Then
            Timer1.Enabled = True
            btn_RUN.Text = "Stop"
        Else
            Timer1.Enabled = False
            btn_RUN.Text = "Run"
            lbl_STATUS_ROZSYLKI.Text = "Stop"
        End If
    End Sub

    Public Sub check_radio()
        '********************************************** procedura sprawdzajaca dostepnosc radia w systemie
        Dim br As Bluetooth.BluetoothRadio = Bluetooth.BluetoothRadio.PrimaryRadio
        If Not br Is Nothing Then
            search_devices()
        Else
            MsgBox("W systemie brak radia Bluetooth!" & Environment.NewLine & "Sprawdź urządzenie Bluetooth i uruchom program ponownie.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
            End
        End If
    End Sub

    Private Sub get_next_session()
        '********************************************** procedura generujaca kolejny numer sesji
        Try
            Dim conn As SqlCeConnection = GetConn()
            conn.Open()
            Dim SQLcmd1 As SqlCeCommand = New SqlCeCommand("INSERT INTO ba_sessions (date, time) VALUES ('" & Now().ToString("yyyy-MM-dd") & "', '" & Now().ToString("HH:mm:ss") & "')", conn)
            SQLcmd1.ExecuteNonQuery()
            Dim SQLcmd2 As SqlCeCommand = New SqlCeCommand("SELECT id FROM ba_sessions ORDER BY id desc", conn)
            Dim dr As SqlCeDataReader = SQLcmd2.ExecuteReader()
            dr.Read()
            numer_sesji = dr.GetInt32(0)
            dr.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox("Błąd przydziału ID sesji." & Environment.NewLine & ex.Message & Environment.NewLine & "Uruchom program ponownie.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
            End
        End Try
    End Sub

    Public Sub search_devices()
        '********************************************** procedura wyszukujaca urzadzenia z ktorymi mozna nawiazac polaczenie i zapisujaca dane tych urzadzen do tabeli tymczasowej
        Dim conn As SqlCeConnection = GetConn()
        conn.Open()
        Dim SQLcmd1 As SqlCeCommand = New SqlCeCommand("DELETE FROM ba_temp", conn)
        SQLcmd1.ExecuteNonQuery()

        Dim cli As New BluetoothClient()
        Dim devices() As BluetoothDeviceInfo = cli.DiscoverDevices()
        For Each curDevice As BluetoothDeviceInfo In devices
            Dim SQLcmd2 As SqlCeCommand = New SqlCeCommand("INSERT INTO ba_temp (dev_mac, dev_name) VALUES ('" & curDevice.DeviceAddress.ToString & "', '" & curDevice.DeviceName.ToString & "')", conn)
            SQLcmd2.ExecuteNonQuery()
        Next
        conn.Close()
        send_data()
    End Sub

    Public Sub send_data()
        '********************************************** procedura wysylajaca przesylke do wykrytych urzadzen
        Dim x As Int32 = 0
        Dim conn As SqlCeConnection = GetConn()
        conn.Open()
        Dim SQLcmd As SqlCeCommand = New SqlCeCommand("SELECT dev_mac, dev_name FROM ba_temp", conn)
        Dim objAdapter1 As New SqlCeDataAdapter
        objAdapter1.SelectCommand = SQLcmd
        Dim objDataset1 As New DataSet
        objAdapter1.Fill(objDataset1, "ba_Data")
        DataGridView1.DataSource = objDataset1.Tables(0).DefaultView

        get_next_session()

        Do While x <= DataGridView1.Rows.Count - 2 ' -2 poniewaz pierwszy wiersz jest naglowkiem w DataGrid
            Dim SQLcmd2 As SqlCeCommand = New SqlCeCommand("INSERT INTO ba_sessions_details (id_sessions, dev_mac, dev_name, request, file_name) VALUES ('" & numer_sesji & "', '" & DataGridView1.Item(0, x).Value.ToString & "', '" & DataGridView1.Item(1, x).Value.ToString & "', 'Request', '" & plik & "')", conn)
            SQLcmd2.ExecuteNonQuery()

            Dim Result As MsgBoxResult
            Result = MsgBox("Wysłać plik do " & DataGridView1.Item(1, x).Value.ToString, vbOKCancel + MsgBoxStyle.Question, "Info")
            If Result = MsgBoxResult.Ok Then
                Dim t As Thread
                Dim oSend As New SendClass_offline()
                t = New Thread(AddressOf oSend.send_data)
                oSend.dev_mac = DataGridView1.Item(0, x).Value.ToString
                oSend.file_name = plik

                t.Start()
            End If
            x = x + 1
        Loop
        conn.Close()
    End Sub

    Public Shared Function StrToByteArray(ByVal str As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        Return encoding.GetBytes(str)
    End Function

    Private Sub btn_OPEN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OPEN.Click
        OpenFileDialog1.Title = "Otwórz plik do wysłania..."
        OpenFileDialog1.Filter = "All files (*.*)|*.*|GIF files (*.gif)|*.gif|JPG files (*.jpg)|*.jpg|MP3 files (*.mp3)|*.mp3|TXT files (*.txt)|*.txt"
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.Multiselect = False

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            If OpenFileDialog1.FileName.Length > 0 Then
                If OpenFileDialog1.OpenFile.Length.ToString > 200000 Then
                    MsgBox("Plik nie może być wiekszy niż 200 kB!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
                Else
                    Dim stFilePathAndName As String = OpenFileDialog1.FileName
                    Dim MyFile As FileInfo = New FileInfo(stFilePathAndName)
                    pliczek = MyFile.Name
                    lbl_NAZWA_PLIKU.Text = OpenFileDialog1.FileName
                    btn_SAVE.Enabled = True
                End If
            End If
        Else
            lbl_NAZWA_PLIKU.Text = ""
            btn_SAVE.Enabled = False
        End If
    End Sub

    Private Sub btn_SAVE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SAVE.Click
        Try
            If txt_NAZWA_NADAWCY.Text = "" Or txt_PRZESYLKA.Text = "" Then
                MsgBox("Nazwa nadawcy i/lub nazwa kampanii nie mogą być puste!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
            Else
                '********************************************** zapis danych o przesylce do pliku app.config.xml
                FileCopy(pliczek, System.AppDomain.CurrentDomain.BaseDirectory & zamien_znaki(txt_PRZESYLKA.Text) & ".gif")
                Dim al As New XmlDocument()
                Dim dec_al As XmlDeclaration = al.CreateXmlDeclaration("1.0", "utf-8", Nothing)
                al.AppendChild(dec_al)
                Dim configuration As XmlElement = al.CreateElement("configuration")
                al.AppendChild(configuration)
                Dim przesylka As XmlElement = al.CreateElement("przesylka")
                configuration.AppendChild(przesylka)
                przesylka.InnerText = zamien_znaki(txt_PRZESYLKA.Text)
                Dim nadawca As XmlElement = al.CreateElement("nadawca")
                configuration.AppendChild(nadawca)
                nadawca.InnerText = zamien_znaki(txt_NAZWA_NADAWCY.Text)
                Dim data_od As XmlElement = al.CreateElement("data_od")
                configuration.AppendChild(data_od)
                data_od.InnerText = de_data_od.Text
                Dim data_do As XmlElement = al.CreateElement("data_do")
                configuration.AppendChild(data_do)
                data_do.InnerText = de_data_do.Text
                Dim godz_od As XmlElement = al.CreateElement("godz_od")
                configuration.AppendChild(godz_od)
                godz_od.InnerText = te_godz_od.Text
                Dim godz_do As XmlElement = al.CreateElement("godz_do")
                configuration.AppendChild(godz_do)
                godz_do.InnerText = te_godz_do.Text
                Dim rozsylka As XmlElement = al.CreateElement("rozsylka")
                configuration.AppendChild(rozsylka)
                rozsylka.InnerText = tbc_ROZSYLKA.Value / 2
                al.Save(System.AppDomain.CurrentDomain.BaseDirectory & "app.config.xml")
                MsgBox("Dane zapisano prawidłowo." & vbNewLine & "Uruchom ponownie komputer aby aktywować kampanię.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Info")
                End
            End If
        Catch ex As Exception
            MsgBox("Błąd zapisu danych o kampanii." & Environment.NewLine & ex.Message & Environment.NewLine & "Uruchom program ponownie.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd!")
            End
        End Try
    End Sub

    Private Sub btn_RAP_RUN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RAP_RUN.Click
    End Sub

    Private Sub lbl_LINK_LinkClicked_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbl_LINK.LinkClicked
        System.Diagnostics.Process.Start("http:\\netstudio.waw.pl")
    End Sub

    Private Sub txt_NAZWA_NADAWCY_KeyPress(sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NAZWA_NADAWCY.KeyPress
        '********************************************** niedozwolone wpisywanie spacji w polu NAZWA_NADAWCY
        If (Microsoft.VisualBasic.Asc(e.KeyChar) = 32) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_PRZESYLKA_KeyPress(sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PRZESYLKA.KeyPress
        '********************************************** niedozwolone wpisywanie spacji w polu PRZESYLKA
        If (Microsoft.VisualBasic.Asc(e.KeyChar) = 32) Then
            e.Handled = True
        End If
    End Sub

    Function zamien_znaki(ByVal tekst As String) As String
        '********************************************** funkcja zamieniajaca polskie znaki diakrytyczne
        Dim RE, MC, M As Object
        Dim ZamienNa As String
        RE = CreateObject("VBScript.RegExp")
        RE.Global = True
        RE.Pattern = "([ÊêÓó¡±¦¶£³¬¼¯¿ÆæÑñĘęÓóĄąŚśŁłŹźŻżĆćŃń'])"
        ZamienNa = "  EeOoAaSsLlZzZzCcNnEeOoAaSsLlZzZzCcNn,"
        MC = RE.Execute(tekst)
        RE.Global = False

        For Each M In MC
            tekst = RE.Replace(tekst, Mid(ZamienNa, InStr(1, RE.Pattern, M.Value), 1))
        Next

        MC = Nothing
        RE = Nothing
        Return tekst
    End Function

    Private Sub TrackBarControl1_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles tbc_ROZSYLKA.EditValueChanged
        Select Case tbc_ROZSYLKA.Value
            Case 1
                lbl_ROZSYLKA.Text = "0.5" & " min."
            Case 2
                lbl_ROZSYLKA.Text = "1" & " min."
            Case 3
                lbl_ROZSYLKA.Text = "1.5" & " min."
            Case 4
                lbl_ROZSYLKA.Text = "2" & " min."
            Case 5
                lbl_ROZSYLKA.Text = "2.5" & " min."
            Case 6
                lbl_ROZSYLKA.Text = "3" & " min."
            Case 7
                lbl_ROZSYLKA.Text = "3.5" & " min."
            Case 8
                lbl_ROZSYLKA.Text = "4" & " min."
            Case 9
                lbl_ROZSYLKA.Text = "4.5" & " min."
            Case 10
                lbl_ROZSYLKA.Text = "5" & " min."
        End Select
    End Sub
End Class

Public Class SendClass_offline
    Public dev_mac As String
    Public file_name As String

    Public Sub send_data()
        '********************************************** procedura wysylajaca dane do urzadzenia
        Cursor.Current = Cursors.WaitCursor
        Dim theuri As New Uri("obex://" + dev_mac + "/" + file_name)
        Dim request As New ObexWebRequest(theuri)
        request.ReadFile(file_name)

        '********************************************** procedura zapisujaca status transmisji do bazy
        Dim response As ObexWebResponse = CType(request.GetResponse(), ObexWebResponse)
        Dim conn As New SqlCeConnection()
        conn.ConnectionString = "Persist Security Info=False; Data Source = blueAdv_dbf.sdf; Password = otop95otop;"
        conn.Open()
        Dim SQLcmd As SqlCeCommand = New SqlCeCommand("UPDATE ba_sessions_details SET request = '" & response.StatusCode.ToString & "' WHERE dev_mac='" & dev_mac & "'", conn)
        SQLcmd.ExecuteNonQuery()
        conn.Close()
        response.Close()
        Cursor.Current = Cursors.Default
    End Sub
End Class
