using Game.Domain.GameCommands;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.GameStates
{
    [CreateAssetMenu(fileName = "GameStateCommandsSO", menuName = "SO/GameStateCommandsSO")]
    public class GameStateCommandsSO : ScriptableObject
    {
        public List<GameCommandPairList> OnEnterStateCommands;
        public List<GameCommandPairList> OnExitStateCommands;
    }
}