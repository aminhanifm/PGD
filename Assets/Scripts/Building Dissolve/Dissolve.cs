using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public Material dissolvemat;
    public Vector2 halfsize;
    public float dist;
    private float maxradius = 2f;
    Renderer rend;
    float scaleX = 1f;
    float scaleY = 1f;
    float globalScale = 2f;

    public Transform origin;
    Renderer originrend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        Vector2 sz = gameObject.GetComponent<SpriteRenderer>().size;
        halfsize = new Vector2(sz.x*transform.localScale.x / 2, sz.y* transform.localScale.y / 2);
        dist = halfsize.magnitude;

        originrend = origin.GetComponent<Renderer>();

        if(sz.x > sz.y)
        {
            scaleX = sz.x / sz.y;
        }
        else
        {
            scaleY = sz.y / sz.x;
        }

        float max = Mathf.Min(sz.x, sz.y);
        globalScale = max/maxradius;


        scaleX *= globalScale;
        scaleY *= globalScale;

        rend.material.SetFloat("_scaleX", scaleX);
        rend.material.SetFloat("_scaleY", scaleY);
        rend.material.SetFloat("_X", 30);
        rend.material.SetFloat("_Y", 30);
    }

    private void Update()
    {
        //if (originrend. != "Objects") return;
        //if (originrend.sortingOrder >= rend.sortingOrder) return;        

        Vector2 pos = new Vector2(transform.position.x - origin.position.x , transform.position.y- origin.position.y );

        if (pos.y > 0) return;

        if (pos.magnitude < Screen.width)
        {
            rend.material.SetFloat("_X", -pos.x*(scaleX/2) / halfsize.x);
            rend.material.SetFloat("_Y", -pos.y*(scaleY/2) / halfsize.y);
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if(dissolvemat != null)
        {
            Graphics.Blit(source, destination, dissolvemat);
        }
    }
}
