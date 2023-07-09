using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. generate notes
//    a. notes have a string index, a color, a beat, 
// 2. take user input and remember which strings 
// 3. detect when they hit 0; check whether it was muted and whether it should have been
//    a. buffer around 0 in the future?

public enum NColor {
  YELLOW,
  BLUE,
  GREEN,
  RED
}

public class InstrumentController : MonoBehaviour
{
    public int bpm = 100;
    public int nBeats = 12; // TODO: should maybe work in 16th notes instead of beats...
    public float noteFrequency = 1f; // every beat; TODO: orchestrate actual level
    public float totalLength = 55f; // 100 units length
    public float strangHeight = 2f;

    public GameObject notePrefab;
    public float correctProb = .8f;
    
    public GameObject[] strangObjs;
    public KeyCode[] strangKeys;
    public NColor[] strangColors;
    private bool[] strangMuted;

    private float bps;
    private static float lengthPerBeat;
    private static float noteSpeed;

    private int currBeat = 0;
    private float beatFrac = 0f;

    private int numCorrect = 0;
    private int totalNum = 0;

    public Meter meter;

    public int pcMeter = 1;
    public int mcMeter = 10;
    public int piMeter = 5;
    public int miMeter = 5;

    public Tutorializer tutorializer = null;
    public MusicController music = null;

    // Our instrument has strings, but C# doesn't have variables named "string". So in this fun alternate universe
    // our instrument has "strangs".
    public void MakeNote(int strang, NColor col, bool abridged) {
      GameObject note = Instantiate(notePrefab, transform);
      NoteController nc = note.GetComponent<NoteController>();
      nc.Init(strang, col, this);
      if (abridged) {
        nc.transform.position = new Vector3(totalLength / 2.0f, nc.transform.position.y, nc.transform.position.z);
      }
    }

    void GenerateRandomNote() {
      bool correctNote = Random.Range(0.0f, 1.0f) < correctProb;
      int strang = Random.Range(0, 4);
      if (correctNote) {
        MakeNote(strang, (NColor) strang, false);
      } else {
        NColor col = (NColor) Random.Range(0, 4);
        while (strang == (int)col) {
          col = (NColor) Random.Range(0, 4);
        }
        MakeNote(strang, col, false);
      }
    }

    public float getNoteSpeed() {
      return noteSpeed;
    }

    public void PlayNote(NoteController n) {
      bool shouldPlay = n.color == strangColors[n.strang];
      bool didPlay = !strangMuted[n.strang];
      if (shouldPlay && didPlay) { // note played correctly
        n.StartPlayedCorrectlyAnim();
        meter.Correct(pcMeter);
      } else if (!shouldPlay && !didPlay) { // note muted correctly
        n.StartMutedCorrectlyAnim();
        meter.Correct(mcMeter);
      } else if (shouldPlay && !didPlay) { // note muted incorrectly
        n.StartMutedIncorrectlyAnim();
        meter.Incorrect(miMeter);
        music.MuteMusic();
      } else if (!shouldPlay && didPlay) { // note played incorrectly
        n.StartPlayedIncorrectlyAnim();
        meter.Incorrect(piMeter);
        music.PlayMistake();
      }

      totalNum++;
      if (shouldPlay == didPlay) {
        numCorrect++;
      }
      // Debug.Log(numCorrect + "/" + totalNum);
    }

    public void SetStrangMuted(int strang, bool muted) {
      strangMuted[strang] = muted;
      strangObjs[strang].GetComponent<StrangController>().SetMuted(muted);
    }

    // Start is called before the first frame update
    void Start()
    {
      strangMuted = new bool[4];
      lengthPerBeat = totalLength / nBeats;
      bps = bpm / 60f;
      noteSpeed = bps * lengthPerBeat;
    }

    // Update is called once per frame
    void Update()
    {
      if (!tutorializer.tutorializing) {
        beatFrac += (Time.deltaTime * bps);
        if (beatFrac >= 1.0f) {
          beatFrac -= 1.0f;
          currBeat += 1;
          if (currBeat % noteFrequency == 0) {
            GenerateRandomNote();
          }
        }

        for (int i = 0; i < strangMuted.Length; i++) {
          bool wasMuted = strangMuted[i];
          SetStrangMuted(i, Input.GetKey(strangKeys[i]));
        }
      }
    }
}
