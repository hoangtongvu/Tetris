using Game.Domain.GameModes;
using Game.Domain.GameModes.GameModeSettings;
using Game.ScriptableObjects.GameModes;
using Game.UI.Common;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen.GameModeViews;

public abstract class BaseGameModeView_Ctrl<TSettings> : BaseUITKCtrl
    where TSettings : BaseGameModeSettings
{
    [Inject] private GameModeProfilesSO gameModeProfiles;

    protected TSettings Settings { get; private set; }

    private Label gameModeNameLabel;
    private Label gameModeDescriptionLabel;
    [SerializeField] private CommandButton startGameButton;

    protected override void Awake()
    {
        base.Awake();
        GameObjectInjector.InjectRecursive(gameObject, Container.RootContainer);
        this.startGameButton.Initialize(this.uiDocument);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        var root = this.uiDocument.rootVisualElement;

        this.gameModeNameLabel = root.Q<Label>("game-mode-name__label");
        this.gameModeDescriptionLabel = root.Q<Label>("game-mode-description__label");
        this.startGameButton.Bind();

        var gameModeData = this.gameModeProfiles.Value[this.GetGameMode()];
        this.gameModeNameLabel.text = gameModeData.Name.ToUpperInvariant();
        this.gameModeDescriptionLabel.text = gameModeData.LongDescription.ToUpperInvariant();

        this.Settings = (TSettings)gameModeData.Settings;
        this.BindSettings();
    }

    public void InjectGameModeSettings(TSettings settings) => this.Settings = settings;

    protected virtual void BindSettings() { }

    protected void ClearOptionsContent()
    {
        var options = this.uiDocument.rootVisualElement.Q<VisualElement>("options");

        for (var i = options.childCount - 1; i >= 0; i--)
        {
            var child = options[i];

            if (child.ClassListContains("section-title"))
                continue;

            child.RemoveFromHierarchy();
        }
    }

    public abstract GameMode GetGameMode();
}