
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AGGtH.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Card))]

    public class CardEditor : UnityEditor.Editor
    {
        public VisualTreeAsset m_customCardInspector;
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

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI.
            VisualElement myInspector = new VisualElement();

            // Add a simple label.
            myInspector.Add(new Label("This is a custom Inspector"));
            // Load the UXML file.
            m_customCardInspector = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIDocuments/CardEditorUIDoc.uxml");
            // Instantiate the UXML.
            myInspector = m_customCardInspector.Instantiate();

            // Return the finished Inspector UI.
            return myInspector;
        }

        //    private void OnEnable()
        //    {
        //        card = target as Card;
        //        m_cardName = serializedObject.FindProperty("cardName");

        //        m_cardLoveLanguageType = serializedObject.FindProperty("cardLoveLanguageType");
        //        m_cardActionType = serializedObject.FindProperty("cardActionType");
        //        m_actionTargetType = serializedObject.FindProperty("actionTargetType");
        //        m_buffType = serializedObject.FindProperty("buffType");
        //        m_debuffType = serializedObject.FindProperty("debuffType");

        //        m_damageAmt = serializedObject.FindProperty("damageAmt");
        //        m_healAmt = serializedObject.FindProperty("healAmt");
        //        m_blockAmt = serializedObject.FindProperty("blockAmt");
        //        m_drawCardAmt = serializedObject.FindProperty("drawCardAmt");
        //        m_energyGainAmt = serializedObject.FindProperty("energyGainAmt");

        //    }
        //    public override void OnInspectorGUI()
        //    {
        //        //base.OnInspectorGUI();
        //        serializedObject.Update();
        //        EditorGUI.BeginChangeCheck();

        //        m_cardName.stringValue = (string)EditorGUILayout.TextField("Card Name", m_cardName.stringValue);

        //        m_cardActionType.enumValueFlag = (int)(CardActionType)EditorGUILayout.EnumFlagsField("Action Type", (CardActionType)m_cardActionType.enumValueIndex);

        //        m_cardLoveLanguageType.enumValueIndex = (int)(CardLoveLanguageType)EditorGUILayout.EnumPopup("Love Language Type", (CardLoveLanguageType)m_cardLoveLanguageType.enumValueIndex);
        //        m_actionTargetType.enumValueIndex = (int)(ActionTargetType)EditorGUILayout.EnumPopup("Target Type", (ActionTargetType)m_actionTargetType.enumValueIndex);
        //        m_buffType.enumValueIndex = (int)(BuffType)EditorGUILayout.EnumPopup("Buff Type", (BuffType)m_buffType.enumValueIndex);
        //        m_debuffType.enumValueIndex = (int)(DebuffType)EditorGUILayout.EnumPopup("Debuff Type", (DebuffType)m_debuffType.enumValueIndex);


        //        //if (m_cardActionType.enumValueIndex is (int)CardActionType.Attack)
        //        //{
        //        //    m_damageAmt.intValue = EditorGUILayout.IntField("Damage Amount", m_damageAmt.intValue);

        //        //}
        //        EditorGUI.EndChangeCheck();
        //        serializedObject.ApplyModifiedProperties();

        //    }
    }
}

