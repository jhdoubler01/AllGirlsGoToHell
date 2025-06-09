using System;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using UnityEngine;

namespace AGGtH.Runtime.Data.Containers
{
    [CreateAssetMenu(fileName = "Special Keyword", menuName = "AGGtH/Card/Special Keyword Data")]
    public class SpecialKeywordData : ScriptableObject
    {
        [SerializeField] private List<SpecialKeywordBase> specialKeywordBaseList;
        public List<SpecialKeywordBase> SpecialKeywordBaseList => specialKeywordBaseList;


    }

    [Serializable]
    public class SpecialKeywordBase
    {
        [SerializeField] private SpecialKeywords specialKeyword;
        [SerializeField][TextArea] private string contentText;

        public SpecialKeywords SpecialKeyword => specialKeyword;


        public string GetHeader(string overrideKeywordHeader = "")
        {
            return string.IsNullOrEmpty(overrideKeywordHeader) ? specialKeyword.ToString() : overrideKeywordHeader;
        }

        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }
    }
}