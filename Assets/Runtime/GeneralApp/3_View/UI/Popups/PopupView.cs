/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/06/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies

namespace YannickSCF.GeneralApp.View.UI.Popups {
    public abstract class PopupView : MonoBehaviour {
        public abstract void Init();
        public abstract void Open();
        public abstract void Close();
    }
}
