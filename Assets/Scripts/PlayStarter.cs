using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStarter : MonoBehaviour
{
  public InstrumentController ic;

  void OnEnable() {
    Debug.Log("Starting play");
    ic.StartGenerating();
  }

  void OnDisable() {
    Debug.Log("Play ending");
    ic.Stop();
    ic.Reset();
  }
}
