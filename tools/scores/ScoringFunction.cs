public abstract class ScoringFunction
{
    
    ///<summary>The state that cannot be shared across threads.</summary>
    public abstract Scorer CreateScorer();


}