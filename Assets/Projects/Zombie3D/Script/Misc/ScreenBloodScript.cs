using UnityEngine;
using System.Collections;
using Zombie3D;
public class ScreenBloodScript : MonoBehaviour
{

    public float scrollSpeed = 1.0f;

    protected float alpha;
    protected float startTime;
    protected float deltaTime = 0f;
    public string alphaPropertyName = "_Alpha";
    // Use this for initialization
    void Start()
    {
        alpha = GetComponent<Renderer>().material.GetFloat(alphaPropertyName);
        startTime = Time.time;
    }


    public void NewBlood(float damage)
    {

        //Transform t = transform.Find("Screen_BloodBack");
        //float x = Random.Range(-4f, 4f);
        //float y = Random.Range(-4f, 4f);
        //transform.localPosition = new Vector3(x, y, 20f);
        GetComponent<Renderer>().enabled = true;
        alpha = damage;
        alpha = Mathf.Clamp(alpha, 0f, 1f);


    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < 0.03f)
        {
            return;
        }

        alpha -= 0.5f * deltaTime;
        if (alpha <= 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
        GetComponent<Renderer>().material.SetFloat(alphaPropertyName, alpha);
        deltaTime = 0f;

    }
}
