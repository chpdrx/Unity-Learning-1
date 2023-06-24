using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakaImpact : Items, IInteractable
{
    public override void Stats()
    {
        StatHolder.bakaimpact = true;
    }
}
