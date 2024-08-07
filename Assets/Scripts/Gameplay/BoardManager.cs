using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //Board
    [Header("Board Properties")]
    [SerializeField] private float tileSize = 1.0f;
    private const int BOARD_X = 9;
    private const int BOARD_Y = 10;
    private const int RED_ID = 0;
    private const int BLACK_ID = 1;
    private GameObject[,] board;
    private ChessPiece[,] chessPieces;
    
    //Logic
    private ChessPiece currentChooseCP;
    private Camera currentCamera;
    private List<ChessPiece> deadBlacks = new List<ChessPiece>();
    private List<ChessPiece> deadReds = new List<ChessPiece>();
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private List<GameObject> movePoints = new List<GameObject>();
    private Stack<Move> moveStack = new Stack<Move>();
    private bool isRedTurn = false;

    //Prefabs
    [Header("Chess Pieces Prefabs")]
    [SerializeField] private GameObject[] blackPrefabs;
    [SerializeField] private GameObject[] redPrefabs;

    [Header("Others")]
    [SerializeField] private GameObject dotPrefabs;

    private void Awake() {
        board = new GameObject[BOARD_X, BOARD_Y];
        tileSize = CalculateTileSize(Screen.width, Screen.height);
        isRedTurn = true;

        InitializeBoard(tileSize, BOARD_X, BOARD_Y);
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
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            if (hit.transform != null)
            {
                GameObject touchedObject = hit.transform.gameObject;
                Vector2Int hitPosition = FindTileIndex(touchedObject);
                if(currentChooseCP != null){
                    bool validMove = MoveTo(currentChooseCP, hitPosition.x, hitPosition.y);
                    if(validMove){
                        currentChooseCP.UnselectPiece();
                        currentChooseCP = null;
                        HidePossibleMoves();
                    }
                    return;
                }
                else{
                    if(chessPieces[hitPosition.x, hitPosition.y] != null){
                        if((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isRedTurn) ||
                            (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isRedTurn)){
                                currentChooseCP = chessPieces[hitPosition.x, hitPosition.y];
                                currentChooseCP.SelectPiece();
                                availableMoves = currentChooseCP.GetAvailableMoves(ref chessPieces, BOARD_X, BOARD_Y);
                                ShowPossibleMoves();
                        }
                    }
                }
            }
            else{
                Debug.Log("RAY CAST FAIL");
                if(currentChooseCP != null){
                    currentChooseCP.UnselectPiece();
                    currentChooseCP = null;
                    HidePossibleMoves();
                }
            }
        }    
    }

    private float CalculateTileSize(int width, int height)
    {
        return (float) width / height;
    }

    private void InitializeBoard(float tileSize, float xboard, float yboard)
    {
        for (int x = 0; x < xboard; x++)
        {
            for (int y = 0; y < yboard; y++)
            {
                GameObject tile = new GameObject($"Tile:{x}-{y}");
                tile.transform.parent = transform;
                board[x, y] = tile;

                Mesh mesh = new Mesh();
                tile.AddComponent<MeshFilter>().mesh = mesh;
                tile.AddComponent<MeshRenderer>();

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x * tileSize, y * tileSize);
                vertices[1] = new Vector3(x * tileSize, (y + 1)  * tileSize);
                vertices[2] = new Vector3((x + 1) * tileSize, y * tileSize);
                vertices[3] = new Vector3((x + 1) * tileSize, (y + 1) * tileSize);

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
            cp = Instantiate(redPrefabs[(int) type - 1], transform).GetComponent<ChessPiece>();
        }
        else{
            cp = Instantiate(blackPrefabs[(int) type - 1], transform).GetComponent<ChessPiece>();
        }

        cp.type = type;
        cp.team = team;
        cp.SetScale(new Vector3(tileSize, tileSize, tileSize), true);

        return cp;
    }

    //Spawn All Pieces
    private void SpawnAllPieces(){
        chessPieces = new ChessPiece[BOARD_X, BOARD_Y];

        //Black Team
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Chariot, RED_ID);
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Horse, RED_ID);
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Elephant, RED_ID);
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Advisor, RED_ID);
        chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.General, RED_ID);
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Advisor, RED_ID);
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Elephant, RED_ID);
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Horse, RED_ID);
        chessPieces[8, 0] = SpawnSinglePiece(ChessPieceType.Chariot, RED_ID);

        chessPieces[1, 2] = SpawnSinglePiece(ChessPieceType.Cannon, RED_ID);
        chessPieces[7, 2] = SpawnSinglePiece(ChessPieceType.Cannon, RED_ID);

        chessPieces[0, 3] = SpawnSinglePiece(ChessPieceType.Soldier, RED_ID);
        chessPieces[2, 3] = SpawnSinglePiece(ChessPieceType.Soldier, RED_ID);
        chessPieces[4, 3] = SpawnSinglePiece(ChessPieceType.Soldier, RED_ID);
        chessPieces[6, 3] = SpawnSinglePiece(ChessPieceType.Soldier, RED_ID);
        chessPieces[8, 3] = SpawnSinglePiece(ChessPieceType.Soldier, RED_ID);

        //Red Team
        chessPieces[0, 9] = SpawnSinglePiece(ChessPieceType.Chariot, BLACK_ID);
        chessPieces[1, 9] = SpawnSinglePiece(ChessPieceType.Horse, BLACK_ID);
        chessPieces[2, 9] = SpawnSinglePiece(ChessPieceType.Elephant, BLACK_ID);
        chessPieces[3, 9] = SpawnSinglePiece(ChessPieceType.Advisor, BLACK_ID);
        chessPieces[4, 9] = SpawnSinglePiece(ChessPieceType.General, BLACK_ID);
        chessPieces[5, 9] = SpawnSinglePiece(ChessPieceType.Advisor, BLACK_ID);
        chessPieces[6, 9] = SpawnSinglePiece(ChessPieceType.Elephant, BLACK_ID);
        chessPieces[7, 9] = SpawnSinglePiece(ChessPieceType.Horse, BLACK_ID);
        chessPieces[8, 9] = SpawnSinglePiece(ChessPieceType.Chariot, BLACK_ID);

        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Cannon, BLACK_ID);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Cannon, BLACK_ID);

        chessPieces[0, 6] = SpawnSinglePiece(ChessPieceType.Soldier, BLACK_ID);
        chessPieces[2, 6] = SpawnSinglePiece(ChessPieceType.Soldier, BLACK_ID);
        chessPieces[4, 6] = SpawnSinglePiece(ChessPieceType.Soldier, BLACK_ID);
        chessPieces[6, 6] = SpawnSinglePiece(ChessPieceType.Soldier, BLACK_ID);
        chessPieces[8, 6] = SpawnSinglePiece(ChessPieceType.Soldier, BLACK_ID);
    }

    //Position All Pieces
    private void PositionAllPieces(){
        for (int x = 0; x < BOARD_X; x++){
            for (int y = 0; y < BOARD_Y; y++){
                if(chessPieces[x, y] != null){
                    PositionSinglePiece(x, y, true);
                }
            }
        }
    }

    //Position Single Piece
    private void PositionSinglePiece(int x, int y, bool force = false){
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y;
        Vector3 pos = new Vector3(x * tileSize, y * tileSize) + new Vector3(tileSize / 2, tileSize / 2) + transform.position;
        chessPieces[x, y].SetPostion(pos, force);
    }

    //Show Possible Moves
    private void ShowPossibleMoves(){
        for(int i = 0; i < availableMoves.Count; i++){
            GameObject dot = Instantiate(dotPrefabs, transform);
            dot.transform.position = 
                new Vector3(availableMoves[i].x * tileSize, availableMoves[i].y * tileSize, -0.1f) 
                + 
                new Vector3(tileSize / 2, tileSize / 2) 
                + 
                transform.position;
            dot.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
            movePoints.Add(dot);
        }
    }

    private void HidePossibleMoves(){
        foreach(GameObject point in movePoints){
            Destroy(point);
        }

        movePoints.Clear();
        availableMoves.Clear();
    }

    //Operations
    private Vector2Int FindTileIndex(GameObject hitInfo){
        for(int x = 0; x < BOARD_X; x++){
            for(int y = 0; y < BOARD_Y; y++){
                if(board[x, y] == hitInfo){
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one;
    }
    private bool MoveTo(ChessPiece cp, int x, int y){
        Vector2Int previousPos = new Vector2Int(cp.currentX, cp.currentY);
        bool isInAvailablesMoves = false;
        ChessPiece ocp = null;

        if(chessPieces[x, y] != null){
            ocp = chessPieces[x, y];
        }


        foreach(Vector2Int move in availableMoves){
            if(move.x == x && move.y == y){
                isInAvailablesMoves = true;
                break;
            }
        }

        if(isInAvailablesMoves){
            Move move = new Move(previousPos, new Vector2Int(x, y), isRedTurn);

            if(ocp != null){                
                if(ocp.team == BLACK_ID){
                    deadBlacks.Add(ocp);
                }
                else{
                    deadReds.Add(ocp);
                }
                
                move.eliminatedPiece = ocp;
                chessPieces[x, y] = null;
                ocp.gameObject.SetActive(false);
            
            }

            chessPieces[x, y] = cp;
            chessPieces[previousPos.x, previousPos.y] = null;

            PositionSinglePiece(x, y);

            isRedTurn = !isRedTurn;
            moveStack.Push(move);
            return true;
        }
        
        else{
            HidePossibleMoves();
            currentChooseCP.UnselectPiece();
            currentChooseCP = null;

            if(ocp != null){
                if(cp.team == ocp.team && cp != ocp){
                    currentChooseCP = ocp;
                    currentChooseCP.SelectPiece();
                    availableMoves = currentChooseCP.GetAvailableMoves(ref chessPieces, BOARD_X, BOARD_Y);
                    ShowPossibleMoves();
                    return false;
                }
            }

            return false;
        }
    }

    public void BackToPreviousMove(){
        if(moveStack.Count > 0){
            Move move = moveStack.Pop();
            
            ChessPiece cp = chessPieces[move.currentMove.x, move.currentMove.y];

            chessPieces[move.lastMove.x, move.lastMove.y] = cp;
            chessPieces[move.currentMove.x, move.currentMove.y] = null;

            PositionSinglePiece(move.lastMove.x, move.lastMove.y);

            isRedTurn = move.isRedTurn;
            
            if(move.eliminatedPiece != null){
                ChessPiece ocp = move.eliminatedPiece;

                ocp.gameObject.SetActive(true);
                chessPieces[move.currentMove.x, move.currentMove.y] = ocp;
            }
            
        }
        else{
            Debug.LogWarning("There are no moves in the stack");
        }
    }
}
