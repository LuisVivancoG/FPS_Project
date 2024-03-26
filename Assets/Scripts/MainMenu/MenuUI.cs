using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("HomeScreen")]
    [SerializeField] Text Prompt;
    [SerializeField] float flickrSeconds;
    [SerializeField] float transition;
    bool inputReceived;

    [Space]
    public bool outputReceived;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForInput());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            inputReceived = true;
            Prompt.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForInput()
    {
        float minSize = 25;
        float maxSize = 30;
        float timer = 0;


        while (inputReceived == false)
        {
            float newSize = Mathf.Lerp(minSize, maxSize, Mathf.PingPong(timer, 1f));
            Prompt.fontSize = (int)newSize;
            timer += Time.deltaTime / transition;

            yield return null;
        }
    }
}
