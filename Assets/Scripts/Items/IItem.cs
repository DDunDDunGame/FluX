using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerStat을 대상으로 하는 아이템
public interface IItem
{
    public void Use(PlayerStat target);
}
