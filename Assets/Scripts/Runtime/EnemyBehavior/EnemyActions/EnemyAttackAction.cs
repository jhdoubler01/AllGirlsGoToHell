using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.EnemyBehavior.EnemyActions
{
    public class EnemyAttackAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Attack;

        public override void DoAction(EnemyActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;
            var value = Mathf.RoundToInt(actionParameters.Value +
                                         actionParameters.SelfCharacter.CharacterStats.StatusDict[StatusType.Strength]
                                             .StatusValue);
            actionParameters.TargetCharacter.CharacterStats.Damage(value);
            UIManager.SetDialogueBoxText(actionParameters.Dialogue);
            //if (FxManager != null)
            //{
            //    FxManager.PlayFx(actionParameters.TargetCharacter.transform, FxType.Attack);
            //    FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot, value.ToString());
            //}

            //if (AudioManager != null)
            //    AudioManager.PlayOneShot(AudioActionType.Attack);
        }
    }
}
