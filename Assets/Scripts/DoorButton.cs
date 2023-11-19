using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : ButtonFunction
{
    public Animator anim;
    public override void activate()
    {
        anim.SetFloat("Open",1);
    }
}
