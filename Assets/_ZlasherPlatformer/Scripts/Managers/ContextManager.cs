using UnityEngine;

public class ContextManager : MonoBehaviour
{
    public static ContextManager Instance { get; private set; }
     

    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }



}
