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

        private Dictionary<string, WindowController<WindowView>> _windowsVisible;
        private Dictionary<string, WindowController<WindowView>> _windowsHidden;

        #region Mono
        private void Awake() {
            _windowsVisible = new Dictionary<string, WindowController<WindowView>>();
            _windowsHidden = new Dictionary<string, WindowController<WindowView>>();
        }
        #endregion

        public T ShowWindow<T, U>(string windowId) where T : WindowController<U> where U : WindowView {
            WindowController<WindowView> window = null;
            if (_windowsDatabase != null) {
                if (_windowsHidden.ContainsKey(windowId)) {
                    window = UnhideWindow(windowId);
                } else {
                    window = CreateWindow(windowId);
                }
            } else {
                Debug.LogError("No Window Database selected!");
            }

            if (!window is T) {
                Debug.LogError("Window is not of the type specified!");
                return null;
            }

            return window as T;
        }

        public void HideWindow(string windowId) {
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out WindowController<WindowView> windowToHide)) {
                    windowToHide.Hide(OnWindowHidden);
                } else {
                    Debug.LogError($"Window NOT found on visible windows list! ({windowId})");
                }
            } else {
                Debug.LogWarning($"Window '{windowId}' not found on visible area!");
            }
        }
        private void OnWindowHidden(WindowController<WindowView> windowToHide, string windowId) {
            windowToHide.transform.SetParent(_hiddenWindowsDisplay);
            _windowsVisible.Remove(windowId);
            _windowsHidden.Add(windowId, windowToHide);
        }

        public void CloseWindow(string windowId) {
            WindowController<WindowView> window;
            if (_windowsVisible.ContainsKey(windowId)) {
                if (_windowsVisible.TryGetValue(windowId, out window)) {
                    _windowsVisible.Remove(windowId);
                    window.Close();
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on visible windows list!");
                }
            }

            if (_windowsHidden.ContainsKey(windowId)) {
                if (_windowsHidden.TryGetValue(windowId, out window)) {
                    _windowsHidden.Remove(windowId);
                    window.Close();
                    return;
                } else {
                    Debug.LogWarning($"Window '{windowId}' NOT found on hidden windows list!");
                }
            }

            Debug.LogWarning($"Window '{windowId}' already doesn't exists!");
        }

        #region Private methods
        private WindowController<WindowView> UnhideWindow(string windowId) {
            WindowController<WindowView> window;
            if (_windowsHidden.TryGetValue(windowId, out window)) {
                window.Show(OnWindowShown);
            } else {
                Debug.LogError($"Window NOT found on hidden windows list! ({windowId})");
            }

            return window;
        }
        private void OnWindowShown(WindowController<WindowView> windowToShow, string windowId) {
            windowToShow.transform.SetParent(_windowsDisplay);
            _windowsHidden.Remove(windowId);
            _windowsVisible.Add(windowId, windowToShow);
        }

        private WindowController<WindowView> CreateWindow(string windowId) {
            GameObject windowToShow = _windowsDatabase.GetWindowById(windowId);
            if (windowToShow != null) {
                GameObject instantiatedObject = Instantiate(windowToShow, _windowsDisplay);
                WindowController<WindowView> window = instantiatedObject.GetComponent<WindowController<WindowView>>();

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
