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
using YannickSCF.GeneralApp.View.UI.Popups;

namespace YannickSCF.GeneralApp.Controller.UI.Popups {
    public class PopupsController : MonoBehaviour {

        [Header("Database")]
        [SerializeField] private PopupDatabase _popupsDatabase;

        [Header("Popups displays")]
        [SerializeField] private Transform _popupsDisplay;
        [SerializeField] private Transform _hiddenPopupsDisplay;

        [Header("Background values")]
        [SerializeField] private Image _popupBackground;
        [SerializeField, Range(0f, 1f)] private float _backgroundActiveAlpha = 0.5f;
        [SerializeField, Range(0f, 3f)] private float _timeToSetFinalBackground = 0f;

        private Dictionary<string, PopupView> _popupsVisible;
        private Dictionary<string, PopupView> _popupsHidden;

        private Coroutine _backgroundCoroutine = null;

        #region Mono
        protected virtual void Awake() {
            _popupsVisible = new Dictionary<string, PopupView>();
            _popupsHidden = new Dictionary<string, PopupView>();

            _popupBackground.gameObject.SetActive(false);
            _popupBackground.color = new Color(
                _popupBackground.color.r,
                _popupBackground.color.g,
                _popupBackground.color.b,
                0f);
        }
        #endregion

        public PopupView ShowPopup(string popupId) {
            PopupView popupView = null;
            if (_popupsDatabase != null) {
                if (_popupsHidden.ContainsKey(popupId)) {
                    popupView = UnhidePopup(popupId);
                } else {
                    popupView = CreatePopup(popupId);
                }
            } else {
                Debug.LogError("No Popup Database selected!");
            }

            return popupView;
        }

        public void HidePopup(string popupId) {
            if (_popupsVisible.ContainsKey(popupId)) {
                if (_popupsVisible.TryGetValue(popupId, out PopupView popupToHide)) {
                    ToggleBackground(false);
                    popupToHide.transform.SetParent(_hiddenPopupsDisplay);
                    _popupsVisible.Remove(popupId);
                    _popupsHidden.Add(popupId, popupToHide);
                } else {
                    Debug.LogError($"Popup NOT found on visible popups list! ({popupId})");
                }
            } else {
                Debug.LogWarning($"Popup '{popupId}' not found on visible area!");
            }
        }

        public void ClosePopup(string popupId) {
            PopupView popupView;
            if (_popupsVisible.ContainsKey(popupId)) {
                if (_popupsVisible.TryGetValue(popupId, out popupView)) {
                    ToggleBackground(false);
                    _popupsVisible.Remove(popupId);
                    Destroy(popupView.gameObject);
                    return;
                } else {
                    Debug.LogWarning($"Popup '{popupId}' NOT found on visible popups list!");
                }
            }

            if (_popupsHidden.ContainsKey(popupId)) {
                if (_popupsHidden.TryGetValue(popupId, out popupView)) {
                    ToggleBackground(false);
                    _popupsHidden.Remove(popupId);
                    Destroy(popupView.gameObject);
                    return;
                } else {
                    Debug.LogWarning($"Popup '{popupId}' NOT found on hidden popups list!");
                }
            }

            Debug.LogWarning($"Popup '{popupId}' already doesn't exists!");
        }

        #region Private methods
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

        private PopupView UnhidePopup(string popupId) {
            PopupView popupView;
            if (_popupsHidden.TryGetValue(popupId, out popupView)) {
                ToggleBackground(true);
                popupView.transform.SetParent(_popupsDisplay);
                _popupsHidden.Remove(popupId);
                _popupsVisible.Add(popupId, popupView);
            } else {
                Debug.LogError($"Popup NOT found on hidden popups list! ({popupId})");
            }

            return popupView;
        }

        private PopupView CreatePopup(string popupId) {
            GameObject popupToShow = _popupsDatabase.GetPopupById(popupId);
            if (popupToShow != null) {
                ToggleBackground(true);
                GameObject instantiatedObject = Instantiate(popupToShow, _popupsDisplay);
                PopupView popupView = instantiatedObject.GetComponent<PopupView>();
                _popupsVisible.Add(popupId, popupView);

                return popupView;
            } else {
                Debug.LogWarning($"There is no popup related to Id '{popupId}'");
            }

            return null;
        }
        #endregion
    }
}
