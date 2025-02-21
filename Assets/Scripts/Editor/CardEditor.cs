using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using AGGtH.Editor.Extensions;
using System.Linq;
using System.Text;

namespace AGGtH.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CardData))]

    public class CardEditor : UnityEditor.Editor
    {
#if UNITY_EDITOR
        private static CardEditor currentWindow { get; set; }
        private static VisualTreeAsset m_customCardInspector;
        private SerializedObject m_serializedObject;
        
        private const string CardDataDefaultPath = "Assets/Data/CardData/";

        #region Cache Card Data
        private static CardData cachedCardData { get; set; }
        private List<CardData> allCardDataList { get; set; }
        private CardData selectedCardData { get; set; }
        private string cardID { get; set; }
        private string cardName { get; set; }
        private int energyCost { get; set; }
        private Sprite cardSprite { get; set; }
        private bool usableWithoutTarget { get; set; }
        private bool exhaustAfterPlay { get; set; }
        private List<CardActionData> cardActionDataList { get; set; }

        private void CacheCardData()
        {
            cardID = selectedCardData.Id;
            cardName = selectedCardData.CardName;
            energyCost = selectedCardData.EnergyCost;
            cardSprite = selectedCardData.CardSprite;
            usableWithoutTarget = selectedCardData.UsableWithoutTarget;
            exhaustAfterPlay = selectedCardData.ExhaustAfterPlay;
            cardActionDataList = selectedCardData.CardActionDataList;
        }
        private void ClearCachedCardData()
        {
            cardID = string.Empty;
            cardName = string.Empty;
            energyCost = 0;
            cardSprite = null;
            usableWithoutTarget = false;
            exhaustAfterPlay = false;
            cardActionDataList?.Clear();

        }

        #endregion

        #region Setup
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
        private void OnEnable()
        {
            allCardDataList?.Clear();
            allCardDataList = ListExtensions.GetAllInstances<CardData>().ToList();
            if (cachedCardData)
            {
                selectedCardData = cachedCardData;
                m_serializedObject = new SerializedObject(selectedCardData);
                CacheCardData();
            }
            Selection.selectionChanged += Repaint;
        }
        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            cachedCardData = null;
            selectedCardData = null;
        }
        #endregion
#region Layout Methods
        private void CreateNewCard()
        {
            var clone = CreateInstance<CardData>();
            var str = new StringBuilder();
            var count = allCardDataList.Count;

            str.Append(count + 1).Append("_").Append("new_card_name");
            clone.EditId(str.ToString());
            clone.EditCardName(str.ToString());
            //clone.EditCardActionDataList(new List<CardActionData>());
            //clone.EditCardDescriptionDataList(new List<CardDescriptionData>());
            //clone.EditSpecialKeywordsList(new List<SpecialKeywords>());
            //clone.EditRarity(RarityType.Common);
            var path = str.Insert(0, CardDataDefaultPath).Append(".asset").ToString();
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(clone, uniquePath);
            AssetDatabase.SaveAssets();
            RefreshCardData();
            selectedCardData = allCardDataList.Find(x => x.Id == clone.Id);
            CacheCardData();
        }
        #endregion
        #region Card Data Methods
        private void ChangeId()
        {
            cardID = EditorGUILayout.TextField(cardID);
        }
        private void ChangeCardName()
        {
            cardName = EditorGUILayout.TextField(cardName);
        }
        private void ChangeCardLoveLanguageType()
        {
            
        }
        #endregion
        private void SaveCardData()
        {
            if (!selectedCardData) return;

            selectedCardData.EditId(cardID);
            selectedCardData.EditCardName(cardName);
            //SelectedCardData.EditManaCost(ManaCost);
            //SelectedCardData.EditCardSprite(CardSprite);
            //SelectedCardData.EditUsableWithoutTarget(UsableWithoutTarget);
            //SelectedCardData.EditExhaustAfterPlay(ExhaustAfterPlay);
            //SelectedCardData.EditCardActionDataList(CardActionDataList);
            //SelectedCardData.EditCardDescriptionDataList(CardDescriptionDataList);
            //SelectedCardData.EditSpecialKeywordsList(SpecialKeywordsList);
            //SelectedCardData.EditAudioType(AudioType);
            EditorUtility.SetDirty(selectedCardData);
            AssetDatabase.SaveAssets();
        }
        private void RefreshCardData(){
            selectedCardData = null;
            ClearCachedCardData();
            allCardDataList?.Clear();
            allCardDataList = ListExtensions.GetAllInstances<CardData>().ToList();
        }
#endif
    }
}

