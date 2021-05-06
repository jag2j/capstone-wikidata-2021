
/// <summary>
/// For entity type, the frequencies for each property.
/// </summary>
public class ClassFrequencies {
    
    /// <summary>the ID of the entity representing the type</summary>
    public int Id;
    
    /// <summary>
    /// The frequencies of the properties,
    /// where the frequency of ith property is stored
    /// in index i.
    /// </summary>
    // We prefer to allocate an array to save on processing
    // at the cost of memory!
    public float[] Properties;
    
    // need the maximum number of properties
    public ClassFrequencies(int maxProp) {
        Properties = new float[maxProp + 1];
    }
}