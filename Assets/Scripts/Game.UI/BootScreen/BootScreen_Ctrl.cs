using Game.Domain.PubSub.Messengers;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using SaintsField.Playa;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace Game.UI.BootScreen
{
    public class BootScreen_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private CommandButton backButton;

        private OverlayViewStack overlayViewStack;
        [ShowInInspector] private readonly Stack<UIType> viewHistory = new();
        private ISubscription changeToPreviousViewMessageSub;

        public BeforeGameplayData BeforeGameplayData = new();

        public OverlayViewStack OverlayViewStack => overlayViewStack;

        public override UIType GetUIType() => UIType.BootScreen;

        protected override void LoadUIElements()
        {
            this.overlayViewStack = this.uiDocument.rootVisualElement.Q<OverlayViewStack>();
            this.backButton.Initialize(this.uiDocument);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.backButton.Bind();

            this.overlayViewStack.StackPushedEvent += this.PushViewToHistory;

            this.changeToPreviousViewMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeToPreviousViewMessage>(_ => ChangeToPreviousView());
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.overlayViewStack.StackPushedEvent -= this.PushViewToHistory;
            this.changeToPreviousViewMessageSub.Dispose();
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }

        private void PushViewToHistory(BaseUICtrl uiCtrl)
        {
            this.viewHistory.Push(uiCtrl.GetUIType());
        }

        private void ChangeToPreviousView()
        {
            if (this.viewHistory.Count <= 1)
                return;

            this.viewHistory.Pop();// Pop the current view
            var prevView = this.viewHistory.Pop();
            var prevViewCtrl = (BaseUITKCtrl)SharedUICtrlPoolMap.Rent(prevView);
            prevViewCtrl.gameObject.SetActive(true);

            this.overlayViewStack.Pop();
            this.overlayViewStack.Push(prevViewCtrl);
        }
    }
}