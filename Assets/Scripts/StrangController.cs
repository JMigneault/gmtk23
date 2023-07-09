using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangController : MonoBehaviour
{

    public Sprite mutedSpr;
    public Sprite unmutedSpr;

    public bool muted = false;

    // Start is called before the first frame update
    void Start()
    {
      SetMuted(false);
    }

    public void SetMuted(bool m) {
      muted = m;
      GetComponent<SpriteRenderer>().sprite = m ? mutedSpr : unmutedSpr;
    }
}
