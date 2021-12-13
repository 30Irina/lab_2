using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
//
using System.Globalization;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


[Serializable]
public class V4DataArray : V4Data, IEnumerable<DataItem>
{
    public int Ox { get; private set; }
    public int Oy { get; private set; }
    public System.Numerics.Vector2 step { get; private set; }
    public System.Numerics.Vector2[,] matrix { get; private set; }
    public V4DataArray(string dataType, System.DateTime time) : base(dataType, time)
    {
        this.matrix = new System.Numerics.Vector2[0, 0];
    }

    public V4DataArray(string dataType, System.DateTime time, int x, int y, System.Numerics.Vector2 Step, Fv2Vector2 F) : base(dataType, time)
    {
        this.Ox = x;
        this.Oy = y;
        this.step = Step;
        this.matrix = new System.Numerics.Vector2[x, y];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                this.matrix[i, j] = F(new System.Numerics.Vector2(i * step.X, j * step.Y));
            }
    }
    public override int Count { get { return Ox * Oy; } }

    public override float MaxFromOrigin
    {
        get
        {
            if (Count != 0) return System.Numerics.Vector2.Distance(System.Numerics.Vector2.Zero, new System.Numerics.Vector2((Ox - 1) * step.X, (Oy - 1) * step.Y));
            else return 0;
        }
    }

    public override string ToString()
    {
        return "Тип: " + type + "\n" +
                    "Ox: " + Ox + ", Oy: " + Oy + "\n" +
                    "step.x: " + step.X + " step.y: " + step.Y + "\n";
    }

    public override string ToLongString(string format)
    {
        string res = this.ToString();
        for (int i = 0; i < this.Ox; i++)
            for (int j = 0; j < this.Oy; j++)
            {
                res += "(x, y) = (" + String.Format(format, i * step.X) + ", " +
                   String.Format(format, j * step.Y) + ")\n";
                res += "значение = " + String.Format(format, matrix[i, j]) + "\n" +
                    " значение модуля поля = " + String.Format(format, System.Numerics.Vector2.Abs(matrix[i, j])) + "\n";
            }
        return res;
    }
    public V4DataList Transformation()
    {
        V4DataList list = new V4DataList(this.type, this.createdAt);
        var v_1 = new System.Numerics.Vector2();
        var v_2 = new System.Numerics.Vector2();
        for (int i = 0; i < this.Ox; i++)
            for (int j = 0; j < this.Oy; j++)
            {
                v_1.X = i * step.X;
                v_1.Y = j * step.Y;
                v_2.X = matrix[i, j].X;
                v_2.Y = matrix[i, j].Y;
                list.Add(new DataItem(v_1, v_2));
            }
        return list;
    }

    public override IEnumerator<DataItem> GetEnumerator()
    {
        for (int i = 0; i < Ox; i++)
            for (int j = 0; j < Oy; j++)
            {
                var point = new Vector2(i, j);
                yield return new DataItem(point, matrix[i, j]);
            }
    }
    
    public static bool LoadBinary(string filename, ref V4DataArray v4)
    {
        bool ok = true;
        FileStream file_stream = File.Open(filename, FileMode.Open);
        try
        {
            BinaryReader sw = new BinaryReader(file_stream);
            v4.type = sw.ReadString();
            v4.createdAt = DateTime.Parse(sw.ReadString(), culture_info);
            int count = sw.ReadInt32();
            v4.Ox = sw.ReadInt32();
            v4.Oy = sw.ReadInt32();
            var step = new Vector2();
            step.X = (float)sw.ReadDouble();
            step.Y = (float)sw.ReadDouble();
            v4.step = step;
            v4.matrix = new Vector2[v4.Ox, v4.Oy];
            for (int i = 0; i < v4.Ox; i++)
                for (int j = 0; j < v4.Oy; j++)
                {
                    v4.matrix[i, j] = new Vector2((float)sw.ReadDouble(), (float)sw.ReadDouble());
                }
            sw.Dispose();
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in V4DataArray.LoadBinary: {e.Message}");
            return false;
        }
        finally
        {
            file_stream.Close();
        }
        return true;
    }

    public static CultureInfo culture_info = new CultureInfo("en-US");
    public static bool SaveBinary(string filename, V4DataArray v4)
    {
        FileStream file_stream = File.Open(filename, FileMode.OpenOrCreate);
        try
        {
            BinaryWriter sw = new BinaryWriter(file_stream);
            sw.Write(v4.type);
            sw.Write(v4.createdAt.ToString(culture_info.DateTimeFormat));
            sw.Write(v4.Count);
            sw.Write(v4.Ox);
            sw.Write(v4.Oy);
            sw.Write((double)v4.step.X);
            sw.Write((double)v4.step.Y);
            for (int i = 0; i < v4.Ox; i++)
                for (int j = 0; j < v4.Oy; j++)
                {
                    sw.Write((double)v4.matrix[i, j].X);
                    sw.Write((double)v4.matrix[i, j].Y);
                }
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in V4DataArray.SaveBinary: {e.Message}");
            return false;
        }
        finally
        {
            file_stream.Close();
        }
        return true;
    }
}

