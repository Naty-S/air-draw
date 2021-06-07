using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSampler : MonoBehaviour
{
    /// <summary>
    /// Maximum ammount of vertex to store
    /// </summary>
    [SerializeField]
    private int _maxVertexBufferSize = 300;

    /// <summary>
    /// How many vertex per second to store
    /// </summary>
    [SerializeField]
    private int _samplingRate = 3;

    /// <summary>
    /// Currently stored vertices 
    /// </summary>
    private Vector3[] _storedVertices;

    /// <summary>
    /// Time passed since the last position sample 
    /// </summary>
    private float _sinceLastSampling = 0.0f;

    /// <summary>
    /// How many vertices are valid ones from the start
    /// </summary>
    private int _validVertex = 0;

    /// <summary>
    /// If in the middle of a sampling process
    /// </summary>
    private bool _sampling = false; 

    // Start is called before the first frame update
    void Start()
    {
        // Init vertices array
        _storedVertices = new Vector3[_maxVertexBufferSize];

        // Init time to wait since the last sampling 
        _sinceLastSampling = 1.0f / _samplingRate;
    }

    // Update is called once per frame
    void Update()
    {
        // @TODO DEBUG ONLY CHANGE FOR A BETTER TRIGGERING SYSTEM
        if (Input.GetKeyDown(KeyCode.Space))
            StartSampling();
    }

    // DBUG ONLY, draw position of valid vertex so far
    private void OnDrawGizmos()
    {
        for (var i = 0; i < _validVertex; i++)
            Gizmos.DrawSphere(_storedVertices[i], 1f);  
    }


    /// <summary>
    /// start a sampling process
    /// </summary>
    private void StartSampling()
    {
        Debug.Log("Starting to sample");

        // If currently sampling, return 
        if (_sampling)
            return;

        // Start sampling
        _sampling = true;
        _validVertex = 0;

        StartCoroutine(Sample());

    }

    IEnumerator Sample()
    {
        // Sanity check
        Debug.Assert(_sampling);
        Debug.Assert(_storedVertices != null);

        // sample position every 1/_samplingRate seconds
        for (var i = 0; i < _maxVertexBufferSize; i++)
        {
            _storedVertices[i] = transform.position;
            _validVertex = i + 1;

            Debug.Log($"Sampled position {_storedVertices[i]}");

            yield return new WaitForSeconds(_sinceLastSampling);
        }

        _sampling = false;
    }

}
