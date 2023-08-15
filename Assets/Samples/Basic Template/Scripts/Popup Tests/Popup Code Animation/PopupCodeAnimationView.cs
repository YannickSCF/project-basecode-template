/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/08/2023
 **/

/// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.Popups;

namespace YannickSCF.GeneralApp.BasicSample.PopupsTests.Popups {
    public class PopupCodeAnimationViewData : PopupViewData {
        public string Title;
        public string Description;
    }

    public class PopupCodeAnimationView : PopupView {

        public event CommonEventsDelegates.SimpleEventDelegate CloseButtonClicked;
        public event CommonEventsDelegates.IntegerEventDelegate ContentButtonClicked;

        [SerializeField] private Button _close;

        [Header("Content")]
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;
        [SerializeField] private List<Button> _buttons;
        [Header("Extra settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _waitShowTime;
        [SerializeField] private float _waitHideTime;

        #region Mono
        private void OnEnable() {
            _close.onClick.AddListener(() => CloseButtonClicked?.Invoke());
            for (int i = 0; i < _buttons.Count; ++i) {
                int indexButton = i;
                _buttons[i].onClick.AddListener(delegate { OnContentButtonClicked(indexButton); });
            }
        }

        private void OnDisable() {
            _close.onClick.RemoveAllListeners();
            for (int i = 0; i < _buttons.Count; ++i) {
                _buttons[i].onClick.RemoveAllListeners();
            }
        }
        #endregion

        #region Event Listener methods
        private void OnContentButtonClicked(int index) {
            ContentButtonClicked?.Invoke(index);
        }
        #endregion

        public override void Init(PopupViewData popupData) {
            PopupCodeAnimationViewData popupCodeAnimationData = (PopupCodeAnimationViewData)popupData;

            _title.text = popupCodeAnimationData.Title;
            _description.text = popupCodeAnimationData.Description;
        }

        public override void Show() {
            StartCoroutine(WaitToEndShow());
        }

        private IEnumerator WaitToEndShow() {
            yield return FadeCanvasGroup(true);
            base.Show();
        }

        public override void Hide() {
            StartCoroutine(WaitToEndHide());
        }

        private IEnumerator WaitToEndHide() {
            yield return FadeCanvasGroup(false);
            base.Hide();
        }

        private IEnumerator FadeCanvasGroup(bool fadeType) {
            float _cTime = 0;
            float _time = fadeType ? _waitShowTime : _waitHideTime;
            while (_cTime < _time) {
                _canvasGroup.alpha = Mathf.Lerp(fadeType ? 0f : 1f, fadeType ? 1f : 0f, _cTime / _time);
                _cTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _canvasGroup.alpha = fadeType ? 1f : 0f;
        }
    }
}
