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
        protected string _popupId;
        public string PopupId { get => _popupId; }

        public PopupData(string popupId) {
            _popupId = popupId;
        }
    }

    public abstract class PopupController : MonoBehaviour {

        public event CommonEventsDelegates.SimpleEventDelegate OnPopupShown;
        public event CommonEventsDelegates.SimpleEventDelegate OnPopupHidden;

        [SerializeField] private PopupView _view;
        protected T GetView<T>() where T : PopupView {
            return (T)_view;
        }

        private string _popupId;
        public string PopupId { get => _popupId; protected set => _popupId = value; }


        #region Mono
        protected virtual void OnEnable() {
            _view.OnViewShown += ThrowPopupShown;
            _view.OnViewHidden += ThrowPopupHidden;
        }

        protected virtual void OnDisable() {
            _view.OnViewShown += ThrowPopupShown;
            _view.OnViewHidden += ThrowPopupHidden;
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
            _view.Init(/*Can have a PopupViewData*/);
        }

        public virtual void ResetPopup() { }

        public virtual void Show(PopupData popupData = null) {
            if (popupData != null) {
                Init(popupData);
            }

            _view.Show();
        }

        public virtual void Hide() {
            _view.Hide();
        }

        public void CleanOnShownEvents() {
            OnPopupShown = null;
        }

        public void CleanOnHiddenEvents() {
            OnPopupHidden = null;
        }
    }
}
