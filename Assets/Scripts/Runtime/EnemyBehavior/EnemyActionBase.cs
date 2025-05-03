using UnityEngine;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.EnemyBehavior
{
    public abstract class EnemyActionBase
    {
        protected EnemyActionBase() { }
        public abstract EnemyActionType ActionType { get; }
        public abstract void DoAction(EnemyActionParameters actionParameters);

        protected FxManager FxManager => FxManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
    }
    public class EnemyActionParameters
    {
        public readonly float Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly StatusType StatusType;
        public readonly string Dialogue;

        public EnemyActionParameters(float value, CharacterBase target, CharacterBase self, StatusType statusType, string dialogue = "")
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            StatusType = statusType;
            Dialogue = dialogue;
        }
    }
}
