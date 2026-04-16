
using ZBase.Foundation.PubSub;

namespace Game.Common.UnityComponentRegistering;

public static class UnityComponentRegisterMessenger
{
    private static readonly Messenger messenger = new();

    public static Messenger Instance => messenger;
    public static AnonPublisher AnonPublisher => messenger.AnonPublisher;
    public static AnonSubscriber AnonSubscriber => messenger.AnonSubscriber;
    public static MessagePublisher MessagePublisher => messenger.MessagePublisher;
    public static MessageSubscriber MessageSubscriber => messenger.MessageSubscriber;
}