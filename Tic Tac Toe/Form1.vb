
Public Class Form1

    Private Context As New BufferedGraphicsContext

    Private Buffer As BufferedGraphics

    Private FrameCount As Integer = 0

    Private StartTime As DateTime = Now

    Private TimeElapsed As TimeSpan

    Private SecondsElapsed As Double

    Private FPS As Integer

    Private ReadOnly FPSFont As New Font(FontFamily.GenericSansSerif, 25)

    Private Rect As New Rectangle(0, 100, 300, 100)

    Private WinMarkerStart As Point
    Private WinMarkerEnd As Point
    Private WinMarkerVisable As Boolean = False


    Enum Cell
        Empty
        X
        O
    End Enum

    Dim board(2, 2) As Cell

    Enum Player
        X
        O
    End Enum

    Dim currentPlayer As Player = Player.X

    Dim cellSize As Integer = 200
    Dim cellPadding As Integer = 30
    'Dim cellPaddingWidth As Integer = 30
    'Dim OPen As New Pen(Brushes.Red, 10)
    'Dim XPen As New Pen(Brushes.Blue, 10)
    'Dim LinePen As New Pen(Brushes.White, 8)
    Dim XOffset As Integer = 100

    Dim cellwidth As Integer = 0
    Dim cellPaddingWidth As Integer = 0
    Dim cellHeight As Integer = 0
    Dim cellPaddingHeight As Integer = 0
    Dim LinePenWidth As Integer = 0
    Dim OPenWidth As Integer = 0
    Dim XPenWidth As Integer = 0
    Dim OneThirdClientWidth As Integer = 0


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'currentPlayer = Player.X

        InitializeBoard()

        'board(1, 1) = Cell.O
        'board(0, 0) = Cell.X
        'board(2, 2) = Cell.X

        'board(2, 0) = Cell.X

        'board(0, 2) = Cell.O

        Text = "Tic Tac Toe - Code with Joe"

        SetStyle(ControlStyles.UserPaint, True)

        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        InitBuffer()

        InitTimer1()

    End Sub

    Private Sub InitializeBoard()

        For x = 0 To 2

            For y = 0 To 2

                board(x, y) = Cell.Empty

            Next
        Next

    End Sub

    Private Sub InitTimer1()

        'Set tick rate to 60 ticks per second. 1 second = 1000 milliseconds.
        Timer1.Interval = 15 '16.66666666666667 ms = 1000 ms / 60 ticks

        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Every tick of timer do the following...

        UpdateGame()

        Refresh() 'Calls OnPaint Event

    End Sub

    Private Sub UpdateGame()

        Rect.X += 2

        Rect.Width = ClientSize.Width \ 6

        If Rect.X > ClientSize.Width Then

            Rect.X = 0 - Rect.Width

        End If

        Rect.Height = ClientSize.Height \ 10

        Rect.Y = (ClientSize.Height \ 2) - (Rect.Height \ 2)

    End Sub
    Private Sub InitBuffer()

        'Set context to the context of this app.
        Context = BufferedGraphicsManager.Current

        'Set buffer size to the primary working area.
        Context.MaximumBuffer = Screen.PrimaryScreen.WorkingArea.Size

        'Create buffer.
        Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Buffer.Graphics.Clear(Color.Black)

        DrawGridLines()

        DrawXsAndOs()

        If WinMarkerVisable = True Then

            Dim WinPen As New Pen(Brushes.Purple, XPenWidth)

            Buffer.Graphics.DrawLine(WinPen, WinMarkerStart, WinMarkerEnd)

            WinPen.Dispose()

        End If

        'Draw frames per second display.
        'Buffer.Graphics.DrawString(FPS & " FPS", FPSFont, Brushes.MediumOrchid, 0, ClientRectangle.Bottom - 75)

        'Show buffer on form.
        Buffer.Render(e.Graphics)

        'Release memory used by buffer.
        Buffer.Dispose()
        Buffer = Nothing

        'Create new buffer.
        Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)
        'Buffer.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        'Use these settings when drawing to the backbuffer.
        With Buffer.Graphics
            'Bug Fix
            .CompositingMode = Drawing2D.CompositingMode.SourceOver 'Don't Change.
            'To fix draw string error with anti aliasing: "Parameters not valid."
            'I set the compositing mode to source over.
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            .CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
            .InterpolationMode = Drawing2D.InterpolationMode.Bicubic
            .PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        End With

        UpdateFrameCounter()

    End Sub

    Private Sub DrawXsAndOs()

        For x = 0 To 2
            For y = 0 To 2

                'Does the cell contain an x?
                If board(x, y) = Cell.X Then
                    'Yes, the cell contains an x.

                    DrawX(x, y)

                    'Does the cell contain an o?
                ElseIf board(x, y) = Cell.O Then
                    'Yes, the cell contains an o.

                    DrawO(x, y)

                End If
            Next
        Next

    End Sub

    Private Sub DrawX(x As Integer, y As Integer)

        Dim XPen As New Pen(Brushes.Blue, XPenWidth)

        Buffer.Graphics.DrawLine(XPen,
                                 x * cellwidth + cellPaddingWidth,
                                 y * cellHeight + cellPaddingHeight,
                                 (x + 1) * cellwidth - cellPaddingWidth,
                                 (y + 1) * cellHeight - cellPaddingHeight)

        Buffer.Graphics.DrawLine(XPen,
                                 x * cellwidth + cellPaddingWidth,
                                 (y + 1) * cellHeight - cellPaddingHeight,
                                 (x + 1) * cellwidth - cellPaddingWidth,
                                 y * cellHeight + cellPaddingHeight)

        XPen.Dispose()

    End Sub

    Private Sub DrawO(x As Integer, y As Integer)

        Dim OPen As New Pen(Brushes.Red, OPenWidth)

        Buffer.Graphics.DrawEllipse(OPen,
                                    x * cellwidth + cellPaddingWidth,
                                    y * cellHeight + cellPaddingHeight,
                                    cellwidth - 2 * cellPaddingWidth,
                                    cellHeight - 2 * cellPaddingHeight)

        OPen.Dispose()

    End Sub

    Private Sub DrawGridLines()

        Dim LinePen As New Pen(Brushes.White, LinePenWidth)

        ' Draw vertical lines
        Buffer.Graphics.DrawLine(LinePen, cellwidth, 0, cellwidth, ClientSize.Height)
        Buffer.Graphics.DrawLine(LinePen, ClientSize.Width * 2 \ 3, 0, ClientSize.Width * 2 \ 3, ClientSize.Height)

        ' Draw horizontal lines
        Buffer.Graphics.DrawLine(LinePen, 0, cellHeight, ClientSize.Width, ClientSize.Height \ 3)
        Buffer.Graphics.DrawLine(LinePen, 0, ClientSize.Height * 2 \ 3, ClientSize.Width, ClientSize.Height * 2 \ 3)

        LinePen.Dispose()

    End Sub

    Private Sub UpdateFrameCounter()

        TimeElapsed = Now.Subtract(StartTime)

        SecondsElapsed = TimeElapsed.TotalSeconds

        If SecondsElapsed < 1 Then

            FrameCount += 1

        Else

            FPS = FrameCount.ToString

            FrameCount = 0

            StartTime = Now

        End If

    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)

        'Intentionally left blank. Do not remove.

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        'cellSize = ClientSize.Height \ 3
        OneThirdClientWidth = ClientSize.Width \ 3

        cellwidth = ClientSize.Width \ 3
        cellPaddingWidth = cellwidth \ 8
        cellHeight = ClientSize.Height \ 3
        cellPaddingHeight = cellHeight \ 8


        If cellwidth <= cellHeight Then

            LinePenWidth = cellwidth \ 32
            OPenWidth = cellwidth \ 16
            XPenWidth = cellwidth \ 16

        Else

            LinePenWidth = cellHeight \ 32
            OPenWidth = cellHeight \ 16
            XPenWidth = cellHeight \ 16

        End If

    End Sub

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        UpdateMouse(e)

    End Sub

    Private Sub UpdateMouse(e As MouseEventArgs)

        Dim x As Integer = MouseToBoardX(e)

        Dim y As Integer = MouseToBoardY(e)

        If board(x, y) = Cell.Empty Then

            If currentPlayer = Player.X Then

                My.Computer.Audio.Play(My.Resources.tone700freq, AudioPlayMode.Background)

                board(x, y) = Cell.X

                currentPlayer = Player.O

            End If

            If CheckForWin(Cell.X) Then

                MsgBox("You win!", MsgBoxStyle.DefaultButton1, "End Game")

                ResetGame()

            ElseIf CheckForDraw() Then

                MsgBox("Draw!", MsgBoxStyle.DefaultButton1, "End Game")

                ResetGame()

            Else
                ' Computer player's turn

                ComputerMove()

                If CheckForWin(Cell.O) Then

                    MsgBox("Computer wins!", MsgBoxStyle.DefaultButton1, "End Game")

                    ResetGame()

                ElseIf CheckForDraw() Then

                    MsgBox("Draw!", MsgBoxStyle.DefaultButton1, "End Game")

                    ResetGame()

                Else

                    currentPlayer = Player.X

                End If

            End If

        End If

    End Sub

    Private Sub ResetGame()

        WinMarkerVisable = False

        For x = 0 To 2
            For y = 0 To 2
                board(x, y) = Cell.Empty
            Next
        Next

        currentPlayer = Player.X

    End Sub

    Private Function MouseToBoardY(e As MouseEventArgs) As Integer

        If e.Y < ClientSize.Height Then

            Return e.Y * 3 \ ClientSize.Height 'Returns the column number.

        Else

            Return 2 'The upper bounds of the Y axis.

        End If

    End Function

    Private Function MouseToBoardX(e As MouseEventArgs) As Integer

        If e.X < ClientSize.Width Then

            Return e.X * 3 \ ClientSize.Width 'Returns the row number.

        Else

            Return 2 'The upper bounds of the X axis.

        End If

    End Function

    Private Sub ComputerMove()

        Dim rnd As New Random()

        Dim x, y As Integer

        Do
            'Initialize random-number generator.
            Randomize()

            x = rnd.Next(3)

            'Initialize random-number generator.
            Randomize()

            y = rnd.Next(3)

        Loop While board(x, y) <> Cell.Empty

        'My.Computer.Audio.Play(My.Resources.tone700freq, AudioPlayMode.Background)

        board(x, y) = Cell.O

    End Sub

    Private Function CheckForWin(player As Cell) As Boolean

        ' Check rows ---
        For y = 0 To 2

            If board(0, y) = player AndAlso board(1, y) = player AndAlso board(2, y) = player Then

                Select Case y
                    Case 0 'Top row
                        WinMarkerVisable = True
                        'ClientSize.Height \ 6
                        WinMarkerStart.X = ClientRectangle.Left + cellPaddingWidth
                        WinMarkerStart.Y = ClientSize.Height \ 6
                        WinMarkerEnd.X = ClientRectangle.Right - cellPaddingWidth
                        WinMarkerEnd.Y = ClientSize.Height \ 6
                    Case 1 'Mid row
                        WinMarkerVisable = True
                        'ClientSize.Height \ 6
                        WinMarkerStart.X = ClientRectangle.Left + cellPaddingWidth
                        WinMarkerStart.Y = ClientSize.Height \ 2
                        WinMarkerEnd.X = ClientRectangle.Right - cellPaddingWidth
                        WinMarkerEnd.Y = ClientSize.Height \ 2
                    Case 2 'Bottom rom
                        WinMarkerVisable = True
                        'ClientSize.Height \ 6
                        WinMarkerStart.X = ClientRectangle.Left + cellPaddingWidth
                        WinMarkerStart.Y = ClientSize.Height - ClientSize.Height \ 6
                        WinMarkerEnd.X = ClientRectangle.Right - cellPaddingWidth
                        WinMarkerEnd.Y = ClientSize.Height - ClientSize.Height \ 6
                End Select


                Return True

            End If

        Next

        ' Check columns |
        For x = 0 To 2

            If board(x, 0) = player AndAlso board(x, 1) = player AndAlso board(x, 2) = player Then

                Return True

            End If

        Next

        ' Check diagonals
        If board(0, 0) = player AndAlso board(1, 1) = player AndAlso board(2, 2) = player Then

            WinMarkerVisable = True
            WinMarkerStart.X = ClientRectangle.Left + cellPaddingWidth
            WinMarkerStart.Y = ClientRectangle.Top + cellPaddingHeight
            WinMarkerEnd.X = ClientRectangle.Right - cellPaddingWidth
            WinMarkerEnd.Y = ClientRectangle.Bottom - cellPaddingHeight

            Return True

        End If

        If board(2, 0) = player AndAlso board(1, 1) = player AndAlso board(0, 2) = player Then

            Return True

        End If

        Return False

    End Function

    Private Function CheckForDraw() As Boolean

        For x = 0 To 2

            For y = 0 To 2

                If board(x, y) = Cell.Empty Then

                    Return False

                End If

            Next

        Next

        Return True

    End Function

End Class



