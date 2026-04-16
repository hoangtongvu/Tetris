using Game.UI.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.MainMenu
{
    [GenerateUIType("MainMenuView")]
    public partial class MainMenuView_Ctrl : BaseUITKCtrl
    {
        public Label Label;
        [SerializeField] private StartGameButton startGameButton = new();

        protected override void LoadUIElements()
        {
            this.Label = this.uiDocument.rootVisualElement.Q<Label>();
        }

        protected override void Awake()
        {
            base.Awake();
            this.startGameButton.Initialize(this.uiDocument.rootVisualElement);
            this.Label.text = "Test";
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}