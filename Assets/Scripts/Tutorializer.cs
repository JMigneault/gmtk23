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
  public GameObject decoration = null;

  public Sprite[] digits = null;
  public GameObject scoreContainer = null;
  public Transform superScoreContainer = null;
  private GameObject currScoreContainer = null;
  public GameObject digit = null;


  private int realBpm;
  private float realNf;

  void Start() {
    StartTutorial(); // TODO TEMP
  }

  public void StartTutorial() {
    gameObject.SetActive(true);
    if (decoration) {
      decoration.SetActive(true);
    }
    tutorializing = true;
    events[0].Start(ic);
    realBpm = ic.bpm;
    realNf = ic.noteFrequency;
    ic.SetBpm(240);
    ic.noteFrequency = 1.0f;
    ic.music.music.volume = .25f;
  }

  public void SetScore(int score, Transform parentSeq) {
    Debug.Log("Setting score");

    currScoreContainer = Instantiate(scoreContainer, superScoreContainer);
    int remaining = score;

    float xOff = 0.0f;

    while (remaining > 0) {
      int nextDigit = remaining % 10;
      Debug.Log("Rendering digit " + nextDigit);
      GameObject d = Instantiate(digit, currScoreContainer.transform);
      d.transform.localPosition = d.transform.localPosition + new Vector3(xOff, 0, 0);;
      d.GetComponent<SpriteRenderer>().sprite = digits[nextDigit];
      xOff -= 4.5f;
      remaining = remaining / 10;
    }

  }

  void Update() {
    if (tutorializing) {
      if (events[i].Ready()) {
        if (events[i].done || Input.GetMouseButtonDown(0)) {
          events[i].End(ic);
          if (++i >= events.Length) { // tutorial finished
            // RESET
            ic.Reset();
            ic.SetBpm(realBpm);
            ic.noteFrequency = realNf;
            
            ic.menu.gameObject.SetActive(true);
            ic.menu.menuing = true;
            ic.menu.Play();
            tutorializing = false;
            if (decoration) {
              decoration.SetActive(false);
            }
            gameObject.SetActive(false);
            return;
          }
          events[i].Start(ic);
        }
      } else {
        events[i].Do(ic);
      }
    } else {
      if (currScoreContainer) { // SO HACKY
        // Destroy(currScoreContainer);
      }
    }
  }
}
