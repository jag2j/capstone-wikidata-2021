using System.Collections.Generic;
using System.Linq;

public abstract class Scorer {
    public string Name => this.GetType().Name;
    public abstract float Score(EntityProperties entity, ClassFrequencies classFrequencies);
}
