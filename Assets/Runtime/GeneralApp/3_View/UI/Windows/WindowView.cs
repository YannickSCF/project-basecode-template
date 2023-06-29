/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     24/06/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.GeneralApp.View.UI.Windows {
    public class WindowView : MonoBehaviour {

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
