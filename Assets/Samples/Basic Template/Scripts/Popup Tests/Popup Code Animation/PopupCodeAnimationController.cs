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
        public string Title;
        public string Description;

        public Action ClosePopupAction;
        public Action<int> ContentPopupAction;
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
