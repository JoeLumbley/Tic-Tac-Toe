'Tic Tac Toe
'In this app we remake the classic three in a row game, also known as
'Noughts and Crosses or X's and O's. This version is resizable, supports
'mouse input and has a computer player. It was written in VB.NET in 2023 and
'is compatible with Windows 10 and 11.
'I'm making a video to explain the code on my YouTube channel.
'https://www.youtube.com/@codewithjoe6074
'
'MIT License
'Copyright(c) 2023 Joseph W. Lumbley

'Permission Is hereby granted, free Of charge, to any person obtaining a copy
'of this software And associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
'copies of the Software, And to permit persons to whom the Software Is
'furnished to do so, subject to the following conditions:

'The above copyright notice And this permission notice shall be included In all
'copies Or substantial portions of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
'IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
'LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
'OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
'SOFTWARE.

'Monica is our an AI assistant.
'https://monica.im/

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

    Private Enum GameStateEnum
        StartScreen
        Instructions
        Playing
        EndScreen
    End Enum

    Private GameState As GameStateEnum = GameStateEnum.Playing

    Private ReadOnly AlineCenter As New StringFormat
    Private ReadOnly AlineCenterMiddle As New StringFormat

    Private Enum Winning
        RightColumn
        MidColumn
        LeftColumn
        TopRow
        MidRow
        BottomRow
        TopRightBottomLeft
        TopLeftBottomRight
    End Enum

    Private WinningSet As Winning = Winning.TopRow

    Private ResultFontSize As Integer = 25

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeBoard()

        InitializeStringAlinement()

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

        UpdateGame()

        Refresh() 'Calls OnPaint Event

    End Sub

    Private Sub UpdateGame()

        Select Case GameState

            Case GameStateEnum.StartScreen

                'UpdateStartScreen()

            Case GameStateEnum.Instructions

                'UpdateInstructions()

            Case GameStateEnum.Playing

                UpdatePlaying()

            Case GameStateEnum.EndScreen

                'UpdateEndScreen()

        End Select

    End Sub
    Private Sub UpdatePlaying()

        If CurrentPlayer = Cell.O Then
            'Computer player's turn

            ComputerMove()

            If CheckForWin(Cell.O) Then

                Winner = Win.Computer

                GameState = GameStateEnum.EndScreen

            ElseIf CheckForDraw() Then

                Winner = Win.Draw

                GameState = GameStateEnum.EndScreen

            Else

                'We switch to the human player's turn.
                CurrentPlayer = Cell.X

            End If

        End If

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

        DrawGame()

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
            .CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            .PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        End With

        UpdateFrameCounter()

    End Sub

    Private Sub DrawGame()

        Buffer.Graphics.Clear(Color.Black)

        Select Case GameState

            Case GameStateEnum.StartScreen

                'DrawStartScreen()

            Case GameStateEnum.Instructions

                'DrawInstructions()

            Case GameStateEnum.Playing

                DrawPlaying()

            Case GameStateEnum.EndScreen

                DrawEndScreen()

        End Select

    End Sub

    Private Sub DrawPlaying()

        DrawGridLines()

        DrawXsAndOs()

        DrawCoordinates()

    End Sub
    Private Sub InitializeStringAlinement()

        AlineCenter.Alignment = StringAlignment.Center
        AlineCenterMiddle.Alignment = StringAlignment.Center
        AlineCenterMiddle.LineAlignment = StringAlignment.Center

    End Sub

    Private Sub DrawEndScreen()

        DrawGridLines()

        DrawXsAndOs()

        DrawWinMarker()



        Dim ResultFont As New Font(FontFamily.GenericSansSerif, ResultFontSize)

        Select Case Winner

            Case Win.Computer

                Buffer.Graphics.DrawString("Computer wins!", ResultFont, Brushes.Black, ClientSize.Width \ 2 + 2, ClientSize.Height \ 2 + 2, AlineCenterMiddle)

                Buffer.Graphics.DrawString("Computer wins!", ResultFont, Brushes.Yellow, ClientSize.Width \ 2, ClientSize.Height \ 2, AlineCenterMiddle)

            Case Win.Human

                Buffer.Graphics.DrawString("You win!", ResultFont, Brushes.Black, ClientSize.Width \ 2 + 2, ClientSize.Height \ 2 + 2, AlineCenterMiddle)

                Buffer.Graphics.DrawString("You win!", ResultFont, Brushes.Yellow, ClientSize.Width \ 2, ClientSize.Height \ 2, AlineCenterMiddle)

            Case Win.Draw

                Buffer.Graphics.DrawString("Draw!", ResultFont, Brushes.Black, ClientSize.Width \ 2 + 2, ClientSize.Height \ 2 + 2, AlineCenterMiddle)

                Buffer.Graphics.DrawString("Draw!", ResultFont, Brushes.Yellow, ClientSize.Width \ 2, ClientSize.Height \ 2, AlineCenterMiddle)

        End Select

        ResultFont.Dispose()

    End Sub

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        UpdateMouse(e)

    End Sub

    Private Sub UpdateMouse(e As MouseEventArgs)

        Select Case GameState

            Case GameStateEnum.Playing

                Dim X As Integer = MouseToBoardX(e)

                Dim Y As Integer = MouseToBoardY(e)

                If Board(X, Y) = Cell.Empty Then

                    If CurrentPlayer = Cell.X Then

                        My.Computer.Audio.Play(My.Resources.tone700freq, AudioPlayMode.Background)

                        'Human move.
                        Board(X, Y) = Cell.X

                        If CheckForWin(Cell.X) Then

                            Winner = Win.Human

                            GameState = GameStateEnum.EndScreen

                        ElseIf CheckForDraw() Then

                            Winner = Win.Draw

                            GameState = GameStateEnum.EndScreen

                        Else

                            'We switch to the computer player's turn.
                            CurrentPlayer = Cell.O

                        End If

                    End If

                End If

            Case GameStateEnum.EndScreen

                ResetGame()

                GameState = GameStateEnum.Playing

        End Select

    End Sub

    Private Sub ComputerMove()

        'Grab center cell if empty.
        If Board(1, 1) = Cell.Empty Then

            Board(1, 1) = Cell.O

            'Block right column as needed.
        ElseIf Board(2, 2) = Cell.X AndAlso Board(2, 1) = Cell.X AndAlso Board(2, 0) = Cell.Empty Then

            Board(2, 0) = Cell.O

            'Block right column as needed.
        ElseIf Board(2, 0) = Cell.X AndAlso Board(2, 1) = Cell.X AndAlso Board(2, 2) = Cell.Empty Then

            Board(2, 2) = Cell.O

            'Block right column as needed.
        ElseIf Board(2, 0) = Cell.X AndAlso Board(2, 2) = Cell.X AndAlso Board(2, 1) = Cell.Empty Then

            Board(2, 1) = Cell.O

            'Block bottom row as needed.
        ElseIf Board(1, 2) = Cell.X AndAlso Board(2, 2) = Cell.X AndAlso Board(0, 2) = Cell.Empty Then

            Board(0, 2) = Cell.O

            'Block bottom row as needed.
        ElseIf Board(0, 2) = Cell.X AndAlso Board(1, 2) = Cell.X AndAlso Board(2, 2) = Cell.Empty Then

            Board(2, 2) = Cell.O

            'Block bottom row as needed.
        ElseIf Board(0, 2) = Cell.X AndAlso Board(2, 2) = Cell.X AndAlso Board(1, 2) = Cell.Empty Then

            Board(1, 2) = Cell.O

            'Block Left column as needed.
        ElseIf Board(0, 0) = Cell.X AndAlso Board(0, 1) = Cell.X AndAlso Board(0, 2) = Cell.Empty Then

            Board(0, 2) = Cell.O

            'Block Left column as needed.
        ElseIf Board(0, 0) = Cell.X AndAlso Board(0, 2) = Cell.X AndAlso Board(0, 1) = Cell.Empty Then

            Board(0, 1) = Cell.O

            'Block Left column as needed.
        ElseIf Board(0, 1) = Cell.X AndAlso Board(0, 2) = Cell.X AndAlso Board(0, 0) = Cell.Empty Then

            Board(0, 0) = Cell.O

            'Block Mid row as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(2, 1) = Cell.X AndAlso Board(0, 1) = Cell.Empty Then

            Board(0, 1) = Cell.O

            'Block Mid row as needed.
        ElseIf Board(0, 1) = Cell.X AndAlso Board(2, 1) = Cell.X AndAlso Board(1, 1) = Cell.Empty Then

            Board(1, 1) = Cell.O

            'Block Mid row as needed.
        ElseIf Board(0, 1) = Cell.X AndAlso Board(1, 1) = Cell.X AndAlso Board(2, 1) = Cell.Empty Then

            Board(2, 1) = Cell.O

            'Block Mid column as needed.
        ElseIf Board(1, 0) = Cell.X AndAlso Board(1, 1) = Cell.X AndAlso Board(1, 2) = Cell.Empty Then

            Board(1, 2) = Cell.O

            'Block Mid column as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(1, 2) = Cell.X AndAlso Board(1, 0) = Cell.Empty Then

            Board(1, 0) = Cell.O

            'Block Mid column as needed.
        ElseIf Board(1, 0) = Cell.X AndAlso Board(1, 2) = Cell.X AndAlso Board(1, 1) = Cell.Empty Then

            Board(1, 1) = Cell.O

            'Block diagonal as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(2, 2) = Cell.X AndAlso Board(0, 0) = Cell.Empty Then

            Board(0, 0) = Cell.O

            'Block diagonal as needed.
        ElseIf Board(2, 0) = Cell.X AndAlso Board(1, 1) = Cell.X AndAlso Board(0, 2) = Cell.Empty Then

            Board(0, 2) = Cell.O

            'Block diagonal as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(0, 0) = Cell.X AndAlso Board(2, 2) = Cell.Empty Then

            Board(2, 2) = Cell.O

            'Block diagonal as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(2, 2) = Cell.X AndAlso Board(0, 0) = Cell.Empty Then

            Board(0, 0) = Cell.O

            'Block diagonal as needed.
        ElseIf Board(1, 1) = Cell.X AndAlso Board(0, 2) = Cell.X AndAlso Board(2, 0) = Cell.Empty Then

            Board(2, 0) = Cell.O

            'Block top row as needed.
        ElseIf Board(1, 0) = Cell.X AndAlso Board(2, 0) = Cell.X AndAlso Board(0, 0) = Cell.Empty Then

            Board(0, 0) = Cell.O

            'Block top row as needed.
        ElseIf Board(0, 0) = Cell.X AndAlso Board(1, 0) = Cell.X AndAlso Board(2, 0) = Cell.Empty Then

            Board(2, 0) = Cell.O

            'Block top row as needed.
        ElseIf Board(0, 0) = Cell.X AndAlso Board(2, 0) = Cell.X AndAlso Board(1, 0) = Cell.Empty Then

            Board(1, 0) = Cell.O

        Else
            'Generate random move.

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

        End If

    End Sub

    Private Function CheckForWin(player As Cell) As Boolean

        ' Check rows ---
        For Y = 0 To 2

            If Board(0, Y) = player AndAlso Board(1, Y) = player AndAlso Board(2, Y) = player Then

                Select Case Y
                    Case 0 'Top Row

                        MarkWinningTopRow()

                        WinningSet = Winning.TopRow

                    Case 1 'Mid Row

                        MarkWinningMidRow()

                        WinningSet = Winning.MidRow


                    Case 2 'Bottom row

                        MarkWinningBottomRow()

                        WinningSet = Winning.BottomRow


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

                        WinningSet = Winning.LeftColumn

                    Case 1

                        MarkWinningMidColumn()

                        WinningSet = Winning.MidColumn

                    Case 2

                        MarkWinningRightColumn()

                        WinningSet = Winning.RightColumn

                End Select

                Return True

            End If

        Next

        ' Check diagonals
        If Board(0, 0) = player AndAlso Board(1, 1) = player AndAlso Board(2, 2) = player Then

            MarkWinningTopLeftBottomRight()

            WinningSet = Winning.TopLeftBottomRight

            Return True

        End If

        If Board(2, 0) = player AndAlso Board(1, 1) = player AndAlso Board(0, 2) = player Then

            MarkWinningTopRightBottomLeft()

            WinningSet = Winning.TopRightBottomLeft

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

        For X = 0 To 2

            For Y = 0 To 2

                'Does the cell contain an x?
                If Board(X, Y) = Cell.X Then
                    'Yes, the cell contains an x.

                    DrawX(X, Y)

                    'Does the cell contain an o?
                ElseIf Board(X, Y) = Cell.O Then
                    'Yes, the cell contains an o.

                    DrawO(X, Y)

                End If

            Next

        Next

    End Sub

    Private Sub DrawCoordinates()

        For X = 0 To 2

            For Y = 0 To 2

                DrawCoordinate(X, Y)

            Next

        Next

    End Sub

    Private Sub DrawCoordinate(X As Integer, Y As Integer)


        Dim CoordinatesFont As New Font(FontFamily.GenericSansSerif, ResultFontSize)

        Buffer.Graphics.DrawString("(" & X & "," & Y & ")", CoordinatesFont, Brushes.White,
                                   X * CellWidth + CellWidth \ 2,
                                   Y * CellHeight + CellHeight \ 2,
                                   AlineCenterMiddle)

        CoordinatesFont.Dispose()

    End Sub

    Private Sub DrawX(X As Integer, Y As Integer)

        Dim XPen As New Pen(Color.Blue, XPenWidth)

        Buffer.Graphics.DrawLine(XPen,
                                 X * CellWidth + CellPaddingWidth,
                                 Y * CellHeight + CellPaddingHeight,
                                 (X + 1) * CellWidth - CellPaddingWidth,
                                 (Y + 1) * CellHeight - CellPaddingHeight)

        Buffer.Graphics.DrawLine(XPen,
                                 X * CellWidth + CellPaddingWidth,
                                 (Y + 1) * CellHeight - CellPaddingHeight,
                                 (X + 1) * CellWidth - CellPaddingWidth,
                                 Y * CellHeight + CellPaddingHeight)

        XPen.Dispose()

    End Sub

    Private Sub DrawO(X As Integer, Y As Integer)

        Dim OPen As New Pen(Color.Red, OPenWidth)

        Buffer.Graphics.DrawEllipse(OPen,
                                    X * CellWidth + CellPaddingWidth,
                                    Y * CellHeight + CellPaddingHeight,
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
            ResultFontSize = CellWidth \ 10
            If ResultFontSize < 12 Then
                ResultFontSize = 12
            End If

        Else

            LinePenWidth = CellHeight \ 32
            OPenWidth = CellHeight \ 16
            XPenWidth = CellHeight \ 16
            WinPenWidth = CellHeight \ 12
            ResultFontSize = CellHeight \ 10
            If ResultFontSize < 12 Then
                ResultFontSize = 12
            End If
        End If

        If GameState = GameStateEnum.EndScreen Then

            Select Case WinningSet

                Case Winning.TopRow

                    MarkWinningTopRow()

                Case Winning.MidRow

                    MarkWinningMidRow()

                Case Winning.BottomRow

                    MarkWinningBottomRow()

                Case Winning.LeftColumn

                    MarkWinningLeftColumn()

                Case Winning.MidColumn

                    MarkWinningMidColumn()

                Case Winning.RightColumn

                    MarkWinningRightColumn()

                Case Winning.TopLeftBottomRight

                    MarkWinningTopLeftBottomRight()

                Case Winning.TopRightBottomLeft

                    MarkWinningTopRightBottomLeft()

            End Select

        End If

    End Sub

    Private Function MouseToBoardY(e As MouseEventArgs) As Integer

        'Check for error condition.
        If e.Y < ClientSize.Height Then
            'No Error.

            Return e.Y * 3 \ ClientSize.Height 'Returns the column number.

        Else
            'Error Fix: Don't Change.

            'Fixes: IndexOutOfRangeException when mouse Y is equal or greater than then client height
            'e.X * 3 \ ClientSize.Height Returns 3 which is out of range.

            Return 2 '2 is the upper bound of the Y axis.

        End If

    End Function

    Private Function MouseToBoardX(e As MouseEventArgs) As Integer

        'Check for error condition.
        If e.X < ClientSize.Width Then
            'No Error.

            Return e.X * 3 \ ClientSize.Width 'Returns the row number.

        Else
            'Error Fix: Don't Change.

            'Fixes: IndexOutOfRangeException when mouse X is equal or greater than then client width
            'e.X * 3 \ ClientSize.Width returns 3 which is out of range.

            Return 2 '2 is the upper bound of the X axis.

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



