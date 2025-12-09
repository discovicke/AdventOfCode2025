namespace AdventOfCode.Y2025.Day08;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Playground")]
class Solution : Solver {
    public object PartOne(string input) {
        var junctionBoxes = ParseJunctionBoxes(input);
        var edges = BuildEdges(junctionBoxes);
        var (parent, size) = InitializeUnionFind(junctionBoxes.Length);
        
        foreach (var (from, to, _) in edges.Take(1000)) {
            Union(from, to, parent, size);
        }
        
        var circuitSizes = CalculateCircuitSizes(junctionBoxes.Length, parent, size);
        var topThree = circuitSizes.OrderByDescending(x => x).Take(3).ToList();
        return (long)topThree[0] * topThree[1] * topThree[2];
    }

    public object PartTwo(string input) {
        var junctionBoxes = ParseJunctionBoxes(input);
        var edges = BuildEdges(junctionBoxes);
        var (parent, size) = InitializeUnionFind(junctionBoxes.Length);
        
        int numberOfComponents = junctionBoxes.Length;
        long lastConnection = 0;
        
        foreach (var (from, to, _) in edges) {
            if (!Union(from, to, parent, size)) continue;
            numberOfComponents--;
            if (numberOfComponents != 1) continue;
            long x1 = junctionBoxes[from].X;
            long x2 = junctionBoxes[to].X;
            lastConnection = x1 * x2;
        }
        
        return lastConnection;
    }

    private static (int X, int Y, int Z)[] ParseJunctionBoxes(string input) {
        return input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line => {
                var parts = line.Split(',');
                return (
                    X: int.Parse(parts[0]),
                    Y: int.Parse(parts[1]),
                    Z: int.Parse(parts[2])
                );
            }).ToArray();
    }

    private static List<(int from, int to, double distance)> BuildEdges((int X, int Y, int Z)[] junctionBoxes) {
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
        return edges;
    }

    private static (int[] parent, int[] size) InitializeUnionFind(int count) {
        var parent = Enumerable.Range(0, count).ToArray();
        var size = Enumerable.Repeat(1, count).ToArray();
        return (parent, size);
    }

    private static List<int> CalculateCircuitSizes(int count, int[] parent, int[] size) {
        var circuitSizes = new Dictionary<int, int>();
        for (int i = 0; i < count; i++) {
            int root = Find(i, parent);
            circuitSizes.TryAdd(root, 0);
            circuitSizes[root]++;
        }
        return circuitSizes.Values.ToList();
    }

    private static bool Union(int x, int y, int[] parent, int[] size) {
        int rootX = Find(x, parent);
        int rootY = Find(y, parent);
        if (rootX == rootY) return false;
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
            parent[x] = Find(parent[x], parent);
        }
        return parent[x];
    }
}
