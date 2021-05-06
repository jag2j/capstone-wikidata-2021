using System.Collections.Generic;

// <summary>An entity and the properties it has.</summary>
public class EntityProperties {

    ///<summary>The ID of the entity</summary>
    public int Id;

    /// <summary>
    /// The list of property IDs for the entity.
    /// Duplicates should be removed.
    /// </summary>
    public ICollection<int> Properties;    
}