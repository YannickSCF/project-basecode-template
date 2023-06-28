/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/06/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.View.UI.Popups {
    public abstract class PopupView : MonoBehaviour {
        public virtual void Init() { }
        public virtual void Open() {
            gameObject.SetActive(true);
        }
        public virtual void Show() {
            gameObject.SetActive(true);
        }
        public virtual void Hide() {
            gameObject.SetActive(false);
        }
        public virtual void Close() {
            Destroy(gameObject);
        }
    }
}
