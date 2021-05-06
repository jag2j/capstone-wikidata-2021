using System.Collections.Generic;
using System;
using System.Linq;

///<summary>Calculate the Manhattan distance</summary>
public class ManhattanScoringFunction : ScoringFunction
{    
    public override Scorer CreateScorer()
    {
        return new ManhattanScorer();
    }

    private class ManhattanScorer : Scorer
    {
        public override float Score(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            var score = 0.0f;

            // add up frequencies where we match
            var classProperties = classFrequencies.Properties;
            for(var i=0; i<classProperties.Length; i++)
            {
                var offset = entity.Properties.Contains(i) ? 1 : 0;
                score += Math.Abs(offset - classProperties[i]);
            }

            return score;
        }
    }
}

