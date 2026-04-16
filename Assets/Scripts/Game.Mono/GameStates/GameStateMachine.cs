using Game.Common;
using Game.Domain.GameStates;
using Game.Domain.Utilities.GameCommands;
using Game.ScriptableObjects.GameStates;
using SaintsField;
using UnityEngine;

namespace Game.Mono.GameStates
{
    public class GameStateMachine : SaiMonoBehaviour
    {
        [SerializeField] private GameState initialState;
        [SerializeField, ReadOnly] private GameState currentState;
        [SerializeField] private GameState2CommandsMapSO gameState2CommandsMapSO;

        private void Start()
        {
            this.currentState = this.initialState;

            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnEnterStateCommands.Execute();
        }

        public void ChangeGameState(GameState newState)
        {
            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnExitStateCommands.Execute();

            this.currentState = newState;

            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnEnterStateCommands.Execute();
        }
    }
}