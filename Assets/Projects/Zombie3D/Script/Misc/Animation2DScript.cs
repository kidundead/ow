using UnityEngine;
using System.Collections;
using Zombie3D;
public class Animation2DScript : MonoBehaviour
{
    
    public float frameRate = 0.02f;
    protected int currentIndex = 0;
    public Texture2D[] textures;
    public string texturePropertyName = "_MainTex";

    protected float deltaTime = 0;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > frameRate)
        {
            deltaTime = 0;
            int count = textures.Length;
            currentIndex++;
            
            if (currentIndex >= textures.Length)
            {
                currentIndex = 0;
            }
            GetComponent<Renderer>().material.SetTexture(texturePropertyName, textures[currentIndex]);
        }
       
    }
}
