using AYellowpaper.SerializedCollections;
using Game.Domain.GameCommands;
using Game.UI.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.UI
{
    [Serializable]
    public class Commands
    {
        public List<GameCommandPairList> OnEnterStateCommands;
        public List<GameCommandPairList> OnExitStateCommands;
    }

    [CreateAssetMenu(fileName = "GameScreen2CommandsMapSO", menuName = "SO/GameScreen2CommandsMapSO")]
    public class GameScreen2CommandsMapSO : ScriptableObject
    {
        public SerializedDictionary<UIType, Commands> Value;
    }
}