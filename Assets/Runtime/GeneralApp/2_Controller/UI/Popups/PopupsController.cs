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
    public sealed class PopupsController : MonoBehaviour {

        [Header("Database")]
        [SerializeField] private PopupDatabase _popupsDatabase;

        [Header("Popups displays")]
        [SerializeField] private Transform _popupsDisplay;
        [SerializeField] private Transform _hiddenPopupsDisplay;

        [Header("Background values")]
        [SerializeField] private Image _popupBackground;
        [SerializeField, Range(0f, 1f)] private float _backgroundActiveAlpha = 0.5f;
        [SerializeField, Range(0f, 3f)] private float _timeToSetFinalBackground = 0f;

        private Dictionary<string, object> _popupsVisible;
        private Dictionary<string, object> _popupsHidden;

        private Coroutine _backgroundCoroutine = null;

        #region Mono
        private void Awake() {
            _popupsVisible = new Dictionary<string, object>();
            _popupsHidden = new Dictionary<string, object>();

            _popupBackground.gameObject.SetActive(false);
            _popupBackground.color = new Color(
                _popupBackground.color.r,
                _popupBackground.color.g,
                _popupBackground.color.b,
                0f);
        }
        #endregion

        public T ShowPopup<T, U>(string popupId) where T : PopupController<U> where U : PopupView {
            T popup = null;
            if (_popupsDatabase != null) {
                if (_popupsHidden.ContainsKey(popupId)) {
                    popup = UnhidePopup<T, U>(popupId);
                } else {
                    popup = CreatePopup<T, U>(popupId);
                }
            } else {
                Debug.LogError("No Popup Database selected!");
            }

            return popup;
        }

        public void HidePopup<T, U>(string popupId) where T : PopupController<U> where U : PopupView {
            if (_popupsVisible.ContainsKey(popupId)) {
                if (_popupsVisible.TryGetValue(popupId, out object objectToHide)) {
                    T popupToHide = objectToHide as T;
                    
                    ToggleBackground(false);

                    _popupsVisible.Remove(popupId);
                    _popupsHidden.Add(popupId, popupToHide);
                    popupToHide.Hide(OnPopupHidden);
                } else {
                    Debug.LogError($"Popup NOT found on visible popups list! ({popupId})");
                }
            } else {
                Debug.LogWarning($"Popup '{popupId}' not found on visible area!");
            }
        }
        private void OnPopupHidden<T>(PopupController<T> popupToHide, string popupId) where T : PopupView {
            popupToHide.transform.SetParent(_hiddenPopupsDisplay);
        }

        public void ClosePopup<T, U>(string popupId) where T : PopupController<U> where U : PopupView {
            T popupToClose = null;
            object objectToClose;
            if (_popupsVisible.ContainsKey(popupId)) {
                if (_popupsVisible.TryGetValue(popupId, out objectToClose)) {
                    popupToClose = objectToClose as T;

                    ToggleBackground(false);
                    
                    _popupsVisible.Remove(popupId);
                    popupToClose.Close();
                    return;
                } else {
                    Debug.LogWarning($"Popup '{popupId}' NOT found on visible popups list!");
                }
            }

            if (_popupsHidden.ContainsKey(popupId)) {
                if (_popupsHidden.TryGetValue(popupId, out objectToClose)) {
                    popupToClose = objectToClose as T;

                    ToggleBackground(false);
                    
                    _popupsHidden.Remove(popupId);
                    popupToClose.Close();
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

        private T UnhidePopup<T, U>(string popupId) where T : PopupController<U> where U : PopupView {
            T popupToUnhide = null;
            if (_popupsHidden.TryGetValue(popupId, out object objectToUnhide)) {
                popupToUnhide = objectToUnhide as T;

                ToggleBackground(true);

                _popupsHidden.Remove(popupId);
                _popupsVisible.Add(popupId, popupToUnhide);
                popupToUnhide.Show(OnPopupShown);
            } else {
                Debug.LogError($"Popup NOT found on hidden popups list! ({popupId})");
            }

            return popupToUnhide;
        }
        private void OnPopupShown<T>(PopupController<T> popupToUnhide, string popupId) where T : PopupView {
            popupToUnhide.transform.SetParent(_popupsDisplay);
        }

        private T CreatePopup<T, U>(string popupId) where T : PopupController<U> where U : PopupView {
            GameObject popupToShow = _popupsDatabase.GetPopupById(popupId);
            if (popupToShow != null) {
                ToggleBackground(true);
                GameObject instantiatedObject = Instantiate(popupToShow, _popupsDisplay);
                T popup = instantiatedObject.GetComponent<T>();

                _popupsVisible.Add(popupId, popup as PopupController<PopupView>);
                popup.Init(popupId);
                popup.Open();

                return popup;
            } else {
                Debug.LogWarning($"There is no popup related to Id '{popupId}'");
            }

            return null;
        }
        #endregion
    }
}
