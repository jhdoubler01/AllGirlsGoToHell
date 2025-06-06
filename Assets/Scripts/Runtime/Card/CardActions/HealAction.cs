using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using UnityEngine;

namespace AGGtH.Runtime.Card.CardActions
{
    public class HealAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Heal;

        public override void DoAction(CardActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;

            newTarget.CharacterStats.Heal(Mathf.RoundToInt(actionParameters.Value));

            //if (FxManager != null)
                //FxManager.PlayFx(newTarget.transform, FxType.Heal);

            //if (AudioManager != null)
                //AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}