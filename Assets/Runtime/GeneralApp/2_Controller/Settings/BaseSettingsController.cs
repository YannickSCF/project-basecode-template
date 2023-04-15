/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     04/03/2023
 **/

/// Dependencies
using UnityEngine;
using UnityEngine.Localization.Settings;
/// Custom dependencies
using YannickSCF.GeneralApp.View.Settings;
using YannickSCF.GeneralApp.View.Settings.Events;

namespace YannickSCF.GeneralApp.Controller.Settings {
    public class BaseSettingsController : MonoBehaviour {

        [SerializeField] protected GameObject settingsViewPrefab;

        protected BaseSettingsView settingsView;

        #region Mono
        protected virtual void OnEnable() {
            BaseSettingsViewEvents.OnClosePressed += CloseSettingsView;

            BaseSettingsViewEvents.OnLanguageSetted += SetLanguage;
        }

        protected virtual void OnDisable() {
            BaseSettingsViewEvents.OnClosePressed -= CloseSettingsView;

            BaseSettingsViewEvents.OnLanguageSetted -= SetLanguage;
        }
        #endregion

        public virtual void OpenSettingsView(Transform canvasGameObject) {
            settingsView = Instantiate(settingsViewPrefab, canvasGameObject).GetComponent<BaseSettingsView>();
        }

        public virtual void CloseSettingsView() {
            settingsView.Close();
        }

        public virtual void SetLanguage(int languageIndex) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        }
    }
}
