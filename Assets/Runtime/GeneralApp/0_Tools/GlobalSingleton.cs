using UnityEngine;

namespace YannickSCF.GeneralApp {
    public class GlobalSingleton<T> : MonoBehaviour where T : GlobalSingleton<T> {

        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    CreateInstance();
                }
                return _instance;
            }

            private set {
                _instance = Instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                CreateInstance();
            } else if (_instance != this) {
                DestroyImmediate(gameObject);
            }
        }

        private static void CreateInstance() {
            _instance = FindObjectOfType<T>();

            if (_instance == null) {
                GameObject go = new GameObject();
                go.name = typeof(T).Name;
                _instance = go.AddComponent<T>();
            }

            GlobalSingleton<T> obj = _instance as GlobalSingleton<T>;
            DontDestroyOnLoad(obj);
        }
    }
}
