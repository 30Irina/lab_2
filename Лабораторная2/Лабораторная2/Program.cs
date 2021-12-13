using System;

namespace Task1
{
    class Program
    {
		static void TestWriteRead()
		{
			Console.WriteLine("----------1----------");
            V4DataArray testArray1 = new V4DataArray("testArray1", System.DateTime.Now, 2, 4, new System.Numerics.Vector2(0.3f, 0.6f), Fv2.TestVectorF);
            V4DataArray testArray2 = new V4DataArray("testArray2", System.DateTime.Now);
            V4DataArray.SaveBinary("file_1.txt", testArray1);
            V4DataArray.LoadBinary("file_1.txt", ref testArray2);
            Console.WriteLine("Saved V4DataArray:\n" + testArray1.ToLongString("{0:f}"));
            Console.WriteLine("Loaded V4DataArray:\n" + testArray2.ToLongString("{0:f}"));

            V4DataList testList1 = new V4DataList("testList1", DateTime.Now);
			testList1.AddDefaults(5, Fv2.TestVectorF);
			V4DataList testList2 = new V4DataList("testList2", DateTime.Now);
			V4DataList.SaveAsText("file_2.txt", testList1);
            V4DataList.LoadAsText("file_2.txt", ref testList2);
            Console.WriteLine("Saved V4DataList:\n" + testList1.ToLongString("{0:f}"));
            Console.WriteLine("Loaded V4DataList:\n" + testList2.ToLongString("{0:f}"));
		}

		static void TestLinq()
		{
			Console.WriteLine("----------2----------");
			V4MainCollection collection1 = new V4MainCollection();

			V4DataArray testArray1 = new V4DataArray("testArray1", DateTime.Now, 2, 1, new System.Numerics.Vector2(1.5f, 1f), Fv2.TestVectorF);
			V4DataArray testArray2 = new V4DataArray("testArray2", DateTime.Now, 5, 0, new System.Numerics.Vector2(2.5f, 1.0f), Fv2.TestVectorF);
			V4DataList list1 = new V4DataList("list1", DateTime.Now);
			list1.AddDefaults(2, Fv2.TestVectorF);
			V4DataList list2 = new V4DataList("list2", DateTime.Now);
			list2.AddDefaults(3, Fv2.TestVectorF);
			V4DataList list3 = new V4DataList("list3", DateTime.Now);

			collection1.Add(testArray1);
			collection1.Add(testArray2);
			collection1.Add(list1);
			collection1.Add(list2);
			collection1.Add(list3);

			Console.WriteLine(collection1.ToLongString("{0:f}"));

            Console.WriteLine(String.Format("MaxVal = {0}\n\n", collection1.MaxVal));

            Console.WriteLine("IncrVal\n");
			foreach (var f in collection1.IncrVal)
			{
				Console.WriteLine($"{f}\n");
			}

			Console.WriteLine("EnumPoints:\n");
			foreach (var x in collection1.EnumPoints)
			{	
				Console.WriteLine(x.ToString());
			}
		}

		static void Main(string[] args)
		{
			try
			{
				TestWriteRead();
				TestLinq();
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception in Program: {e.Message}");
			}
		}
	}
}

