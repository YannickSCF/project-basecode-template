/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.View.UI.LoadingPanel.Fade {
    public class FadeVolumeView : FadeView {

        // TODO: All showTextUpdates class
        #region Abstract methods overrided
        public override bool IsFaded() {
            throw new System.NotImplementedException();
        }

        public override Color GetColor() {
            throw new System.NotImplementedException();
        }

        public override void SetColor(Color colorToSet) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
