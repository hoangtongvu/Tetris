using Cysharp.Threading.Tasks;
using Game.Domain.GameCommands;
using System.Collections.Generic;

namespace Game.Domain.Utilities.GameCommands;

public static class GameCommandPairListsExtensions
{
    public static async UniTask ExecuteAsyncFrameByFrame(this List<GameCommandPairList> pairLists)
    {
        foreach (var pairList in pairLists)
        {
            foreach(var pair in pairList.Value)
            {
                pair.GameCommand?.Execute();

                if (pair.GameCommandAsync != null)
                    await pair.GameCommandAsync.Execute();
            }
            await UniTask.DelayFrame(1);
        }
    }
}