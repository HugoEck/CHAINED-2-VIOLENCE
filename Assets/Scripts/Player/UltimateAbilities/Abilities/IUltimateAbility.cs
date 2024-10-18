using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUltimateAbility
{
    public void UseUltimate();
    public void Activate();
    public void Deactivate();
    public void UpdateUltimateAttack();
}
