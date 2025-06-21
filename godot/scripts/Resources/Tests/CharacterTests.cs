using System.Collections.Generic;
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

[TestSuite]
[RequireGodotRuntime]
public class CharacterTests
{
    [TestCase]
    public void CanCastTest()
    {
        var character = new Character();
        AssertBool(character.HasGems(new List<MatchData>())).IsTrue();
        List<MatchData> material = new() { new MatchData(5, StoneTypeEnum.GemRed) };
        AssertBool(character.HasGems(material)).IsFalse();
        character.AddGems(material);
        AssertBool(character.HasGems(material)).IsTrue();
        
        List<MatchData> material2 = new() { new MatchData(1, StoneTypeEnum.GemRed) };
        character.RemoveGems(material2);
        AssertBool(character.HasGems(material)).IsFalse();
    }
    
    [TestCase]
    public void AddMaterialTest()
    {
        var character = new Character();
        AssertInt(character.Stash.Count).IsEqual(3);
        AssertThat(character.Stash[StoneTypeEnum.GemRed]).IsEqual(0);
        AssertThat(character.Stash[StoneTypeEnum.GemGreen]).IsEqual(0);
        AssertThat(character.Stash[StoneTypeEnum.GemBlue]).IsEqual(0);
        {
            List<MatchData> material = new()
            {
                new MatchData(2, StoneTypeEnum.GemRed),
                new MatchData(3, StoneTypeEnum.GemGreen),
                new MatchData(4, StoneTypeEnum.GemBlue)
            };
            character.AddGems(material);
            AssertThat(character.Stash[StoneTypeEnum.GemRed]).IsEqual(2);
            AssertThat(character.Stash[StoneTypeEnum.GemGreen]).IsEqual(3);
            AssertThat(character.Stash[StoneTypeEnum.GemBlue]).IsEqual(4);
        }
        {
            List<MatchData> material = new()
            {
                new MatchData(1, StoneTypeEnum.GemRed),
                new MatchData(1, StoneTypeEnum.GemGreen),
                new MatchData(4, StoneTypeEnum.GemBlue)
            };
            character.RemoveGems(material);
            AssertThat(character.Stash[StoneTypeEnum.GemRed]).IsEqual(1);
            AssertThat(character.Stash[StoneTypeEnum.GemGreen]).IsEqual(2);
            AssertThat(character.Stash[StoneTypeEnum.GemBlue]).IsEqual(0);
        }
        
    }
    
}
