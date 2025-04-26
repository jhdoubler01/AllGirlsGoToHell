using UnityEngine;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.Card.CardActions
{
    public class AttackAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Attack;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;

            var targetCharacter = actionParameters.TargetCharacter;
            var selfCharacter = actionParameters.SelfCharacter;

            float loveLanguageModifier = LoveLanguageComparison.CompareLoveLanguages(actionParameters.CardData.CardLoveLanguageType, targetCharacter.CharacterStats.LoveLanguageType);

            var value = (actionParameters.Value * loveLanguageModifier) + selfCharacter.CharacterStats.StatusDict[StatusType.Strength].StatusValue;

            targetCharacter.CharacterStats.Damage(Mathf.RoundToInt(value));
            Debug.Log("Enemy health: " + targetCharacter.CharacterStats.CurrentHealth + "/" + targetCharacter.CharacterStats.MaxHealth);

            if (FxManager != null)
            {
                //FxManager.PlayFx(actionParameters.TargetCharacter.transform, FxType.Attack);
                //FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot, value.ToString());
            }

            //if (AudioManager != null)
                //AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}