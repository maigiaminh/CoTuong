using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.U2D.IK;

public class BotAI : MonoBehaviour
{
    private BoardManager boardManager;
    private GameManager gameManager;
    private const int BOARD_X = 9;
    private const int BOARD_Y = 10;
    public int maxDepth;

    readonly int[,] chariotPositionTable = {
        {-2, 8, 4, 6, 12, 12, 12, 12, 16, 14},
        {10, 4, 8, 10, 16, 14, 18, 12, 20, 14},
        {6, 8, 6, 8, 14, 12, 16, 12, 18, 12},
        {14, 16, 14, 14, 20, 18, 22, 18, 24, 18},
        {12, 8, 12, 14, 20, 18, 22, 18, 26, 16},
        {14, 16, 14, 14, 20, 18, 22, 18, 24, 18},
        {6, 8, 6, 8, 14, 12, 16, 12, 18, 12},
        {10, 4, 8, 10, 16, 14, 18, 12, 20, 14},
        {-2, 8, 4, 6, 12, 12, 12, 12, 16, 14},
    };

    readonly int[,] horsePositionTable = {
        {0, 0, 4, 2, 4, 6, 8, 12, 4, 4},
        {-4, 2, 2, 6, 12, 16, 24, 14, 10, 8},
        {0, 4, 8, 8, 16, 14, 18, 16, 28, 16},
        {0, 4, 8, 6, 14, 18, 24, 20, 16, 12},
        {0, -2, 4, 10, 12, 16, 20, 18, 8, 4},
        {0, 4, 8, 6, 14, 18, 24, 20, 16, 12},
        {0, 4, 8, 8, 16, 14, 18, 16, 28, 16},
        {-4, 2, 2, 6, 12, 16, 24, 14, 10, 8},
        {0, 0, 4, 2, 4, 6, 8, 12, 4, 4},
    };

    readonly int[,] cannonPositionTable = {
        { 0, 0, 4, 0, -2, 0, 0, 2, 2, 6},
        { 0, 2, 0, 0, 0, 0, 0, 2, 2, 4 },
        { 2, 4, 8, 0, 4, 0, -2, 0, 0, 0 },
        { 6, 6, 6, 2, 2, 2, 4, -10, -4, -10 },
        { 6, 6, 10, 4, 6, 8, 10, -8, -14, -12 },
        { 6, 6, 6, 2, 2, 2, 4, -10, -4, -10 },
        { 2, 4, 8, 0, 4, 0, -2, 0, 0, 0 },
        { 0, 2, 0, 0, 0, 0, 0, 2, 2, 4 },
        { 0, 0, 4, 0, -2, 0, 0, 2, 2, 6 },
    };

