# Tic-Tac-Toe
In this app we remake the classic three in a row game, also known as
Noughts and Crosses or X's and O's. This version is resizable, supports
mouse input and has a computer player. It was written in VB.NET in 2023 and
is compatible with Windows 10 and 11.
I'm making a video to explain the code on my YouTube channel.
https://www.youtube.com/@codewithjoe6074

![001](https://github.com/JoeLumbley/Tic-Tac-Toe/assets/77564255/2d3e72ac-cee3-4715-857a-6fe81cdc20f6)

We define the board as a two-dimensional array that holds the state of each cell.
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

![002](https://github.com/JoeLumbley/Tic-Tac-Toe/assets/77564255/4dea998f-d56c-427f-8d2f-abc3718af36d)





