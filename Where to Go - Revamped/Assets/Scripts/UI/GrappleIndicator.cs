using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleIndicator : MonoBehaviour
{
    float targetY;

    // Start is called before the first frame update
    void Start()
    {
        targetY = transform.parent.position.y + 1.2f;
        StartCoroutine("IndicatorAnim");
    }

    IEnumerator IndicatorAnim()
    {
        while(transform.position.y < targetY - 0.2f)
        {
            targetY = transform.parent.position.y + 1.2f;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, 0.01f), 1);
            yield return null;
        }
    }
}
