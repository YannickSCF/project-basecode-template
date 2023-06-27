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
    public abstract class WindowController<T> : MonoBehaviour where T : WindowView {

        [SerializeField] protected T View;

        protected Action<WindowController<T>, string> OnWindowShown;
        protected Action<WindowController<T>, string> OnWindowHidden;

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

            View.Init();
        }

        public virtual void Open() {
            gameObject.SetActive(true);

            View.Open();
        }

        public virtual void Show(Action<WindowController<T>, string> onWindowShown = null) {
            OnWindowShown = onWindowShown;
            gameObject.SetActive(true);

            View.Show();
        }

        public virtual void Hide(Action<WindowController<T>, string> onWindowHidden = null) {
            OnWindowHidden = onWindowHidden;
            gameObject.SetActive(false);

            View.Hide();
        }

        public virtual void Close() {
            Destroy(gameObject);

            View.Close();
        }
    }
}
