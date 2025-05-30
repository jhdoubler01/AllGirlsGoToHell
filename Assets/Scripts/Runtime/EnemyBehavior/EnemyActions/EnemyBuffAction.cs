using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.EnemyBehavior.EnemyActions
{
    public class EnemyBuffAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.ApplyBuff;

        public override void DoAction(EnemyActionParameters actionParameters)
        {

            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;

            newTarget.CharacterStats.ApplyStatus(actionParameters.StatusType,
                (actionParameters.Value));


            //if (FxManager != null)
            //    FxManager.PlayFx(newTarget.transform, FxType.Block);

            //if (AudioManager != null)
            //    AudioManager.PlayOneShot(AudioActionType.Block);
        }
    }
}
