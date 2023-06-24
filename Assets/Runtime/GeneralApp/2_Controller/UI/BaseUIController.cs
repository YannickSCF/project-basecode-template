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
using YannickSCF.GeneralApp.Controller.UI.Windows;
using YannickSCF.GeneralApp.View.UI.Popups;
using YannickSCF.GeneralApp.View.UI.Windows;

namespace YannickSCF.GeneralApp.Controller.UI {
    public class BaseUIController : MonoBehaviour {

        [Header("Main UI Controllers")]
        [SerializeField] protected WindowsController WindowsController;
        [SerializeField] protected PopupsController PopupsController;
        [Header("Basic Fade In/Out Controller")]
        [SerializeField] private LoadingPanelController _loadingController;

        public LoadingPanelController LoadingController { get => _loadingController; }

        #region Methods for popups
        public virtual PopupView ShowPopup(string popupId) {
            return PopupsController.ShowPopup(popupId);
        }

        public virtual void HidePopup(string popupId) {
            PopupsController.HidePopup(popupId);
        }

        public virtual void ClosePopup(string popupId) {
            PopupsController.ClosePopup(popupId);
        }
        #endregion

        public virtual WindowsView ShowView(string windowId) {
            return WindowsController.ShowWindow(windowId);
        }

        public virtual void HideView(string windowId) {
            WindowsController.HideWindow(windowId);
        }

        public virtual void CloseView(string windowId) {
            WindowsController.CloseWindow(windowId);
        }
    }
}
