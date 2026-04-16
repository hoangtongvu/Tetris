using Game.Domain.GameCommands;
using Game.Domain.GameStates;
using SaintsField;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.GameStates
{
    [Serializable]
    public class Commands
    {
        public List<GameCommandPairList> OnEnterStateCommands;
        public List<GameCommandPairList> OnExitStateCommands;
    }

    [CreateAssetMenu(fileName = "GameState2CommandsMapSO", menuName = "SO/GameState2CommandsMapSO")]
    public class GameState2CommandsMapSO : ScriptableObject
    {
        public SaintsDictionary<GameState, Commands> Value;
    }
}