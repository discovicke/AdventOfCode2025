using System.ComponentModel.Design;
using System.Runtime.InteropServices.JavaScript;

namespace AdventOfCode.Y2025.Day07;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Laboratories")]
class Solution : Solver {
    public object PartOne(string input) {
        var lines = input.Split("\n");
        var grid = lines.Select(line => line.ToCharArray()).ToArray();
        var timesParted = 0;
        int startCol = -1;
        for (int col = 0; col < grid[0].Length; col++) {
            if (grid[0][col] != 'S') {
                continue;
            }

            startCol = col;
            break;
        }

        var activeColumns = new HashSet<int> { startCol };

        for (int row = 1; row < grid.Length; row++) {
            var nextActiveColumns = new HashSet<int>();
            foreach (var col in activeColumns) {
                if (grid[row][col] == '^') {
                    timesParted++;
                    if (col > 0) {
                        nextActiveColumns.Add(col - 1);
                    }

                    if (col < grid[row].Length - 1) {
                        nextActiveColumns.Add(col + 1);
                    }
                } else {
                    nextActiveColumns.Add(col);
                }
            }

            activeColumns = nextActiveColumns;
        }

        return timesParted;
        }

        public object PartTwo(string input) {
            var lines = input.Split("\n");
            var grid = lines.Select(line => line.ToCharArray()).ToArray();
            int startCol = -1;
            for (int col = 0; col < grid[0].Length; col++) {
                if (grid[0][col] != 'S') {
                    continue;
                }

                startCol = col;
                break;
            }
            var activeColumns = new Dictionary<int, long> { { startCol, 1 } };
            for (int row = 1; row < grid.Length; row++) {
                var nextActiveColumns = new Dictionary<int, long>();
        
                foreach (var (col, pathCount) in activeColumns) {
                    if (grid[row][col] == '^') {
                        if (col > 0) {
                            if (!nextActiveColumns.ContainsKey(col - 1)) {
                                nextActiveColumns[col - 1] = 0;
                            }
                            nextActiveColumns[col - 1] += pathCount;
                        }
                        if (col < grid[row].Length - 1) {
                            if (!nextActiveColumns.ContainsKey(col + 1)) {
                                nextActiveColumns[col + 1] = 0;
                            }
                            nextActiveColumns[col + 1] += pathCount;
                        }
                    } else {
                        nextActiveColumns.TryAdd(col, 0);
                        nextActiveColumns[col] += pathCount;
                    }
                }
                activeColumns = nextActiveColumns;
            }
            return activeColumns.Values.Sum();
        }
    }
