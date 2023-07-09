using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Gaming gaming;
    public bool shouldDeplete => gaming.isPossessing;

    float PERCENT = 1.0f;
    public float healrate = 0.1f;
    public float depleterate = 0.2f;

    public RectTransform barbg;
    public RectTransform bar;
    float barmaxpx;

    public UnityEvent onDeplete;

    void Start()
    {
        barmaxpx = barbg.sizeDelta.x;
        gaming = FindObjectOfType<Gaming>();
    }


    // Update is called once per frame
    void Update()
    {
        if (shouldDeplete)
        {
            PERCENT -= depleterate * Time.deltaTime;
        }
        else
        {
            PERCENT += healrate * Time.deltaTime;
        }

        PERCENT = Mathf.Clamp(PERCENT, 0, 1);
        bar.offsetMax = new Vector2(-(1-PERCENT) * barmaxpx, bar.offsetMax.y);
        //bar.offsetMin = new Vector2(percent * barmaxpx, bar.offsetMin.y);

        if (PERCENT == 0)
        {
            onDeplete.Invoke();
        }
    }
}
