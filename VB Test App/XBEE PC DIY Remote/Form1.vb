Imports System.Text.Encoding 'From VB Pipe
Public Class Form1
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Private Shared mut As New Mutex()

    Public SCListBox As New StringCollection        ' use this to communicate between background thread and main thread...

    ' Lets define a bunch of global junk, later should clean up!
    Public fDataHasChanged As Boolean        '
    Public XBSW As Stopwatch
    Public MaxListCount As Integer
    Public XBeeCommport As String
    Public RobotCommport As String




    Private Function intToByte(ByVal i As Integer)

        Dim bArray As Byte() = {i And 255, (i >> 8) And 255, (i >> 16) And 255, (i >> 24) And 255}
        intToByte = bArray

    End Function

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        My.Settings.MainFormSize = Size

    End Sub
    '========= VB Pipe end==============




    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Show all available COM ports.
        Size = My.Settings.MainFormSize   ' see if we can save and restore the size

        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComLB.Items.Add(sp)
            RobotComLB.Items.Add(sp)
        Next

        ComLB.SelectedIndex = ComLB.FindString(My.Settings.Commport)
        RobotComLB.SelectedIndex = RobotComLB.FindString(My.Settings.RobotCommPort)

        If ComLB.SelectedIndex = -1 Then
            Connect.Enabled = False

        End If

        MaxListCount = My.Settings.MaxListCount


        ' Lets try to reload the collection of destinations into the list

        ' Initialize the state of our cached buttons.
        fDataHasChanged = False
        ' create our timer
        XBSW = New Stopwatch()
    End Sub



    Private Sub Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Connect.Click
        If Connect.Text = "Connect" Then
            Timer1.Enabled = True       ' also get our timer running...
            ' Connect.Text = "Disconnect"
            XBeeCommport = ComLB.Items(ComLB.SelectedIndex)
            RobotCommport = RobotComLB.Items(RobotComLB.SelectedIndex)

            CommThread.RunWorkerAsync()
        Else
            '            Connect.Text = "Connect"
            CommThread.CancelAsync()            ' turn off the thread...
        End If

    End Sub


    Public Sub ClearInputBuffer(ByRef com1 As IO.Ports.SerialPort)
        Dim cbRead As Byte
        Dim b(1) As Byte

        com1.DiscardInBuffer()    ' flush out anything we have
        cbRead = 1
        com1.ReadTimeout = 100 'wait a maximum of .1 seconds for a response
        Try
            While cbRead > 0
                cbRead = com1.Read(b, 0, 1)
            End While

        Catch e As TimeoutException
            'Debug.Print("TO: Clear Input Buffer")
        Catch ex As Exception

        End Try

    End Sub


    '==============================================================================
    ' [DisplayRemoteString (pStr, cbOffset, cbStr)]
    ' 
    ' This function takes care of displaying string and or a number sent to us 
    ' rom the remote robot.  For now hard coded to a specific location on line 2...
    '
    ' This function will also switch to the appropriate display mode if necessary.
    '==============================================================================
    Public Sub DisplayRemoteString(ByRef pstr() As Byte, ByVal cbOffset As Byte, ByVal cbStr As Byte)
        ' Make sure we are in the right mode to display the data

        If cbStr Then
            Dim s As String = Mid(System.Text.Encoding.ASCII.GetString(pstr), cbOffset + 1, cbStr)

            SyncLock SCListBox.SyncRoot
                SCListBox.Add(s)
            End SyncLock

        End If
        Return
    End Sub


    Private Sub CommThread_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles CommThread.DoWork

        CommThread.ReportProgress(0)
        XBSW.Start()
        Dim ab(120) As Byte
        Dim abCommander(4096) As Byte
        Dim cbCommander As UInt16
        Dim cb As Byte
        Dim fDidSomething As Boolean
        'Dim Counter As Byte

        Try

            Using com1 As IO.Ports.SerialPort = _
                My.Computer.Ports.OpenSerialPort(XBeeCommport, 38400, IO.Ports.Parity.None)
                com1.Handshake = IO.Ports.Handshake.None
                com1.DtrEnable = False
                com1.NewLine = Chr(13)

                Using com2 As IO.Ports.SerialPort = _
                    My.Computer.Ports.OpenSerialPort(RobotCommport, 38400, IO.Ports.Parity.None)
                    com2.Handshake = IO.Ports.Handshake.None
                    com2.DtrEnable = False
                    com2.NewLine = Chr(13)

                    cb = 0
                    While Not CommThread.CancellationPending              ' turn off the thread...
                        fDidSomething = False
                        ' Echo any characters we receive on Comm 1 to Comm 2
                        cbCommander = com1.BytesToRead
                        If (cbCommander) Then
                            com1.Read(abCommander, 0, cbCommander)
                            com2.Write(abCommander, 0, cbCommander)
                            com1.BaseStream.Flush()
                            fDidSomething = True
                        End If

                        ' and echo anything robot sends us to our listbox...
                        While (com2.BytesToRead)
                            fDidSomething = True
                            ab(cb) = com2.ReadByte()

                            If ab(cb) = 13 Then
                                DisplayRemoteString(ab, 0, cb)
                                cb = 0
                            ElseIf ab(cb) >= 32 Then
                                cb = cb + 1
                                If (cb >= 120) Then
                                    DisplayRemoteString(ab, 0, cb)
                                    cb = 0
                                End If
                            End If
                            If Not fDidSomething Then
                                System.Threading.Thread.Sleep(2)  ' try to completely hog the system!
                            End If

                        End While

                    End While

                End Using

            End Using

        Catch ex As Exception

        End Try

        CommThread.ReportProgress(99)

    End Sub


    Private Sub CommThread_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles CommThread.ProgressChanged
        If e.ProgressPercentage < 10 Then
            LCDLB.Items.Add("*** Thread Start ***")
            LCDLB.TopIndex = LCDLB.Items.Count - 1
            Connect.Text = "Disconnect"
        ElseIf e.ProgressPercentage > 90 Then
            LCDLB.Items.Add("*** Thread Canceled ***")
            LCDLB.TopIndex = LCDLB.Items.Count - 1
            Timer1.Enabled = False  'We can stop our timer now...
            Connect.Text = "Connect"

            Try

                Using com1 As IO.Ports.SerialPort = _
                        My.Computer.Ports.OpenSerialPort(XBeeCommport, 38400, IO.Ports.Parity.None)
                    com1.Handshake = IO.Ports.Handshake.None
                    System.Threading.Thread.Sleep(500)
                    ClearInputBuffer(com1)      ' make sure there is nothing there to start with...

                    ClearInputBuffer(com1)      ' make sure there is nothing there to start with...

                End Using
            Catch ex As Exception

            End Try
        End If
    End Sub


    Private Sub ComLB_DropDown(sender As Object, e As System.EventArgs) Handles ComLB.DropDown
        ComLB.Items.Clear() ' first clear our list.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComLB.Items.Add(sp)
        Next

        ComLB.SelectedIndex = ComLB.FindString(My.Settings.Commport)

        Connect.Enabled = False
    End Sub
    Private Sub RobotComLB_DropDown(sender As Object, e As System.EventArgs) Handles RobotComLB.DropDown

        RobotComLB.Items.Clear() ' first clear our list.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            RobotComLB.Items.Add(sp)
        Next

        RobotComLB.SelectedIndex = RobotComLB.FindString(My.Settings.RobotCommPort)

        Connect.Enabled = False
    End Sub


    Private Sub ComLB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComLB.SelectedIndexChanged
        ' Show all available COM ports.

        If (ComLB.SelectedIndex >= 0) Then
            My.Settings.Commport = ComLB.Items(ComLB.SelectedIndex).ToString
            Connect.Enabled = True
        Else
            ' nothing selected, disable a few controls.
            Connect.Enabled = False
        End If

    End Sub

    Private Sub RobotComLB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RobotComLB.SelectedIndexChanged
        ' Show all available COM ports.

        If (RobotComLB.SelectedIndex >= 0) Then
            My.Settings.RobotCommPort = RobotComLB.Items(RobotComLB.SelectedIndex).ToString
            Connect.Enabled = True
        Else
            ' nothing selected, disable a few controls.
            Connect.Enabled = False
        End If

    End Sub
    Private Sub LCDLB_MouseDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LCDLB.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            LCDLB.ContextMenuStrip = PMTerminal
        End If

    End Sub

    Private Sub LCDLBClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBTerminalClear.Click
        LCDLB.Items.Clear() ' clear out the listbox
    End Sub

    Private Sub LBTerminalSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBTerminalSelectAll.Click
        Dim i As Integer
        For i = 0 To LCDLB.Items.Count - 1
            LCDLB.SetSelected(i, True)
        Next

    End Sub

    Private Sub LBTerminalCopytoClipBoard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBTerminalCopytoClipBoard.Click
        Dim i As Integer
        Dim s As String = ""
        For i = 0 To LCDLB.Items.Count - 1
            If LCDLB.GetSelected(0) Then
                s = s + LCDLB.Items.Item(i).ToString + ControlChars.CrLf
            End If
        Next
        If s <> "" Then
            Clipboard.SetDataObject(s)
        End If

    End Sub

    Private Sub LBTerminalCount_Click(sender As Object, e As EventArgs) Handles LBTerminalCount.Click
        Dim NewCountString As String
        NewCountString = InputBox("Enter Terminal List Max Count: ", "XBee DIY Remote", MaxListCount.ToString)
        Try
            MaxListCount = Integer.Parse(NewCountString, Globalization.NumberStyles.Integer)
            My.Settings.MaxListCount = MaxListCount     ' save away the maximum count

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim NewObj As Object
        If SCListBox.Count > 0 Then
            LCDLB.BeginUpdate()
            While SCListBox.Count > 0
                SyncLock SCListBox.SyncRoot                 ' Minimize the time we hold the semaphore to simply remove an item...
                    NewObj = SCListBox(0)                   'get items from the start and add to our listbox
                    SCListBox.RemoveAt(0)                   'Delete that item from our list
                End SyncLock
                LCDLB.Items.Add(NewObj)                     'get items from the start and add to our listbox
                If LCDLB.Items.Count > MaxListCount Then
                    LCDLB.Items.RemoveAt(0)                 ' Getting to large so lets remove the first item...
                End If
            End While
            ' Duplicate loop to remove excess items... Will take care of cases where stuff was added elsewhere
            While LCDLB.Items.Count > MaxListCount
                LCDLB.Items.RemoveAt(0)                 ' Getting to large so lets remove the first item...
            End While
            LCDLB.TopIndex = LCDLB.Items.Count - 1  ' And Scroll to that location...
            LCDLB.EndUpdate()
        End If
    End Sub

  
End Class


