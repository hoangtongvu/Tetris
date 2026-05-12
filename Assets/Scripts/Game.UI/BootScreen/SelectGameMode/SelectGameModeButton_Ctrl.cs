using Game.UI.Common;
using UnityEngine;

namespace Game.UI.BootScreen.SelectGameMode
{
    public class SelectGameModeButton_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private CommandButton button;

        public override UIType GetUIType() => UIType.SelectGameModeButton;

        protected override void LoadUIElements()
        {
            this.button.Initialize(this.uiDocument);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.button.Bind();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}