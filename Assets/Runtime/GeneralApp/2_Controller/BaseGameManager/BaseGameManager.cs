/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial-base).
    /// </summary>
    public partial class BaseGameManager : GlobalSingleton<BaseGameManager> {

        [SerializeField] protected GameObject canvasUI;
    }
}
