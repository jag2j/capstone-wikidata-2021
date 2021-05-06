using System.IO;

public static class EdgeFormat {
    /// <summary>
    /// Parse an edge from the edge file (src, dst).
    /// Return null if the line is corrupt or edges are not signed 32-bit numeric.
    /// </summary>
    public static Edge ParseEdge(string line)
    {
        var fields = line.Split(',');
        if (fields.Length < 3) return null;
        if (!int.TryParse(fields[0], out var e1)) return null;
        if (!int.TryParse(fields[1], out var e2)) return null;
        if (!int.TryParse(fields[2], out var e3)) return null;
        return new Edge(e1, e2, e3);
    }

    public static void WriteEdge(TextWriter writer, Edge e) {
        writer.WriteLine($"{e.S},{e.P},{e.O}");
    }    
}