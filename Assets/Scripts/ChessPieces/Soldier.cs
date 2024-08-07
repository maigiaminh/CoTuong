using System.Collections.Generic;
using UnityEngine;

public class Soldier : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        int direction = (team == 0) ? 1 : -1;
        
        /** MOVEMENT **/

        /*UP*/
        if(currentY + direction > 0 && currentY + direction < boardY){
            //Possible Moves
            if(board[currentX, currentY + direction] == null){
                moves.Add(new Vector2Int(currentX, currentY + direction));
            }

            //Possible Kill Moves
            if(board[currentX, currentY + direction] != null){
                if(board[currentX, currentY + direction].team != team){
                    moves.Add(new Vector2Int(currentX, currentY + direction));
                }
            }
        }

        /*LEFT*/
        if(currentX - 1 >= 0){
            if((team == 0 && currentY >= 5) || (team == 1 && currentY <= 4)){
                //Possible Moves
                if(board[currentX - 1, currentY] == null){
                    moves.Add(new Vector2Int(currentX - 1, currentY ));
                }

                //Possible Kill Moves
                if(board[currentX - 1, currentY] != null){
                    if(board[currentX - 1, currentY].team != team){
                        moves.Add(new Vector2Int(currentX - 1, currentY));
                    }
                }
            }
        }

        /*RIGHT*/
        if(currentX + 1 < boardX){
            if((team == 0 && currentY >= 5) || (team == 1 && currentY <= 4)){
                //Possible Moves
                if(board[currentX + 1, currentY] == null){
                    moves.Add(new Vector2Int(currentX + 1, currentY ));
                }

                //Possible Kill Moves
                if(board[currentX + 1, currentY] != null){
                    if(board[currentX + 1, currentY].team != team){
                        moves.Add(new Vector2Int(currentX + 1, currentY));
                    }
                }
            }
        }
        
        return moves;
    }
}
