using System;
using UnityEngine;

namespace Game.Domain.GameModes.GameModeSettings;

[Serializable]
public class FortyLinesSettings : BaseGameModeSettings
{
    public AudioClip BackgroundMusic;
    public bool ProMode;
}