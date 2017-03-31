<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_HASLO
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.txt_PASSWORD = New System.Windows.Forms.TextBox
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.btn_HASLO = New DevExpress.XtraEditors.SimpleButton
        Me.lbl_HASLO = New DevExpress.XtraEditors.LabelControl
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.lbl_HASLO)
        Me.PanelControl1.Controls.Add(Me.txt_PASSWORD)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.btn_HASLO)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(229, 128)
        Me.PanelControl1.TabIndex = 0
        '
        'txt_PASSWORD
        '
        Me.txt_PASSWORD.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.txt_PASSWORD.Location = New System.Drawing.Point(67, 36)
        Me.txt_PASSWORD.Name = "txt_PASSWORD"
        Me.txt_PASSWORD.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt_PASSWORD.Size = New System.Drawing.Size(100, 23)
        Me.txt_PASSWORD.TabIndex = 0
        Me.txt_PASSWORD.UseSystemPasswordChar = True
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Location = New System.Drawing.Point(6, 12)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(216, 18)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "Podaj hasło do zamkniecia aplikacji"
        '
        'btn_HASLO
        '
        Me.btn_HASLO.Location = New System.Drawing.Point(89, 65)
        Me.btn_HASLO.Name = "btn_HASLO"
        Me.btn_HASLO.Size = New System.Drawing.Size(50, 23)
        Me.btn_HASLO.TabIndex = 1
        Me.btn_HASLO.Text = "OK"
        '
        'lbl_HASLO
        '
        Me.lbl_HASLO.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lbl_HASLO.Appearance.ForeColor = System.Drawing.Color.Red
        Me.lbl_HASLO.Appearance.Options.UseFont = True
        Me.lbl_HASLO.Appearance.Options.UseForeColor = True
        Me.lbl_HASLO.Location = New System.Drawing.Point(78, 101)
        Me.lbl_HASLO.Name = "lbl_HASLO"
        Me.lbl_HASLO.Size = New System.Drawing.Size(0, 15)
        Me.lbl_HASLO.TabIndex = 2
        '
        'form_HASLO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(229, 128)
        Me.ControlBox = False
        Me.Controls.Add(Me.PanelControl1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "form_HASLO"
        Me.Text = "Hasło"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btn_HASLO As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txt_PASSWORD As System.Windows.Forms.TextBox
    Friend WithEvents lbl_HASLO As DevExpress.XtraEditors.LabelControl
End Class
