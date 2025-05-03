using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using UnityEngine;

namespace AGGtH.Runtime.Card.CardActions
{
    public class ApplyBuffAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.ApplyBuff;
        public override void DoAction(CardActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;

            newTarget.CharacterStats.ApplyStatus(actionParameters.StatusType, actionParameters.Value);

            //Debug.Log("block added to " + actionParameters.SelfCharacter + ": " + actionParameters.Value + ". Total block: " + actionParameters.SelfCharacter.CharacterStats
            //        .StatusDict[StatusType.Block].StatusValue);

            //if (FxManager != null)
            //FxManager.PlayFx(newTarget.transform, FxType.Block);

            //if (AudioManager != null)
            //AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}