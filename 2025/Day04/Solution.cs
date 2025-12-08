namespace AdventOfCode.Y2025.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Printing Department")]
class Solution : Solver {

    public object PartOne(string input) {
        var rolls = input.Split("\n");
        int height = rolls.Length;
        int width  = rolls[0].Length;
        char rollChar = '@';
        var grid = new char[height, width];
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                grid[r, c] = rolls[r][c];
            }
        }
        var deltas = new (int dx, int dy)[]
        {
            (-1,-1),(0,-1),(1,-1),
            (-1, 0),       (1, 0),
            (-1, 1),(0, 1),(1, 1)
        };

        int totalRemoved = RemoveForRounds(grid, rollChar, deltas, 1);
        return totalRemoved;
    }

    public object PartTwo(string input)
    {
        var rolls = input.Split("\n");
        int height = rolls.Length;
        int width  = rolls[0].Length;
        char rollChar = '@';
        var grid = new char[height, width];
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                grid[r, c] = rolls[r][c];
            }
        }
        var deltas = new (int dx, int dy)[]
        {
            (-1,-1),(0,-1),(1,-1),
            (-1, 0),       (1, 0),
            (-1, 1),(0, 1),(1, 1)
        };
        int totalRemoved = RemoveUntilStable(grid, rollChar, deltas);
        return totalRemoved;
    }
    int RemoveWeakRolls(char[,] grid, char rollChar, (int dx, int dy)[] deltas)
    {
        int height = grid.GetLength(0);
        int width  = grid.GetLength(1);
        var toRemove = new List<(int r, int c)>();
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                if (grid[r, c] != rollChar) continue;
                int neighbors = 0;
                foreach (var (dr, dc) in deltas)
                {
                    int nr = r + dr, nc = c + dc;
                    if (nr >= 0 && nr < height && nc >= 0 && nc < width &&
                        grid[nr, nc] == rollChar)
                    {
                        neighbors++;
                    }
                }
                if (neighbors < 4)
                    toRemove.Add((r, c));
            }
        }
        foreach (var (r, c) in toRemove)
            grid[r, c] = '.';
        return toRemove.Count;
    }
    int RemoveForRounds(char[,] grid, char rollChar, (int dx, int dy)[] deltas, int rounds)
    {
        int total = 0;
        for (int i = 0; i < rounds; i++)
        {
            int removed = RemoveWeakRolls(grid, rollChar, deltas);
            total += removed;

            if (removed == 0)
                break;
        }
        return total;
    }
    int RemoveUntilStable(char[,] grid, char rollChar, (int dx, int dy)[] deltas)
    {
        int total = 0;

        while (true)
        {
            int removed = RemoveWeakRolls(grid, rollChar, deltas);
            if (removed == 0)
                break;

            total += removed;
        }

        return total;
    }
}
