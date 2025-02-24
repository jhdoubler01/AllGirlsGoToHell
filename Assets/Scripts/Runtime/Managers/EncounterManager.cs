using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;

namespace AGGtH.Runtime.Managers
{
    public class EncounterManager : MonoBehaviour
    {
        private EncounterManager() { }
        public static EncounterManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private List<Transform> enemyPosList;

        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } 
        public PlayerBase Player { get; private set; }

        public Action OnPlayerTurnStarted;
        public Action OnEnemyTurnStarted;

        public List<Transform> EnemyPosList => enemyPosList;

        //public EnemyEncounter CurrentEncounter { get; private set; }
        #endregion
        private CombatStateType _currentCombatStateType;

        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                _currentCombatStateType = value;
                switch (value)
                {
                    case CombatStateType.PrepareCombat:
                        break;
                    case CombatStateType.PlayerTurn:
                        OnPlayerTurnStarted?.Invoke();
                        break;
                    case CombatStateType.EnemyTurn:
                        OnEnemyTurnStarted?.Invoke();
                        break;
                    default:
                        break;
                }
            }
        }

        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }
        private void Start()
        {
            StartCombat();
        }
        public void StartCombat()
        {
            CurrentEnemiesList = new List<EnemyBase>();
            Player = GameManager.Instance.Player;
            Player.OnPlayerTurnEnded += OnPlayerTurnEnded;
            Player.OnPlayerCardPlayed += OnPlayerCardPlayed;
            Player.OnPlayerCardDiscarded += OnPlayerCardDiscarded;

            foreach (var enemy in GameManager.Instance.CurrentEncounter.EnemyList)
            {
                var enemyClone = Instantiate(enemy, EnemyPosList[GameManager.Instance.CurrentEncounter.EnemyList.IndexOf(enemy)]);
                CurrentEnemiesList.Add(enemyClone);
            }

            CurrentCombatStateType = CombatStateType.PlayerTurn;
        }
        #endregion
    }
}

