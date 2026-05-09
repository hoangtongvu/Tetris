using Game.Domain.GameModes;
using Game.ScriptableObjects.GameModes;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen.SelectGameMode
{
    [GenerateUIType("SelectGameModeView")]
    public partial class SelectGameModeView_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private GameModeProfilesSO gameModeProfilesSO;
        private VisualElement buttonsContainer;
        private List<SelectGameModeButton_Ctrl> buttonCtrls = new();

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

            for (int i = 0; i < GameMode_Length.Value; i++)
            {
                var mode = (GameMode)i;
                if (mode == GameMode.None) continue;

                var modeData = map[mode];
                var buttonCtrl = SharedUICtrlPoolMap.Rent(UIType.SelectGameModeButton) as SelectGameModeButton_Ctrl;

                buttonCtrl.gameObject.SetActive(true);
                buttonCtrl.transform.SetParent(transform);

                var button = buttonCtrl.UIDocument.rootVisualElement.Q<Button>();

                var title = button.Q<Label>("title__label");
                title.text = modeData.Name.ToUpper();

                var description = button.Q<Label>("description__label");
                description.text = modeData.ShortDescription.ToUpper();

                button.clicked += () => BootScreen_Ctrl.Instance.BeforeGameplayData.GameMode = mode;

                this.buttonsContainer.Add(button);
                this.buttonCtrls.Add(buttonCtrl);
            }
        }
    }
}