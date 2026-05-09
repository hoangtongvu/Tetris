using System;
using UnityEngine;

namespace Game.Domain.GameModes;

[Serializable]
public class GameModeData
{
    public string Name;
    [TextArea] public string ShortDescription;
    [TextArea] public string LongDescription;
}