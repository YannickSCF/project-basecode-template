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
    public abstract class PopupController<T> : MonoBehaviour where T : PopupView {

        public delegate void PopupControllerEventDelegate(PopupController<T> popup);
        public event PopupControllerEventDelegate OnPopupShown;
        public event PopupControllerEventDelegate OnPopupHidden;

        [SerializeField] protected T View;

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
            OnPopupShown?.Invoke(this);
        }

        protected virtual void ThrowPopupHidden() {
            OnPopupHidden?.Invoke(this);
        }

        public virtual void Init(string popupId) {
            _popupId = popupId;
            View.Init();
        }

        public virtual void Open() {
            View.Open();
        }

        public virtual void Show() {
            View.Show();
        }

        public virtual void Hide() {
            View.Hide();
        }

        public virtual void Close() {
            View.Close();
        }
    }
}
