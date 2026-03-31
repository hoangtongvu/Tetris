using System;

namespace Game.Domain;

[Serializable]
public struct CellData
{
    public bool IsValid;
    // Coordination grid xy (Optional)
    // Block id
    // Presenter ref (Update on block move / block changed)
}