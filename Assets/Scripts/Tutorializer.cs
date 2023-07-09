using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class TutorialEvent {
  public GameObject text = null;
  public int[] noteStrangs = null;
  public NColor[] noteColors = null;
  private int currNote = 0;
  public float noteDelay = 0.5f;
  public float musicDelay = 0.5f;
  public float finalDelay = 2.0f;
  private float t = 0.0f;
  private float mt = 0.0f;
  public bool done = false;
  public bool muteStrings = false;
  public AudioClip music = null;
  public bool flipped = false;

  public TutorialEvent() { }

  public bool Ready() {
    return noteColors.Length == 0 || done;
  }

  public void Start(InstrumentController ic) {
    mt = musicDelay;
    if (text != null) {
      text.SetActive(true);
      text.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().flipX = flipped; // horrible coding; eek!
    }
    if (muteStrings) {
      ic.SetStrangMuted(3, true);
    }
  }

  public void End(InstrumentController ic) {
    if (text) { text.SetActive(false); }
    if (muteStrings) {
      ic.SetStrangMuted(3, false);
    }
  }

  public void Do(InstrumentController ic) {
    Debug.Log("do");
    t -= Time.deltaTime;
    AudioSource a = ic.music.music;
    if (music != null && mt >= 0) {
      mt -= Time.deltaTime;
      if (mt < 0) {
        a.clip = music;
        a.Play();
      }
    }
    if (t <= 0.0f) {
      if (currNote < noteColors.Length) {
        ic.MakeNote(noteStrangs[currNote], noteColors[currNote], /* abridged */ true);
        if (++currNote == noteColors.Length) {
          t = finalDelay;
        } else {
          t = noteDelay;
        }
      } else {
        if (noteColors.Length > 0) {
          done = true;
        }
      }
    }
  }
}

public class Tutorializer : MonoBehaviour
{
  public InstrumentController ic;
  public TutorialEvent[] events;
  private int i = 0;
  public bool tutorializing = false;

  private int realBpm;
  private float realNf;

  void Start() {
    StartTutorial(); // TODO TEMP
  }

  public void StartTutorial() {
    gameObject.SetActive(true);
    tutorializing = true;
    events[0].Start(ic);
    realBpm = ic.bpm;
    realNf = ic.noteFrequency;
    ic.SetBpm(240);
    ic.noteFrequency = 1.0f;
    ic.music.music.volume = .25f;
  }

  void Update() {
    if (tutorializing) {
      if (events[i].Ready()) {
        if (events[i].done || Input.GetMouseButtonDown(0)) {
          events[i].End(ic);
          if (++i >= events.Length) { // tutorial finished
            // RESET
            ic.meter.Reset(); // TODO: other reset?
            ic.music.Reset();
            ic.SetBpm(realBpm);
            ic.noteFrequency = realNf;
            
            ic.menu.gameObject.SetActive(true);
            ic.menu.menuing = true;
            tutorializing = false;
            gameObject.SetActive(false);
            return;
          }
          events[i].Start(ic);
        }
      } else {
        events[i].Do(ic);
      }
    }
  }
}
