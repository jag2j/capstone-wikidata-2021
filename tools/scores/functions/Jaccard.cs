using System.Collections.Generic;
using System;
using System.Linq;

public class JaccardScoringFunction : ScoringFunction
{    
    public override Scorer CreateScorer()
    {
        return new JaccardScorer();
    }

    private class JaccardScorer : Scorer
    {
        private readonly ISet<int> Left = new HashSet<int>();
        private readonly ISet<int> Right = new HashSet<int>();
        private readonly ISet<int> Union = new HashSet<int>();
        
        public override float Score(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            // clear
            Left.Clear();
            Right.Clear();
            Union.Clear();

            // build sets of properties
            for(var i=0; i<classFrequencies.Properties.Length; i++) {
                if(classFrequencies.Properties[i] > 0)
                    Left.Add(i);
            }

            foreach(var p in entity.Properties)
                Right.Add(p);

            // build intersection and union sets
            Union.UnionWith(Left);
            Union.UnionWith(Right);
            Left.IntersectWith(Right);
            
            return Left.Count / (1.0f * Union.Count);
        }
    }
}

