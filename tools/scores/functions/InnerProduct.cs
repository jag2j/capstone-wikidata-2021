using System.Collections.Generic;

public class InnerProductScoringFunction : ScoringFunction
{
    public override Scorer CreateScorer()
    {
        return new InnerProductScorer();
    }

    private class InnerProductScorer : Scorer
    {
        public override float Score(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            var score = 0.0f;
            foreach (var p in entity.Properties)
            {
                score += classFrequencies.Properties[p];
            }
            return score;
        }
    }
}

