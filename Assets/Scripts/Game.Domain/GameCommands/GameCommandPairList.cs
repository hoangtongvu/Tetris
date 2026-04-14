using System;
using System.Collections.Generic;

namespace Game.Domain.GameCommands;

[Serializable]
public class GameCommandPairList
{
    public List<GameCommandPair> Value;
}