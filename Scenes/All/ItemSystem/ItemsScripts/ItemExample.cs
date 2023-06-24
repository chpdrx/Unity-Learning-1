using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExample : Items
{
    public override void Stats()
    {
        StatHolder.Damage += 5.0f;
    }
}
