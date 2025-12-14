using System.IO;

namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Movie Theater")]
class Solution : Solver {
    public object PartOne(string input) {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var points = lines.Select(line => {
            var parts = line.Split(',');
            return (X: long.Parse(parts[0]), Y: long.Parse(parts[1]));
        }).ToArray();
        long maxArea = 0;
        // Jämför alla par av punkter
        for (int i = 0; i < points.Length; i++) {
            for (int j = i + 1; j < points.Length; j++) {
                var p1 = points[i];
                var p2 = points[j];
                // Beräkna rektangelns bredd och höjd
                long width = Math.Abs(p1.X - p2.X) + 1;
                long height = Math.Abs(p1.Y - p2.Y) + 1;
                // Beräkna arean
                long area = width * height;
                if (area > maxArea) {
                    maxArea = area;
                }
            }
        }

        return maxArea;
    }

    public object PartTwo(string input) {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var points = lines.Select(line => {
            var parts = line.Split(',');
            return (X: long.Parse(parts[0]), Y: long.Parse(parts[1]));
        }).ToArray();

        long maxArea = 0;

        // Förbered polygonens kanter (inklusive sista punkten tillbaka till första)
        var edges = new List<((long X, long Y) p1, (long X, long Y) p2)>();
        for (int i = 0; i < points.Length; i++) {
            edges.Add((points[i], points[(i + 1) % points.Length]));
        }

        for (int i = 0; i < points.Length; i++) {
            for (int j = i + 1; j < points.Length; j++) {
                var p1 = points[i];
                var p2 = points[j];

                // Skapa rektangelns gränser
                long minX = Math.Min(p1.X, p2.X);
                long maxX = Math.Max(p1.X, p2.X);
                long minY = Math.Min(p1.Y, p2.Y);
                long maxY = Math.Max(p1.Y, p2.Y);


                // 1. Kolla om rektangelns mittpunkt är inuti polygonen.
                // Vi använder doubles för mitten för att undvika att hamna exakt på en kant.
                double midX = (minX + maxX) / 2.0;
                double midY = (minY + maxY) / 2.0;

                if (!IsPointInPolygon(midX, midY, edges)) {
                    continue;
                }

                // 2. Kolla så att ingen av polygonens kanter skär rakt IGENOM rektangeln.
                // Om en kant går längs med rektangelns kant är det OK, men den får inte korsa insidan.
                if (EdgesIntersectRect(minX, maxX, minY, maxY, edges)) {
                    continue;
                }

                // Om vi kommer hit är rektangeln giltig (helt innesluten av röda/gröna tiles)
                long width = maxX - minX + 1;
                long height = maxY - minY + 1;
                long area = width * height;

                if (area > maxArea) {
                    maxArea = area;
                }
            }
        }

        return maxArea;
    }

    // Ray Casting algoritm för att se om en punkt är inuti en polygon
    private bool IsPointInPolygon(double testX, double testY, List<((long X, long Y) p1, (long X, long Y) p2)> edges) {
        bool inside = false;
        foreach (var edge in edges) {
            // Kolla om vår stråle (mot höger längs X-axeln) korsar kanten
            // Vi kollar om testY ligger inom kantens Y-intervall
            bool isBetweenY = (edge.p1.Y > testY) != (edge.p2.Y > testY);

            if (isBetweenY) {
                // Räkna ut X-koordinaten för skärningspunkten
                double intersectX = (edge.p2.X - edge.p1.X) * (testY - edge.p1.Y) / (double)(edge.p2.Y - edge.p1.Y) +
                                    edge.p1.X;

                // Om skärningen är till höger om vår punkt, räkna den
                if (testY < Math.Max(edge.p1.Y, edge.p2.Y) && testX < intersectX) {
                    inside = !inside;
                }
            }
        }

        return inside;
    }

    // Kontrollera om någon polygon-kant inkräktar på rektangelns inre
    private bool EdgesIntersectRect(long rMinX, long rMaxX, long rMinY, long rMaxY,
        List<((long X, long Y) p1, (long X, long Y) p2)> edges) {
        foreach (var edge in edges) {
            bool isVertical = edge.p1.X == edge.p2.X;

            if (isVertical) {
                long edgeX = edge.p1.X;
                long edgeMinY = Math.Min(edge.p1.Y, edge.p2.Y);
                long edgeMaxY = Math.Max(edge.p1.Y, edge.p2.Y);

                // För att "skära igenom" måste linjen vara STRIGT mellan rektangelns X-väggar
                // OCH ha ett Y-intervall som överlappar STRIGT med rektangelns Y-intervall.
                if (edgeX > rMinX && edgeX < rMaxX) {
                    // Kolla överlapp i Y-led (max av min-värden < min av max-värden)
                    if (Math.Max(edgeMinY, rMinY) < Math.Min(edgeMaxY, rMaxY)) {
                        return true; // Korsar insidan
                    }
                }
            } else // Horizontal
            {
                long edgeY = edge.p1.Y;
                long edgeMinX = Math.Min(edge.p1.X, edge.p2.X);
                long edgeMaxX = Math.Max(edge.p1.X, edge.p2.X);

                // Samma logik men byt X mot Y
                if (edgeY > rMinY && edgeY < rMaxY) {
                    if (Math.Max(edgeMinX, rMinX) < Math.Min(edgeMaxX, rMaxX)) {
                        return true; // Korsar insidan
                    }
                }
            }
        }

        return false;
    }
}
