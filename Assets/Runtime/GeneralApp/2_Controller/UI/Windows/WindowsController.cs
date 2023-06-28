/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     24/06/2023
 **/

/// Dependencies
using System.Collections.Generic;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Scriptables.Window;
using YannickSCF.GeneralApp.View.UI.Windows;

namespace YannickSCF.GeneralApp.Controller.UI.Windows {
    public sealed class WindowsController : MonoBehaviour {

        [Header("Database")]
        [SerializeField] private WindowsDatabase _windowsDatabase;

        [Header("Windows displays")]
        [SerializeField] private Transform _windowsDisplay;
        [SerializeField] private Transform _hiddenWindowsDisplay;

        private Dictionary<string, object> _windowsVisible;
        private Dictionary<string, object> _windowsHidden;

        #region Mono
        private void Awake() {
            _windowsVisible = new Dictionary<string, object>();
            _windowsHidden = new Dictionary<string, object>();
        }
        #endregion

        public T ShowWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            T window = null;
            if (_windowsDatabase != null) {
                if (_windowsHidden.ContainsKey(windowId)) {
                    window = UnhideWindow<T, U>(windowId);
                } else {
                    window = CreateWindow<T, U>(windowId);
                }
            } else {
                Debug.LogError("No Window Database selected!");
            }

            return window;
        }

        public void HideWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out object objectToHide)) {
                    T windowToHide = objectToHide as T;

                    _windowsVisible.Remove(windowId);
                    _windowsHidden.Add(windowId, windowToHide);
                    windowToHide.Hide(OnWindowHidden);
                } else {
                    Debug.LogError($"Window NOT found on visible windows list! ({windowId})");
                }
            } else {
                Debug.LogWarning($"Window '{windowId}' not found on visible area!");
            }
        }
        private void OnWindowHidden<T>(WindowController<T> windowToHide, string windowId) where T : WindowView {
            windowToHide.transform.SetParent(_hiddenWindowsDisplay);
        }

        public void CloseWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            T windowToClose = null;
            object objectToClose;
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out objectToClose)) {
                    windowToClose = objectToClose as T;
                    _windowsVisible.Remove(windowId);
                    windowToClose.Close();
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on visible windows list!");
                }
            }

            if (_windowsHidden.ContainsKey(windowId)) {
                if (_windowsHidden.TryGetValue(windowId, out objectToClose)) {
                    windowToClose = objectToClose as T;
                    _windowsHidden.Remove(windowId);
                    windowToClose.Close();
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on hidden windows list!");
                }
            }

            Debug.LogWarning($"Window '{windowId}' already doesn't exists!");
        }

        #region Private methods
        private T UnhideWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            T windowToUnhide = null;
            if (_windowsHidden.TryGetValue(windowId, out object objectToUnhide)) {
                windowToUnhide = objectToUnhide as T;

                _windowsHidden.Remove(windowId);
                _windowsVisible.Add(windowId, windowToUnhide);
                windowToUnhide.Show(OnWindowShown);
            } else {
                Debug.LogError($"Window NOT found on hidden windows list! ({windowId})");
            }

            return windowToUnhide;
        }
        private void OnWindowShown<T>(WindowController<T> windowToShow, string windowId) where T : WindowView {
            windowToShow.transform.SetParent(_windowsDisplay);
        }

        private T CreateWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            GameObject windowToShow = _windowsDatabase.GetWindowById(windowId);
            if (windowToShow != null) {
                GameObject instantiatedObject = Instantiate(windowToShow, _windowsDisplay);
                T window = instantiatedObject.GetComponent<T>();

                _windowsVisible.Add(windowId, window);
                window.Init(windowId);
                window.Open();

                return window;
            } else {
                Debug.LogWarning($"There is no window related to Id '{windowId}'");
            }

            return null;
        }
        #endregion
    }
}
