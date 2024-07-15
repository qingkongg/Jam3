using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorstate;
using System.Drawing;

public class BlockController : MonoBehaviour
{
    public SpriteRenderer MySpriteRenderer;


    public Sprite[] SpriteRed;
    public Sprite[] SpriteGreen;
    public Sprite[] SpriteCyan;
    public Sprite[] SpritePurple;
    // Start is called before the first frame update

    private int colorindex;
    public ColorState Color;
    void Start()
    {
        colorindex = Random.Range(1, 4);
        //初始化随机颜色
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        Color = (ColorState)colorindex;
        ChangeSpriteInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //根据对应颜色更改初始化sprite
    void ChangeSpriteInit()
    {
        if(Color == ColorState.Red)
        {
            MySpriteRenderer.sprite = SpriteRed[0];
        }
        else if(Color == ColorState.Green)
        {
            MySpriteRenderer .sprite = SpriteGreen[0];
        }
        else if (Color == ColorState.Cyan)
        {
            MySpriteRenderer.sprite = SpriteCyan[0];
        }
        else if(Color== ColorState.Purple)
        {
            MySpriteRenderer.sprite= SpritePurple[0];
        }
    }
}
