using System;
using System.Collections.Generic;
using Game.Domain.GameModes;
using Game.Domain.GameModes.GameModeSettings;
using Game.UI.Common;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen.GameModeViews
{
    public class Blitz_GameModeView_Ctrl : BaseGameModeView_Ctrl<BlitzSettings>
    {
        // TODO: Replace with real track names when music assets are added.
        private static readonly List<string> MusicChoices = new()
        {
            "DEFAULT",
            "TRACK 1",
            "TRACK 2",
        };

        public override GameMode GetGameMode() => GameMode.Blitz;

        public override UIType GetUIType() => UIType.Blitz_GameModeView;

        protected override void BindSettings()
        {
            this.ClearOptionsContent();

            var options = this.uiDocument.rootVisualElement.Q<VisualElement>("options");

            var proModeToggle = new Toggle("PRO MODE");
            proModeToggle.AddToClassList("option-toggle");
            proModeToggle.value = this.Settings.ProMode;
            proModeToggle.RegisterValueChangedCallback(evt => this.Settings.ProMode = evt.newValue);
            options.Add(proModeToggle);

            var musicIndex = 0;

            if (this.Settings.BackgroundMusic != null)
            {
                musicIndex = MusicChoices.FindIndex(choice =>
                    string.Equals(choice, this.Settings.BackgroundMusic.name, StringComparison.OrdinalIgnoreCase));

                if (musicIndex < 0)
                    musicIndex = 0;
            }

            var musicDropdown = new DropdownField("MUSIC", MusicChoices, musicIndex);
            musicDropdown.AddToClassList("option-dropdown");
            musicDropdown.RegisterValueChangedCallback(_ =>
            {
                // TODO: Map selection to AudioClip when music assets are added.
            });
            options.Add(musicDropdown);
        }

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}