using System;
using System.Collections.Generic;
using GdUnit4;
using static GdUnit4.Assertions;


namespace PuzzleFight.Common.Tests;

[TestSuite]
public class BoardTests
{
    private StoneTypeEnum skipStone = StoneTypeEnum.GemRed;
    static Board MakeTestBoard(int width, int height, StoneTypeEnum skipPiece = StoneTypeEnum.GemRed)
    {
        var stones = new List<StoneTypeEnum>();
        {
            var values = Enum.GetValues(typeof(StoneTypeEnum));
            foreach (var value in values) stones.Add((StoneTypeEnum)value);
            stones.Remove(StoneTypeEnum.None);
            stones.Remove(skipPiece);
        }
        
        var array = new Array2D<StoneTypeEnum>(width, height);
        int shift = (width % stones.Count == 0) ? 1 : 0;
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                array[x, y] = stones[(x + y)%stones.Count];
            }
        }
        return new Board(array);
    }

    [TestCase]
    public void RandomTest()
    {
        var board = MakeTestBoard(4, 4);
        var (matchList, matches) = board.GetMatches();
        AssertInt(matchList.Count).IsEqual(0);
        board.Data.ForEach( v => AssertThat(v != skipStone) );
    }

    [TestCase]
    public void HorizontalMatch3Test()
    {
        var board = MakeTestBoard(5, 5, StoneTypeEnum.GemRed);
        board.Dump();
        var (matchList, matches) = board.GetMatches();
        
        AssertInt(matchList.Count).IsEqual(0);
        
        
        board.Data[1,1] = StoneTypeEnum.GemRed;
        board.Data[2,1] = StoneTypeEnum.GemRed;
        board.Data[3,1] = StoneTypeEnum.GemRed;
        
        (matchList, _) = board.GetMatches();
        AssertInt(matchList.Count).IsEqual(1);
        AssertThat(matchList[0]).IsEqual(new MatchData(3,skipStone));
    }
    
    [TestCase]
    public void VerticalMatch3Test()
    {
        var board = MakeTestBoard(5, 5, StoneTypeEnum.GemRed);
        var (matchList, _) = board.GetMatches();
        AssertThat(matchList.Count == 0);
        
        
        board.Data[1,1] = StoneTypeEnum.GemRed;
        board.Data[1,2] = StoneTypeEnum.GemRed;
        board.Data[1,3] = StoneTypeEnum.GemRed;
        
        (matchList, _) = board.GetMatches();
        AssertInt(matchList.Count).IsEqual(1);
        AssertThat(matchList[0]).IsEqual(new MatchData(3,skipStone));
    }
    
    [TestCase]
    public void ThreeThreeShapeMatchTest()
    {

        for (int y = 0; y < 3; ++y)
        {
            var board = MakeTestBoard(3, 3, StoneTypeEnum.GemRed);
            var (matchList, matches) = board.GetMatches();
            AssertThat(matchList.Count == 0);

            board.Data[0,0] = StoneTypeEnum.GemRed;
            board.Data[0,1] = StoneTypeEnum.GemRed;
            board.Data[0,2] = StoneTypeEnum.GemRed;
            
            board.Data[1,y] = StoneTypeEnum.GemRed;
            board.Data[2,y] = StoneTypeEnum.GemRed;
            

            (matchList, matches) = board.GetMatches();
            // for (int yy = 0; yy < matches.Height; ++yy)
            // {
            //     Console.Write($"{yy}:");
            //     for (int xx = 0; xx < matches.Width; ++xx)
            //     {
            //         Console.Write($"{(matches[xx,yy]?1:0)}");
            //     }
            //     Console.WriteLine();
            // }
            
            AssertInt(matchList.Count).IsEqual(2);
            AssertThat(matchList[0]).IsEqual(new MatchData(3,skipStone));
            AssertThat(matchList[1]).IsEqual(new MatchData(3,skipStone));
        }
    }
    
    [TestCase]
     public void FiveThreeShapeMatchTest()
     {
         for (int y = 1; y < 4; ++y)
         {
             var board = MakeTestBoard(5, 5, StoneTypeEnum.GemRed);
             var (matchList, matches) = board.GetMatches();
             AssertThat(matchList.Count == 0);
             
             var items = new (int, int)[]
             {
                 (1,0), (1,1), (1,2), (1,3), (1,4),
                 (2,y), (3,y)
             };
             
             foreach (var item in items)
             {
                 board.Data[item.Item1, item.Item2] = StoneTypeEnum.GemRed;
             }
             
             board.Dump();
             (matchList, matches) = board.GetMatches();
             AssertThat(matchList.Count).IsEqual(2);
             
             AssertThat(matchList[0]).IsEqual(new MatchData(5,skipStone));
             AssertThat(matchList[1]).IsEqual(new MatchData(3,skipStone));
             foreach (var item in items)
             {
                 AssertThat(matches[item.Item1, item.Item2]);
             }
         }
     }
}