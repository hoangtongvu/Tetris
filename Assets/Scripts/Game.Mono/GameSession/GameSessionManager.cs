using Game.Common;
using Game.Domain.GameModes;
using Game.Domain.GameSession;
using Game.Domain.PubSub.Messengers;
using Reflex.Attributes;
using SaintsField.Playa;
using ZBase.Foundation.PubSub;

namespace Game.Mono.GameSession
{
    public class GameSessionManager : SaiMonoBehaviour
    {
        [Inject, ShowInInspector] private GameSessionData SessionData;
        private ISubscription changeGameModeMessageSubscription;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            this.changeGameModeMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeGameModeMessage>(message => this.ChangeGameMode(message.GameMode));
        }

        private void OnDisable()
        {
            this.changeGameModeMessageSubscription.Dispose();
        }

        private void ChangeGameMode(GameMode gameMode)
        {
            this.SessionData.GameMode = gameMode;
        }
    }
}