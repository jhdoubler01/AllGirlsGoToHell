
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using UnityEngine;
using UnityEditor;

namespace AGGtH.Editor
{   
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Card))]

    public class CardEditor : UnityEditor.Editor
    {
        private Card card;
        private SerializedProperty d_damageAmt;
        private void OnEnable()
        {
            card = target as Card;
            d_damageAmt = serializedObject.FindProperty("damageAmt");
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            card.cardName = EditorGUILayout.TextField("Card Name",  card.cardName);
            card.cardLoveLanguageType = (CardLoveLanguageType)EditorGUILayout.EnumPopup("Love Language Type",card.cardLoveLanguageType);
            card.cardActionType = (CardActionType)EditorGUILayout.EnumPopup("Action Type", card.cardActionType);
            card.actionTargetType = (ActionTargetType)EditorGUILayout.EnumPopup("Target Type", card.actionTargetType);

            if(card.cardActionType is CardActionType.Attack)
            {
                card.damageAmt = EditorGUILayout.IntField("Damage Amount",  card.damageAmt);
            }
            else
            {
                card.damageAmt = 0;
            }

        }
    }
}

