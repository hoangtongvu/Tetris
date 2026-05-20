namespace Game.Domain.GameStates;

public enum GameState : byte
{
    None = 0,
    Previous = 254,

    MainMenu = 2,

    Started = 5,
    Playing = 6,
    Over = 7,
}