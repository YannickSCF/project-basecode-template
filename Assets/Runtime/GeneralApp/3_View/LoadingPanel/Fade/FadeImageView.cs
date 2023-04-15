/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.GeneralApp.View.LoadingPanel.Fade {
    public class FadeImageView : FadeView {

        [Header("Fade In/Out Image")]
        [SerializeField] private Image fadeImage;

        #region Abstract methods overrided
        public override bool IsFaded() {
            return fadeImage.gameObject.activeSelf &&
                   fadeImage.color.a >= 1f;
        }

        public override Color GetColor() {
            return fadeImage.color;
        }

        public override void SetColor(Color colorToSet) {
            fadeImage.color = colorToSet;
        }
        #endregion

        #region Virtual methods overrided
        public override void InstantFadeIn() {
            fadeImage.gameObject.SetActive(true);
            base.InstantFadeIn();
        }

        protected override void OnInitFadeIn() {
            base.OnInitFadeIn();
            fadeImage.gameObject.SetActive(true);
        }

        public override void InstantFadeOut() {
            base.InstantFadeOut();
            fadeImage.gameObject.SetActive(false);
        }

        protected override void OnFinishFadeOut() {
            base.OnFinishFadeOut();
            fadeImage.gameObject.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Method to set the fade image
        /// </summary>
        /// <param name="_sprite">New image to show on fade</param>
        public void SetFadeImage(Sprite _sprite) {
            fadeImage.sprite = _sprite;
        }
    }
}
