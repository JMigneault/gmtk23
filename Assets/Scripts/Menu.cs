using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool menuing = false;

    public GameObject credits = null;
    private bool creditsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        menuing = true;
    }

    public void EnableCredits() {
      creditsEnabled = true;
      credits.SetActive(true);
    }

    void Update() {
      if (creditsEnabled && (Input.GetMouseButtonDown(0)) || Input.GetKeyDown(KeyCode.Escape)) {
        creditsEnabled = false;
        credits.SetActive(false);
      }
    }
}
