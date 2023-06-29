/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/06/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.GeneralApp.View.UI.Popups {
    public abstract class PopupView : MonoBehaviour {

        public event SimpleEventDelegate OnViewShown;
        public event SimpleEventDelegate OnViewHidden;

        public virtual void Init() { }
        public virtual void Open() {
            gameObject.SetActive(true);
        }
        public virtual void Show() {
            OnViewShown?.Invoke();
        }
        public virtual void Hide() {
            OnViewHidden?.Invoke();
        }
        public virtual void Close() {
            Destroy(gameObject);
        }
    }
}
