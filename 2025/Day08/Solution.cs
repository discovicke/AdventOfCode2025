namespace AdventOfCode.Y2025.Day08;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Playground")]
class Solution : Solver {
    public object PartOne(string input) {
        var lines = input.Split("\n");
        var junctionBoxes = lines.Select(line => {
            var parts = line.Split(',');
            return (
                X: int.Parse(parts[0]),
                Y: int.Parse(parts[1]),
                Z: int.Parse(parts[2])
            );
        }).ToArray();
        
        var edges = new List<(int from, int to, double distance)>();
        for (int i = 0; i < junctionBoxes.Length; i++) {
            for (int j = i + 1; j < junctionBoxes.Length; j++) {
                var a = junctionBoxes[i];
                var b = junctionBoxes[j];
                var distance = Math.Sqrt(
                    Math.Pow(a.X - b.X, 2) +
                    Math.Pow(a.Y - b.Y, 2) +
                    Math.Pow(a.Z - b.Z, 2)
                );
                edges.Add((i, j, distance));
            }
        }
        edges.Sort((a, b) => a.distance.CompareTo(b.distance));
        var parent = Enumerable.Range(0, junctionBoxes.Length).ToArray();
        var size = Enumerable.Repeat(1, junctionBoxes.Length).ToArray();
        foreach (var (from, to, _) in edges.Take(1000)) {
            Union(from, to, parent, size);
        }
        var circuitSizes = new Dictionary<int, int>();
        for (int i = 0; i < junctionBoxes.Length; i++) {
            int root = Find(i, parent);
            if (!circuitSizes.ContainsKey(root)) {
                circuitSizes[root] = 0;
            }

            circuitSizes[root]++;
        }
        var topThree = circuitSizes.Values.OrderByDescending(x => x).Take(3).ToList();
        return (long)topThree[0] * topThree[1] * topThree[2];
    }


public object PartTwo(string input) {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var junctionBoxes = lines.Select(line => {
            var parts = line.Split(',');
            return (
                X: int.Parse(parts[0]),
                Y: int.Parse(parts[1]),
                Z: int.Parse(parts[2])
            );
        }).ToArray();

        // Skapa alla kanter (Samma som del 1)
        var edges = new List<(int from, int to, double distance)>();
        for (int i = 0; i < junctionBoxes.Length; i++) {
            for (int j = i + 1; j < junctionBoxes.Length; j++) {
                var a = junctionBoxes[i];
                var b = junctionBoxes[j];
                // Vi behöver egentligen inte ens Sqrt för jämförelsen, 
                // men vi behåller det för enkelhets skull då prestandan räcker.
                var distance = Math.Sqrt(
                    Math.Pow(a.X - b.X, 2) +
                    Math.Pow(a.Y - b.Y, 2) +
                    Math.Pow(a.Z - b.Z, 2)
                );
                edges.Add((i, j, distance));
            }
        }

        edges.Sort((a, b) => a.distance.CompareTo(b.distance));

        var parent = Enumerable.Range(0, junctionBoxes.Length).ToArray();
        var size = Enumerable.Repeat(1, junctionBoxes.Length).ToArray();
        
        // Vi startar med 1000 separata öar (komponenter)
        int numberOfComponents = junctionBoxes.Length;
        long lastConnection = 0;
        foreach (var (from, to, _) in edges) {
            // Om Union returnerar true betyder det att vi slog ihop två separata grupper
            if (Union(from, to, parent, size)) {
                numberOfComponents--;

                // Om vi nu bara har 1 enda grupp kvar, så var detta den sista kopplingen
                // som behövdes för att allt ska hänga ihop.
                if (numberOfComponents == 1) {
                    long x1 = junctionBoxes[from].X;
                    long x2 = junctionBoxes[to].X;
                    
                    Console.WriteLine($"Sista kopplingen var mellan index {from} (X={x1}) och {to} (X={x2})");
                    lastConnection = x1 * x2;
                    // Returnera produkten (casta till long för säkerhets skull)
                }
            }
        }

        return lastConnection;
    }

    private static bool Union(int x, int y, int[] parent, int[] size) {
        int rootX = Find(x, parent);
        int rootY = Find(y, parent);

        if (rootX == rootY) return false; // Redan i samma circuit

        // Union by size
        if (size[rootX] < size[rootY]) {
            parent[rootX] = rootY;
            size[rootY] += size[rootX];
        } else {
            parent[rootY] = rootX;
            size[rootX] += size[rootY];
        }

        return true;
    }

    private static int Find(int x, int[] parent) {
        if (parent[x] != x) {
            parent[x] = Find(parent[x], parent); // Path compression
        }

        return parent[x];
    }
}
