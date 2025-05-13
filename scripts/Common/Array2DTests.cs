using System.Collections;
using PuzzleFight.Common;
using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class Array2DTests
{
    // A Test behaves as an ordinary method
    
    [TestCase]
    public void Initialization()
    {
        var a = new Array2D<int>(2, 3, 4);
        AssertThat(a.Width).IsEqual(2);
        AssertThat(a.Height).IsEqual(3);
        AssertThat(a[0,0]).IsEqual(4);
        AssertThat(a[1,1]).IsEqual(4);
    }

    [TestCase]
    public void AccessInt()
    {
        var a = new Array2D<int>(2, 3, 4);

        AssertThat(a[0,0]).IsEqual(4);
        a[0, 0] = 1;
        AssertThat(a[0,0]).IsEqual(1);

        AssertThat(a[1,1]).IsEqual(4);
        a.Set(1, 1, 1);
        AssertThat(a[1,1]).IsEqual(1);
    }

    [TestCase]
    public void AccessVecInt()
    {
        var a = new Array2D<int>(2, 3, 4);

        var v00 = new Vector2I(0, 0);

        AssertThat(a[v00]).IsEqual(4);
        a[v00] = 1;
        AssertThat(a[v00]).IsEqual(1);
    }

    [TestCase]
    public void SwapInt()
    {
        var a = new Array2D<int>(2, 3, 4);
        a[0, 0] = 1;
        a.Swap(0, 0, 1, 1);
        AssertThat(a[0,0]).IsEqual(4);
        AssertThat(a[1,1]).IsEqual(1);
    }

    [TestCase]
    public void SwapVecInt()
    {
        var a = new Array2D<int>(2, 3, 4);
        var v00 = new Vector2I(0, 0);
        var v11 = new Vector2I(1, 1);

        a[v00] = 1;
        a.Swap(v00, v11);
        AssertThat(a[v11]).IsEqual(1);
        AssertThat(a[v00]).IsEqual(4);
    }
}
