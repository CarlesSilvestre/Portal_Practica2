using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandle : MonoBehaviour
{
    public Sprite[] sprites;
    public Portal bluePortal;
    public Portal orangePortal;

    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bluePortal.Set && orangePortal.Set)
            img.sprite = sprites[0];
        else if (bluePortal.Set && !orangePortal.Set)
            img.sprite = sprites[1];
        else if (!bluePortal.Set && orangePortal.Set)
            img.sprite = sprites[2];
        else img.sprite = sprites[3];

    }
}
