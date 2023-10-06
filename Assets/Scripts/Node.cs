public class Node
{
    public int x;
    public int y;
    public bool walkable;
    public Node parent;
    public int gCost, hCost;
    public int FCost { get { return gCost + hCost; } }
    
    public Node(int x, int y, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
    }
    public int CompareTo(Node other)
    {
        if (FCost < other.FCost)
        {
            return -1;
        }
        else if (FCost > other.FCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}