using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
  public float delay = 3.0f;
  private float t = 0.0f;
  bool started = false;
  bool muted = false;
  public float muteTime = 0.1f;
  float muteT = 0.0f;

  public InstrumentController ic = null;

  public AudioSource music;
  public AudioSource sounds;

  public AudioClip mainSong;
  public AudioClip[] mistakeClips;
  public AudioClip mutedClip;

  // Update is called once per frame
  void Update()
  {
    if (!started && !ic.tutorializer.tutorializing && !ic.menu.menuing) {
      if (t >= delay) {
        GetComponent<AudioSource>().Play();
        started = true;
      }
      t += Time.deltaTime;
    }

    if (muted) {
      if (muteT >= muteTime) {
        muted = false;
        music.mute = false;
      }
      muteT += Time.deltaTime;
    }
  }

  public void PlayMistake() {
    sounds.clip = mistakeClips[Random.Range(0, mistakeClips.Length)];
    sounds.Play();
  }

  public void MuteMusic() {
    muted = true;
    music.mute = true;
    muteT = 0;
    sounds.clip = mutedClip;
    sounds.Play();
  }

  public void Reset() {
    music.clip = mainSong;
    music.volume = 1.0f;
  }
}
