using Game.UI.Common;
using ZBase.Foundation.PubSub;

namespace Game.UI;

public readonly record struct ChangeGameScreenMessage(UIType NewScreen) : IMessage;