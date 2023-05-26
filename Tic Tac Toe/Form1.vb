
Public Class Form1

    Private Context As New BufferedGraphicsContext

    Private Buffer As BufferedGraphics

    Private FrameCount As Integer = 0

    Private StartTime As DateTime = Now

    Private TimeElapsed As TimeSpan

    Private SecondsElapsed As Double

    Private FPS As Integer

    Private ReadOnly FPSFont As New Font(FontFamily.GenericSansSerif, 25)

    Private WinMarkerStart As Point
    Private WinMarkerEnd As Point
    Private WinMarkerVisable As Boolean = False

    Enum Cell
        Empty
        X
        O
    End Enum

    Enum Win
        Draw
        Computer
        Human
    End Enum

    Private Winner As Win = Win.Draw

    Private ReadOnly Board(2, 2) As Cell

    Dim CurrentPlayer As Cell = Cell.X
    Dim CellWidth As Integer = 0
    Dim CellPaddingWidth As Integer = 0
    Dim CellHeight As Integer = 0
    Dim CellPaddingHeight As Integer = 0
    Dim LinePenWidth As Integer = 0
    Dim OPenWidth As Integer = 0
    Dim XPenWidth As Integer = 0
    Dim WinPenWidth As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeBoard()

        Text = "Tic Tac Toe - Code with Joe"

        SetStyle(ControlStyles.UserPaint, True)

        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        InitBuffer()

        InitTimer1()

    End Sub

    Private Sub InitializeBoard()

        For X = 0 To 2

            For Y = 0 To 2

                Board(X, Y) = Cell.Empty

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

        DrawWinMarker()

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

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        UpdateMouse(e)

    End Sub

    Private Sub UpdateMouse(e As MouseEventArgs)

        Dim X As Integer = MouseToBoardX(e)

        Dim Y As Integer = MouseToBoardY(e)

        If Board(X, Y) = Cell.Empty Then

            If CurrentPlayer = Cell.X Then

                My.Computer.Audio.Play(My.Resources.tone700freq, AudioPlayMode.Background)

                Board(X, Y) = Cell.X

                CurrentPlayer = Cell.O

            End If

            If CheckForWin(Cell.X) Then

                Winner = Win.Human

                MsgBox("You win!", MsgBoxStyle.DefaultButton1, "End Game")

                ResetGame()

            ElseIf CheckForDraw() Then

                Winner = Win.Draw

                MsgBox("Draw!", MsgBoxStyle.DefaultButton1, "End Game")

                ResetGame()

            Else
                ' Computer player's turn

                ComputerMove()

                If CheckForWin(Cell.O) Then

                    Winner = Win.Computer

                    MsgBox("Computer wins!", MsgBoxStyle.DefaultButton1, "End Game")

                    ResetGame()

                ElseIf CheckForDraw() Then

                    Winner = Win.Draw

                    MsgBox("Draw!", MsgBoxStyle.DefaultButton1, "End Game")

                    ResetGame()

                Else

                    CurrentPlayer = Cell.X

                End If

            End If

        End If

    End Sub

    Private Sub ComputerMove()

        Dim Rnd As New Random()

        Dim X, Y As Integer

        Do
            Randomize()

            'Get random number from 0 to 2.
            X = Rnd.Next(3)

            Randomize()

            Y = Rnd.Next(3)

        Loop While Board(X, Y) <> Cell.Empty

        Board(X, Y) = Cell.O

    End Sub

    Private Function CheckForWin(player As Cell) As Boolean

        ' Check rows ---
        For Y = 0 To 2

            If Board(0, Y) = player AndAlso Board(1, Y) = player AndAlso Board(2, Y) = player Then

                Select Case Y
                    Case 0 'Top Row

                        MarkWinningTopRow()

                    Case 1 'Mid Row

                        MarkWinningMidRow()

                    Case 2 'Bottom row

                        MarkWinningBottomRow()

                End Select

                Return True

            End If

        Next

        ' Check columns |
        For X = 0 To 2

            If Board(X, 0) = player AndAlso Board(X, 1) = player AndAlso Board(X, 2) = player Then

                Select Case X
                    Case 0

                        MarkWinningLeftColumn()

                    Case 1

                        MarkWinningMidColumn()

                    Case 2

                        MarkWinningRightColumn()

                End Select

                Return True

            End If

        Next

        ' Check diagonals
        If Board(0, 0) = player AndAlso Board(1, 1) = player AndAlso Board(2, 2) = player Then

            MarkWinningTopLeftBottomRight()

            Return True

        End If

        If Board(2, 0) = player AndAlso Board(1, 1) = player AndAlso Board(0, 2) = player Then

            MarkWinningTopRightBottomLeft()

            Return True

        End If

        Return False

    End Function

    Private Function CheckForDraw() As Boolean

        For X = 0 To 2

            For Y = 0 To 2

                If Board(X, Y) = Cell.Empty Then

                    Return False

                End If

            Next

        Next

        Return True

    End Function

    Private Sub ResetGame()

        WinMarkerVisable = False

        For X = 0 To 2
            For Y = 0 To 2
                Board(X, Y) = Cell.Empty
            Next
        Next

        CurrentPlayer = Cell.X

    End Sub

    Private Sub DrawXsAndOs()

        For x = 0 To 2
            For y = 0 To 2

                'Does the cell contain an x?
                If Board(x, y) = Cell.X Then
                    'Yes, the cell contains an x.

                    DrawX(x, y)

                    'Does the cell contain an o?
                ElseIf Board(x, y) = Cell.O Then
                    'Yes, the cell contains an o.

                    DrawO(x, y)

                End If
            Next
        Next

    End Sub

    Private Sub DrawX(x As Integer, y As Integer)

        Dim XPen As New Pen(Color.Blue, XPenWidth)

        Buffer.Graphics.DrawLine(XPen,
                                 x * CellWidth + CellPaddingWidth,
                                 y * CellHeight + CellPaddingHeight,
                                 (x + 1) * CellWidth - CellPaddingWidth,
                                 (y + 1) * CellHeight - CellPaddingHeight)

        Buffer.Graphics.DrawLine(XPen,
                                 x * CellWidth + CellPaddingWidth,
                                 (y + 1) * CellHeight - CellPaddingHeight,
                                 (x + 1) * CellWidth - CellPaddingWidth,
                                 y * CellHeight + CellPaddingHeight)

        XPen.Dispose()

    End Sub

    Private Sub DrawO(x As Integer, y As Integer)

        Dim OPen As New Pen(Color.Red, OPenWidth)

        Buffer.Graphics.DrawEllipse(OPen,
                                    x * CellWidth + CellPaddingWidth,
                                    y * CellHeight + CellPaddingHeight,
                                    CellWidth - 2 * CellPaddingWidth,
                                    CellHeight - 2 * CellPaddingHeight)

        OPen.Dispose()

    End Sub

    Private Sub DrawGridLines()

        Dim LinePen As New Pen(Color.White, LinePenWidth)

        ' Draw vertical lines
        Buffer.Graphics.DrawLine(LinePen, CellWidth, 0, CellWidth, ClientSize.Height)
        Buffer.Graphics.DrawLine(LinePen, ClientSize.Width * 2 \ 3, 0, ClientSize.Width * 2 \ 3, ClientSize.Height)

        ' Draw horizontal lines
        Buffer.Graphics.DrawLine(LinePen, 0, CellHeight, ClientSize.Width, ClientSize.Height \ 3)
        Buffer.Graphics.DrawLine(LinePen, 0, ClientSize.Height * 2 \ 3, ClientSize.Width, ClientSize.Height * 2 \ 3)

        LinePen.Dispose()

    End Sub

    Private Sub DrawWinMarker()

        If WinMarkerVisable = True Then

            Dim WinPen As New Pen(Brushes.Purple, WinPenWidth)

            Buffer.Graphics.DrawLine(WinPen, WinMarkerStart, WinMarkerEnd)

            Buffer.Graphics.DrawString(Winner.ToString, FPSFont, Brushes.White, 0, ClientRectangle.Bottom - 75)

            WinPen.Dispose()

        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        CellWidth = ClientSize.Width \ 3
        CellPaddingWidth = CellWidth \ 8
        CellHeight = ClientSize.Height \ 3
        CellPaddingHeight = CellHeight \ 8

        If CellWidth <= CellHeight Then

            LinePenWidth = CellWidth \ 32
            OPenWidth = CellWidth \ 16
            XPenWidth = CellWidth \ 16
            WinPenWidth = CellWidth \ 12
        Else

            LinePenWidth = CellHeight \ 32
            OPenWidth = CellHeight \ 16
            XPenWidth = CellHeight \ 16
            WinPenWidth = CellHeight \ 12

        End If

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

    Private Sub MarkWinningRightColumn()

        WinMarkerStart.X = ClientRectangle.Width - ClientRectangle.Width \ 6
        WinMarkerStart.Y = ClientRectangle.Top + CellPaddingHeight

        WinMarkerEnd.X = ClientRectangle.Width - ClientRectangle.Width \ 6
        WinMarkerEnd.Y = ClientSize.Height - CellPaddingHeight

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningMidColumn()

        WinMarkerStart.X = ClientRectangle.Width \ 2
        WinMarkerStart.Y = ClientRectangle.Top + CellPaddingHeight

        WinMarkerEnd.X = ClientRectangle.Width \ 2
        WinMarkerEnd.Y = ClientSize.Height - CellPaddingHeight

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningLeftColumn()

        WinMarkerStart.X = ClientRectangle.Width \ 6
        WinMarkerStart.Y = ClientRectangle.Top + CellPaddingHeight

        WinMarkerEnd.X = ClientRectangle.Width \ 6
        WinMarkerEnd.Y = ClientSize.Height - CellPaddingHeight

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningBottomRow()

        WinMarkerStart.X = ClientRectangle.Left + CellPaddingWidth
        WinMarkerStart.Y = ClientSize.Height - ClientSize.Height \ 6

        WinMarkerEnd.X = ClientRectangle.Right - CellPaddingWidth
        WinMarkerEnd.Y = ClientSize.Height - ClientSize.Height \ 6

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningMidRow()

        WinMarkerStart.X = ClientRectangle.Left + CellPaddingWidth
        WinMarkerStart.Y = ClientSize.Height \ 2

        WinMarkerEnd.X = ClientRectangle.Right - CellPaddingWidth
        WinMarkerEnd.Y = ClientSize.Height \ 2

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningTopRow()

        WinMarkerStart.X = ClientRectangle.Left + CellPaddingWidth
        WinMarkerStart.Y = ClientSize.Height \ 6

        WinMarkerEnd.X = ClientRectangle.Right - CellPaddingWidth
        WinMarkerEnd.Y = ClientSize.Height \ 6

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningTopRightBottomLeft()

        WinMarkerStart.X = ClientRectangle.Right - CellPaddingWidth
        WinMarkerStart.Y = ClientRectangle.Top + CellPaddingHeight

        WinMarkerEnd.X = ClientRectangle.Left + CellPaddingWidth
        WinMarkerEnd.Y = ClientRectangle.Bottom - CellPaddingHeight

        WinMarkerVisable = True

    End Sub

    Private Sub MarkWinningTopLeftBottomRight()

        WinMarkerStart.X = ClientRectangle.Left + CellPaddingWidth
        WinMarkerStart.Y = ClientRectangle.Top + CellPaddingHeight

        WinMarkerEnd.X = ClientRectangle.Right - CellPaddingWidth
        WinMarkerEnd.Y = ClientRectangle.Bottom - CellPaddingHeight

        WinMarkerVisable = True

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

End Class



