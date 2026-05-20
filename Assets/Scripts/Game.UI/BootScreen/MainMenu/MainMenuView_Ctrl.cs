using Game.UI.Common;
using UnityEngine;

namespace Game.UI.BootScreen.MainMenu
{
    public class MainMenuView_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private CommandButton startGameButton;

        public override UIType GetUIType() => UIType.MainMenuView;

        protected override void LoadUIElements()
        {
            base.LoadUIElements();
            this.startGameButton.Initialize(this.uiDocument);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.startGameButton.Bind();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}