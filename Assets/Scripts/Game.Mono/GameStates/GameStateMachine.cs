using Game.Common;
using Game.Domain.GameStates;
using Game.Domain.GameStates.Messages;
using Game.Domain.PubSub.Messengers;
using Game.Domain.Utilities.GameCommands;
using Game.ScriptableObjects.GameStates;
using SaintsField;
using SaintsField.Playa;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono.GameStates
{
    public class GameStateMachine : SaiMonoBehaviour
    {
        [SerializeField] private GameState initialState;
        [ShowInInspector] private readonly Stack<GameState> stateHistory = new();
        [SerializeField, ReadOnly] private GameState currentState;
        [SerializeField] private GameState2CommandsMapSO gameState2CommandsMapSO;
        private ISubscription changeGameStateMessageSubscription;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            this.currentState = this.initialState;

            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnEnterStateCommands.Execute();
        }

        private void OnEnable()
        {
            this.changeGameStateMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeGameStateMessage>(message => this.ChangeGameState(message.NewGameState));
        }

        private void OnDisable()
        {
            this.changeGameStateMessageSubscription.Dispose();
        }

        public void ChangeGameState(GameState newState)
        {
            if (newState == GameState.Previous)
            {
                if (this.stateHistory.Count == 0)
                    return;

                newState = this.stateHistory.Pop();
            }
            else
            {
                this.stateHistory.Push(this.currentState);
            }

            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnExitStateCommands.Execute();

            this.currentState = newState;

            this.gameState2CommandsMapSO.Value[this.currentState]
                .OnEnterStateCommands.Execute();
        }
    }
}