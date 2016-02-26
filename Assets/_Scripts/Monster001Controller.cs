using UnityEngine;
using System.Collections;

public class Monster001Controller : MonoBehaviour
{
    private Vector3 pos1 = new Vector3(-30, -178, 0);
    private Vector3 pos2 = new Vector3(350, -178, 0);
    private float secondsForOneLength = 3.0f;

    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time / secondsForOneLength, 1f));
    }
}
