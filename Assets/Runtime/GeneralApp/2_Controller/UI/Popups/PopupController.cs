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

        [SerializeField] protected T View;

        protected Action<PopupController<T>, string> OnPopupShown;
        protected Action<PopupController<T>, string> OnPopupHidden;

        private string _popupId;
        public string PopupId { get => _popupId; protected set => _popupId = value; }

        #region Mono
        protected virtual void OnEnable() {
            OnPopupShown?.Invoke(this, _popupId);
        }

        protected virtual void OnDisable() {
            OnPopupHidden?.Invoke(this, _popupId);
        }
        #endregion

        public virtual void Init(string popupId) {
            _popupId = popupId;
            View.Init();
        }

        public virtual void Open() {
            View.Open();
        }

        public virtual void Show(Action<PopupController<T>, string> onPopupShown = null) {
            OnPopupShown = onPopupShown;
            View.Show();
        }

        public virtual void Hide(Action<PopupController<T>, string> onPopupHidden = null) {
            OnPopupHidden = onPopupHidden;
            View.Hide();
        }

        public virtual void Close() {
            View.Close();
        }
    }
}
