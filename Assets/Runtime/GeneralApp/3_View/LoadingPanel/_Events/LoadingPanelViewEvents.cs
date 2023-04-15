/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

namespace YannickSCF.GeneralApp.View.LoadingPanel.Events {
    /// <summary>
    /// Simple event class oriented to notify <b>FADE IN</b> and <b> FADE OUT</b> important events.<br/>
    /// In this case there are only two important events for each functionality:<br/>
    /// <list type="bullet">
    /// <item> Start: Executed at the beginning of the animation</item>
    /// <item>Finish: Executed the animation finished</item>
    /// </list>
    /// </summary>
    public class LoadingPanelViewEvents {
        /// --------------------- Delegate types ---------------------
        public delegate void SimpleEventDelegate();


        /// ------------------------- Events -------------------------
        // Fade IN events
        public static event SimpleEventDelegate OnFadeInStarted;
        public static event SimpleEventDelegate OnFadeInFinished;

        // Fade OUT events
        public static event SimpleEventDelegate OnFadeOutStarted;
        public static event SimpleEventDelegate OnFadeOutFinished;


        /// ---------------------- Throw methods ---------------------
        // Fade IN
        public static void ThrowOnFadeInStarted() {
            OnFadeInStarted?.Invoke();
        }
        public static void ThrowOnFadeInFinished() {
            OnFadeInFinished?.Invoke();
        }

        // Fade OUT
        public static void ThrowOnFadeOutStarted() {
            OnFadeOutStarted?.Invoke();
        }
        public static void ThrowOnFadeOutFinished() {
            OnFadeOutFinished?.Invoke();
        }
    }
}
