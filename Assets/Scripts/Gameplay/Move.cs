using UnityEngine;

public class Move
{
    public Vector2Int lastMove {  get; set; }
    public Vector2Int currentMove { get;  set; }
    public bool isRedTurn { get;  set; }
    public ChessPiece eliminatedPiece { get;  set; }
    
    public Move(Vector2Int lastMove, Vector2Int currentMove, bool isRedTurn){
        this.lastMove = lastMove;
        this.currentMove = currentMove;
        this.isRedTurn = isRedTurn;
    }   

    public override string ToString(){
        return "Move: " + "\nLast Move: " + lastMove + "\nCurrent Move " + currentMove + "\nIs Red Turn " + isRedTurn + "\nEliminated Pieced " + eliminatedPiece;
    }  

}
