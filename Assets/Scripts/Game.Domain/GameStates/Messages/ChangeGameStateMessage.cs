using ZBase.Foundation.PubSub;

namespace Game.Domain.GameStates.Messages;

public readonly record struct ChangeGameStateMessage(GameState NewGameState) : IMessage;