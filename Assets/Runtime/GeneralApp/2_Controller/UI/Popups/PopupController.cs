/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     27/06/2023
 **/

/// Dependencies
using System;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.Popups;

namespace YannickSCF.GeneralApp.Controller.UI.Popups {
    public abstract class PopupData {
        public string PopupId;
    }

    public abstract class PopupController : MonoBehaviour {

        public event CommonEventsDelegates.SimpleEventDelegate OnPopupShown;
        public event CommonEventsDelegates.SimpleEventDelegate OnPopupHidden;

        [SerializeField] protected PopupView View;

        private string _popupId;
        public string PopupId { get => _popupId; protected set => _popupId = value; }

        #region Mono
        protected virtual void OnEnable() {
            View.OnViewShown += ThrowPopupShown;
            View.OnViewHidden += ThrowPopupHidden;
        }

        protected virtual void OnDisable() {
            View.OnViewShown += ThrowPopupShown;
            View.OnViewHidden += ThrowPopupHidden;
        }
        #endregion

        protected virtual void ThrowPopupShown() {
            OnPopupShown?.Invoke();
        }

        protected virtual void ThrowPopupHidden() {
            OnPopupHidden?.Invoke();
        }

        public virtual void Init(PopupData popupData) {
            _popupId = popupData.PopupId;

            PopupViewData viewData = null;
            View.Init(viewData);
        }

        public virtual void ResetPopup() {

        }

        public virtual void Show(PopupData popupData = null) {
            if (popupData != null) {
                Init(popupData);
            }

            View.Show();
        }

        public virtual void Hide() {
            View.Hide();
        }

        public void CleanOnShownEvents() {
            OnPopupShown = null;
        }

        public void CleanOnHiddenEvents() {
            OnPopupHidden = null;
        }
    }
}
