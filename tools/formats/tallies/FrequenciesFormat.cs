using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class FrequenciesFormat
{

    public static ClassFrequencies[] Read(TextReader reader)
    {
        // read in the raw data
        var freq = ReadLines(reader)
              .Skip(1) // skip header
              .Select(e => e.Split(","))
              .Select(e => new
              {
                  type = int.Parse(e[0]),
                  prop = int.Parse(e[1]),
                  freq = float.Parse(e[2])
              })
              .ToArray();

        // max frequency number
        var maxFreq = freq.Select(f => f.prop).Max();

        // group on type and convert into frequency lists
        var freqMatrix =
            freq.GroupBy(f => f.type)
            .Select(g =>
            {
                var pv = new ClassFrequencies(maxFreq) { Id = g.Key };
                foreach (var tpf in g)
                {
                    pv.Properties[tpf.prop] = tpf.freq;
                }
                return pv;
            })
            .ToArray();

        return freqMatrix;
    }


    /// <summary>Build a list of entities.  Set <paramref name="forceDistinct"/> 
    ///          to false if the type parameter is an ISet.</summary>
    public static EntityProperties[] BuildEntityProperties<T>(IEnumerable<Edge> edges, bool forceDistinct = true)
                where T : ICollection<int>, new()
    {
        var dict = new Dictionary<int, T>();
        foreach (var e in edges)
        {
            if (!dict.TryGetValue(e.S, out var list))
            {
                list = new T();
                dict[e.S] = list;
            }
            list.Add(e.P);
        }

        var entities = dict.Select(kv => new EntityProperties
        {
            Id = kv.Key,
            Properties = kv.Value
        })
        .ToArray();

        // if necessary, force distinct
        if(forceDistinct) {
            var set = new HashSet<int>();
            foreach(var e in entities) {
                set.Clear();
                foreach(var p in e.Properties) {
                    set.Add(p);
                }
                e.Properties = set.ToList();
            }
        }

        return entities;
    }


    private static IEnumerable<string> ReadLines(TextReader reader)
    {
        while (true)
        {
            var line = reader.ReadLine();
            if (line == null) yield break;
            yield return line;
        }
    }
}
