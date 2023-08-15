/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/06/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.GeneralApp.View.UI.Popups {
    public abstract class PopupViewData {

    }

    public abstract class PopupView : MonoBehaviour {

        public event SimpleEventDelegate OnViewShown;
        public event SimpleEventDelegate OnViewHidden;

        public virtual void Init(PopupViewData popupData) { }

        public virtual void Show() {
            OnViewShown?.Invoke();
        }

        public virtual void Hide() {
            OnViewHidden?.Invoke();
        }
    }
}
