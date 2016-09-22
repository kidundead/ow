using UnityEngine;
using System.Collections;
using Zombie3D;
public class UVAnimationScript : MonoBehaviour
{
    public bool u = true;
    public bool v = true;

    public float scrollSpeed = 1.0f;

    protected float alpha;
    protected float startTime;
    public string alphaPropertyName = "_Alpha";
    public string texturePropertyName = "_MainTex";
    // Use this for initialization
    void Start()
    {

        //Color color = renderer.material.GetColor("_TintColor");
        //alpha = color.a;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        float offset = Time.time * scrollSpeed % 1;
        if (u == true && v == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(offset, offset));
        }
        else if (u == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(offset, 0));
        }
        else if (v == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(0, offset));
        }
        //(1 - (Time - startTime)*0.15f)
        //renderer.material.SetFloat("_Alpha", (0.9f - (Time.time - startTime) * 0.4f));
    }
}
