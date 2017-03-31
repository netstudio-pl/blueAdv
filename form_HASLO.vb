Imports Microsoft.Win32
Imports System.Security.Cryptography

Public Class form_HASLO

    Private Sub form_HASLO_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txt_PASSWORD.Focus()
    End Sub

    Private Sub btn_HASLO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_HASLO.Click
        If txt_PASSWORD.Text = "haslo" Then
            form_MAIN.NotifyIcon1.Visible = False
            End
        Else
            lbl_HASLO.Text = "Błędne hasło!"
            txt_PASSWORD.Text = ""
            txt_PASSWORD.Focus()
        End If
    End Sub

    Private Sub txt_PASSWORD_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txt_PASSWORD.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            btn_HASLO.Focus()
        End If
    End Sub

End Class