using System.Collections.Generic;
using UnityEngine;

public class Soldier : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();
        int direction = (team == 0) ? 1 : -1;
        
        /** MOVEMENT **/
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
        
        return moves;
    }
}
