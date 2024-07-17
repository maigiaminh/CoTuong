using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const int BOARD_ROWS = 10;
    private const int BOARD_COLUMNS = 9;
    private GameObject[,] board;

    private void Awake() {
        board = new GameObject[BOARD_ROWS, BOARD_COLUMNS];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < BOARD_ROWS; row++)
        {
            for (int col = 0; col < BOARD_COLUMNS; col++)
            {
                GameObject tile = new GameObject($"Tile:{row}-{col}");
                tile.transform.parent = this.transform;
                tile.transform.position = new Vector3(col, row, 0);
                board[row, col] = tile;
            }
        }
    }
}
