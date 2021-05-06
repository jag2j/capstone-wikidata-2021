///<summary>
/// An edge that connects two entities, 
/// e.g., Q42 -> P31 -> Q5 (Douglas Adams is an instance of human)
/// </summary>
// NOTE: We only support entity IDs to the maximum 32-bit signed integer value
//          = ~ 2.1 million
public class Edge {
    ///<summary>The subject entity ID (without the Q)</summary>
    public int S;
    ///<summary>The property</summary>
    public int P;
    ///<summary>The object</summary>
    public int O;
    public Edge(int s, int p, int o) {
        S = s; P = p; O = o;
    }
}
