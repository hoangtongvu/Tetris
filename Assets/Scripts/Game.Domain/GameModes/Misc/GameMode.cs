using EnumLengthGenerator;

namespace Game.Domain.GameModes;

[GenerateEnumLength]
public enum GameMode : byte
{
    None = 0,

    FortyLines = 1,
    Blitz = 2,
    Zen = 3,
}