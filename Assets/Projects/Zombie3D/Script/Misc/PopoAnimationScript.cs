using UnityEngine;
using System.Collections;
using Zombie3D;
public class PopoAnimationScript : MonoBehaviour
{
    public bool u = true;
    public bool v = true;

    public float scrollSpeed = 1.0f;
    public float scaleSpeed = 0.1f;
    public string alphaPropertyName = "_Alpha";
    public string texturePropertyName = "_MainTex";
    protected float rndUV;
    protected int rndScale;
    protected float alpha;
    protected float startTime;
    protected float currentScale;
    // Use this for initialization
    void Start()
    {
        alpha = GetComponent<Renderer>().material.GetFloat(alphaPropertyName);
        startTime = Time.time;
        rndUV = Random.Range(0.0f, 1.0f);
        rndScale = Random.Range(2, 5);

    }

    // Update is called once per frame
    void Update()
    {

        float offset = (Time.time * scrollSpeed + rndUV) % 1;
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
        currentScale += Time.deltaTime * scaleSpeed;
        currentScale = Mathf.Clamp(currentScale, 0.01f, 0.1f + 0.1f * rndScale);
        transform.localScale = currentScale * new Vector3(1, 1, 1);


    }
}
