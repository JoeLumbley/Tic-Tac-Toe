# Tic-Tac-Toe
In this app we remake the classic three in a row game, also known as
Noughts and Crosses or X's and O's. This version is resizable, supports
mouse input and has a computer player. It was written in VB.NET in 2023 and
is compatible with Windows 10 and 11.


![001](https://github.com/JoeLumbley/Tic-Tac-Toe/assets/77564255/2d3e72ac-cee3-4715-857a-6fe81cdc20f6)


# Set Up Board

The Board is a 3x3 grid of cells where players place their mark to try to get three in a row. We represent The Board as a two-dimensional array of cells, where each cell can be empty or occupied by a player's mark.

We define the Board as a two-dimensional array that holds the state of each cell.

```
Private ReadOnly Board(2, 2) As Cell
```

We use the Cell enumeration to represent the possible states of each cell.

```
Enum Cell
        Empty
        X
        O
End Enum
```

The possible states of each cell on the Board being: Empty, X, or O.


![002](https://github.com/JoeLumbley/Tic-Tac-Toe/assets/77564255/4dea998f-d56c-427f-8d2f-abc3718af36d)

We initialize the Board to all Empty cells using a loop:

```
Private Sub InitializeBoard()

        For X = 0 To 2

            For Y = 0 To 2

                Board(X, Y) = Cell.Empty

            Next

        Next

End Sub
```

This will set every cell on the Board to Cell.Empty.

Now we're ready to play.

# Making Moves

For the human player we handle mouse clicks on the form to update the state of the Board.

```

Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        UpdateMouse(e)

End Sub

```

We convert the mouse coordinates to cell coordinates.


```

Private Function MouseToBoardX(e As MouseEventArgs) As Integer

        'Check for error condition.
        If e.X < ClientSize.Width Then
            'No Error.

            Return e.X * 3 \ ClientSize.Width 'Returns the row number.

        Else
            'Error Fix: Don't Change.

            'Fixes: IndexOutOfRangeException
            'Happens when mouse X is equal or greater than then client width
            'e.X * 3 \ ClientSize.Width returns 3 which is out of range.

            Return 2 '2 is the upper bound of the X axis.

        End If

End Function

Private Function MouseToBoardY(e As MouseEventArgs) As Integer

        'Check for error condition.
        If e.Y < ClientSize.Height Then
            'No Error.

            Return e.Y * 3 \ ClientSize.Height 'Returns the column number.

        Else
            'Error Fix: Don't Change.
            Return 2 '2 is the upper bound of the Y axis.

        End If

End Function

```


If the clicked cell is empty we place the human player's mark.


```

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
    
```

We check if the human player has won or the game is a draw.

If the human player didn't win and the game isn't a draw we switch to the computer player's turn.




We draw the buffer to the form in the Paint event handler.

```

Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        DrawGame()

        'Draw frames per second display.
        Buffer.Graphics.DrawString(FPS & " FPS", FPSFont, Brushes.Purple, 0, ClientRectangle.Bottom - 75)

        'Show buffer on form.
        Buffer.Render(e.Graphics)

        'Release memory used by buffer.
        Buffer.Dispose()
        Buffer = Nothing

        'Create new buffer.
        Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)


        'Use these settings when drawing to the backbuffer.
        With Buffer.Graphics
            'Bug Fix
            .CompositingMode = Drawing2D.CompositingMode.SourceOver 'Don't Change.
            'To fix draw string error with anti aliasing: "Parameters not valid."
            'I set the compositing mode to source over.
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            .CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            .PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

            .TextContrast = 6 'a value between 0 and 12
        End With

        UpdateFrameCounter()

End Sub

```










I'm making a video to explain the code on my YouTube channel.
https://www.youtube.com/@codewithjoe6074



