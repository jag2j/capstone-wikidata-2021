using System.Collections.Generic;

// the list of scores for an entity
public class Match {
    /// <summary>The proposed match</summary>
    public int MatchedEntity;
    
    /// <summary>The array of scores, one for each scorer considered.</summary>
    public List<Score> Scores;
}