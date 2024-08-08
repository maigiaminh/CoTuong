using System.Collections.Generic;
using UnityEngine;

public class General : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        /** MOVEMENT **/
        
        /*UP*/
        if((currentY + 1 <= 2 && team == 0) || (currentY + 1 < boardY && team == 1)){
            //Posible Moves
            if(board[currentX, currentY + 1] == null){
                moves.Add(new Vector2Int(currentX, currentY + 1));
            }

            //Possible Kill Moves
            if(board[currentX, currentY + 1] != null){
                if(board[currentX, currentY + 1].team != team){
                    moves.Add(new Vector2Int(currentX, currentY + 1));
                }
                
            }

        }

        /*DOWN*/
        if((currentY - 1 >= 0 && team == 0) || (currentY - 1 >= 7 && team == 1)){
            //Posible Moves
            if(board[currentX, currentY - 1] == null){
                moves.Add(new Vector2Int(currentX, currentY - 1));
            }

            //Possible Kill Moves
            if(board[currentX, currentY - 1] != null){
                if(board[currentX, currentY - 1].team != team){
                    moves.Add(new Vector2Int(currentX, currentY - 1));
                }
                
            }

        }

        /*LEFT*/
        if(currentX - 1 >= 3){
            //Posible Moves
            if(board[currentX - 1, currentY] == null){
                moves.Add(new Vector2Int(currentX - 1, currentY));
            }

            //Possible Kill Moves
            if(board[currentX - 1, currentY] != null){
                if(board[currentX - 1, currentY].team != team){
                    moves.Add(new Vector2Int(currentX - 1, currentY));
                }
                
            }

        }

        /*RIGHT*/
        if(currentX + 1 <= 5){
            //Posible Moves
            if(board[currentX + 1, currentY] == null){
                moves.Add(new Vector2Int(currentX + 1, currentY));
            }

            //Possible Kill Moves
            if(board[currentX + 1, currentY] != null){
                if(board[currentX + 1, currentY].team != team){
                    moves.Add(new Vector2Int(currentX + 1, currentY));
                }
            }
        }
        
        //Possible Checkmate
        if(currentY < 3){
            for(int i = currentY + 1; i < boardY; i++){
                if(board[currentX, i] != null){
                    if(board[currentX, i].type == ChessPieceType.General){
                        moves.Add(new Vector2Int(currentX, i));
                    }
                    
                    break;
                }
            }
        }
        
        else{
            for(int i = currentY - 1; i >= 0; i--){
                if(board[currentX, i] != null){
                    if(board[currentX, i].type == ChessPieceType.General){
                        moves.Add(new Vector2Int(currentX, i));
                    }
                    
                    break;
                }
            }
        }
               

        return moves;
    }
}
