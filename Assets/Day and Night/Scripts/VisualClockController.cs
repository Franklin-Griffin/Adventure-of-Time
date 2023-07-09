using UnityEngine;
using System.Collections;

public class VisualClockController : MonoBehaviour
{

    public Transform hourHand;
    public Transform minuteHand;

    float hoursToDegrees = 30f;
    float minsToDegrees = 6f;
    DayAndNightControl controller;

    // Use this for initialization
    void Awake()
    {
        controller = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float currHour = 24 * controller.currentTime;
        float currMin = 60 * (currHour - Mathf.Floor(currHour));

        hourHand.localRotation = Quaternion.Euler(0, 0, currHour * hoursToDegrees);
        minuteHand.localRotation = Quaternion.Euler(0, 0, currMin * minsToDegrees);
    }
}
