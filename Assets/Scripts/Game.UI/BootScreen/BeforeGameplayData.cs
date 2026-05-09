using Game.Domain.GameModes;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class BeforeGameplayData
{
    public GameMode GameMode;
}