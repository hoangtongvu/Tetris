using Game.Domain.GameCommands;
using Game.Domain.Utilities.GameCommands;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.UI.BootScreen;

[Serializable]
public class CommandButton
{
    public UIDocument RootUIDocument;
    public Button Button;

    public string ButtonName;
    public string ButtonClassName;

    public List<GameCommandPairList> CommandPairLists;

    public void Initialize(UIDocument uiDoc)
    {
        this.RootUIDocument = uiDoc;
    }

    public void Bind()
    {
        var root = this.RootUIDocument.rootVisualElement;

        this.Button = root.Q<Button>(
            name: string.IsNullOrEmpty(ButtonName) ? null : ButtonName,
            className: string.IsNullOrEmpty(ButtonClassName) ? null : ButtonClassName);

        this.Button.clicked += () => this.CommandPairLists.Execute();
    }
}