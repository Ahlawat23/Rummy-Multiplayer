using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceUpandDown : MonoBehaviour
{
    public float timeOfHalfAnimation = 1;
    public float maxY = 60;
    public float minY = 20;


    private void Start()
    {
        BounceUP();
    }

    void BounceUP()
    {
        transform.LeanMoveLocalY(maxY, timeOfHalfAnimation).setEaseInOutSine();
       StartCoroutine(bounceDown());
    }

    IEnumerator bounceDown()
    {
        yield return new WaitForSeconds(timeOfHalfAnimation);
        transform.LeanMoveLocalY(minY, timeOfHalfAnimation).setEaseInOutSine();
        yield return new WaitForSeconds(timeOfHalfAnimation);
        BounceUP();

    }
  
}
