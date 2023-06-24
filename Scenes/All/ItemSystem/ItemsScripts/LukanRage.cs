using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LukanRage : Items, IInteractable
{
    public override void Stats()
    {
        StatHolder.lukanrage = true;
    }
}
