using System;
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
    
    //Logic
    private ChessPiece currentChooseCP;
    private Vector2Int currentChoosePos;
    private Camera currentCamera;
    //Prefabs
    [Header("Chess Pieces Prefabs")]
    [SerializeField] private GameObject[] blackPrefabs;
    [SerializeField] private GameObject[] redPrefabs;

    private void Awake() {
        board = new GameObject[BOARD_ROWS, BOARD_COLUMNS];

        tileSize = CalculateTileSize(Screen.width, Screen.height);

        InitializeBoard(tileSize, BOARD_ROWS, BOARD_COLUMNS);
        PositionBoard();
        SpawnAllPieces();
        PositionAllPieces();
    }

    private void PositionBoard()
    {
        Vector2 boardSize = CalculateBoardSize(board[0, 0]);
        Camera mainCamera = Camera.main;

        Vector3 screenCenterWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                    (Screen.width / 2) - (boardSize.x / 2),
                    (Screen.height / 2) - (boardSize.y / 2), 
                    mainCamera.nearClipPlane
                )
            );

        screenCenterWorldPosition.z = transform.position.z;

        transform.position = screenCenterWorldPosition;

        Debug.Log("Board moved to: " + transform.position);
    }

    private Vector2 CalculateBoardSize(GameObject gameObject)
    {
        BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 worldSize = boxCollider.size;
            worldSize.x *= transform.localScale.x;
            worldSize.y *= transform.localScale.y;
            
            Vector2 pixelSize = WorldToPixelSize(worldSize);

            return new Vector2(pixelSize.x * 9, pixelSize.y * 10);
        }
        else
        {
            Debug.LogError("No BoxCollider2D found on the object.");
            return Vector2.zero;
        }
    }

    Vector2 WorldToPixelSize(Vector2 worldSize)
    {
        Camera mainCamera = Camera.main;

        float ppu = mainCamera.pixelHeight / (2f * mainCamera.orthographicSize); //Pixel per units

        float pixelWidth = worldSize.x * ppu;
        float pixelHeight = worldSize.y * ppu;

        return new Vector2(pixelWidth, pixelHeight);
    }
    private void Update() {
        if(currentCamera == null){
            currentCamera = Camera.current;
            return;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Debug.Log(touchPosition);
            Debug.DrawRay(touchPosition, Vector2.zero, Color.red, 1.0f);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            Debug.Log(hit.collider);
            if (hit.transform != null)
            {
                Debug.Log("TEST");
                GameObject touchedObject = hit.transform.gameObject;
                Vector2Int hitPosition = FindTileIndex(touchedObject);
                
                if(currentChoosePos == -Vector2.one){
                    currentChoosePos = hitPosition;
                }

                if(currentChoosePos != hitPosition){
                    currentChoosePos = hitPosition;
                    Debug.Log(currentChoosePos);
                }
                
                if(currentChooseCP != null && chessPieces[hitPosition.x, hitPosition.y] == null){
                    bool validMove = MoveTo(currentChooseCP, hitPosition.x, hitPosition.y);
                    //currentChooseCP.SetPostion(hitPosition.x, hitPosition.y)
                    currentChooseCP.UnselectPiece();
                    currentChooseCP = null;
                    return;
                }

                if(chessPieces[hitPosition.x, hitPosition.y] != null){
                    currentChooseCP = chessPieces[hitPosition.x, hitPosition.y];
                    currentChooseCP.SelectPiece();
                }
                else{
                    currentChooseCP = null;
                }
            }
            else{
                Debug.Log("RAY CAST FAIL");
                if(currentChoosePos != -Vector2.zero){
                    currentChooseCP = null;
                    currentChoosePos = -Vector2Int.one;
                }
            }
        }    
    }

    private float CalculateTileSize(int width, int height)
    {
        return (float) width / height;
    }

    private void InitializeBoard(float tileSize, float rows, float columns)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = new GameObject($"Tile:{row}-{col}");
                tile.transform.parent = transform;
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

                tile.layer = LayerMask.NameToLayer("Tile");
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
        cp.SetScale(new Vector3(tileSize, tileSize, tileSize), true);

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
    private void PositionAllPieces(){
        for (int row = 0; row < BOARD_ROWS; row++){
            for (int col = 0; col < BOARD_COLUMNS; col++){
                if(chessPieces[row, col] != null){
                    PositionSinglePiece(row, col, true);
                }
            }
        }
    }

    //Position Single Piece
    private void PositionSinglePiece(int row, int col, bool force = false){
        chessPieces[row, col].currentX = col;
        chessPieces[row, col].currentY = row;
        Vector3 pos = new Vector3(col * tileSize, row * tileSize) + new Vector3(tileSize / 2, tileSize / 2) + transform.position;
        chessPieces[row, col].SetPostion(pos, force);
    }

    //Operations
    private Vector2Int FindTileIndex(GameObject hitInfo){
        for(int row = 0; row < BOARD_ROWS; row++){
            for(int col = 0; col < BOARD_COLUMNS; col++){
                if(board[row, col] == hitInfo){
                    return new Vector2Int(row, col);
                }
            }
        }

        return -Vector2Int.one;
    }
    private bool MoveTo(ChessPiece cp, int row, int col){
        Vector2Int previousPos = new Vector2Int(cp.currentX, cp.currentY);

        if(chessPieces[row, col] != null){
            ChessPiece ocp = chessPieces[row, col];
            
            if(cp.team == ocp.team){
                return false;
            }
        }
        
        chessPieces[row, col] = cp;
        chessPieces[previousPos.x, previousPos.y] = null;

        PositionSinglePiece(row, col);
        return true;
    }
}
