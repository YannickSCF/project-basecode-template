/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     24/06/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.View.UI.Windows {
    public abstract class WindowsView : MonoBehaviour {
        public abstract void Init();
        public abstract void Open();
        public abstract void Close();
    }
}
