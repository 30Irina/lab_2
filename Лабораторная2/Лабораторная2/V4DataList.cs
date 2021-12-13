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
public class V4DataList : V4Data
{
    public List<DataItem> list { get; }

    public V4DataList(string dataType, System.DateTime time) : base(dataType, time)
    {
        list = new List<DataItem>();
    }

    public bool Add(DataItem newItem)
    {
        foreach (DataItem item in list)
        {
            if (item.point.X == newItem.point.X && item.point.Y == newItem.point.Y)
                return false;
        }
        list.Add(newItem);
        return true;
    }

    public int AddDefaults(int nItems, Fv2Vector2 F)
    {
        int n = 0;
        var randdom = new System.Random();
        var v1 = new System.Numerics.Vector2();
        for (int i = 0; i < nItems; i++)
        {
            v1.X = ((float)(randdom.NextDouble())) * nItems;
            v1.Y = ((float)(randdom.NextDouble())) * nItems;
            DataItem DtItm = new DataItem(v1, F(v1));
            n += Convert.ToInt32(this.Add(DtItm));
        }
        return n;
    }

    public override int Count
    {
        get
        {
            return list.Count;
        }
    }

    public override float MaxFromOrigin
    {
        get
        {
            float rasst = 0;
            System.Numerics.Vector2 z;
            System.Numerics.Vector2 v;
            float maxrasst = 0;

            if (Count != 0)
            {
                foreach (DataItem i in this.list)
                {
                    z = System.Numerics.Vector2.Zero;
                    v = i.point;
                    rasst = System.Numerics.Vector2.Distance(z, v);
                    if (rasst > maxrasst)
                    {
                        maxrasst = rasst;
                    }
                }
                return maxrasst;
            }
            return 0;

        }
    }

    public override string ToString()
    {
        string res = "Тип: " + type + "\n" +
                    "Count = " + list.Count + "\n";
        foreach (DataItem item in list)
            res += item.ToString();
        return res;
    }

    public override string ToLongString(string format)
    {
        string res = "Тип: " + type + "\n" +
            "Count = " + list.Count + "\n";

        foreach (DataItem item in list)
        {
            res += item.ToLongString(format);
            res += "x = " + item.point.X + "\n";
            res += "y = " + item.point.Y + "\n";
            //res += "значение = " + item.value + "\n";
        }

        return res;
    }

    private static List<(double x, double y)> getPoints(int n)
    {
        var rand = new System.Random();
        var points = new List<(double X, double Y)>();

        while (points.Count < n)
            points.Add((rand.NextDouble() * 100, rand.NextDouble() * 100));

        return points;
    }

    public override IEnumerator<DataItem> GetEnumerator()
    {
        return this.list.GetEnumerator();
    }


    public static CultureInfo culture_info = new CultureInfo("en-US");
    public static bool SaveAsText(string filename,  V4DataList v4)
    {
        bool ok = true;
        FileStream file_stream = File.Open(filename, FileMode.OpenOrCreate);
        try
        {
            StreamWriter sw = new StreamWriter(file_stream);
            sw.WriteLine(v4.type);
            sw.WriteLine(v4.createdAt.ToString(culture_info.DateTimeFormat));
            sw.WriteLine(v4.Count);
            foreach (DataItem data in v4.list)
            {
                sw.WriteLine(data.value.X.ToString());
                sw.WriteLine(data.value.Y.ToString());
                sw.WriteLine(data.point.X.ToString());
                sw.WriteLine(data.point.Y.ToString());
            }
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in SaveAsText: {e.Message}");
            ok = false;
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
        return ok;
    }

    public static bool LoadAsText(string filename, ref V4DataList v4)
    {
        bool ok = true;
        FileStream file_stream = File.Open(filename, FileMode.Open);
        try
        {
            StreamReader sw = new StreamReader(file_stream);
            v4.type = sw.ReadLine();
            v4.createdAt = DateTime.Parse(sw.ReadLine(), culture_info);
            int count = int.Parse(sw.ReadLine());
            string[] data;
            Vector2 v;
            double x, y;
            for (int i = 0; i < count; i++)
            {
                var point = new Vector2();
                var value = new Vector2();

                value.X = (float)double.Parse(sw.ReadLine());
                value.Y = (float)double.Parse(sw.ReadLine());
                point.X = (float)double.Parse(sw.ReadLine());
                point.Y = (float)double.Parse(sw.ReadLine());
                v4.Add(new DataItem(point, value));
            }
            sw.Dispose();
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in LoadAsText: {e.Message}");
            ok = false;
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
        return ok;
    }
}

