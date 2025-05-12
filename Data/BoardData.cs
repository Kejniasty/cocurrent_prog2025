namespace Data;

public class BoardData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int YOffset { get; set; }
    public int XOffset { get; set; }


    public BoardData(int width, int height, int y_offset, int x_offset)
    {
        Width = width;
        Height = height;
        YOffset = y_offset;
        XOffset = x_offset;

    }
}