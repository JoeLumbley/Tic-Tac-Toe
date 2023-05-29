# Tic-Tac-Toe
In this app we remake the classic three in a row game, also known as
Noughts and Crosses or X's and O's. This version is resizable, supports
mouse input and has a computer player. It was written in VB.NET in 2023 and
is compatible with Windows 10 and 11.
I'm making a video to explain the code on my YouTube channel.
https://www.youtube.com/@codewithjoe6074

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









