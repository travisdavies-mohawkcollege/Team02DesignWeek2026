using UnityEngine;

public class Lights : MonoBehaviour
{
    public GameObject darkness;
    public Lever lever;
    public bool lightsOff = false;
    public float timerMax = 3f;
    public float timer = 3f;
    void Start()
    {
        lever = FindFirstObjectByType<Lever>();
        lever.lights.Add(this);
        darkness.SetActive(false);
    }

    public void TurnOffLights()
    {
        darkness.SetActive(true);
        lightsOff = true;
    }

    public void TurnOnLights()
    {
        darkness.SetActive(false);
        lightsOff = false;
    }

    public void Update()
    {
        if (!lightsOff) return;
        else if (lightsOff)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && lightsOff)
        {
            Debug.Log("timer turned off lights");
            lightsOff = false;
            TurnOnLights();
            timer = timerMax;
        }

    }


}
