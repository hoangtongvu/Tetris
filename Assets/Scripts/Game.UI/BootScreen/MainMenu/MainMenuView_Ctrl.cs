using Game.UI.Common;
using UnityEngine;

namespace Game.UI.BootScreen.MainMenu
{
    [GenerateUIType("MainMenuView", 2)]
    public partial class MainMenuView_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private StartGameButton startGameButton = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            this.startGameButton.Bind(this.uiDocument.rootVisualElement);
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}