using System.Collections.Generic;
using System;
using System.Diagnostics;
using Godot;

namespace PuzzleFight.Common;

// TODO Bug in detection, does not detect a match 
// *
// *
// *** <- The two extra pieces did not get removed
// *
// *
public class Board
{
    public int Width;
    public int Height;

    public float SwapTime = 0.25f;
    public float DropSpeedPerSquare = 0.2f;

    private Array2D<StoneTypeEnum> _data;
    public Array2D<StoneTypeEnum> Data => _data;

    // Board sized, contains true if the piece at that location
    // matches anything
    private Array2D<bool> _matches;
    public Array2D<bool> Matches => _matches;
    
    private List<MatchData> _matchData = new();
    
    // Start is called before the first frame update
    // Lower Left is 0/0
    public Board(int width, int height)
    {
        Height = 5;
        Width = width;
        Height = height;
        _data = new Array2D<StoneTypeEnum>(Width, Height, StoneTypeEnum.None);
        _matches = new Array2D<bool>(Width, Height);

        for (int i = 0; i < Width; ++i)
        {
            for (int j = 0; j < Height; ++j)
            {
                _data[i, j] = SuggestNewColor(new Vector2I(i, j));
            }
        }
    }
    
    public bool Swap(Vector2I a, Vector2I b)
    {
        int aDiff = Math.Abs(a.X - b.X);
        int bDiff = Math.Abs(a.Y - b.Y);

        if (aDiff > 1 || bDiff > 1 || aDiff == bDiff) return false;
        if (!IsValid(a, b) && !IsValid(b, a))
        {
            return false;
        }

        var pieceA = _data[a.X, a.Y];
        var pieceB = _data[b.X, b.Y];
        _data[a.X, a.Y] = pieceB;
        _data[b.X, b.Y] = pieceA;

        return true;
    }
    
    private void DropNewPiece(StoneTypeEnum stone, Vector2I from, Vector2I to)
    {
        Debug.Assert(_data[to.X, to.Y] == StoneTypeEnum.None);
        _data[to.X, to.Y] = stone;
        StoneTypeEnum color = SuggestNewColor(to);
        stone = color;
    }
    
