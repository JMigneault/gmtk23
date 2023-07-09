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
  public float mmFadeTime = 0.1f;
  float smmT = 0.0f;

  public InstrumentController ic = null;

  public AudioSource music;
  public AudioSource sounds;
  public AudioSource menuMusic;

  public AudioClip mainSong;
  public AudioClip[] mistakeClips;
  public AudioClip mutedClip;

  // Update is called once per frame
  void Update()
  {
    if (!started && ic.generating) {
      if (t >= delay) {
        Debug.Log(music.clip);
        Debug.Log(music.mute);
        music.Play();
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

    if (smmT > 0) {
      const float volume = 1.0f;
      menuMusic.volume = (smmT / mmFadeTime) * volume;
      smmT -= Time.deltaTime;
      if (smmT <= 0) {
        menuMusic.volume = volume;
        menuMusic.Stop();
      }
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
    music.Stop();
    started = false;
    t = 0;
  }

  public void StopMenuMusic() {
    smmT = mmFadeTime;
  }

}
