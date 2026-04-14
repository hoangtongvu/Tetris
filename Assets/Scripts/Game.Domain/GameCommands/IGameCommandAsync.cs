using Cysharp.Threading.Tasks;

namespace Game.Domain.GameCommands;

public interface IGameCommandAsync
{
    UniTask Execute();
}