    /// <summary>
    /// Calculate all possible moves
    /// </summary>
    /// <returns>A list of moves of the size 2 * possible moves, with element n
    /// and n+1 being a move, might contain duplicates</returns>
    public List<Vector2I> GetAllMoves()
    {
        // Maybe turn into member variable
        var result = new List<Vector2I>();
        var from = new Vector2I();
        var to = new Vector2I();
        for (int x = 0; x < Width; ++x)
        {
            from.X = x;
            for (int y = 0; y < Height; ++y)
            {
                from.Y = y;
                to.X = from.X + 1;
                to.Y = from.Y;
                if (to.X < Width && IsValid(from, to))
                {
                    result.Add(from);
                    result.Add(to);
                }
                to.X = from.X;
                to.Y = from.Y + 1;
                if (to.Y < Height && IsValid(from, to))
                {
                    result.Add(from);
                    result.Add(to);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Determine if there is _any_ match if the stones on the two
    /// locations in the parameters are being swapped
    /// </summary>
    /// <param name="from">Source Location</param>
    /// <param name="to">Target Location</param>
    /// <returns>true if the given swap creates a match</returns>
    public bool IsValid(Vector2I from, Vector2I to)
    {
        _data.Swap(from, to);
        bool result = HasMatches(from) || HasMatches(to);
        _data.Swap(from, to);
        return result;
    }

    bool HasMatches(Vector2I location)
    {
        int x = location.X;
        int y = location.Y;
        // Start with 1 match (the target stone itself)
        int matches = 1;
        var type = _data[location];
        for (int i = x - 1; i >= x - 2 && i >= 0; --i)
        {
            if (_data[i, y] == type) matches++;
            else break;
        }
        for (int i = x + 1; i <= x + 2 && i < Width; ++i)
        {
            if (_data[i, y] == type) matches++;
            else break;
        }
        if (matches > 2) return true;
        matches = 1;
        for (int j = y - 1; j >= y - 2 && j >=0; --j)
        {
            if (_data[x, j] == type) matches++;
            else break;
        }
        for (int j = y + 1; j <= y + 2 && j < Height; ++j)
        {
            if (_data[x, j] == type) matches++;
            else break;
        }
        if (matches > 2) return true;

        return false;
    }

    /// <summary>
    /// Fill internal data with stones that match other stones
    /// </summary>
    /// <returns>true if there where any matches, false otherwise</returns>
    public (List<MatchData>, Array2D<bool>) GetMatches()
    {
        ClearMatches();

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                const int maxCount = 5;
                int matched = 0;
                // Check for matches along X
                for (int i = 0; i <= maxCount && x + i < Width; ++i)
                {
                    if (_data[x, y] == _data[x + i, y] && 
                        (!_matches[x, y] || !_matches[x+i, y])) matched++;
                    else break;
                }
                
                if (matched > 2)
                {
                    for (int i = 0; i < matched;++i)
                    {
                        _matches[x + i, y] = true;
                    }
                    _matchData.Add(new MatchData(matched, _data[x, y]));
                }

                // Check for matches along Y
                matched = 0;
                for (int i = 0; i <= maxCount && y + i < Height; ++i)
                {
                    if (_data[x, y] == _data[x , y + i] &&
                        (!_matches[x, y] || !_matches[x, y + i])) matched++;
                    else break;
                }

                if (matched > 2)
                {
                    for (int i = 0; i < matched; ++i)
                    {
                        _matches[x , y + i] = true;
                    }
                    _matchData.Add(new MatchData(matched, _data[x, y]));
                }
            }
        }
        return (_matchData, _matches);
    }

    private void ClearMatches()
    {
        _matches.Fill(false);
        _matchData.Clear();
    }

    /// <summary>
    /// Removes all matches from the board and sets up a new stones to drop down
    /// </summary>
    public void RemoveMatching()
    {
        for (int i = 0; i < Width; ++i)
        {
            int moveDist = 0;
            for (int j = Height-1; j >=0 ; --j)
            {
                // Clean up the piece that was at the matching location
                // For now, just deactivate
                if (_matches[i,j])
                {
                    _data[i, j] = StoneTypeEnum.None;
                    moveDist += 1;
                } else if (moveDist > 0)
                {
                    _data[i, j+moveDist] = _data[i, j];
                    _data[i, j] = StoneTypeEnum.None;
                }
            }
            for (int j = moveDist - 1; j >= 0; --j)
            {
                var stone = SuggestNewColor(new Vector2I(i, j));
                _data[i, j] = stone;
            }
        }
        ClearMatches();
    }

    public void RefreshBoard()
    {
        _data.ForEach(_ => StoneType.Random());

        for (var x = 0; x < Width; ++x)
        {
            for (var y = 0; y < Height; ++y)
            {
                _data[x,y] = SuggestNewColor(new Vector2I(x,y));
            }
        }
    }

    int ColorInRow(StoneTypeEnum color, Vector2I origin, int range = 1)
    {
        int result = 0;
        int y = origin.Y;
        for (int x = origin.X - range; x < origin.X + range; ++x)
        {
            if (x < 0 || x > Width) continue;
            result += (_data[x, y] == color) ? 1 : 0;
        }
        return result;
    }

    private int ColorInColumn(StoneTypeEnum color, Vector2I origin, int range = 1)
    {
        int result = 0;
        int x = origin.X;
        for (int y = origin.Y - range; y < origin.Y + range; ++y)
        {
            if (y < 0 || y > Height) continue;
            result += (_data[x, y] == color) ? 1 : 0;
        }
        return result;
    }
    private StoneTypeEnum SuggestNewColor(Vector2I to)
    {
        var type = StoneType.Random();
        while (ColorInColumn(type, to) >= 1 || ColorInRow(type, to) >= 1)
        {
            type = StoneType.Random();
        }
        return type;
    }

    public Vector2I PositionToBoard(Vector3 localPos)
    {
        return new Vector2I((int)(localPos.X + 0.5), (int)(localPos.Y + 0.5));
    }

    public Vector3 BoardToPosition(Vector2I gridPos)
    {
        return new Vector3(gridPos.X, gridPos.Y, 0.0f);
    }

    public void Dump()
    {
        for (var x = 0; x < Width; x++)
        {
            string line = "";
            for (var y = Height - 1; y >= 0; y--)
            {
                // Print enum values 
                var type = _data[x, y];
                line += ((int)type).ToString();

            }
            GD.Print(line);
        }
    }
}