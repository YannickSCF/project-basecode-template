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

            _windowsDisplay.gameObject.SetActive(true);
            _hiddenWindowsDisplay.gameObject.SetActive(false);
        }
        #endregion

        public T ShowWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            T window = null;
            if (_windowsDatabase != null) {
                if (_windowsHidden.ContainsKey(windowId)) {
                    window = UnhideWindow<T, U>(windowId);
                } else if (!_windowsVisible.ContainsKey(windowId)) {
                    window = CreateWindow<T, U>(windowId);
                } else {
                    Debug.LogWarning($"Window '{windowId}' is already in scene!");
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
                    // Set windows lists
                    _windowsVisible.Remove(windowId);
                    _windowsHidden.Add(windowId, windowToHide);
                    // Hide and listen on hide finished
                    windowToHide.OnWindowHidden += OnWindowHidden;
                    windowToHide.Hide();
                } else {
                    Debug.LogError($"Window NOT found on visible windows list! ({windowId})");
                }
            } else {
                Debug.LogWarning($"Window '{windowId}' not found on visible area!");
            }
        }
        private void OnWindowHidden<T>(WindowController<T> windowToHide) where T : WindowView {
            windowToHide.transform.SetParent(_hiddenWindowsDisplay);
            windowToHide.OnWindowHidden -= OnWindowHidden;
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
                // Set windows lists
                _windowsHidden.Remove(windowId);
                _windowsVisible.Add(windowId, windowToUnhide);
                // Show and listen on show finished
                windowToUnhide.transform.SetParent(_windowsDisplay);
                windowToUnhide.OnWindowShown += OnWindowUnhide;
                windowToUnhide.Show();
            } else {
                Debug.LogError($"Window NOT found on hidden windows list! ({windowId})");
            }

            return windowToUnhide;
        }
        private void OnWindowUnhide<T>(WindowController<T> windowToUnhide) where T : WindowView {
            windowToUnhide.OnWindowShown -= OnWindowUnhide;
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
