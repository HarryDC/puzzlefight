using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class AiPlayerNode : Participant
{
    public override void Setup(BoardNode boardNode)
    {
        Character = GameManager.Instance.Opponent;
        if (Character == null) GD.PrintErr("Character is NULL in AiPlayerNode");
        BoardNode = boardNode;
        Actions = Character.Actions;
    }

    public override void BeginRound()
    {
        Actions = Character.Actions;
    }

    public override void TakeTurn()
    {
        Actions -= 1;
        Character.PreMoveUpdate();
        var moves = BoardNode.Board.GetAllMoves();
        var selected = HierarchicalMove(moves, BoardNode.Board);

        var tween = CreateTween();
        tween.TweenCallback(Callable.From(() => BoardNode.SelectPiece(moves[selected])));
        tween.TweenCallback(Callable.From(() => BoardNode.SelectPiece(moves[selected+1]))).SetDelay(0.25);
    }

    public override void EndRound()
    {
    }

    private int RandomMove(List<Vector2I> moves)
    {
        var count = moves.Count/2;
        var chosen = (int)(GD.Randi() % count);
        return chosen * 2;
    }
    /// <summary>
    /// Very basic AI, will attack first, defend second and then just
    /// pick the move that results in the most gems overall
    /// </summary>
    /// <param name="moves"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private int HierarchicalMove(List<Vector2I> moves, Board board)
    {
        if (moves.Count == 2) return 0;
        GD.Print(moves);
        var startState = board.Data.DeepCopy();
        List<List<MatchData>> matches = new();
        List<(int, int)> scores = new();
        
        for (int i = 0; i < moves.Count; i += 2)
        {
            var newBoard = new Board(startState);
            newBoard.Swap(moves[i], moves[i + 1]);
            var (iterationMatches, _) = newBoard.GetMatches();
            matches.Add(iterationMatches);
            scores.Add((i,0));
        }

        int maxIndex = -1;
        int maxScore = 0;
        
        // Find Highest Attack
        for (int i = 0; i < matches.Count; i++)
        {
            var attackCount =  (from match in matches[i]
                where match.Type == StoneTypeEnum.Sword select match).Sum(m => m.Count);

            if (attackCount > maxScore)
            {
                maxIndex = i;
                maxScore = attackCount;
            }
        }

        if (maxIndex != -1)
        {
            GD.Print($"{maxScore} Swords");
            return maxIndex * 2;
        }
        
        // Find Highest Defence
        maxIndex = -1;
        maxScore = 0;
        for (int i = 0; i < matches.Count; i++)
        {
            var defenceCount = (from match in matches[i]
                where match.Type == StoneTypeEnum.Shield select match).Sum(m => m.Count);

            if (defenceCount > maxScore)
            {
                maxIndex = i;
                maxScore = defenceCount;
            }
        }

        if (maxIndex != -1)
        {
            GD.Print($"{maxScore} Shields");
            return maxIndex * 2;
        }
        
        // Find Highest Sum
        maxIndex = -1;
        maxScore = 0;
        for (int i = 0; i < matches.Count; i++)
        {
            var count = (from match in matches[i] select match).Sum(m => m.Count);

            if (count > maxScore)
            {
                maxIndex = i;
                maxScore = count;
            }
        }

        if (maxIndex != -1)
        {
            GD.Print($"{maxScore} Gems");
            return maxIndex * 2;
        }

        // Should not have gottent here
        GD.PrintErr("Returning Random move ... odd");
        return RandomMove(moves);
    }
}