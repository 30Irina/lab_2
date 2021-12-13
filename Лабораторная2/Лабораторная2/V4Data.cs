using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]

public abstract class V4Data : IEnumerable<DataItem>
{
    public string type { get; protected set; }
    public System.DateTime createdAt { get; protected set; }
    public abstract int Count { get; }
    public abstract float MaxFromOrigin { get; }
    public abstract string ToLongString(string format);

    public abstract override string ToString();

    public V4Data(string type, System.DateTime createdAt)
    {
        this.type = type;
        this.createdAt = createdAt;
    }

    public abstract IEnumerator<DataItem> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
