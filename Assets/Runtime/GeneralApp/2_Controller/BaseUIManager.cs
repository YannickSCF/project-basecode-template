/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 **/

/// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// Custom Dependencies
using YannickSCF.GeneralApp.Controller.LoadingPanel;

namespace YannickSCF.GeneralApp.UIManager {
    public class BaseUIManager : MonoBehaviour {

        [SerializeField] protected LoadingPanelController _loadingController;
        [SerializeField] protected Transform _mainUIGameObject;

        public LoadingPanelController LoadingController { get => _loadingController; }
        public Transform MainUIGameObject { get => _mainUIGameObject; }

        // TODO: Queda teerminar mas o menos el UI Manager y popups manager
    }
}
