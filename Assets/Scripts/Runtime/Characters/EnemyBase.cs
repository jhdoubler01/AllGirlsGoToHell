using UnityEngine;
using AGGtH.Runtime.Data.Characters;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.EnemyBehavior;
using System.Collections;
using AGGtH.Runtime.UI;
using AGGtH.Runtime.Interfaces;
using DG.Tweening;

namespace AGGtH.Runtime.Characters
{
    public class EnemyBase : CharacterBase,IEnemy
    {
        [Header("Enemy Base References")]
        [SerializeField] protected EnemyCharacterData enemyCharacterData;
        [SerializeField] protected EnemyCanvas enemyCanvas;

        protected EnemyAbilityData NextAbility;

        //TEMPORARY FOR TRAILER PURPOSES
        [SerializeField] private SpriteRenderer mySpriteRend;
        [SerializeField] private Sprite hitSprite;
        [SerializeField] private Sprite talkingSprite;
        [SerializeField] private Sprite normalSprite;


        private float actionDelay = 2f;

        public EnemyCharacterData EnemyCharacterData => enemyCharacterData;
        public EnemyCanvas EnemyCanvas => enemyCanvas;

        #region Setup
        public override void BuildCharacter()
        {
            SetHealthBar(EnemyCanvas.HealthBar);
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(EnemyCharacterData.MaxHealth, EnemyCharacterData.LoveLanguageType, EnemyCanvas);
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.OnHealthChanged += ChangeHealthBarFill;
            CharacterStats.OnTakeDamageAction += OnHit;

            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);


            EncounterManager.OnPlayerTurnStarted += ShowNextAbility;
            EncounterManager.OnEnemyTurnStarted += CharacterStats.TriggerAllStatus;

            base.BuildCharacter();

        }
        protected override void OnDeath()
        {
            base.OnDeath();
            EncounterManager.OnPlayerTurnStarted -= ShowNextAbility;
            EncounterManager.OnEnemyTurnStarted -= CharacterStats.TriggerAllStatus;

            EncounterManager.OnEnemyDeath(this);
            //AudioManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
        }
        #endregion
        #region Private Methods

        private int _usedAbilityCount;
        private void ShowNextAbility()
        {
            NextAbility = EnemyCharacterData.GetAbility(_usedAbilityCount);
            EnemyCanvas.IntentImage.sprite = NextAbility.Intention.IntentionSprite;

            if (NextAbility.HideActionValue)
            {
                //EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            else
            {
                //EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                //EnemyCanvas.NextActionValueText.text = NextAbility.ActionList[0].ActionValue.ToString();
            }

            _usedAbilityCount++;
            EnemyCanvas.IntentImage.gameObject.SetActive(true);
        }
        #endregion
        #region Action Routines
        public virtual IEnumerator ActionRoutine()
        {
            if (CharacterStats.IsStunned)
                yield break;

            UIManager.SetDialogueBoxText(EnemyCharacterData.EnemyAbilityList[0].Dialogue, EnemyCharacterData.DialogueColor);
            //EnemyCanvas.IntentImage.gameObject.SetActive(false);
            if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack || NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                yield return StartCoroutine(AttackRoutine(NextAbility));
            }
            else
            {
                yield return StartCoroutine(BuffRoutine(NextAbility));
            }
            ResetSprite();

        }
        protected virtual IEnumerator AttackRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            if (EncounterManager == null) yield break;

            var target = EncounterManager.Player;

            //var startPos = transform.position;
            //var endPos = target.transform.position;

            //var startRot = transform.localRotation;
            //var endRot = Quaternion.Euler(60, 0, 60);

            //yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
            OnTalk();
            yield return new WaitForSeconds(actionDelay);
            targetAbility.ActionList.ForEach(x => EnemyActionProcessor.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue, target, this, x.StatusType)));
            //yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }

        protected virtual IEnumerator BuffRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            if (EncounterManager == null) yield break;

            var target = EncounterManager.CurrentEnemiesList.RandomItem();

            //var startPos = transform.position;
            //var endPos = startPos + new Vector3(0, 0.2f, 0);

            //var startRot = transform.localRotation;
            //var endRot = transform.localRotation;

            //yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
            yield return new WaitForSeconds(actionDelay);

            targetAbility.ActionList.ForEach(x => EnemyActionProcessor.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue, target, this, x.StatusType)));
            //yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }
        #endregion

        #region Other Routines
        private void OnHit()
        {
            mySpriteRend.sprite = hitSprite;
        }
        private void OnTalk()
        {
            mySpriteRend.sprite = talkingSprite;
        }
        private void ResetSprite()
        {
            mySpriteRend.sprite = normalSprite;
        }
        private IEnumerator MoveToTargetRoutine(WaitForEndOfFrame waitFrame, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float speed)
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime * speed;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                if (timer >= 1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }

        #endregion
    }
}
