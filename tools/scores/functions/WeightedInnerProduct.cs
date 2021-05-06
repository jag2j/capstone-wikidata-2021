using System.Collections.Generic;
using System.Linq;

///<summary>
/// Like the <see cref="InnerProductScoringFunction"/>
/// except we apply a weight when only one of the
/// inputs has a property.
///
/// <para>
/// There are four weights:
/// classMissingPropertyWeight,
/// entityMissingPropertyWeight,
/// bothPresentWeight,
/// bothMissingWeight.
/// </para>
///
/// <para>
///  The score is the sum of the score over each property.
/// </para><para>
///  If both are present, the property score is
///      classPropFrequency * bothPresentWeight.
/// </para><para>
///  If the entity has a property P but the class doesn't, we use
///      classMissingPropertyWeight.
/// </para><para>
///  If the entity is missing a property P but the class has it, we use
///      classPropFrequency * entityMissingPropertyWeight.
/// </para><para>
///  If the entity is missing in both, we use
///      bothMissingWeight.
/// </para>
/// 
///</summary>
public class WeightedInnerProductScoringFunction : ScoringFunction
{
    private readonly float BothPresentWeight;
    private readonly float EntityMissingPropertyWeight;
    private readonly float ClassMissingPropertyWeight;
    private readonly float BothMissingWeight;

    private readonly bool ForceHash;

    public WeightedInnerProductScoringFunction(
        float bothPresentWeight = 1,
        float entityMissingPropertyWeight = -1,
        float classMissingPropertyWeight = -1,
        float bothMissingWeight = 0,
        bool forceHash = false
    )
    {
        BothPresentWeight = bothPresentWeight;
        EntityMissingPropertyWeight = entityMissingPropertyWeight;
        ClassMissingPropertyWeight = classMissingPropertyWeight;
        BothMissingWeight = bothMissingWeight;
        ForceHash = forceHash;
    }

    public override Scorer CreateScorer()
    {
        return new WeightedInnerProductScorer(this);
    }

    private class WeightedInnerProductScorer : Scorer
    {
        private WeightedInnerProductScoringFunction F;

        public WeightedInnerProductScorer(WeightedInnerProductScoringFunction function)
        {
            F = function;
        }

        public override float Score(EntityProperties entity, ClassFrequencies classFrequencies)
        {
            var score = 0.0f;
            var eprops = entity.Properties;

            for(var i=0; i < classFrequencies.Properties.Length; i++) {
                var inEntity = eprops.Contains(i);
                var inClass = classFrequencies.Properties[i] > 0;
                
                if(inEntity) {
                    if(inClass) {
                        score += F.BothPresentWeight * classFrequencies.Properties[i];
                    }
                    else {
                        score += F.ClassMissingPropertyWeight;
                    }
                }
                else {
                    if(inClass) {
                        score += F.EntityMissingPropertyWeight * classFrequencies.Properties[i];
                    }
                    else {
                        score += F.BothMissingWeight;
                    }
                }
            }

            // handle any entity properties not even known to the class frequencies
            var unknownCnt = entity.Properties.Where(p => p >= classFrequencies.Properties.Length).Count();
            score += unknownCnt * F.ClassMissingPropertyWeight;

            return score;
        }
    }
}

