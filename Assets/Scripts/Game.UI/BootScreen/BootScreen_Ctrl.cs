using Game.UI.Common;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen
{
    [GenerateUIType("BootScreen")]
    public partial class BootScreen_Ctrl : BaseUITKCtrl
    {
        public static BootScreen_Ctrl Instance;

        private OverlayViewStack overlayViewStack;

        public OverlayViewStack OverlayViewStack => overlayViewStack;

        protected override void LoadUIElements()
        {
            this.overlayViewStack = this.uiDocument.rootVisualElement.Q<OverlayViewStack>();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}