namespace AdventOfCode.Y2025.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Secret Entrance")]
class Solution : Solver {

    public object PartOne(string input) {
        var stringArr = input.Split("\n");
        var zeroCount = 0;
        var dial = 50;
        foreach (var line in stringArr)
        {
            var bokstav = line[0];
            var tal = int.Parse(line[1..]);
            switch (bokstav)
            {
                case 'L':
                {
                    for (var i = 0; i < tal; i++)
                    {
                        dial--;
                        if (dial == -1) dial = 99;
                        
                    }
                    if (dial == 0) zeroCount++;
                    break;
                }
                case 'R':
                {
                    for (var i = 0; i < tal; i++)
                    {
                        dial++;
                        if (dial == 100) dial = 0;
                    }
                    if (dial == 0) zeroCount++;
                    break;
                }
            }
        }
        return zeroCount;
    }

    public object PartTwo(string input) {
        var stringArr= input.Split("\n");
        var zeroCount = 0;
        var dial = 50;
        foreach (var line in stringArr)
        {
            var direction = line[0];
            var steps = int.Parse(line[1..]);
            var dir = direction == 'L' ? -1 : 1;
            var fullRotations = steps / 100;
            zeroCount += fullRotations;
            var remainder = steps % 100;

            for (var i = 0; i < remainder; i++)
            {
                dial += dir;
                if (dial < 0) dial = 99;
                if (dial > 99) dial = 0;
                if (dial == 0) zeroCount++;
            }
        }
        return zeroCount;
    }
}
