using System.Collections.Generic;
using UnityEngine;
public class Cannon : ChessPiece
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
                for(int j = i + 1; j < boardY; j ++){
                    if(board[currentX, j] != null){
                        if(board[currentX, j].team != team){
                            moves.Add(new Vector2Int(currentX, j));
                        }

                        break;
                    }
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
                for(int j = i - 1; j >= 0; j --){
                    if(board[currentX, j] != null){
                        if(board[currentX, j].team != team){
                            moves.Add(new Vector2Int(currentX, j));
                        }

                        break;
                    }
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
                for(int j = i - 1; j >= 0; j --){
                    if(board[j, currentY] != null){
                        if(board[j, currentY].team != team){
                            moves.Add(new Vector2Int(j, currentY));
                        }

                        break;
                    }
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
                for(int j = i + 1; j < boardX; j ++){
                    if(board[j, currentY] != null){
                        if(board[j, currentY].team != team){
                            moves.Add(new Vector2Int(j, currentY));
                        }

                        break;
                    }
                }
                
                
                break;
            }
        }
        return moves;
    }
}
