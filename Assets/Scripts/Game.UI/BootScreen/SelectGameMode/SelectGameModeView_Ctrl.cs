using Game.Domain.GameModes;
using Game.Domain.GameSession;
using Game.Domain.PubSub.Messengers;
using Game.ScriptableObjects.GameModes;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace Game.UI.BootScreen.SelectGameMode
{
    public partial class SelectGameModeView_Ctrl : BaseUITKCtrl
    {
        [Inject] private GameModeProfilesSO gameModeProfilesSO;
        private VisualElement buttonsContainer;
        private List<SelectGameModeButton_Ctrl> buttonCtrls = new();

        public override UIType GetUIType() => UIType.SelectGameModeView;

        protected override void Awake()
        {
            base.Awake();
            GameObjectInjector.InjectRecursive(gameObject, Container.RootContainer);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.AddButtons();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }

        private void AddButtons()
        {
            this.buttonsContainer = this.uiDocument.rootVisualElement.Q<VisualElement>("body-panel");
            this.buttonsContainer.Clear();

            foreach (var button in this.buttonCtrls)
            {
                button.ReturnSelfToPool();
            }
            this.buttonCtrls.Clear();

            var map = this.gameModeProfilesSO.Value;
            const long delayBetweenMs = 120;

            for (int i = 0; i < GameMode_Length.Value; i++)
            {
                var mode = (GameMode)i;
                if (mode == GameMode.None) continue;

                var modeData = map[mode];
                var buttonCtrl = SharedUICtrlPoolMap.Rent(UIType.SelectGameModeButton) as SelectGameModeButton_Ctrl;

                buttonCtrl.gameObject.SetActive(true);
                buttonCtrl.transform.SetParent(transform);

                var button = buttonCtrl.UIDocument.rootVisualElement.Q<Button>();

                button.AddToClassList("hidden");
                button.schedule.Execute(() => button.RemoveFromClassList("hidden")).ExecuteLater(10 + i * delayBetweenMs);

                var title = button.Q<Label>("title__label");
                title.text = modeData.Name.ToUpper();

                var description = button.Q<Label>("description__label");
                description.text = modeData.ShortDescription.ToUpper();

                button.clicked += () =>
                {
                    GameplayMessenger.MessagePublisher
                        .Publish(new ChangeGameModeMessage(mode));

                    new PopViewFromBootScreenCommand().Execute();
                    new PushViewToBootScreenCommand
                    {
                        UIAssetId = new() { Value = new() { Type = modeData.ViewUIType } }
                    }.Execute();
                };

                this.buttonsContainer.Add(button);
                this.buttonCtrls.Add(buttonCtrl);
            }
        }
    }
}