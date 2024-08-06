using System.Collections.Generic;
using UnityEngine;

public class Advisor : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = currentX;
        int y = currentY;

        /** MOVEMENT **/
        
        /*UP*/
        y = currentY + 1;

        if((y <= 2 && team == 0) || (y < boardY && team == 1)){

            //UP - RIGHT
            x = currentX + 1;
            
            if(x <= 5){
                //Possible Moves
                if(board[x, y] == null){
                    moves.Add(new Vector2Int(x, y));
                }

                //Possible Kill Moves
                if(board[x, y] != null){
                    if(board[x, y].team != team){
                        moves.Add(new Vector2Int(x, y));
                    }
                }
            }   

            //UP - LEFT
            x = currentX - 1;

            if(x >= 3){
                //Possible Moves
                if(board[x, y] == null){
                    moves.Add(new Vector2Int(x, y));
                }

                //Possible Kill Moves
                if(board[x, y] != null){
                    if(board[x, y].team != team){
                        moves.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        /*DOWN*/
        y = currentY - 1;

        if((y >= 0 && team == 0) || (y >= 7 && team == 1)){

            //DOWN - RIGHT
            x = currentX + 1;
            
            if(x <= 5){
                //Possible Moves
                if(board[x, y] == null){
                    moves.Add(new Vector2Int(x, y));
                }

                //Possible Kill Moves
                if(board[x, y] != null){
                    if(board[x, y].team != team){
                        moves.Add(new Vector2Int(x, y));
                    }
                }
                
            }   

            //DOWN - LEFT
            x = currentX - 1;

            if(x >= 3){
                //Possible Moves
                if(board[x, y] == null){
                    moves.Add(new Vector2Int(x, y));
                }

                //Possible Kill Moves
                if(board[x, y] != null){
                    if(board[x, y].team != team){
                        moves.Add(new Vector2Int(x, y));
                    }
                }
                
            }
        }
        

        return moves;
    }
}
