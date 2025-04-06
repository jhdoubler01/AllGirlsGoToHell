using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Interfaces;
using TMPro;

namespace AGGtH.Runtime.Managers
{
    public class EncounterManager : MonoBehaviour
    {
        private EncounterManager() { }
        public static EncounterManager Instance { get; private set; }

        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        [SerializeField] private CardBase selectedCard;

        private LayerMask targetLayer;

        [Header("References")]
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private Transform playerPos;

        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public PlayerBase Player { get; private set; }


        public Action OnPlayerTurnStarted;
        public Action OnEnemyTurnStarted;

        public List<Transform> EnemyPosList => enemyPosList;

        public EnemyEncounter CurrentEncounter { get; private set; }
        private CombatStateType currentCombatStateType;

        public CombatStateType CurrentCombatStateType
        {
            get => currentCombatStateType;
            private set
            {
                ExecuteCombatState(value);
                currentCombatStateType = value;
            }
        }


        #endregion


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
            targetLayer = LayerMask.NameToLayer("Enemies");
        }
        void Update()
        {
            if (GameManager.PersistentGameplayData.CanSelectCards)
            {
                if (selectedCard != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        PlayCard(Input.mousePosition);
                    }
                }
            }
        }
        private void Start()
        {
            StartCombat();
        }
        public void StartCombat()
        {
            BuildEnemies();
            BuildPlayer();
            CardCollectionManager.SetGameDeck();

            CurrentCombatStateType = CombatStateType.PlayerTurn;
        }

        void PlayerTurn()
        {
            OnPlayerTurnStarted?.Invoke();
            if (Player.CharacterStats.IsStunned)
            {
                EndTurn();
                return;
            }
            GameManager.PersistentGameplayData.CurrentEnergy = GameManager.PersistentGameplayData.MaxEnergy;
            Debug.Log("Player energy: " + GameManager.PersistentGameplayData.CurrentEnergy);
            CardCollectionManager.DrawCards(GameManager.PersistentGameplayData.DrawCount);
            GameManager.PersistentGameplayData.CanSelectCards = true;
        }
        void EnemyTurn()
        {

            OnEnemyTurnStarted?.Invoke();

            CardCollectionManager.DiscardHand();

            StartCoroutine(nameof(EnemyTurnRoutine));

            GameManager.PersistentGameplayData.CanSelectCards = false;
        }

        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.PlayerTurn:
                    PlayerTurn();
                    break;
                case CombatStateType.EnemyTurn:
                    EnemyTurn();
                    break;
                case CombatStateType.EndCombat:
                    GameManager.PersistentGameplayData.CanSelectCards = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }
        #endregion

        #region Public Methods
        public void SelectedCard(CardBase targetCard, bool isSelected)
        {
            if (!isSelected)
            {
                selectedCard = null;
                return;
            }
            foreach(CardBase card in CardCollectionManager.HandController.Hand)
            {
                card.IsSelected = false;
            }
            selectedCard = targetCard;
            targetCard.IsSelected = true;
        }
        public void EndTurn()
        {
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }
        public void OnPlayerDeath(PlayerBase player)
        {
            LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);
            if (CurrentEnemiesList.Count <= 0) WinCombat();
        }
        public void DeactivateCardHighlights()
        {

        }
        public void IncreaseEnergy(int target)
        {
            GameManager.PersistentGameplayData.CurrentEnergy += target;
        }
        public void HighlightCardTarget(ActionTargetType targetType)
        {

        }
        #endregion
        #region Private Methods
        private void BuildEnemies()
        {
            CurrentEncounter = GameManager.EncounterData.GetEnemyEncounter(
                GameManager.PersistentGameplayData.CurrentStageId,
                GameManager.PersistentGameplayData.CurrentEncounterId,
                GameManager.PersistentGameplayData.IsFinalEncounter);

            var enemyList = CurrentEncounter.EnemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                CurrentEnemiesList.Add(clone);
            }
        }
        private void BuildPlayer()
        {
            var player = Instantiate(GameManager.PersistentGameplayData.Player);
            player.BuildCharacter();
            Player = player;
        }
        private void LoseCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;

            CurrentCombatStateType = CombatStateType.EndCombat;

            CardCollectionManager.DiscardHand();
            CardCollectionManager.DiscardPile.Clear();
            CardCollectionManager.DrawPile.Clear();
            CardCollectionManager.HandPile.Clear();
            CardCollectionManager.HandController.Hand.Clear();

        }
        private void WinCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
            CurrentCombatStateType = CombatStateType.EndCombat;
            GameManager.PersistentGameplayData.SetPlayerHealthData(Player.PlayerCharacterData.CharacterID, Player.CharacterStats.CurrentHealth, Player.CharacterStats.MaxHealth);
            CardCollectionManager.ClearPiles();

            if (GameManager.PersistentGameplayData.IsFinalEncounter)
            {
                //show win screen
            }
            else
            {
                Player.CharacterStats.ClearAllStatus();
                GameManager.PersistentGameplayData.CurrentEncounterId++;
                //show rewards screen
            }
        }
        private void PlayCard(Vector2 mousePos)
        {
            // Use Card
            //var mouseButtonUp = Input.GetMouseButtonUp(0);
            //if (!mouseButtonUp) return;

            //Remove highlights
            //CombatManager.DeactivateCardHighlights();
            bool backToHand = true;

            if (GameManager.PersistentGameplayData.CanUseCards && GameManager.PersistentGameplayData.CurrentEnergy >= selectedCard.CardData.EnergyCost)
            {
                RaycastHit hit;
                var mainRay = Camera.main.ScreenPointToRay(mousePos);
                var _canUse = false;
                CharacterBase selfCharacter = Player;
                CharacterBase targetCharacter = null;

                _canUse = selectedCard.CardData.UsableWithoutTarget || CheckPlayOnCharacter(mainRay, _canUse, ref selfCharacter, ref targetCharacter);

                if (_canUse)
                {
                    backToHand = false;
                    selectedCard.Use(selfCharacter, targetCharacter, CurrentEnemiesList, Player);
                }
            }

            if (backToHand) // Cannot use card / Not enough mana! Return card to hand!
            selectedCard = null;
        }
        private bool CheckPlayOnCharacter(Ray mainRay, bool _canUse, ref CharacterBase selfCharacter,
            ref CharacterBase targetCharacter)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainRay, out hit, 1000, targetLayer))
            {
                var character = hit.collider.gameObject.GetComponent<ICharacter>();

                if (character != null)
                {
                    var checkEnemy = (selectedCard.CardData.CardActionDataList[0].ActionTargetType == ActionTargetType.Enemy &&
                                      character.GetCharacterType() == CharacterType.Enemy);
                    var checkAlly = (selectedCard.CardData.CardActionDataList[0].ActionTargetType == ActionTargetType.Player &&
                                     character.GetCharacterType() == CharacterType.Player);

                    if (checkEnemy || checkAlly)
                    {
                        _canUse = true;
                        selfCharacter = Player;
                        targetCharacter = character.GetCharacterBase();
                    }
                }
            }

            return _canUse;
        }
        #endregion
        #region Routines
        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);
            foreach(var currentEnemy in CurrentEnemiesList)
            {
                yield return currentEnemy.StartCoroutine(nameof(currentEnemy.ActionRoutine));
                yield return waitDelay;
            }

            if (CurrentCombatStateType != CombatStateType.EndCombat)
                CurrentCombatStateType = CombatStateType.PlayerTurn;
        }
        #endregion
    }
}

