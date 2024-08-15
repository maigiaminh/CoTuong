public class AIMove
{
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int EndX { get; set; }
    public int EndY { get; set; }

    public AIMove(int startX, int startY, int endX, int endY)
    {
        StartX = startX;
        StartY = startY;
        EndX = endX;
        EndY = endY;
    }

    public override string ToString(){
        return "Start: " + StartX + "-" + StartY + "\nEnd: " + EndX + "-" + EndY;
    }
}
