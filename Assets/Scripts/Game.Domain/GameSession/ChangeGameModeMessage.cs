using Game.Domain.GameModes;
using ZBase.Foundation.PubSub;

namespace Game.Domain.GameSession;

public readonly record struct ChangeGameModeMessage(GameMode GameMode) : IMessage;