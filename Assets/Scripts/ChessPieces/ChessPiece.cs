using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType{
    None = 0,
    General = 1,
    Advisor = 2,
    Elephant = 3,
    Horse = 4,
    Chariot = 5,
    Cannon = 6,
    Soldier = 7
}
public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    public Vector3 desiredPosition = Vector3.zero;
    private Vector3 desiredScale = Vector3.zero;

    private void Start(){
        //transform.rotation = Quaternion.Euler((team == 0) ? Vector3.zero : new Vector3(0, 0, 180));
    }

    private void Update(){
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    public void SelectPiece(){
        GetComponent<SpriteRenderer>().sprite = selectedSprite;
    }

    public void UnselectPiece(){
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    public virtual void SetPostion(Vector3 postion, bool force = false){
        desiredPosition = postion;

        if(force){
            transform.position = desiredPosition;
        }
    }

    public virtual void SetScale(Vector3 scale, bool force = false){
        desiredScale = scale;
        
        if(force){
            transform.localScale = desiredScale;
        }
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int boardX, int boardY){
        List<Vector2Int> moves = new List<Vector2Int>();

        return moves;
    }
}
