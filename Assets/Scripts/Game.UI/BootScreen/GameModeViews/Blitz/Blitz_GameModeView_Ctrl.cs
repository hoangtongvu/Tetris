using Game.Domain.GameModes;
using Game.Domain.GameModes.GameModeSettings;
using Game.UI.Common;

namespace Game.UI.BootScreen.GameModeViews
{
    public class Blitz_GameModeView_Ctrl : BaseGameModeView_Ctrl<BlitzSettings>
    {
        public override GameMode GetGameMode() => GameMode.Blitz;

        public override UIType GetUIType() => UIType.Blitz_GameModeView;

        public override void OnRent()
        {
        }

        public override void OnReturn()
        {
        }
    }
}