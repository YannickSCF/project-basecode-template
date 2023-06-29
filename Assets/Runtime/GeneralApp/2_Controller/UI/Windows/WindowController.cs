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
    public class WindowController<T> : MonoBehaviour where T : WindowView {

        public delegate void WindowControllerEventDelegate(WindowController<T> window);
        public event WindowControllerEventDelegate OnWindowShown;
        public event WindowControllerEventDelegate OnWindowHidden;

        [SerializeField] protected T View;

        private string _windowId;
        public string WindowId { get => _windowId; protected set => _windowId = value; }

        #region Mono
        protected virtual void OnEnable() {
            View.OnViewShown += ThrowWindowShown;
            View.OnViewHidden += ThrowWindowHidden;
        }

        protected virtual void OnDisable() {
            View.OnViewShown -= ThrowWindowShown;
            View.OnViewHidden -= ThrowWindowHidden;
        }
        #endregion

        protected virtual void ThrowWindowShown() {
            OnWindowShown?.Invoke(this);
        }

        protected virtual void ThrowWindowHidden() {
            OnWindowHidden?.Invoke(this);
        }

        public virtual void Init(string windowId) {
            _windowId = windowId;
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