    readonly int[,] elephantPositionTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 20, 0, 0, 0, 0, 20, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    readonly int[,] advisorPositionTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 20, 0, 0, 0, 0, 0, 0, 20, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    readonly int[,] generalPositionTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    };

    readonly int[,] soldierPositionTable = {
        {0, 0, 0, 0, 2, 6, 19, 14, 18, 0},
        {0, 0, 0, 0, 0, 12, 02, 26, 36, 3},
        {0, 0, 0, -2, 8, 18, 30, 42, 56, 6},
        {0, 0, 0, 0, 0, 18, 34, 60, 80, 9},
        {0, 0, 0, 4, 8, 20, 40, 80, 120, 12},
        {0, 0, 0, 0, 0, 18, 34, 60, 80, 9},
        {0, 0, 0, -2, 8, 18, 30, 42, 56, 6},
        {0, 0, 0, 0, 0, 12, 20, 26, 36, 3},
        {0, 0, 0, 0, 2, 6, 10, 14, 18, 0},
    };

    private void Awake(){
        boardManager = FindAnyObjectByType<BoardManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        //Debug.Log(cannonPositionTable[0, 0]);
    }
    
    public BotAI(int depth)
    {
        maxDepth = depth;
    }

    public AIMove GetBestMove(ChessPiece[,] board){
        AIMove move = AlphaBetaRoot(board, maxDepth, int.MinValue, int.MaxValue, true);
        if(move != null){
            return move;
        }
        else{
            gameManager.EndGame(3);
            boardManager.isEndGame = true;
            boardManager.isRedTurn = true;
            return null;
        }
    }

    private AIMove AlphaBetaRoot(ChessPiece[,] board, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        AIMove bestMove = null;
        int bestValue = int.MinValue;
        
        foreach (var move in GetAIMoves(board))
        {   
            ChessPiece[,] simulation = SimulateMove(board, move);
            int moveValue = AlphaBeta(simulation, depth - 1, alpha, beta, false);

            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = move;
            }
            alpha = Mathf.Max(alpha, bestValue);
            if (beta <= alpha)
                break;
        }

        return bestMove;
    }
    private int AlphaBeta(ChessPiece [,] board, int depth, int alpha, int beta, bool isMaximizingPlayer){
        if(depth == 0){
            return Evaluate(board);
        }

        if (isMaximizingPlayer)
        {
            int maxEval = int.MinValue;
            foreach(var move in GetAIMoves(board)){
                ChessPiece[,] simulation = SimulateMove(board, move);
                int eval = AlphaBeta(simulation, depth - 1, alpha, beta, false);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if(beta <= alpha){
                    break;
                }
            }

            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            
            foreach(var move in GetAIMoves(board)){
                ChessPiece[,] simulation = SimulateMove(board, move);
                int eval = AlphaBeta(simulation, depth - 1, alpha, beta, true);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if(beta <= alpha){
                    break;
                }
            }
        

            return minEval;
        }
    }

    private int Evaluate(ChessPiece[,] chessPieces){
        int score = 0;
        for(int x = 0; x < BOARD_X; x ++){
            for(int y = 0; y < BOARD_Y; y ++){
                ChessPiece piece = chessPieces[x, y];
                if(piece != null){
                    int pieceValue = GetPieceValue(piece);
                    Debug.Log(pieceValue);
                    int positionalValue = GetPositionalValue(piece, x, y);
                    int controlValue = GetControlValue(chessPieces, piece, x, y);
                    int attackDefenseValue = GetAttackDefenseValue(chessPieces, piece, x, y);
                    //int positionalValue = 0;
                    int pieceScore = pieceValue + positionalValue + controlValue + attackDefenseValue;
                    
                    bool team = piece.team == 0 ? true: false;
                    Debug.Log(pieceScore);
                    if(team == boardManager.isRedTurn){
                        score += pieceScore;
                    }
                    else{
                        score += -pieceScore;
                    }
                    //score += team == boardManager.isRedTurn ? pieceScore : -pieceScore;

                }
            }
        }

        return score;
    }
    
    private int GetPieceValue(ChessPiece cp)
    {
        switch (cp.type)
        {
            case ChessPieceType.General:
                return 1000;
            case ChessPieceType.Advisor:
                return 20;
            case ChessPieceType.Elephant:
                return 20;
            case ChessPieceType.Horse:
                return 45;
            case ChessPieceType.Chariot:
                return 90;
            case ChessPieceType.Cannon:
                return 50;
            case ChessPieceType.Soldier:
                return 10;
            default:
                return 0;
        }
    }

    private int GetPositionalValue(ChessPiece cp, int x, int y){
        if(cp == null){
            return 0;
        }
        if(cp.type == ChessPieceType.General){
            return cp.team == 0 ? generalPositionTable[x, y] : generalPositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Advisor){
            return cp.team == 0 ? advisorPositionTable[x, y] : advisorPositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Elephant){
            return cp.team == 0 ? elephantPositionTable[x, y] : elephantPositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Horse){
            return cp.team == 0 ? horsePositionTable[x, y] : horsePositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Chariot){
            return cp.team == 0 ? chariotPositionTable[x, y] : chariotPositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Cannon){
            return cp.team == 0 ? cannonPositionTable[x, y] : cannonPositionTable[8 - x, 9 - y];
        }
        else if(cp.type == ChessPieceType.Soldier){
            return cp.team == 0 ? soldierPositionTable[x, y] : soldierPositionTable[8 - x, 9 - y];
        }
        else{
            return 0;
        }
        
    }

    private int GetControlValue(ChessPiece[,] board, ChessPiece cp, int x, int y)
    {
        int controlValue = 0;

        List<Vector2Int> possibleMoves = cp.GetAvailableMoves(ref board, BOARD_X, BOARD_Y);
        
        foreach (var move in possibleMoves){
            if (IsCentralPosition(move.x, move.y))
            {
                controlValue += 5;
            }
        }
            
        return controlValue;
    }

    private bool IsCentralPosition(int x, int y)
    {
        return x >= 3 && x <= 5 && y >= 3 && y <= 6;
    }

    private int GetAttackDefenseValue(ChessPiece [,] board, ChessPiece piece, int x, int y)
    {
        int attackDefenseValue = 0;

        List<Vector2Int> possibleMoves = piece.GetAvailableMoves(ref board, BOARD_X, BOARD_Y);
        foreach (var move in possibleMoves)
        {
            ChessPiece targetPiece = board[move.x, move.y];
            if (targetPiece != null)
            {
                if (targetPiece.team != piece.team)
                {
                    attackDefenseValue += GetPieceValue(targetPiece) / 10;
                }
                else
                {
                    attackDefenseValue += GetPieceValue(targetPiece) / 20;
                }
            }
        }

        return attackDefenseValue;
    }

    private List<AIMove> GetAIMoves(ChessPiece[,] board){
        List<AIMove> moves = new List<AIMove>();

        for(int x = 0; x < BOARD_X; x++){
            for(int y = 0; y < BOARD_Y; y++){
                if(board[x, y] != null){
                    if (board[x, y].team == 1){
                        List<Vector2Int> moveEnd = board[x, y].GetAvailableMoves(ref board, BOARD_X, BOARD_Y);
                        BotPreventCheck(board, board[x, y], moveEnd);
                        foreach(Vector2Int move in moveEnd){
                            moves.Add(new AIMove(x, y, move.x, move.y));
                        }
                    }
                }
            }
        }

        return moves;
    }

    private void BotPreventCheck(ChessPiece [,] board, ChessPiece cp, List<Vector2Int> availableMoves){
        ChessPiece targetGeneral = null;
        for(int x = 0; x < BOARD_X; x++){
            for(int y = 0; y < BOARD_Y; y++){
                if(board[x, y] != null){
                    if(board[x, y].type == ChessPieceType.General){
                        if(board[x, y].team == 1){
                            targetGeneral = board[x, y];
                        }
                    }
                }
            }
        }

        SimulateMoveForSinglePiece(cp, board, ref availableMoves, targetGeneral);
    }

    private void SimulateMoveForSinglePiece(ChessPiece cp, ChessPiece[,] board,  ref List<Vector2Int> moves, ChessPiece targetGeneral){
        int actualX = cp.currentX;
        int actualY = cp.currentY;

        List<Vector2Int> movesToRemove = new List<Vector2Int>();
        for(int i = 0; i < moves.Count; i++){
            int simX = moves[i].x;
            int simY = moves[i].y;

            Vector2Int generalPositionInThisSim = new Vector2Int(targetGeneral.currentX, targetGeneral.currentY);
            if(cp.type == ChessPieceType.General){
                generalPositionInThisSim = new Vector2Int(simX, simY);
            }

            ChessPiece[,] simulation = new ChessPiece[BOARD_X, BOARD_Y];
            List<ChessPiece> simAttackingPieces = new List<ChessPiece>();
            for(int x = 0; x < BOARD_X; x++){
                for(int y = 0; y < BOARD_Y; y++){
                    if(board[x, y] != null){
                        simulation[x, y] = board[x, y];
                        if(simulation[x, y].team != cp.team){
                            simAttackingPieces.Add(simulation[x, y]);
                        }
                    }
                }
            }

            simulation[actualX, actualY] = null;
            cp.currentX = simX;
            cp.currentY = simY;
            simulation[simX, simY] = cp;

            var deadPiece = simAttackingPieces.Find(c => c.currentX == simX && c.currentY == simY);
            if(deadPiece != null){
                simAttackingPieces.Remove(deadPiece);
            }

            List<Vector2Int> simMoves = new List<Vector2Int>();
            for(int a = 0; a < simAttackingPieces.Count; a++){
                var piecesMoves = simAttackingPieces[a].GetAvailableMoves(ref simulation, BOARD_X, BOARD_Y);
                for(int b = 0; b < piecesMoves.Count; b++){
                    simMoves.Add(piecesMoves[b]);
                }
            }

            if(ContainsValidMove(ref simMoves, generalPositionInThisSim)){
                movesToRemove.Add(moves[i]);
            }

            cp.currentX = actualX;
            cp.currentY = actualY;
        }

        for(int i = 0; i < movesToRemove.Count; i++){
            moves.Remove(movesToRemove[i]);
        }
    }

    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2Int pos){
        for(int i = 0; i < moves.Count; i++){
            if(moves[i].x == pos.x && moves[i].y == pos.y){
                return true;
            }
        }

        return false;
    }

    private ChessPiece[,] SimulateMove(ChessPiece[,] board, AIMove moves){
        ChessPiece[,] simulation = new ChessPiece[BOARD_X, BOARD_Y];
        for(int x = 0; x < BOARD_X; x++){
            for(int y = 0; y < BOARD_Y; y++){
                if(board[x, y] != null){
                    simulation[x, y] = board[x, y];
                }
            }
        }

        ChessPiece cp = simulation[moves.StartX, moves.StartY];
        simulation[moves.EndX, moves.EndY] = cp;
        simulation[moves.StartX, moves.StartY] = null;
        return simulation;
    }
}
