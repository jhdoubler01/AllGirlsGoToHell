using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using UnityEngine;

namespace AGGtH.Runtime.Card.CardActions
{
    public class BlockAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Block;
        public override void DoAction(CardActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;

            newTarget.CharacterStats.ApplyStatus(StatusType.Block,
                (actionParameters.Value + actionParameters.SelfCharacter.CharacterStats
                    .StatusDict[StatusType.Nonchalant].StatusValue + actionParameters.SelfCharacter.CharacterStats
                    .StatusDict[StatusType.Flustered].StatusValue));
            Debug.Log("block added to " + actionParameters.SelfCharacter + ": " + actionParameters.Value + ". Total block: " + actionParameters.SelfCharacter.CharacterStats
                    .StatusDict[StatusType.Block].StatusValue);

            //if (FxManager != null)
                //FxManager.PlayFx(newTarget.transform, FxType.Block);

            //if (AudioManager != null)
                //AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}
