using Game.Domain.GameModes.GameModeSettings;
using Game.UI.Common;
using System;
using UnityEngine;

namespace Game.Domain.GameModes;

[Serializable]
public class GameModeData
{
    public string Name;
    [TextArea] public string ShortDescription;
    [TextArea] public string LongDescription;

    public UIType ViewUIType;

    [SerializeReference, SubclassSelector]
    public BaseGameModeSettings Settings;
}