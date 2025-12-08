namespace AdventOfCode.Y2025.Day05;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Cafeteria")]
class Solution : Solver {
    public object PartOne(string input) {
        var inventory = input.Split("\n");
        var productIDs = new List<long>();
        var ranges = new List<(long start, long end)>();
        bool parsingRanges = true;
        foreach (var item in inventory) {
            if (string.IsNullOrWhiteSpace(item)) {
                parsingRanges = false;
                continue;
            }
            if (parsingRanges && item.Contains('-')) {
                var parts = item.Split('-');
                ranges.Add((long.Parse(parts[0]), long.Parse(parts[1])));
            } else if (!parsingRanges && long.TryParse(item, out var id)) {
                productIDs.Add(id);
            }
        }
        var mergedRanges = MergeRanges(ranges);
        int freshCount = 0;
        foreach (var id in productIDs) {
            if (IsFresh(id, mergedRanges))
                freshCount++;
        }
        return freshCount;
    }

    public object PartTwo(string input) {
        var inventory = input.Split("\n");
        var productIDs = new List<long>();
        var ranges = new List<(long start, long end)>();
        bool parsingRanges = true;
        foreach (var item in inventory) {
            if (string.IsNullOrWhiteSpace(item)) {
                parsingRanges = false;
                continue;
            }
            if (parsingRanges && item.Contains('-')) {
                var parts = item.Split('-');
                ranges.Add((long.Parse(parts[0]), long.Parse(parts[1])));
            } else if (!parsingRanges && long.TryParse(item, out var id)) {
                productIDs.Add(id);
            }
        }
        var mergedRanges = MergeRanges(ranges);
        long totalFreshIds = mergedRanges.Sum(r => r.end - r.start + 1);
        return totalFreshIds;
    }

    static List<(long start, long end)> MergeRanges(List<(long start, long end)> ranges) {
        if (ranges.Count == 0)
            return new List<(long, long)>();
        var sorted = ranges.OrderBy(r => r.start).ThenBy(r => r.end).ToList();
        var result = new List<(long start, long end)>();
        var current = sorted[0];
        for (int i = 1; i < sorted.Count; i++) {
            var next = sorted[i];
            if (next.start <= current.end + 1) {
                current.end = Math.Max(current.end, next.end);
            } else {
                result.Add(current);
                current = next;
            }
        }
        result.Add(current);
        return result;
    }
    static bool IsFresh(long id, List<(long start, long end)> ranges) {
        return ranges.Any(r => id >= r.start && id <= r.end);
    }
}
