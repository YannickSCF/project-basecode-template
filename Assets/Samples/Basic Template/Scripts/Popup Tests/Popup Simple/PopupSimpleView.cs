/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/08/2023
 **/

/// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.Popups;

namespace YannickSCF.GeneralApp.BasicSample.PopupsTests.Popups {
    public class PopupSimpleViewData : PopupViewData {
        public string Title;
        public string Description;
    }

    public class PopupSimpleView : PopupView {

        public event CommonEventsDelegates.SimpleEventDelegate CloseButtonClicked;
        public event CommonEventsDelegates.IntegerEventDelegate ContentButtonClicked;

        [SerializeField] private Button _close;

        [Header("Content")]
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;
        [SerializeField] private List<Button> _buttons;

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
            PopupSimpleViewData popupOneData = (PopupSimpleViewData)popupData;

            _title.text = popupOneData.Title;
            _description.text = popupOneData.Description;
        }

        public override void Show() {
            gameObject.SetActive(true);
            base.Show();
        }

        public override void Hide() {
            gameObject.SetActive(false);
            base.Hide();
        }
    }
}
