using UnityEngine;
using System.Collections;

public class RefreshPowerups : MonoBehaviour
{

    public Powerup[] powerups;

    void OnEnable()
    {
        foreach (Powerup p in powerups)
            p.Refresh();
    }
}
