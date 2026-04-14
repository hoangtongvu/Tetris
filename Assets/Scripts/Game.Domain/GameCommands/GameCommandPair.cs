using System;
using UnityEngine;

namespace Game.Domain.GameCommands;

[Serializable]
public class GameCommandPair
{
    [SerializeReference, SubclassSelector]
    public IGameCommand GameCommand;

    [SerializeReference, SubclassSelector]
    public IGameCommandAsync GameCommandAsync;
}