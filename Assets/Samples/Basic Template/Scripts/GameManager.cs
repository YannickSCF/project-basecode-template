using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.GeneralApp.Controller.Audio;

namespace YannickSCF.GeneralApp.Sample.Controller {
    public class GameManager : YannickSCF.GeneralApp.Controller.GameManager {
        [SerializeField, Range(0, 1)] private float _aux;

        public override BaseAudioController BaseAudioController => base.BaseAudioController;
    }
}
