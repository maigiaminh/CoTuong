using System.Collections.Generic;
using UnityEngine;

public class Horse : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        int hobbling = 1;
        int x = currentX;
        int y = currentY;
        /** MOVEMENT **/
        
        /*UP*/
        y = currentY + 2;

        if(y < boardY){
            if(board[currentX, currentY + hobbling] == null){
                //UP - RIGHT
                x = currentX + 1;

                if(x < boardX && y < boardY){
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

                if(x >= 0 && y < boardY){
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

        if(y >= 0){
            if(board[currentX, currentY - hobbling] == null){

                //DOWN - RIGHT
                x = currentX + 1;

                if(x < boardX && y >= 0){
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

                if(x >= 0 && y >= 0){
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
        
        /*LEFT*/
        x = currentX - 2;

        if(x >= 0){
            if(board[currentX - hobbling, currentY] == null){

                //LEFT - TOP
                y = currentY + 1;

                if(x >= 0 && y < boardY){
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

                //LEFT - BOTTOM
                y = currentY - 1;

                if(x >= 0 && y >= 0){
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
        
        /*RIGHT*/
        x = currentX + 2;

        if(x < boardX){
            if(board[currentX + hobbling, currentY] == null){

                //RIGHT - TOP
                y = currentY + 1;

                if(x < boardX && y < boardY){
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

                //RIGHT - BOTTOM
                y = currentY - 1;

                if(x < boardX && y >= 0){
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
