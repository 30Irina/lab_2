using System;


[Serializable]
public struct DataItem
{
    public System.Numerics.Vector2 point { get; set; }
    public System.Numerics.Vector2 value { get; set; }
    public DataItem(System.Numerics.Vector2 point, System.Numerics.Vector2 value)
    {
        this.point = point;
        this.value = value;
    }

    public string ToLongString(string format)
    {
        string res = "(x, y) = (" + String.Format(format, point.X) + ", "
        + String.Format(format, point.Y) + ")\n";
        res = res + "значение = " + String.Format(format, value) + "\n" +
        " значение модуля поля = " + String.Format(format, System.Numerics.Vector2.Abs(value)) + "\n";
        return res;
    }

    public override string ToString()
    {
        return value.ToString() + "\n";
    }

}