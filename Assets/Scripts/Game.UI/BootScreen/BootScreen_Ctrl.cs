using Game.UI.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen
{
    [GenerateUIType("BootScreen")]
    public partial class BootScreen_Ctrl : BaseUITKCtrl
    {
        public static BootScreen_Ctrl Instance;

        [SerializeField] private CommandButton backButton;
        private OverlayViewStack overlayViewStack;
        public BeforeGameplayData BeforeGameplayData = new();

        public OverlayViewStack OverlayViewStack => overlayViewStack;

        protected override void LoadUIElements()
        {
            this.overlayViewStack = this.uiDocument.rootVisualElement.Q<OverlayViewStack>();
            this.backButton.Initialize(this.uiDocument);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.backButton.Bind();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}