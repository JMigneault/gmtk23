using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour
{

  public float pressInterval = 0.25f;
  public float pressTime = 0.1f;

  public SpriteRenderer pressedKey = null;

  private float t = 0;

    // Update is called once per frame
    void Update()
    {
      if (t >= pressTime && pressedKey != null) {
        pressedKey.enabled = false;
      }

      if (t >= pressInterval) {
        t = 0;
        int i = Random.Range(0, transform.childCount);
        pressedKey = transform.GetChild(i).GetComponent<SpriteRenderer>();
        pressedKey.enabled = true;
      }
    
      t += Time.deltaTime;
    }
}
