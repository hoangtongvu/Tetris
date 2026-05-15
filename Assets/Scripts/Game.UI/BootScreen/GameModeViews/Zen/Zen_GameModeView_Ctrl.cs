using Game.Domain.GameModes;
using Game.Domain.GameModes.GameModeSettings;
using Game.UI.Common;

namespace Game.UI.BootScreen.GameModeViews
{
    public class Zen_GameModeView_Ctrl : BaseGameModeView_Ctrl<ZenSettings>
    {
        public override GameMode GetGameMode() => GameMode.Zen;

        public override UIType GetUIType() => UIType.Zen_GameModeView;

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}