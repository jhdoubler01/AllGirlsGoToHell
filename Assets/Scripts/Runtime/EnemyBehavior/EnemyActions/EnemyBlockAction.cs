using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.EnemyBehavior.EnemyActions
{
    public class EnemyBlockAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Block;

        public override void DoAction(EnemyActionParameters actionParameters)
        {

            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;

            newTarget.CharacterStats.ApplyStatus(StatusType.Block,
                Mathf.RoundToInt(actionParameters.Value + actionParameters.SelfCharacter.CharacterStats
                    .StatusDict[StatusType.Dexterity].StatusValue));

            //if (FxManager != null)
            //    FxManager.PlayFx(newTarget.transform, FxType.Block);

            //if (AudioManager != null)
            //    AudioManager.PlayOneShot(AudioActionType.Block);
        }
    }
}
