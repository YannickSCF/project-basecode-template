/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     24/06/2023
 **/

/// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YannickSCF.GeneralApp.Scriptables.Window {
    [CreateAssetMenu(fileName = "Window Database", menuName = "Scriptable Objects/YannickSCF/Windows Database")]
    public class WindowsDatabase : ScriptableObject {

        [Header("Windows list")]
        [SerializeField] private List<WindowData> _allWindows;

        public GameObject GetWindowById(string windowId) {
            return _allWindows.FirstOrDefault(x => x.WindowId.Equals(windowId)).Window;
        }

        [Serializable]
        private class WindowData {
            [SerializeField] private string _windowId;
            [SerializeField] private GameObject _window;

            public string WindowId { get => _windowId; }
            public GameObject Window { get => _window; }
        }
    }
}
