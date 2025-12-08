namespace AdventOfCode.Y2025.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Lobby")]
class Solution : Solver {

    public object PartOne(string input) {
        long power = 0;
        foreach (var battery in input.Split("\n"))
        {
            var digits = LargestFixedOrder(battery, 2);
            var maxValue = long.Parse(digits);
            power += maxValue;
        }
        return power;
    }

    public object PartTwo(string input) { 
        long power = 0;
        foreach (var battery in input.Split("\n"))
        {
            var digits = LargestFixedOrder(battery, 12);
            var maxValue = long.Parse(digits);
            power += maxValue;
        }
        return power;
    }
    private static string LargestFixedOrder(string s, int k)
    {
        long n = s.Length;
        var toRemove = n - k;          // antal vi måste ta bort
        var result = new List<char>();
        foreach (var digit in s)
        {                                   // Ta bort siffror från result om: 
            while (toRemove > 0             // 1. Vi har siffror kvar att ta bort (toRemove > 0)
                   && result.Count > 0      // 2. Result inte är tom
                   && result[^1] < digit)   // 3. Senaste siffran i result är mindre än nuvarande siffra
            {
                result.RemoveAt(result.Count - 1);
                toRemove--;
            }
            result.Add(digit);
        }
        return new string(result.GetRange(0, k).ToArray());
    }
}
