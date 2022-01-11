using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //offset = new Vector3(1f, 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += offset;
    }
}
