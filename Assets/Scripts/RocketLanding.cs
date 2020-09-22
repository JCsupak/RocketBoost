using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLanding : MonoBehaviour
{
    // Cached component references
    LandingPad myLandingPad;
    RocketShip myRocketShip;

    // Start is called before the first frame update
    void Start()
    {
        myRocketShip = GetComponentInParent<RocketShip>();
        myLandingPad = FindObjectOfType<LandingPad>();
    }
    
    //TODO: Add landing struts and vertical alignment correction

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            myRocketShip.SetCurrentState(RocketShip.State.Landing);
            StartCoroutine(myRocketShip.VerticallyCorrectRocket());
            StartCoroutine(myLandingPad.LevelCompleteCelebration());
        }
    }
}
