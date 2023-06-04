# Tic-Tac-Toe
In this app we remake the classic three in a row game, also known as
Noughts and Crosses or X's and O's. This version is resizable, supports
mouse input and has a computer player. It was written in VB.NET in 2023 and
is compatible with Windows 10 and 11.


![001](https://github.com/JoeLumbley/Tic-Tac-Toe/assets/77564255/2d3e72ac-cee3-4715-857a-6fe81cdc20f6)


## Set Up Board

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

## Making Moves

For the human player we handle mouse clicks on the form to update the state of the Board.


```

Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        UpdateMouse(e)

End Sub

```


We convert the mouse coordinates to cell coordinates.

If the clicked cell is empty we place the human player's mark.

We check if the human player has won or the game is a draw.


```


Private Sub UpdateMouse(e As MouseEventArgs)

        Select Case GameState

            Case GameStateEnum.Playing

                Dim X As Integer = MouseToBoardX(e)

                Dim Y As Integer = MouseToBoardY(e)

                If Board(X, Y) = Cell.Empty Then

                    If CurrentPlayer = Cell.X Then

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

                        My.Computer.Audio.Play(My.Resources.tone700freq,
                                               AudioPlayMode.Background)

                    End If

                End If

            Case GameStateEnum.EndScreen

                ResetGame()

                GameState = GameStateEnum.Playing

        End Select

End Sub

    
```


If the human player didn't win and the game isn't a draw we switch to the computer player's turn.


```


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


```


## Draw Board




We draw the game board by first clearing the background to black. 



```

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

        DrawBoardLines()

        DrawXsAndOs()

        DrawCoordinates()

End Sub
    
    
```
    
    

We draw the board lines using a white pen. 



```


Private Sub DrawBoardLines()

        Dim LinePen As New Pen(Color.White, LinePenWidth)

        'Draw vertical board lines
        Buffer.Graphics.DrawLine(LinePen,
                                 CellWidth,
                                 0,
                                 CellWidth,
                                 ClientSize.Height)

        Buffer.Graphics.DrawLine(LinePen,
                                 ClientSize.Width * 2 \ 3,
                                 0,
                                 ClientSize.Width * 2 \ 3,
                                 ClientSize.Height)

        'Draw horizontal board lines
        Buffer.Graphics.DrawLine(LinePen,
                                 0,
                                 CellHeight,
                                 ClientSize.Width,
                                 ClientSize.Height \ 3)

        Buffer.Graphics.DrawLine(LinePen,
                                 0,
                                 ClientSize.Height * 2 \ 3,
                                 ClientSize.Width,
                                 ClientSize.Height * 2 \ 3)

        LinePen.Dispose()

End Sub


```



We then loop through the game board array and draw the players marks it contains.



```


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
    
    
```


We draw the X's using a blue pen. 



```


Private Sub DrawX(X As Integer, Y As Integer)
        'To draw the letter X, we start by drawing two diagonal lines that
        'cross in the middle.

        Dim XPen As New Pen(Color.Blue, XPenWidth)

        'We begin by drawing a diagonal line
        'from the top left corner to the bottom right corner.
        Buffer.Graphics.DrawLine(XPen,
        X * CellWidth + CellPaddingWidth,
                                 Y * CellHeight + CellPaddingHeight,
                                 (X + 1) * CellWidth - CellPaddingWidth,
                                 (Y + 1) * CellHeight - CellPaddingHeight)

        'Then we draw a second diagonal line this time
        'from the top right corner to the bottom left corner.
        Buffer.Graphics.DrawLine(XPen,
                                 X * CellWidth + CellPaddingWidth,
                                 (Y + 1) * CellHeight - CellPaddingHeight,
                                 (X + 1) * CellWidth - CellPaddingWidth,
                                 Y * CellHeight + CellPaddingHeight)

        'The two lines intersect in the middle to form an X shape.

        XPen.Dispose()

End Sub


```


We draw the O's using a red pen.


```


Private Sub DrawO(X As Integer, Y As Integer)

        Dim OPen As New Pen(Color.Red, OPenWidth)

        Buffer.Graphics.DrawEllipse(OPen,
                                    X * CellWidth + CellPaddingWidth,
                                    Y * CellHeight + CellPaddingHeight,
                                    CellWidth - 2 * CellPaddingWidth,
                                    CellHeight - 2 * CellPaddingHeight)

        OPen.Dispose()

End Sub


```




We draw the buffer to the form in the Paint event handler.




```


Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        DrawGame()

        DrawFPS()

        'Show buffer on form.
        Buffer.Render(e.Graphics)

        'Release memory used by buffer.
        Buffer.Dispose()
        Buffer = Nothing

        'Create new buffer.
        Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)

        'Use these settings when drawing to the backbuffer.
        With Buffer.Graphics

            'Bug Fix: Don't Change.
            'To fix draw string error with anti aliasing: "Parameters not valid."
            'I set the compositing mode to: SourceOver.
            .CompositingMode = CompositingMode.SourceOver

            .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = SmoothingMode.AntiAlias
            .CompositingQuality = CompositingQuality.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextContrast = 6 'a value between 0 and 12
        End With

        UpdateFrameCounter()

End Sub


```










I'm making a video to explain the code on my YouTube channel.
https://www.youtube.com/@codewithjoe6074



