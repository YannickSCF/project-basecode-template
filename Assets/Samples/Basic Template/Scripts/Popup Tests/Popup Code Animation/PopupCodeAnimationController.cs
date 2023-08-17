/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/08/2023
 **/

/// Dependencies
using System;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI.Popups;

namespace YannickSCF.GeneralApp.BasicSample.PopupsTests.Popups {
    public class PopupCodeAnimationData : PopupData {
        private string _title;
        private string _description;

        private Action _closePopupAction;
        private Action<int> _contentPopupAction;

        public PopupCodeAnimationData(string popupId) : base(popupId) { }
        public PopupCodeAnimationData(
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

    public class PopupCodeAnimationController : PopupController {

        private PopupCodeAnimationView _view;

        private Action _closePopupClicked;
        private Action<int> _contentPopupClicked;

        #region Mono
        private void Awake() {
            _view = GetView<PopupCodeAnimationView>();
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

            PopupCodeAnimationViewData viewData = new PopupCodeAnimationViewData();
            PopupCodeAnimationData popupCodeAnimationData = (PopupCodeAnimationData)popupData;

            viewData.Title = popupCodeAnimationData.Title;
            viewData.Description = popupCodeAnimationData.Description;
            _closePopupClicked = popupCodeAnimationData.ClosePopupAction;
            _contentPopupClicked = popupCodeAnimationData.ContentPopupAction;

            _view.Init(viewData);
        }
    }
}
