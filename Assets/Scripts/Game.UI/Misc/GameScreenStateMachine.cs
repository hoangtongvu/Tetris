using Game.Common;
using Game.Domain.PubSub.Messengers;
using Game.Domain.Utilities.GameCommands;
using Game.ScriptableObjects.UI;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using Reflex.Core;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.UI
{
    public class GameScreenStateMachine : SaiMonoBehaviour, IInstaller
    {
        public static GameScreenStateMachine Instance; // Temp solution for now

        [SerializeField] private UIType initialScreen;
        [SerializeField] private GameScreen2CommandsMapSO gameScreen2CommandsMapSO; // TODO: Inject later // TODO: Change to UIType2CommandsMapSO
        [ShowInInspector, ReadOnly] private UIType currentScreen;
        [ShowInInspector, ReadOnly] private BaseUICtrl currentScreenCtrl;
        private ISubscription changeGameScreenMessageSubscription;

        public UIType CurrentScreen => currentScreen;
        public BaseUICtrl CurrentScreenCtrl => currentScreenCtrl;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this);
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            this.currentScreen = this.initialScreen;

            this.currentScreenCtrl = SharedUICtrlPoolMap.Rent(this.currentScreen);
            this.currentScreenCtrl.gameObject.SetActive(true);

            this.gameScreen2CommandsMapSO.Value[this.currentScreen]
                .OnEnterStateCommands.Execute();
        }

        private void OnEnable()
        {
            this.changeGameScreenMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeGameScreenMessage>(message => this.ChangeGameScreen(message.NewScreen));
        }

        private void OnDisable()
        {
            this.changeGameScreenMessageSubscription.Dispose();
        }

        public void ChangeGameScreen(UIType newScreen)
        {
            // Handle old screen
            SharedUICtrlPoolMap.Return(this.currentScreenCtrl);

            this.gameScreen2CommandsMapSO.Value[this.currentScreen]
                .OnExitStateCommands.Execute();

            // Handle new screen
            this.currentScreen = newScreen;

            this.currentScreenCtrl = SharedUICtrlPoolMap.Rent(this.currentScreen);
            this.currentScreenCtrl.gameObject.SetActive(true);

            this.gameScreen2CommandsMapSO.Value[this.currentScreen]
                .OnEnterStateCommands.Execute();
        }
    }
}
