using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    private BoardManager boardManager;
    private const int BOARD_X = 9;
    private const int BOARD_Y = 10;
    private int maxDepth;

    int[,] chariotPositionTable = {
        { 14, 14, 12, 18, 16, 18, 12, 14, 14 },
        { 16, 20, 18, 24, 26, 24, 18, 20, 16, },
        { 12, 12, 12, 18, 18, 18, 12, 12, 12, },
        { 12, 18, 16, 22, 22, 22, 16, 18, 12, },
        { 12, 14, 12, 18, 18, 18, 12, 14, 12, },
        { 12, 16, 14, 20, 20, 20, 14, 16, 12, },
        { 6, 10, 8, 14, 14, 14, 8, 10, 6, },
        { 4, 8, 6, 14, 12, 14, 6, 8, 4, },
        { 8, 4, 8, 16, 8, 16, 8, 4, 8, },
        { -2, 10, 6, 14, 12, 14, 6, 10, -2, },
    };

    int[,] horsePositionTable = {
        { 4, 8, 16, 12, 4, 12, 16, 8, 4 },
        { 4, 10, 28, 16, 8, 16, 28, 10, 4, },
        { 12, 14, 16, 20, 18, 20, 16, 14, 12 },
        { 8, 24, 18, 24, 20, 24, 18, 24, 8, },
        { 6, 16, 14, 18, 16, 18, 14, 16, 6, },
        { 4, 12, 16, 14, 12, 14, 16, 12, 4, },
        { 2, 6, 8, 6, 10, 6, 8, 6, 2, },
        { 4, 2, 8, 8, 4, 8, 8, 2, 4, },
        { 0, 2, 4, 4, -2, 4, 4, 2, 0, },
        { 0, -4, 0, 0, 0, 0, 0, -4, 0, }
    };

    int[,] cannonPositionTable = {
        { 6, 4, 0, -10, -12, -10, 0, 4, 6  },
        { 2, 2, 0, -4, -14, -4, 0, 2, 2, },
        { 2, 2, 0, -10, -8, -10, 0, 2, 2, },
        { 0, 0, -2, 4, 10, 4, -2, 0, 0, },
        { 0, 0, 0, 2, 8, 2, 0, 0, 0, },
        { -2, 0, 4, 2, 6, 2, 4, 0, -2, },
        { 0, 0, 0, 2, 4, 2, 0, 0, 0, },
        { 4, 0, 8, 6, 10, 6, 8, 0, 4, },
        { 0, 2, 4, 6, 6, 6, 4, 2, 0, },
        { 0, 0, 2, 6, 6, 6, 2, 0, 0, },
    };

    int[,] elephantPositionTable = {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 20, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 20, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

    int[,] advisorPositionTable = {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 20, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 20, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

    int[,] generalPositionTable = {
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
    };

    int[,] soldierPositionTable = {
        { 0,  3,  6,  9,  12,  9,  6,  3,  0 },
        { 18, 36, 56, 80, 120, 80, 56, 36, 18, },
        { 14, 26, 42, 60, 80, 60, 42, 26, 14, },
        { 10, 20, 30, 34, 40, 34, 30, 20, 10, },
        { 6,  12, 18, 18, 20, 18, 18, 12, 6, },
        { 2,  0,  8,  0,  8,  0,  8,  0,  2, },
        { 0,  0, -2,  0,  4,  0, -2,  0,  0, },
        { 0,  0,  0,  0,  0,  0,  0,  0,  0, },
        { 0,  0,  0,  0,  0,  0,  0,  0,  0, },
        { 0,  0,  0,  0,  0,  0,  0,  0,  0, }
    };

    public BotAI(int depth)
    {
        maxDepth = depth;
    }

    private int Evaluate(ChessPiece[,] chessPieces){
        int score = 0;
        for(int x = 0; x < BOARD_X; x ++){
            for(int y = 0; y < BOARD_Y; y ++){
                ChessPiece piece = chessPieces[x, y];
                if(piece != null){
                    int pieceValue = GetPieceValue(piece);
                    int positionalValue = GetPositionalValue(piece, x, y);
                    int controlValue = GetControlValue(chessPieces, piece, x, y);
                    int attackDefenseValue = GetAttackDefenseValue(chessPieces, piece, x, y);

                    int pieceScore = pieceValue + positionalValue + controlValue + attackDefenseValue;
                    bool team = piece.team == 0 ? true: false;
                    score += team == boardManager.isRedTurn ? pieceScore : -pieceScore;
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
        switch (cp.type)
        {
            case ChessPieceType.General:
                return cp.team == 0 ? generalPositionTable[x, y] : generalPositionTable[8 - x, 9 - y];
            case ChessPieceType.Advisor:
                return cp.team == 0 ? advisorPositionTable[x, y] : advisorPositionTable[8 - x, 9 - y];
            case ChessPieceType.Elephant:
                return cp.team == 0 ? elephantPositionTable[x, y] : elephantPositionTable[8 - x, 9 - y];
            case ChessPieceType.Horse:
                return cp.team == 0 ? horsePositionTable[x, y] : horsePositionTable[8 - x, 9 - y];
            case ChessPieceType.Chariot:
                return cp.team == 0 ? chariotPositionTable[x, y] : chariotPositionTable[8 - x, 9 - y];
            case ChessPieceType.Cannon:
                return cp.team == 0 ? cannonPositionTable[x, y] : cannonPositionTable[8 - x, 9 - y];
            case ChessPieceType.Soldier:
                return cp.team == 0 ? soldierPositionTable[x, y] : soldierPositionTable[8 - x, 9 - y];
            default:
                return 0;
        }
    }

    private int GetControlValue(ChessPiece[,] board, ChessPiece cp, int x, int y)
    {
        int controlValue = 0;

        List<Vector2Int> possibleMoves = cp.GetAvailableMoves(ref board, x, y);
        
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

        List<Vector2Int> possibleMoves = piece.GetAvailableMoves(ref board, x, y);
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
}
