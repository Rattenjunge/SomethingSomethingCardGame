using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * Random.value * 360);
    }

    void KillAfterAnimation() {
        Destroy(gameObject);
    }
}
