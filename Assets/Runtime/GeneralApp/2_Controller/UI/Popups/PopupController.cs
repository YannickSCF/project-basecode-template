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
    public abstract class PopupController : MonoBehaviour {

        [SerializeField] protected PopupView View;

        protected Action<PopupController, string> OnPopupShown;
        protected Action<PopupController, string> OnPopupHidden;

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
        }

        public virtual void Open() {
            gameObject.SetActive(true);
        }

        public virtual void Show(Action<PopupController, string> onPopupShown = null) {
            OnPopupShown = onPopupShown;
            gameObject.SetActive(true);
        }

        public virtual void Hide(Action<PopupController, string> onPopupHidden = null) {
            OnPopupHidden = onPopupHidden;
            gameObject.SetActive(false);
        }

        public virtual void Close(Action onPopupClosed = null) {
            Destroy(gameObject);
        }
    }
}
