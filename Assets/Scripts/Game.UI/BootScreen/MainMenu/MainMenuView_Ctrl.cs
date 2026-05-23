using Game.UI.Common;
using UnityEngine;
using UnityEngine.UIElements;

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

            var root = this.uiDocument.rootVisualElement;
            var headline = root.Q<Label>("main-menu-headline");
            var buttons = root.Query<Button>(className: "menuRegularButton").ToList();

            // Animate headline
            headline.AddToClassList("hidden");
            headline.schedule.Execute(() => headline.RemoveFromClassList("hidden")).ExecuteLater(10);

            // Animate buttons
            int index = 0;
            const long timeBetweenMs = 65;

            buttons.ForEach(btn =>
            {
                btn.AddToClassList("hidden");
                btn.schedule.Execute(() => btn.RemoveFromClassList("hidden")).ExecuteLater(10 + index * timeBetweenMs);
                index++;
            });
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}