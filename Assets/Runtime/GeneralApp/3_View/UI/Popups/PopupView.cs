/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/06/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.View.UI.Popups {
    public abstract class PopupView : MonoBehaviour {
        public abstract void Init();
        public abstract void Open();
        public abstract void Show();
        public abstract void Hide();
        public abstract void Close();
    }
}
