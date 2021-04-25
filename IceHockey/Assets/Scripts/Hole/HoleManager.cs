using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    private Vector3 startingScale = new Vector3(1.5f, 0.1f, 1.5f);

    private void Awake()
    {
        transform.localScale = startingScale;
        ChangePosition_x();
    }

    public void IncreaseScale()
    {
        transform.localScale += new Vector3(0.05f, 0, 0.05f);
    }

    public void InitializeScale()
    {
        transform.localScale = startingScale;
    }

    public void ChangePosition_x()
    {
        transform.position = new Vector3(Random.Range(-3.5f, 3.5f), 0.01f, 0f);
    }
}
