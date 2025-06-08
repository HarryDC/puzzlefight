namespace PuzzleFight.Common;


// Structure used to convey information about a single
// match, count will indicate how many stones of that
// type were in a given match
public struct MatchData
{
    public MatchData(int count, StoneTypeEnum type)
    {
        this.Type = type;
        this.Count = count;
    }

    public override string ToString()
    {
        return this.Type.ToString() + ":" + this.Count;
    }

    public int Count { get; }
    public StoneTypeEnum Type { get; }
}