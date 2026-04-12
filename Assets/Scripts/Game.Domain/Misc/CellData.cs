using System;
using UnityEngine;

namespace Game.Domain;

[Serializable]
public struct CellData
{
    public bool IsValid;
    public Color Color;
    // Coordination grid xy (Optional)
    // Block id
    // Presenter ref (Update on block move / block changed)
}