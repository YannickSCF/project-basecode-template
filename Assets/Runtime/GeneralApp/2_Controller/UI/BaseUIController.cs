/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 **/

/// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// Custom Dependencies
using YannickSCF.GeneralApp.Controller.UI.LoadingPanel;
using YannickSCF.GeneralApp.Controller.UI.Popups;

namespace YannickSCF.GeneralApp.Controller.UI {
    public class BaseUIController : MonoBehaviour {

        [Header("Main Controllers")]
        [SerializeField] private LoadingPanelController _loadingController;
        [SerializeField] private PopupsController _popupsController;

        [Header("Main Controllers")]
        [SerializeField] private Transform _uIDisplay;

        public LoadingPanelController LoadingController { get => _loadingController; }
        public PopupsController PopupsController { get => _popupsController; }
        public Transform UIDisplay { get => _uIDisplay; }

        // TODO: Queda terminar mas o menos el UI Manager y popups manager
        public void ShowPopup(string popupId) {
            _popupsController.ShowPopup(popupId);
        }

        public void HidePopup(string popupId) {
            _popupsController.HidePopup(popupId);
        }

        public void ClosePopup(string popupId) {
            _popupsController.ClosePopup(popupId);
        }
    }
}
