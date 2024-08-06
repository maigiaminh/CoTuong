using System.Collections.Generic;
using UnityEngine;
public class Elephant : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = currentX;
        int y = currentY;
        /** MOVEMENT **/
        
        /*UP*/
        y = currentY + 2;

        if((y <= boardY / 2 && team == 0) || (y <= boardY && team == 1)){

            //UP - RIGHT
            x = currentX + 2;
            
            if(x < boardX){
                if(board[currentX + 1, currentY + 1] == null){
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

            //UP - LEFT
            x = currentX - 2;

            if(x >= 0){
                if(board[currentX - 1, currentY + 1] == null){
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
        }

        /*DOWN*/
        y = currentY - 2;

        if((y >= 0 && team == 0) || (y >= boardY / 2 && team == 1)){

            //DOWN - RIGHT
            x = currentX + 2;
            
            if(x < boardX){
                if(board[currentX + 1, currentY - 1] == null){
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

            //DOWN - LEFT
            x = currentX - 2;

            if(x >= 0){
                if(board[currentX - 1, currentY - 1] == null){
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
        }
        

        return moves;
    }
}
