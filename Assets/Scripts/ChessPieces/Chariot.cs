using System.Collections.Generic;
using UnityEngine;

public class Chariot : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        
        /** MOVEMENT **/

        //UP
        for(int i = currentY + 1; i < boardY; i++){
            //Possible Moves
            if(board[currentX, i] == null){
                moves.Add(new Vector2Int(currentX, i));
            }

            //Possible Kill Moves
            if(board[currentX, i] != null){
                if(board[currentX, i].team != team){
                    moves.Add(new Vector2Int(currentX, i));
                }
                
                break;
            }
        }

        //DOWN
        for(int i = currentY - 1; i >= 0; i--){
            //Possible Moves
            if(board[currentX, i] == null){
                moves.Add(new Vector2Int(currentX, i));
            }

            //Possible Kill Moves
            if(board[currentX, i] != null){
                if(board[currentX, i].team != team){
                    moves.Add(new Vector2Int(currentX, i));
                }
                
                break;
            }
        }

        //LEFT
        for(int i = currentX - 1; i >= 0; i--){
            //Possible Moves
            if(board[i, currentY] == null){
                moves.Add(new Vector2Int(i, currentY));
            }

            //Possible Kill Moves
            if(board[i, currentY] != null){
                if(board[i, currentY].team != team){
                    moves.Add(new Vector2Int(i, currentY));
                }
                
                break;
            }
        }

        //RIGHT
        for(int i = currentX + 1; i < boardX; i++){
            //Possible Moves
            if(board[i, currentY] == null){
                moves.Add(new Vector2Int(i, currentY));
            }

            //Possible Kill Moves
            if(board[i, currentY] != null){
                if(board[i, currentY].team != team){
                    moves.Add(new Vector2Int(i, currentY));
                }
                
                break;
            }
        }
        return moves;
    }
}
