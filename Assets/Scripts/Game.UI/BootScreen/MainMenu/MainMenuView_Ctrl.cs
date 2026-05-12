using Game.UI.Common;
using UnityEngine;

namespace Game.UI.BootScreen.MainMenu
{
    public class MainMenuView_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private StartGameButton startGameButton = new();

        public override UIType GetUIType() => UIType.MainMenuView;

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