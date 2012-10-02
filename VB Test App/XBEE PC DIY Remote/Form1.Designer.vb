<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.ComGB = New System.Windows.Forms.GroupBox()
        Me.ComLB = New System.Windows.Forms.ComboBox()
        Me.Connect = New System.Windows.Forms.Button()
        Me.CommThread = New System.ComponentModel.BackgroundWorker()
        Me.LCDLB = New System.Windows.Forms.ListBox()
        Me.LCDGroup = New System.Windows.Forms.GroupBox()
        Me.PMTerminal = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LBTerminalCount = New System.Windows.Forms.ToolStripMenuItem()
        Me.LBTerminalSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.LBTerminalCopytoClipBoard = New System.Windows.Forms.ToolStripMenuItem()
        Me.LBTerminalClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.LCDLBClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.XBeeDLContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.XBDLCM_DeleteItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.XBDLCM_Clear = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.RoboCommGB = New System.Windows.Forms.GroupBox()
        Me.RobotComLB = New System.Windows.Forms.ComboBox()
        Me.ComGB.SuspendLayout()
        Me.LCDGroup.SuspendLayout()
        Me.PMTerminal.SuspendLayout()
        Me.XBeeDLContextMenu.SuspendLayout()
        Me.RoboCommGB.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComGB
        '
        Me.ComGB.Controls.Add(Me.ComLB)
        Me.ComGB.Location = New System.Drawing.Point(12, 12)
        Me.ComGB.Name = "ComGB"
        Me.ComGB.Size = New System.Drawing.Size(136, 47)
        Me.ComGB.TabIndex = 12
        Me.ComGB.TabStop = False
        Me.ComGB.Text = "Commander Comm Port"
        '
        'ComLB
        '
        Me.ComLB.FormattingEnabled = True
        Me.ComLB.Location = New System.Drawing.Point(1, 19)
        Me.ComLB.Name = "ComLB"
        Me.ComLB.Size = New System.Drawing.Size(123, 21)
        Me.ComLB.TabIndex = 1
        '
        'Connect
        '
        Me.Connect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Connect.Location = New System.Drawing.Point(376, 29)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(97, 23)
        Me.Connect.TabIndex = 15
        Me.Connect.Text = "Connect"
        Me.Connect.UseVisualStyleBackColor = True
        '
        'CommThread
        '
        Me.CommThread.WorkerReportsProgress = True
        Me.CommThread.WorkerSupportsCancellation = True
        '
        'LCDLB
        '
        Me.LCDLB.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LCDLB.FormattingEnabled = True
        Me.LCDLB.Location = New System.Drawing.Point(3, 16)
        Me.LCDLB.Name = "LCDLB"
        Me.LCDLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LCDLB.Size = New System.Drawing.Size(455, 167)
        Me.LCDLB.TabIndex = 0
        '
        'LCDGroup
        '
        Me.LCDGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LCDGroup.Controls.Add(Me.LCDLB)
        Me.LCDGroup.Location = New System.Drawing.Point(15, 64)
        Me.LCDGroup.Name = "LCDGroup"
        Me.LCDGroup.Size = New System.Drawing.Size(461, 186)
        Me.LCDGroup.TabIndex = 43
        Me.LCDGroup.TabStop = False
        Me.LCDGroup.Text = "LCD"
        '
        'PMTerminal
        '
        Me.PMTerminal.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LBTerminalCount, Me.LBTerminalSelectAll, Me.LBTerminalCopytoClipBoard, Me.LBTerminalClear})
        Me.PMTerminal.Name = "PMTerminal"
        Me.PMTerminal.Size = New System.Drawing.Size(123, 92)
        '
        'LBTerminalCount
        '
        Me.LBTerminalCount.Name = "LBTerminalCount"
        Me.LBTerminalCount.Size = New System.Drawing.Size(122, 22)
        Me.LBTerminalCount.Text = "Count..."
        '
        'LBTerminalSelectAll
        '
        Me.LBTerminalSelectAll.Name = "LBTerminalSelectAll"
        Me.LBTerminalSelectAll.Size = New System.Drawing.Size(122, 22)
        Me.LBTerminalSelectAll.Text = "Select All"
        '
        'LBTerminalCopytoClipBoard
        '
        Me.LBTerminalCopytoClipBoard.Name = "LBTerminalCopytoClipBoard"
        Me.LBTerminalCopytoClipBoard.Size = New System.Drawing.Size(122, 22)
        Me.LBTerminalCopytoClipBoard.Text = "Copy"
        '
        'LBTerminalClear
        '
        Me.LBTerminalClear.Name = "LBTerminalClear"
        Me.LBTerminalClear.Size = New System.Drawing.Size(122, 22)
        Me.LBTerminalClear.Text = "Clear"
        '
        'LCDLBClear
        '
        Me.LCDLBClear.Name = "LCDLBClear"
        Me.LCDLBClear.Size = New System.Drawing.Size(152, 22)
        Me.LCDLBClear.Text = "Clear"
        '
        'XBeeDLContextMenu
        '
        Me.XBeeDLContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.XBDLCM_DeleteItem, Me.XBDLCM_Clear})
        Me.XBeeDLContextMenu.Name = "PMTerminal"
        Me.XBeeDLContextMenu.Size = New System.Drawing.Size(135, 48)
        '
        'XBDLCM_DeleteItem
        '
        Me.XBDLCM_DeleteItem.Name = "XBDLCM_DeleteItem"
        Me.XBDLCM_DeleteItem.Size = New System.Drawing.Size(134, 22)
        Me.XBDLCM_DeleteItem.Text = "Delete Item"
        '
        'XBDLCM_Clear
        '
        Me.XBDLCM_Clear.Name = "XBDLCM_Clear"
        Me.XBDLCM_Clear.Size = New System.Drawing.Size(134, 22)
        Me.XBDLCM_Clear.Text = "Clear List"
        '
        'Timer1
        '
        Me.Timer1.Interval = 250
        '
        'RoboCommGB
        '
        Me.RoboCommGB.Controls.Add(Me.RobotComLB)
        Me.RoboCommGB.Location = New System.Drawing.Point(177, 12)
        Me.RoboCommGB.Name = "RoboCommGB"
        Me.RoboCommGB.Size = New System.Drawing.Size(122, 47)
        Me.RoboCommGB.TabIndex = 50
        Me.RoboCommGB.TabStop = False
        Me.RoboCommGB.Text = "Robot Comm Port"
        '
        'RobotComLB
        '
        Me.RobotComLB.FormattingEnabled = True
        Me.RobotComLB.Location = New System.Drawing.Point(1, 19)
        Me.RobotComLB.Name = "RobotComLB"
        Me.RobotComLB.Size = New System.Drawing.Size(122, 21)
        Me.RobotComLB.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 262)
        Me.Controls.Add(Me.RoboCommGB)
        Me.Controls.Add(Me.Connect)
        Me.Controls.Add(Me.ComGB)
        Me.Controls.Add(Me.LCDGroup)
        Me.MinimumSize = New System.Drawing.Size(500, 300)
        Me.Name = "Form1"
        Me.Text = "Commander Debug Terminal"
        Me.ComGB.ResumeLayout(False)
        Me.LCDGroup.ResumeLayout(False)
        Me.PMTerminal.ResumeLayout(False)
        Me.XBeeDLContextMenu.ResumeLayout(False)
        Me.RoboCommGB.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ComGB As System.Windows.Forms.GroupBox
    Friend WithEvents ComLB As System.Windows.Forms.ComboBox
    Friend WithEvents Connect As System.Windows.Forms.Button
    Friend WithEvents CommThread As System.ComponentModel.BackgroundWorker
    Friend WithEvents LCDLB As System.Windows.Forms.ListBox
    Friend WithEvents LCDGroup As System.Windows.Forms.GroupBox
    Friend WithEvents PMTerminal As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents LCDLBClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LBTerminalSelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LBTerminalCopytoClipBoard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LBTerminalClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XBeeDLContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents XBDLCM_DeleteItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XBDLCM_Clear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents LBTerminalCount As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RoboCommGB As System.Windows.Forms.GroupBox
    Friend WithEvents RobotComLB As System.Windows.Forms.ComboBox

End Class
