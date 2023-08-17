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
        public virtual PopupController ShowPopup(PopupData data) {
            return PopupsController.ShowPopup(data);
        }

        public virtual void HidePopup(string popupId) {
            PopupsController.HidePopup(popupId);
        }

        public bool IsShowingPopup(string popupId) {
            return PopupsController.IsShowingPopup(popupId);
        }
        #endregion

        public virtual T ShowView<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            return WindowsController.ShowWindow<T, U>(windowId);
        }

        public virtual void HideView<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            WindowsController.HideWindow<T, U>(windowId);
        }

        public virtual void CloseView<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            WindowsController.CloseWindow<T, U>(windowId);
        }
    }
}
