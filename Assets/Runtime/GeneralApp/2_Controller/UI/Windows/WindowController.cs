/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     27/06/2023
 **/

/// Dependencies
using System;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.Windows;

namespace YannickSCF.GeneralApp.Controller.UI.Windows {
    public abstract class WindowController : MonoBehaviour {

        [SerializeField] protected WindowView View;

        protected Action<WindowController, string> OnWindowShown;
        protected Action<WindowController, string> OnWindowHidden;

        private string _windowId;
        public string WindowId { get => _windowId; protected set => _windowId = value; }

        #region Mono
        protected virtual void OnEnable() {
            OnWindowShown?.Invoke(this, _windowId);
        }

        protected virtual void OnDisable() {
            OnWindowHidden?.Invoke(this, _windowId);
        }
        #endregion

        public virtual void Init(string windowId) {
            _windowId = windowId;
        }

        public virtual void Open() {
            gameObject.SetActive(true);
        }

        public virtual void Show(Action<WindowController, string> onWindowShown = null) {
            OnWindowShown = onWindowShown;
            gameObject.SetActive(true);
        }

        public virtual void Hide(Action<WindowController, string> onWindowHidden = null) {
            OnWindowHidden = onWindowHidden;
            gameObject.SetActive(false);
        }

        public virtual void Close(Action onWindowClosed = null) {
            Destroy(gameObject);
        }
    }
}
