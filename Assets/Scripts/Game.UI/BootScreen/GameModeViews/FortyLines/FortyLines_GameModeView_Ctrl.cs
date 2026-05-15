using Game.Domain.GameModes;
using Game.Domain.GameModes.GameModeSettings;
using Game.UI.Common;

namespace Game.UI.BootScreen.GameModeViews
{
    public class FortyLines_GameModeView_Ctrl : BaseGameModeView_Ctrl<FortyLinesSettings>
    {
        public override GameMode GetGameMode() => GameMode.FortyLines;

        public override UIType GetUIType() => UIType.FortyLines_GameModeView;

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}