using UnityEngine;
using System.Collections;

public class EnablePlayerAfterTerrain : MonoBehaviour
{
    public World worldScript;
    public GameObject playerObj;

    private bool waitingToEnablePlayer = false;
    private bool enabledPlayer = false;

    private void Awake()
    {
        worldScript.OnInitialLoadComplete += OnWorldLoadComplete;
    }

    private void OnWorldLoadComplete()
    {
      waitingToEnablePlayer = true;
    }

    private void Update()
    {
        if( waitingToEnablePlayer && !enabledPlayer )
        {
          playerObj.SetActive( true );
          enabledPlayer = true;
          Debug.Log( "time = " + Time.time );
        }
    }
}
