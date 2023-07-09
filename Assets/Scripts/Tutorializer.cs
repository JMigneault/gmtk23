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
  private bool done = false;
  public bool muteStrings = false;
  public AudioClip music = null;
  public bool flipped = false;

  public TutorialEvent() { }

  public bool Ready() {
    return noteColors == null || done;
  }

  public void Start(InstrumentController ic, AudioSource a) {
    mt = musicDelay;
    if (text != null) {
      text.SetActive(true);
      text.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().flipX = flipped; // horrible coding; eek!
    }
    if (muteStrings) {
      ic.SetStrangMuted(3, true);
    }
  }

  public void End(InstrumentController ic, AudioSource a) {
    if (text) { text.SetActive(false); }
    if (muteStrings) {
      ic.SetStrangMuted(3, false);
    }
  }

  public void Do(InstrumentController ic, AudioSource a) {
    t -= Time.deltaTime;
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
        done = true;
      }
    }
  }
}

public class Tutorializer : MonoBehaviour
{
  public InstrumentController ic;
  private AudioSource a;
  public TutorialEvent[] events;
  private int i = 0;
  public bool tutorializing = false;
  public GameObject ivoryAlone = null;

  private int realBpm;
  private float realNf;

  void Start() {
    a = GetComponent<AudioSource>();
    ivoryAlone.SetActive(false);

    StartTutorial(); // TODO TEMP
  }

  public void StartTutorial() {
    tutorializing = true;
    ivoryAlone.SetActive(true);
    events[0].Start(ic, a);
    realBpm = ic.bpm;
    realNf = ic.noteFrequency;
    ic.bpm = 240;
    ic.noteFrequency = 1.0f;
  }

  void Update() {
    if (tutorializing) {
      if (events[i].Ready()) {
        if (Input.GetMouseButtonDown(0)) {
          events[i].End(ic, a);
          if (++i >= events.Length) {
            ic.meter.Reset(); // TODO: other reset?
            tutorializing = false;
            ivoryAlone.SetActive(false);
            ic.bpm = realBpm;
            ic.noteFrequency = realNf;
            Debug.Log("TUTORIAL COMPLETE");
            return;
          }
          events[i].Start(ic, a);
        }
      } else {
        events[i].Do(ic, a);
      }
    }
  }
}
