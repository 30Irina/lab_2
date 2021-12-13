using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Numerics;

public class V4MainCollection 
{
    private List<V4Data> val = new List<V4Data>();

    public int Count { get { return val.Count; } }

    public V4Data this[int i]
    {
        get { return val[i]; }
    }

    public bool Contains(string ID)
    {
        foreach (V4Data item in val)
            if (item.type == ID)
                return true;
        return false;
    }

    public bool Add(V4Data v4Data)
    {
        if (this.Contains(v4Data.type))
            return false;
        val.Add(v4Data);
        return true;
    }

    public string ToLongString(string format)
    {
        string res = "";
        foreach (V4Data k in this.val)
            res += k.ToLongString(format) + "\n";

        return res;
    }

    public override string ToString()
    {
        string res = "";
        foreach (V4Data k in this.val)
            res += k.ToString() + "\n";

        return res;
    }

    public float MaxVal
    {
        get
        {
            if (this.Count == 0) return float.NaN;
            var query = from v4Data in val.OfType<V4DataArray>()
                        from data_it in v4Data
                        select Vector2.Distance(Vector2.Zero, data_it.value);
            double max_dist;

            try { max_dist = query.Max(); }
            catch (System.InvalidOperationException) { max_dist = float.NaN; }
            return (float)max_dist;
        }
    }

    public IEnumerable<DataItem> IncrVal
    {
        get
        {
            var Items = from i1 in val
                        from i2 in i1
                        select i2;
            var res = Items.OrderBy(x => System.Numerics.Vector2.Distance(System.Numerics.Vector2.Zero, x.point));
            if (res.Any()) { return res.Reverse(); }
            else { return null; }

        }
    }

    public IEnumerable<System.Numerics.Vector2> EnumPoints
    {
        get
        {
            var frst = from i_1 in val.OfType<V4DataArray>()
                       from i_2 in i_1
                       select i_2.point;
            var scnd = from i_1 in val.OfType<V4DataList>()
                       from i_2 in i_1
                       select i_2.point;
         
            var res = frst.Except(scnd).Distinct();
            if (res.Any()) { return res; }
            else { return null; }
        }
    }
}