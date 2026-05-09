namespace Game.Domain.GameStates;

public enum GameState : byte
{
    None = 0,
    Previous = 254,

    Paused = 1,

    MainMenu = 2,
    SelectGameMode = 3,
    ConfigGameMode = 4,

    Started = 5,
    Playing = 6,
    Over = 7,
}