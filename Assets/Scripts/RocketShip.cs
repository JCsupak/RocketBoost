using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RocketShip : MonoBehaviour
{
    public enum State { Alive, Dying, Landing};

    // Configuration parameters
    public AudioClip mainEngineSFX;
    public AudioClip[] deathExplosionsSFX;
    public AudioClip winJingleSFX;
    public ParticleSystem[] explosions;
    public ParticleSystem[] rocketJets;
    float rocketThrust = 1500f;
    float maneuveringThrust = 150.0f;
    float maxFuelLevel = 400;
    float currentFuelLevel;
    float fuelBurnRate = 0.1f;
    float rocketRefuelRate = 3.0f;
    float explosionVolume = 0.75f;
    float winJingleVolume = 0.3f;
     
    // State parameters
    State currentState;

    // Cached component references
    Rigidbody myRigidBody;
    IEnumerator fuelCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        fuelCoroutine = RefuelRocket();
        currentFuelLevel = maxFuelLevel;
        currentState = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == State.Alive)
        {
            Maneuver();
            Boost();
        }
        else if(currentState == State.Landing)
        {
            AudioManager.Instance.PlayAudioClip(winJingleSFX, winJingleVolume);
        }

    }


    public void Maneuver()
    {
        float maneuveringPower = Input.GetAxis("Horizontal");
        if(Mathf.Abs(maneuveringPower) > Mathf.Epsilon)
        {
            float maneuveringForce = -maneuveringPower * maneuveringThrust * Time.deltaTime;
            Vector3 maneuveringVector = new Vector3(0, 0, maneuveringForce);
            myRigidBody.angularVelocity = maneuveringVector;
        }
    }

    public void Boost()
    {
        float boostPower = Input.GetAxis("Boost");
        if (boostPower > Mathf.Epsilon)
        {
            StopCoroutine(fuelCoroutine);
            BurnFuel();

            if (currentFuelLevel > 0)
            {
                AddRocketForce(boostPower);
                FireThrustRockets();
            }
            
            if (!AudioManager.Instance.GetIsPlaying())
            {
                AudioManager.Instance.PlayAudioClip(mainEngineSFX);
            }
        }
        else
        {
            StopThrustRockets();
            AudioManager.Instance.StopAudio();
        }
    }

    private void AddRocketForce(float power)
    {
        float rocketForce = rocketThrust * power;
        Vector3 thrustVector = Vector3.up * rocketForce * Time.deltaTime;
        myRigidBody.AddRelativeForce(thrustVector);
    }

    private void FireThrustRockets()
    {
        for(int i = 0; i < rocketJets.Length; i++)
        {
            rocketJets[i].gameObject.SetActive(true);
        }
    }

    private void StopThrustRockets()
    {
        for(int i = 0; i < rocketJets.Length; i++)
        {
            rocketJets[i].gameObject.SetActive(false);
        }
    }

    IEnumerator RefuelRocket()
    {
        while(true)
        {
            if(currentFuelLevel < maxFuelLevel)
            {
                currentFuelLevel += rocketRefuelRate;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BurnFuel()
    {
        currentFuelLevel -= fuelBurnRate;
    }

    public float GetMaxFuelLevel()
    {
        return maxFuelLevel;
    }

    public float GetCurrentFuelLevel()
    {
        return currentFuelLevel;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(State newState)
    {
        currentState = newState;
    }

    public IEnumerator VerticallyCorrectRocket()
    {
        myRigidBody.angularVelocity = Vector3.zero;
        while (true)
        {
            Debug.Log(transform.rotation.z);
            if(transform.rotation.z < -0.01f)
            {
                transform.Rotate(0,0, 2f, Space.World);
            }
            else if(transform.rotation.z > 0.01f)
            {
                transform.Rotate(0, 0, -2f, Space.World);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void KillRocket()
    {
        AudioManager.Instance.StopAudio();
        StopThrustRockets();
        currentState = State.Dying;
        StartCoroutine(DeathExplosions());
    }

    IEnumerator DeathExplosions()
    {
        for(int i = 0; i < explosions.Length; i++)
        {
            PlayRandomExplosion();
            explosions[i].Play();
            yield return new WaitForSeconds(0.5f);
        }
        GameManager.Instance.DecreaseLives();
    }

    private void PlayRandomExplosion()
    {
        int randIndex = Random.Range(0, deathExplosionsSFX.Length);
        AudioClip randClip = deathExplosionsSFX[randIndex];
        AudioManager.Instance.PlayAudioClip(randClip, explosionVolume);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(currentState == State.Alive)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    break;
                case "Fuel":
                    StartCoroutine(fuelCoroutine);
                    break;
                default:
                    KillRocket();
                    break;
            }
        }
    }
}
