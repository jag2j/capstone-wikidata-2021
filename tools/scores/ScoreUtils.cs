using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class ScoreUtils
{

    private void AddToBag<T>(T item, ConcurrentBag<T> bag)
    {
        bag.Add(item);
    }

    ///<summary>A convenience method for scoring an entity 
    ///         as a possible match to a set of other entities
    ///         using the specified functions.
    ///
    ///         See <see cref="ScoreUtils.GetScoreList()"/>
    ///         for a multi-threaded version.
    ///</summary>
    public EntityMatches GetScore(EntityProperties entity,
                                  List<ClassFrequencies> classFrequencies,
                                  params ScoringFunction[] functions)
    {
        var scorers = functions.Select(s => s.CreateScorer());
        return new EntityMatches {
            BaseEntity = entity.Id,
            Matches = classFrequencies.Select(c => new Match 
            {
                MatchedEntity = c.Id,
                Scores = scorers.Select(s => new Score
                {
                    Scorer = s.Name,
                    Value = s.Score(entity, c)
                })
                .ToList()
            })
            .ToArray()
        };
    }

    ///<summary>Get the list of similarity scores</summary>
    public List<EntityMatches> GetScores(
                   IEnumerable<EntityProperties> entities,
                   List<ClassFrequencies> classFrequencies,
                   IEnumerable<ScoringFunction> scorers,
                   int dop = 0)
    {
        var bag = new ConcurrentBag<EntityMatches>();
        GetScoresToFunction(entities, classFrequencies, scorers,
                  em => AddToBag(em, bag), dop);

        return bag.ToList();
    }

    /// <summary>Append additional scores using type-based weights.</summary>
    public void AddWeightedScores(EntityMatches originalMatches,
                                  IDictionary<int, float> weightByTypes,
                                  float defaultWeight = 1.0f,
                                  string scoreNamePrefix = "weighted_")
    {
        foreach(var m in originalMatches.Matches)
        {
            foreach(var os in m.Scores.ToArray()) 
            {
                if(!weightByTypes.TryGetValue(m.MatchedEntity, out var weight))
                    weight = defaultWeight;
                
                m.Scores.Add(new Score {
                    Scorer = $"{scoreNamePrefix}{os.Scorer}",
                    Value = weight * os.Value
                });
            }
        }
    }


    ///<summary>Calculate class matching scores for each specified entity in parallel 
    ///         and pass results for each completed entity to the specified function</summary>
    public void GetScoresToFunction(
                   IEnumerable<EntityProperties> entities,
                   List<ClassFrequencies> classFrequencies,
                   IEnumerable<ScoringFunction> scoreFunctions,
                   Action<EntityMatches> notify,
                   int dop = 0
                  )
    {
        // default to processor count
        if (dop == 0) dop = Environment.ProcessorCount;

        var queue = new ConcurrentQueue<EntityProperties>(entities);

        // process entities in parallel
        var tasks = Enumerable.Range(1, dop)
                              .Select(_ => Task.Run(() =>
        {
            // set up thread-specific scorers
            var scorers = scoreFunctions.Select(s => s.CreateScorer())
                                        .ToArray();

            // process queue
            while (true)
            {
                if (!queue.TryDequeue(out var entity)) return;

                var entityMatches = new EntityMatches
                {
                    BaseEntity = entity.Id,
                    Matches = new Match[classFrequencies.Count]
                };

                // score against each possible type
                for (var typeIndex = 0; typeIndex < classFrequencies.Count; typeIndex++)
                {
                    var type = classFrequencies[typeIndex];

                    var match = new Match
                    {
                        MatchedEntity = type.Id,
                        Scores = new List<Score>(scorers.Length)
                    };

                    // score this type against each possible scorer function
                    for (var scoreIndex = 0; scoreIndex < scorers.Length; scoreIndex++)
                    {
                        var newScore = new Score
                        {
                            Scorer = scorers[scoreIndex].Name,
                            Value = scorers[scoreIndex].Score(entity, type)
                        };
                        match.Scores.Add(newScore);
                    }

                    entityMatches.Matches[typeIndex] = match;
                }

                notify(entityMatches);
            }
        }));

        Task.WaitAll(tasks.ToArray());
    }
}