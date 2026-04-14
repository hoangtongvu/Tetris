using Game.Domain.GameCommands;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.GameCommands
{
    [CreateAssetMenu(fileName = "GameCommandPairListsSO", menuName = "SO/GameCommandPairListsSO")]
    public class GameCommandPairListsSO : ScriptableObject
    {
        public List<GameCommandPairList> PairLists;
    }
}