/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     04/03/2023
 **/

namespace YannickSCF.GeneralApp.View.Settings.Events {
    /// <summary>
    /// Simple eevent class oriented to notify basic settings changes.<br/>
    /// <list type="bullet">
    /// <item>Mute audio sources (General, Music, SFX)</item>
    /// <item>Change audio source volumes (General, Music, SFX)</item>
    /// <item>Change system language</item>
    /// </list>
    /// </summary>
    public abstract class BaseSettingsViewEvents {

        /// --------------------- Delegate types ---------------------
        public delegate void EmptyEventDelegate();
        public delegate void BoolEventDelegate(AudioSources source, bool newValue);
        public delegate void FloatEventDelegate(AudioSources source, float newValue);
        public delegate void LanguageEventDelegate(int languageIndex);


        /// ------------------------- Events -------------------------
        public static event EmptyEventDelegate OnClosePressed;
        public static event BoolEventDelegate OnSourceMuted;
        public static event FloatEventDelegate OnVolumeChanged;
        public static event LanguageEventDelegate OnLanguageSetted;


        /// ---------------------- Throw methods ---------------------
        public static void ThrowClosePressed() {
            OnClosePressed?.Invoke();
        }
        public static void ThrowOnSourceMuted(AudioSources _source, bool _newValue) {
            OnSourceMuted?.Invoke(_source, _newValue);
        }
        public static void ThrowOnVolumeChanged(AudioSources _source, float _newValue) {
            OnVolumeChanged?.Invoke(_source, _newValue);
        }
        public static void ThrowOnLanguageSetted(int _languageIndex) {
            OnLanguageSetted?.Invoke(_languageIndex);
        }
    }
}
