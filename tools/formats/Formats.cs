using System;
using System.Collections.Generic;
using System.IO;

/// <summary>Utilities for handling the various file formats</summary>
public static class Formats
{
    /// <summary>Open a (disposable) reader on a file</summary>
    public static TextReader GetReader(string path)
    {
        var fr = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var tr = new StreamReader(fr, leaveOpen: false);
        return tr;
    }

    /// <summary>Open a (disposable) writer</summary>
    public static TextWriter GetWriter(string path, bool autoFlush = true) {
        var fw = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        var tw = new StreamWriter(fw, leaveOpen: false);
        tw.AutoFlush = autoFlush; // when true, slower but progress can be seen sooner
        return tw;
    }

    ///<summary>Read in the edges in an edge file. We only support 32-bit entity IDs.</summary>
    public static IEnumerable<Edge> ReadEdgeFile(TextReader reader, int maxCount = -1)
        => Read(reader, EdgeFormat.ParseEdge, maxCount);

    public static ClassFrequencies[] ReadFrequencyFile(TextReader reader)
        => FrequenciesFormat.Read(reader);

    public static EntityProperties[] BuildEntityProperties<T>(IEnumerable<Edge> edges) 
        where T:ICollection<int>, new()
        => FrequenciesFormat.BuildEntityProperties<T>(edges);
    

    ///<summary>
    /// A simple parser that reads the lines of a file, 
    /// passing them through a parse function
    /// and returning the results for those successfully parsed.
    /// You can optional set a maximum line count.
    ///</summary>
    private static IEnumerable<T> Read<T>(TextReader tr, Func<string, T> parse, int maxCount = -1)
    {
        for (var i = 0; maxCount == -1 || i < maxCount; i++)
        {
            var line = tr.ReadLine();
            if (line == null) yield break;
            var parsed = parse(line);
            if (parsed != null)
            {
                yield return parsed;
            }
        }
    }



}