namespace AdventOfCode.Y2025.Day06;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Trash Compactor")]
class Solution : Solver {
    public object PartOne(string input) {
        var lines = input.Split("\n");
        var splitLines = new List<string[]>();
        foreach (var line in lines)
        {
            splitLines.Add(line.Split([" "], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        long solution = 0;
        var operators = splitLines.Last();
        var numbers = splitLines.Take(splitLines.Count - 1);
        for (int i = 0; i < operators.Length; i++)
        {
            var op = operators[i];
            var actual = new List<string>();
            foreach (var num in numbers)
            {
                actual.Add(num[i]);
            }
            solution += CalculateColumnResult(op[0], actual);
        }
        return  solution;  
    }
    public object PartTwo(string input) {
        var lines = input.Split("\n");
        List<Stack<char>> splitLines = CreateStacks(lines);
        long solution = 0;
        while (splitLines.Last().Count > 0)
        {
            var endOfColumn = false;
            var op = ' ';
            var actual = new List<string>();
            var verticalNum = "";
            while (!endOfColumn)
            {
                foreach (var stack in splitLines)
                {
                    var c = stack.Pop();
                    if (c == '+' || c == '*')
                    {
                        endOfColumn = true;
                        op = c;
                        break;
                    }
                    verticalNum += c;
                }
                actual.Add(verticalNum);
                verticalNum = "";
            }
            actual = [.. actual.Where(n => !string.IsNullOrWhiteSpace(n))];

            solution += CalculateColumnResult(op, actual);
        }
        return solution;
    }
    private static long CalculateColumnResult(char op, List<string> actual)
    {
        long result = op == '*' ? 1 : 0;
        foreach (var num in actual)
        {
            if (op == '*')
            {
                result *= int.Parse(num);
            }
            if (op == '+')
            {
                result += int.Parse(num);
            }
        }
        return result;
    }
    private static List<Stack<char>> CreateStacks(string[] lines)
    {
        var splitLines = new List<Stack<char>>();
        foreach (var line in lines)
        {
            var stack = new Stack<char>();
            foreach (var num in line)
            {
                stack.Push(num);
            }
            splitLines.Add(stack);
        }
        return splitLines;
    }
}
