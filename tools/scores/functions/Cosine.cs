using System.Collections.Generic;
using System;
using System.Linq;

public class CosineScoringFunction : ScoringFunction
{    
    public override Scorer CreateScorer()
    {
        return new CosineScorer();
    }

    private class CosineScorer : Scorer
    {
        private readonly Scorer InnerProductScorer = new InnerProductScoringFunction().CreateScorer();

        private (double, double) GetNorms(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            const double pow = 0.5;
            double eSum = entity.Properties.Count;
            double cSum = classFrequencies.Properties
                                         .Select(p => p*p)
                                         .Sum();
            return (Math.Pow(eSum, pow), Math.Pow(cSum, pow));
        }

        public override float Score(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            var ip = InnerProductScorer.Score(entity, classFrequencies);
            (var a, var b) = GetNorms(entity, classFrequencies);
            return (float) (ip / a / b);
        }
    }
}

