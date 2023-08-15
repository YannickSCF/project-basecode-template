/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/08/2023
 **/

/// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.GeneralApp.BasicSample.PopupsTests.Popups;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI;
using YannickSCF.GeneralApp.Samples.Controller;

namespace YannickSCF.GeneralApp.BasicSample.PopupsTests {
    public class SamplePopups : MonoBehaviour {

        [SerializeField] private Button _popupSimple;
        [SerializeField] private Button _popupAnimator;
        [SerializeField] private Button _popupCodeAnimation;

        private BaseUIController _controller;

        #region Mono
        private void Awake() {
            _controller = GameManager.Instance.BaseUIController;
        }

        private void OnEnable() {
            _popupSimple.onClick.AddListener(ShowPopupSimple);
            _popupAnimator.onClick.AddListener(ShowPopupAnimator);
            _popupCodeAnimation.onClick.AddListener(ShowPopupCodeAnimation);
        }

        private void OnDisable() {
            _popupSimple.onClick.RemoveAllListeners();
            _popupAnimator.onClick.RemoveAllListeners();
            _popupCodeAnimation.onClick.RemoveAllListeners();
        }
        #endregion

        public void ShowPopupByIndex(int popupIndex) {
            switch (popupIndex) {
                case 0: ShowPopupSimple(); break;
                case 1: ShowPopupAnimator(); break;
                case 2: ShowPopupCodeAnimation(); break;
                default: Debug.LogError("Button FAIL"); break;
            }
        }

        #region Simple popup methods
        [ContextMenu("Show Popup Simple")]
        private void ShowPopupSimple() {
            PopupSimpleData simpleData = new PopupSimpleData();

            simpleData.PopupId = "PopupSimple";
            simpleData.Title = "Popup Simple";
            simpleData.Description = "This popup has a simple show/hide. Just enable or disable the game object";
            simpleData.ClosePopupAction = HidePopupSimple;
            simpleData.ContentPopupAction = ShowPopupByIndex;

            _controller.ShowPopup("PopupSimple", simpleData);
        }

        [ContextMenu("Hide Popup Simple")]
        private void HidePopupSimple() {
            _controller.HidePopup("PopupSimple");
        }
        #endregion

        #region Animator popup methods
        [ContextMenu("Show Popup 2")]
        private void ShowPopupAnimator() {
            PopupAnimatorData animatorData = new PopupAnimatorData();

            animatorData.PopupId = "PopupAnimator";
            animatorData.Title = "Popup Animator";
            animatorData.Description = "This popup has an animator show/hide.";
            animatorData.ClosePopupAction = HidePopupAnimator;
            animatorData.ContentPopupAction = ShowPopupByIndex;

            _controller.ShowPopup("PopupAnimator", animatorData);
        }

        [ContextMenu("Hide Popup 2")]
        private void HidePopupAnimator() {
            _controller.HidePopup("PopupAnimator");
        }
        #endregion

        #region Code Animation popup methods
        [ContextMenu("Show Popup 3")]
        private void ShowPopupCodeAnimation() {
            PopupCodeAnimationData codeAnimationData = new PopupCodeAnimationData();

            codeAnimationData.PopupId = "PopupCodeAnimation";
            codeAnimationData.Title = "Popup Code Animation";
            codeAnimationData.Description = "This popup has a code animation show/hide.";
            codeAnimationData.ClosePopupAction = HidePopupCodeAnimation;
            codeAnimationData.ContentPopupAction = ShowPopupByIndex;

            _controller.ShowPopup("PopupCodeAnimation", codeAnimationData);
        }

        [ContextMenu("Hide Popup 3")]
        private void HidePopupCodeAnimation() {
            _controller.HidePopup("PopupCodeAnimation");
        }
        #endregion
    }
}
