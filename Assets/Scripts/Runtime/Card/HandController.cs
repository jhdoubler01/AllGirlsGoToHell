using UnityEngine;
using System.Collections.Generic;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.Card
{
    public class HandController : MonoBehaviour
    {
        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        [HideInInspector] public List<CardBase> hand; // Cards currently in hand
    }
}
