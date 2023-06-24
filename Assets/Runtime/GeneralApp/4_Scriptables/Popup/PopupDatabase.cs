/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 **/

/// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YannickSCF.GeneralApp.Scriptables.Popup {
    [CreateAssetMenu(fileName = "Popup Database", menuName = "Scriptable Objects/YannickSCF/Popups Database")]
    public class PopupDatabase : ScriptableObject {

        [Header("Popups list")]
        [SerializeField] private List<PopupData> _allPopups;

        public GameObject GetPopupById(string popupId) {
            return _allPopups.FirstOrDefault(x => x.PopupId.Equals(popupId)).Popup;
        }

        [Serializable]
        private class PopupData {
            [SerializeField] private string _popupId;
            [SerializeField] private GameObject _popup;

            public string PopupId { get => _popupId; }
            public GameObject Popup { get => _popup; }
        }
    }
}
