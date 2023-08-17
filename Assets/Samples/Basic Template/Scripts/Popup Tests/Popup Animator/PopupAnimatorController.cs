/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/08/2023
 **/

/// Dependencies
using System;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI.Popups;

namespace YannickSCF.GeneralApp.BasicSample.PopupsTests.Popups {
    public class PopupAnimatorData : PopupData {
        private string _title;
        private string _description;

        private Action _closePopupAction;
        private Action<int> _contentPopupAction;

        public PopupAnimatorData(string popupId) : base(popupId) { }
        public PopupAnimatorData(
            string popupId, string title, string description,
            Action closeAction, Action<int> contentPopupAction)
            : base(popupId) {

            _title = title;
            _description = description;
            _closePopupAction = closeAction;
            _contentPopupAction = contentPopupAction;
        }

        public string Title { get => _title; }
        public string Description { get => _description; }
        public Action ClosePopupAction { get => _closePopupAction; }
        public Action<int> ContentPopupAction { get => _contentPopupAction; }
    }

    public class PopupAnimatorController : PopupController {

        private PopupAnimatorView _view;

        private Action _closePopupClicked;
        private Action<int> _contentPopupClicked;

        #region Mono
        private void Awake() {
            _view = GetView<PopupAnimatorView>();
        }

        protected override void OnEnable() {
            base.OnEnable();

            _view.CloseButtonClicked += OnCloseButtonClicked;
            _view.ContentButtonClicked += OnContentButtonClicked;
        }

        protected override void OnDisable() {
            base.OnDisable();

            _view.CloseButtonClicked -= OnCloseButtonClicked;
            _view.ContentButtonClicked -= OnContentButtonClicked;

        }
        #endregion

        #region Event listeners methods
        private void OnCloseButtonClicked() {
            _closePopupClicked?.Invoke();
        }

        private void OnContentButtonClicked(int buttonIndex) {
            _contentPopupClicked?.Invoke(buttonIndex);
        }
        #endregion

        public override void Init(PopupData popupData) {
            PopupId = popupData.PopupId;

            PopupAnimatorViewData viewData = new PopupAnimatorViewData();
            PopupAnimatorData popupAnimatorData = (PopupAnimatorData)popupData;

            viewData.Title = popupAnimatorData.Title;
            viewData.Description = popupAnimatorData.Description;
            _closePopupClicked = popupAnimatorData.ClosePopupAction;
            _contentPopupClicked = popupAnimatorData.ContentPopupAction;

            _view.Init(viewData);
        }
    }
}
