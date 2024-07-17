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

    public Vector3 desiredPosition;
    public Vector3 desiredScale;
}
