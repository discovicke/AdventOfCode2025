using System.Globalization;

namespace AdventOfCode.Y2025.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Gift Shop")]
class Solution : Solver {

    public object PartOne(string input) {
        var productRanges = input.Split(',');
        double invalidSum = 0;
        
        foreach (var rangeText in productRanges)
        {
            var rangeParts = rangeText.Split('-');
            var rangeStart = double.Parse(rangeParts[0]);
            var rangeEnd = double.Parse(rangeParts[1]);
            for (var currentId = rangeStart; currentId <= rangeEnd; currentId++)
            {
                var idAsString = currentId.ToString(CultureInfo.InvariantCulture);
                var idLength = idAsString.Length;
                for (var pattern = 1; pattern <= idLength / 2; pattern++)
                {
                    if (idLength != pattern * 2) continue;
                    var pRepeat = idAsString[..pattern];
                    var secondHalf = idAsString[pattern..];
                    if (secondHalf != pRepeat) continue;
                    invalidSum += double.Parse(idAsString);
                    break;
                }
            }
        }
        return invalidSum;
    }

    public object PartTwo(string input) {
        var productRanges = input.Split(',');
        double invalidSum = 0;
        foreach (var rangeText in productRanges)
        {
            var rangeParts = rangeText.Split('-');
            var rangeStart = double.Parse(rangeParts[0]);
            var rangeEnd = double.Parse(rangeParts[1]);
            for (var currentId = rangeStart; currentId <= rangeEnd; currentId++)
            {
                var idAsString = currentId.ToString(CultureInfo.InvariantCulture);
                var idLength = idAsString.Length;
                for (var pattern = 1; pattern <= idLength / 2; pattern++)
                {
                    if (idLength % pattern != 0) continue;
                    var pRepeat = idAsString[..pattern];
                    var match = true;
                    for (var start = pattern; start < idLength; start += pattern)
                    {
                        var currentSegment = idAsString[start..(start + pattern)];
                        if (currentSegment == pRepeat) continue;
                        
                        match = false;
                        break;
                    }
                    if (!match) continue;
                    invalidSum += double.Parse(idAsString);
                    break;
                }
            }
        }
        return  invalidSum;  
    }
    

    private static long SumInvalidIds(string input) {
        var ranges = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        long sum = 0;

        foreach (var range in ranges)
        {
            var dashIndex = range.IndexOf('-');
            var start = long.Parse(range.AsSpan(0, dashIndex), CultureInfo.InvariantCulture);
            var end = long.Parse(range.AsSpan(dashIndex + 1), CultureInfo.InvariantCulture);

            for (var id = start; id <= end; id++)
            {
                if (IsRepeatedTwice(id))
                {
                    sum += id;
                }
            }
        }

        return sum;
    }

    private static bool IsRepeatedTwice(long value)
    {
        var digits = value.ToString(CultureInfo.InvariantCulture);
        if ((digits.Length & 1) == 1)
        {
            return false;
        }

        var half = digits.Length / 2;
        for (var i = 0; i < half; i++)
        {
            if (digits[i] != digits[i + half])
            {
                return false;
            }
        }

        return true;
    }
}
