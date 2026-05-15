using System;
using UnityEngine;

namespace Game.Domain.GameModes.GameModeSettings;

[Serializable]
public class BlitzSettings : BaseGameModeSettings
{
    public AudioClip BackgroundMusic;
    public bool ProMode;
}