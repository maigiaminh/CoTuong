using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //Board
    [Header("Board Properties")]
    [SerializeField] private float tileSize = 1.0f;
    private const int BOARD_ROWS = 10;
    private const int BOARD_COLUMNS = 9;
    private GameObject[,] board;
    private ChessPiece[,] chessPieces;

    //Prefabs
    [Header("Chess Pieces Prefabs")]
    [SerializeField] private GameObject[] blackPrefabs;
    [SerializeField] private GameObject[] redPrefabs;

    private void Awake() {
        board = new GameObject[BOARD_ROWS, BOARD_COLUMNS];
        InitializeBoard(tileSize, BOARD_ROWS, BOARD_COLUMNS);
        SpawnAllPieces();
        PotionAllPieces();
    }

    private void InitializeBoard(float tileSize, float rows, float columns)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = new GameObject($"Tile:{col}-{row}");
                tile.transform.parent = this.transform;
                board[row, col] = tile;

                Mesh mesh = new Mesh();
                tile.AddComponent<MeshFilter>().mesh = mesh;
                tile.AddComponent<MeshRenderer>();

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(col * tileSize, row * tileSize);
                vertices[1] = new Vector3(col * tileSize, (row + 1)  * tileSize);
                vertices[2] = new Vector3((col + 1) * tileSize, row * tileSize);
                vertices[3] = new Vector3((col + 1) * tileSize, (row + 1) * tileSize);

                int[] tris = new int[] { 0, 1, 3, 2, 0, 3};
                mesh.vertices = vertices;
                mesh.triangles = tris;

                tile.AddComponent<BoxCollider2D>(); 
            }
        }
    }

    //Spawn Single Piece
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team){
        ChessPiece cp;

        if(team == 0){
            cp = Instantiate(blackPrefabs[(int) type - 1], transform).GetComponent<ChessPiece>();
        }
        else{
            cp = Instantiate(redPrefabs[(int) type - 1], transform).GetComponent<ChessPiece>();
        }
        

        cp.type = type;
        cp.team = team;
        cp.transform.localScale *= tileSize;

        return cp;
    }

    //Spawn All Pieces
    private void SpawnAllPieces(){
        chessPieces = new ChessPiece[BOARD_ROWS, BOARD_COLUMNS];
        int blackTeam = 0;
        int redTeam = 1;

        //Black Team
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Chariot, blackTeam);
        chessPieces[0, 1] = SpawnSinglePiece(ChessPieceType.Horse, blackTeam);
        chessPieces[0, 2] = SpawnSinglePiece(ChessPieceType.Elephant, blackTeam);
        chessPieces[0, 3] = SpawnSinglePiece(ChessPieceType.Advisor, blackTeam);
        chessPieces[0, 4] = SpawnSinglePiece(ChessPieceType.General, blackTeam);
        chessPieces[0, 5] = SpawnSinglePiece(ChessPieceType.Advisor, blackTeam);
        chessPieces[0, 6] = SpawnSinglePiece(ChessPieceType.Elephant, blackTeam);
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Horse, blackTeam);
        chessPieces[0, 8] = SpawnSinglePiece(ChessPieceType.Chariot, blackTeam);

        chessPieces[2, 1] = SpawnSinglePiece(ChessPieceType.Cannon, blackTeam);
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Cannon, blackTeam);

        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Soldier, blackTeam);
        chessPieces[3, 2] = SpawnSinglePiece(ChessPieceType.Soldier, blackTeam);
        chessPieces[3, 4] = SpawnSinglePiece(ChessPieceType.Soldier, blackTeam);
        chessPieces[3, 6] = SpawnSinglePiece(ChessPieceType.Soldier, blackTeam);
        chessPieces[3, 8] = SpawnSinglePiece(ChessPieceType.Soldier, blackTeam);

        //Red Team
        chessPieces[9, 0] = SpawnSinglePiece(ChessPieceType.Chariot, redTeam);
        chessPieces[9, 1] = SpawnSinglePiece(ChessPieceType.Horse, redTeam);
        chessPieces[9, 2] = SpawnSinglePiece(ChessPieceType.Elephant, redTeam);
        chessPieces[9, 3] = SpawnSinglePiece(ChessPieceType.Advisor, redTeam);
        chessPieces[9, 4] = SpawnSinglePiece(ChessPieceType.General, redTeam);
        chessPieces[9, 5] = SpawnSinglePiece(ChessPieceType.Advisor, redTeam);
        chessPieces[9, 6] = SpawnSinglePiece(ChessPieceType.Elephant, redTeam);
        chessPieces[9, 7] = SpawnSinglePiece(ChessPieceType.Horse, redTeam);
        chessPieces[9, 8] = SpawnSinglePiece(ChessPieceType.Chariot, redTeam);

        chessPieces[7, 1] = SpawnSinglePiece(ChessPieceType.Cannon, redTeam);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Cannon, redTeam);

        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Soldier, redTeam);
        chessPieces[6, 2] = SpawnSinglePiece(ChessPieceType.Soldier, redTeam);
        chessPieces[6, 4] = SpawnSinglePiece(ChessPieceType.Soldier, redTeam);
        chessPieces[6, 6] = SpawnSinglePiece(ChessPieceType.Soldier, redTeam);
        chessPieces[6, 8] = SpawnSinglePiece(ChessPieceType.Soldier, redTeam);
    }

    //Position All Pieces
    private void PotionAllPieces(){
        for (int row = 0; row < BOARD_ROWS; row++){
            for (int col = 0; col < BOARD_COLUMNS; col++){
                if(chessPieces[row, col] != null){
                    PositionSinglePiece(row, col, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int row, int col, bool force = false){
        chessPieces[row, col].currentX = col;
        chessPieces[row, col].currentY = row;
        chessPieces[row, col].transform.position = new Vector3(col * tileSize, row * tileSize) + new Vector3(tileSize / 2, tileSize / 2);
    }
}
