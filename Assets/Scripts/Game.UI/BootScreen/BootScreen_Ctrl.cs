using Game.Domain.PubSub.Messengers;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using SaintsField.Playa;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using ZBase.Foundation.PubSub;

namespace Game.UI.BootScreen
{
    [RequireComponent(typeof(VideoPlayer))]
    public class BootScreen_Ctrl : BaseUITKCtrl
    {
        [SerializeField] private CommandButton backButton;

        [SerializeField] private VideoPlayer bgVideoPlayer;
        [SerializeField] private RenderTexture bgRenderTexture;

        private OverlayViewStack overlayViewStack;
        [ShowInInspector] private readonly Stack<UIType> viewHistory = new();
        private ISubscription changeToPreviousViewMessageSub;

        public OverlayViewStack OverlayViewStack => overlayViewStack;

        public override UIType GetUIType() => UIType.BootScreen;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.bgVideoPlayer);
        }

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

            this.RunBackgroundVideo();
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

            bool inMainMenuView = this.viewHistory.Count <= 1;
            var root = this.uiDocument.rootVisualElement;
            var header = root.Q<VisualElement>("header");
            var body = root.Q<VisualElement>("body");
            var footer = root.Q<VisualElement>("footer");

            if (inMainMenuView)
            {
                this.backButton.Button.RemoveFromClassList("visible");
                this.backButton.Button.AddToClassList("hidden");

                header.RemoveFromClassList("visible");
                header.AddToClassList("hidden");

                body.RemoveFromClassList("not-main-menu");

                footer.RemoveFromClassList("visible");
                footer.AddToClassList("hidden");
            }
            else
            {
                this.backButton.Button.RemoveFromClassList("hidden");
                this.backButton.Button.AddToClassList("visible");

                header.RemoveFromClassList("hidden");
                header.AddToClassList("visible");

                body.AddToClassList("not-main-menu");

                footer.RemoveFromClassList("hidden");
                footer.AddToClassList("visible");
            }
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

        private void RunBackgroundVideo()
        {
            var root = uiDocument.rootVisualElement;

            var videoElement = root.Q<VisualElement>("bg-video");

            videoElement.style.backgroundImage =
                Background.FromRenderTexture(this.bgRenderTexture);

            this.bgVideoPlayer.targetTexture = this.bgRenderTexture;
            this.bgVideoPlayer.Play();
        }
    }
}