
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

        private SerializedProperty m_cardName;

        //enums
        private SerializedProperty m_cardLoveLanguageType;
        private SerializedProperty m_cardActionType;
        private SerializedProperty m_actionTargetType;
        private SerializedProperty m_buffType;
        private SerializedProperty m_debuffType;

        //ints
        private SerializedProperty m_damageAmt;
        private SerializedProperty m_healAmt;
        private SerializedProperty m_blockAmt;
        private SerializedProperty m_drawCardAmt;
        private SerializedProperty m_energyGainAmt;


        private void OnEnable()
        {
            card = target as Card;
            m_cardName = serializedObject.FindProperty("cardName");

            m_cardLoveLanguageType = serializedObject.FindProperty("cardLoveLanguageType");
            m_cardActionType = serializedObject.FindProperty("cardActionType");
            m_actionTargetType = serializedObject.FindProperty("actionTargetType");
            m_buffType = serializedObject.FindProperty("buffType");
            m_debuffType = serializedObject.FindProperty("debuffType");

            m_damageAmt = serializedObject.FindProperty("damageAmt");
            m_healAmt = serializedObject.FindProperty("healAmt");
            m_blockAmt = serializedObject.FindProperty("blockAmt");
            m_drawCardAmt = serializedObject.FindProperty("drawCardAmt");
            m_energyGainAmt = serializedObject.FindProperty("energyGainAmt");

        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            m_cardName.stringValue = (string)EditorGUILayout.TextField("Card Name", m_cardName.stringValue);

            m_cardLoveLanguageType.enumValueIndex = (int)(CardLoveLanguageType)EditorGUILayout.EnumPopup("Love Language Type", (CardLoveLanguageType)m_cardLoveLanguageType.enumValueIndex);
            m_cardActionType.enumValueIndex = (int)(CardActionType)EditorGUILayout.EnumPopup("Action Type", (CardActionType)m_cardActionType.enumValueIndex);
            m_actionTargetType.enumValueIndex = (int)(ActionTargetType)EditorGUILayout.EnumPopup("Target Type", (ActionTargetType)m_actionTargetType.enumValueIndex);
            m_buffType.enumValueIndex = (int)(BuffType)EditorGUILayout.EnumPopup("Buff Type", (BuffType)m_buffType.enumValueIndex);
            m_debuffType.enumValueIndex = (int)(DebuffType)EditorGUILayout.EnumPopup("Debuff Type", (DebuffType)m_debuffType.enumValueIndex);



            if (card.cardActionType is CardActionType.Attack)
            {
                m_damageAmt.intValue = EditorGUILayout.IntField("Damage Amount", m_damageAmt.intValue);

            }


        }
    }
}

