using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AGGtH.Runtime.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class CharacterCanvas : MonoBehaviour
    {
        [SerializeField] protected Transform statusIconRoot;
        [SerializeField] protected Transform highlightRoot;
        [SerializeField] protected StatusIconsData statusIconsData;
        [SerializeField] protected SegmentedHealthBar healthBar;
        #region Cache
        protected Dictionary<StatusType, StatusIconBase> StatusDict = new Dictionary<StatusType, StatusIconBase>();
        protected Canvas TargetCanvas;
        public SegmentedHealthBar HealthBar => healthBar;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        #endregion

        #region Setup
        public void InitCanvas()
        {
            highlightRoot.gameObject.SetActive(false);

            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                StatusDict.Add((StatusType)i, null);

            TargetCanvas = GetComponent<Canvas>();

            if (TargetCanvas)
                TargetCanvas.worldCamera = Camera.main;
        }
        #endregion

        #region Public Methods
        public void ApplyStatus(StatusType targetStatus, float value)
        {
            if (StatusDict[targetStatus] == null)
            {
                var targetData = statusIconsData.StatusIconList.FirstOrDefault(x => x.IconStatus == targetStatus);

                if (targetData == null) return;

                var clone = Instantiate(statusIconsData.StatusIconBasePrefab, statusIconRoot);
                clone.SetStatus(targetData);
                StatusDict[targetStatus] = clone;
            }

            StatusDict[targetStatus].SetStatusValue(value);
        }
        public void ClearStatus(StatusType targetStatus)
        {
            if (StatusDict[targetStatus])
            {
                Destroy(StatusDict[targetStatus].gameObject);
            }

            StatusDict[targetStatus] = null;
        }
        public void UpdateStatusText(StatusType targetStatus, float value)
        {
            if (StatusDict[targetStatus] == null) return;

            StatusDict[targetStatus].StatusValueText.text = $"{value}";
        }
        #endregion
    }
}
