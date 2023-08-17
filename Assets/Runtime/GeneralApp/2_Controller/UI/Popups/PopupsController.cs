/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     20/06/2023
 **/

/// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// Custom dependencies
using YannickSCF.GeneralApp.Scriptables.Popup;

namespace YannickSCF.GeneralApp.Controller.UI.Popups {
    public sealed class PopupsController : MonoBehaviour {

        [Header("Database")]
        [SerializeField] private PopupDatabase _popupsDatabase;

        [Header("Popups displays")]
        [SerializeField] private Transform _popupsDisplay;
        [SerializeField] private Transform _popupsPoolDisplay;

        [Header("Background values")]
        [SerializeField] private Image _popupBackground;
        [SerializeField, Range(0f, 1f)] private float _backgroundActiveAlpha = 0.5f;
        [SerializeField, Range(0f, 3f)] private float _timeToSetFinalBackground = 0f;

        private KeyValuePair<string, PopupController> _popupVisible;
        private Dictionary<string, PopupController> _popupsPool;

        private Stack<string> _popupsStack;

        private Coroutine _backgroundCoroutine = null;

        #region Mono
        private void Awake() {
            _popupVisible = new KeyValuePair<string, PopupController>(string.Empty, null);
            _popupsPool = new Dictionary<string, PopupController>();
            _popupsStack = new Stack<string>();

            _popupBackground.gameObject.SetActive(false);
            _popupBackground.color = new Color(
                _popupBackground.color.r,
                _popupBackground.color.g,
                _popupBackground.color.b,
                0f);

            _popupsDisplay.gameObject.SetActive(true);
            _popupsPoolDisplay.gameObject.SetActive(false);
        }
        #endregion

        #region Event listeners methods
        #endregion

        public PopupController ShowPopup(PopupData data) {
            PopupController popup = null;
            
            // If popup is in scene and visible -> Show warning
            if (_popupVisible.Key.Equals(data.PopupId)) {
                Debug.LogWarning("Popup already visible!");
                return null;
            }

            // Get popup, creating it or from pool
            popup = GetPopupObject(data.PopupId);
            PopupController popupVisible = _popupVisible.Value;

            if (popup != null) {
                // If there is a popup visible...
                if (HasPopupVisible()) {
                    // ... save popup in stack and hide it
                    _popupsStack.Push(_popupVisible.Key);
                    // SHOW THE POPUP when previous hide ends
                    popupVisible.OnPopupHidden += () => {
                        popupVisible.transform.SetParent(_popupsPoolDisplay);
                        // SHOW THE POPUP
                        ShowPopup(popup, data);
                        popupVisible.CleanOnHiddenEvents();
                    };
                    popupVisible.Hide();
                } else {
                    // ... if not, show background
                    ToggleBackground(true);
                    // SHOW THE POPUP
                    ShowPopup(popup, data);
                }
            }

            return popup;
        }

        private void ShowPopup(PopupController popup, PopupData data) {
            popup.transform.SetParent(_popupsDisplay);
            popup.Show(data);
            _popupVisible = new KeyValuePair<string, PopupController>(data.PopupId, popup);
        }

        public void HidePopup(string popupId, bool resetOnHide = true) {
            // If popup is in scene and visible...
            if (_popupVisible.Key.Equals(popupId)) {
                PopupController popupVisible = _popupVisible.Value;
                popupVisible.OnPopupHidden += () => {
                    if (resetOnHide) {
                        popupVisible.ResetPopup();
                    }
                    popupVisible.transform.SetParent(_popupsPoolDisplay);
                    popupVisible.CleanOnHiddenEvents();

                    if (_popupsStack.Count > 0) {
                        string popupIdStacked = _popupsStack.Pop();
                        _popupsPool.TryGetValue(popupIdStacked, out PopupController popup);
                        
                        _popupVisible = new KeyValuePair<string, PopupController>(popupIdStacked, popup);
                        _popupVisible.Value.transform.SetParent(_popupsDisplay);
                        _popupVisible.Value.Show();
                    } else {
                        ToggleBackground(false);
                        _popupVisible = new KeyValuePair<string, PopupController>(string.Empty, null);
                    }
                };
                _popupVisible.Value.Hide();
            } else { // ... If not -> Show warning
                Debug.LogWarning("Popup is not visible!");
            }
        }

        public bool IsShowingPopup(string popupId) {
            return _popupVisible.Key.Equals(popupId);
        }

        #region Private methods
        private bool HasPopupVisible() {
            return !string.IsNullOrEmpty(_popupVisible.Key) &&
                _popupVisible.Value != null;
        }

        private PopupController GetPopupObject(string popupId) {
            PopupController popup = null;

            // If popup is not instantiated...
            if (!_popupsPool.ContainsKey(popupId)) {
                // ... create it
                GameObject popupToCreate = _popupsDatabase.GetPopupById(popupId);
                if (popupToCreate != null) {
                    GameObject popupCreated = Instantiate(popupToCreate, _popupsPoolDisplay);
                    popup = popupCreated.GetComponent<PopupController>();
                    _popupsPool.Add(popupId, popup);
                } else {
                    Debug.LogWarning($"There is no popup related to Id '{popupId}'");
                }
            } else {
                // ... get it from list
                if (!_popupsPool.TryGetValue(popupId, out popup)) {
                    Debug.LogError($"There was a problem extracting popup '{popupId}'");
                }
            }

            return popup;
        }

        private void ToggleBackground(bool turnOn) {
            if (_popupBackground != null) {
                if (_backgroundCoroutine != null) {
                    StopCoroutine(_backgroundCoroutine);
                    _backgroundCoroutine = null;
                }

                if (_timeToSetFinalBackground > 0) {
                    _backgroundCoroutine = StartCoroutine(ToggleBackgroundCoroutine(turnOn));
                } else {
                    _popupBackground.gameObject.SetActive(turnOn);
                    _popupBackground.color = new Color(
                        _popupBackground.color.r,
                        _popupBackground.color.g,
                        _popupBackground.color.b,
                        turnOn ? _backgroundActiveAlpha : 0f);
                }
            } else {
                Debug.LogError("There is no popup background referenced!");
            }
        }
        private IEnumerator ToggleBackgroundCoroutine(bool turnOn) {
            if (turnOn) _popupBackground.gameObject.SetActive(true);

            Color initialColor = new Color(
                _popupBackground.color.r,
                _popupBackground.color.g,
                _popupBackground.color.b,
                turnOn ? 0f : _backgroundActiveAlpha);

            Color endColor = new Color(
                _popupBackground.color.r,
                _popupBackground.color.g,
                _popupBackground.color.b,
                turnOn ? _backgroundActiveAlpha : 0f);

            float c_time = 0f;
            while (c_time < _timeToSetFinalBackground) {
                _popupBackground.color = Color.Lerp(initialColor, endColor, c_time / _timeToSetFinalBackground);
                c_time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (!turnOn) _popupBackground.gameObject.SetActive(false);
            _backgroundCoroutine = null;
        }
        
        #endregion
    }
}
