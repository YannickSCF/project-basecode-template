/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     24/06/2023
 **/

/// Dependencies
using System.Collections.Generic;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.Windows;
using YannickSCF.GeneralApp.Scriptables.Window;

namespace YannickSCF.GeneralApp.Controller.UI.Windows {
    public class WindowsController : MonoBehaviour {

        [Header("Database")]
        [SerializeField] private WindowsDatabase _windowsDatabase;

        [Header("Windows displays")]
        [SerializeField] private Transform _windowsDisplay;
        [SerializeField] private Transform _hiddenWindowsDisplay;

        private Dictionary<string, WindowsView> _windowsVisible;
        private Dictionary<string, WindowsView> _windowsHidden;

        #region Mono
        protected virtual void Awake() {
            _windowsVisible = new Dictionary<string, WindowsView>();
            _windowsHidden = new Dictionary<string, WindowsView>();
        }
        #endregion

        public WindowsView ShowWindow(string windowId) {
            WindowsView windowView = null;
            if (_windowsDatabase != null) {
                if (_windowsHidden.ContainsKey(windowId)) {
                    windowView = UnhideWindow(windowId);
                } else {
                    windowView = CreateWindow(windowId);
                }
            } else {
                Debug.LogError("No Window Database selected!");
            }

            return windowView;
        }

        public void HideWindow(string windowId) {
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out WindowsView windowToHide)) {
                    windowToHide.transform.SetParent(_hiddenWindowsDisplay);
                    _windowsVisible.Remove(windowId);
                    _windowsHidden.Add(windowId, windowToHide);
                } else {
                    Debug.LogError($"Window NOT found on visible windows list! ({windowId})");
                }
            } else {
                Debug.LogWarning($"Window '{windowId}' not found on visible area!");
            }
        }

        public void CloseWindow(string windowId) {
            WindowsView windowView;
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out windowView)) {
                    _windowsVisible.Remove(windowId);
                    Destroy(windowView.gameObject);
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on visible windows list!");
                }
            }

            if (_windowsHidden.ContainsKey(windowId)) {
                if (_windowsHidden.TryGetValue(windowId, out windowView)) {
                    _windowsHidden.Remove(windowId);
                    Destroy(windowView.gameObject);
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on hidden windows list!");
                }
            }

            Debug.LogWarning($"Window '{windowId}' already doesn't exists!");
        }

        #region Private methods
        private WindowsView UnhideWindow(string windowId) {
            WindowsView windowView;
            if (_windowsHidden.TryGetValue(windowId, out windowView)) {
                windowView.transform.SetParent(_windowsDisplay);
                _windowsHidden.Remove(windowId);
                _windowsVisible.Add(windowId, windowView);
            } else {
                Debug.LogError($"Window NOT found on hidden windows list! ({windowId})");
            }

            return windowView;
        }

        private WindowsView CreateWindow(string windowId) {
            GameObject windowToShow = _windowsDatabase.GetWindowById(windowId);
            if (windowToShow != null) {
                GameObject instantiatedObject = Instantiate(windowToShow, _windowsDisplay);
                WindowsView windowView = instantiatedObject.GetComponent<WindowsView>();
                _windowsVisible.Add(windowId, windowView);

                return windowView;
            } else {
                Debug.LogWarning($"There is no window related to Id '{windowId}'");
            }

            return null;
        }
        #endregion
    }
}
