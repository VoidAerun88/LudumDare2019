using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChunkSystem : MonoBehaviour
{
    [SerializeField]
    private int _emptyBeatMin = -2;
    [SerializeField]    
    private int _bpm = 80;
    [SerializeField]
    private List<Chunk> _serializedChunkPrefabs = new List<Chunk>();

    private Dictionary<string, Chunk> _chunkPrefabs = new Dictionary<string, Chunk>();
    private Dictionary<string, Stack<Chunk>> _pooledChunk = new Dictionary<string, Stack<Chunk>>();
    private RectTransform _transform;
    private int _nextStartSequenceInBeat = -1;
    private float _nextStartSequenceInSeconds = -1f;
    private float _lastChunkStartDate = 0f;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        foreach(var chunk in _serializedChunkPrefabs)
        {
            _chunkPrefabs.Add(System.Guid.NewGuid().ToString(), chunk);
        }
        foreach(var key in _chunkPrefabs.Keys)
        {
            _pooledChunk.Add(key, new Stack<Chunk>());
        }
    }

    private void Update()
    {
        if((Time.time - _lastChunkStartDate) >= _nextStartSequenceInSeconds)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        var chunk = PopPool();
        var rectTransform = chunk.GetComponent<RectTransform>();

        rectTransform.anchoredPosition =  new Vector3(
            Random.Range(0, _transform.rect.width - rectTransform.sizeDelta.x),
            Random.Range(0, _transform.rect.height - rectTransform.sizeDelta.y),
            0f
        );

        _nextStartSequenceInBeat = chunk.BeatTargets.Count + _emptyBeatMin;
        _nextStartSequenceInSeconds = _nextStartSequenceInBeat / (_bpm / 60);
        _lastChunkStartDate = Time.time;
    }

    public void PushPool(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
        _pooledChunk[chunk.Key].Push(chunk);
    }

    public Chunk PopPool() 
    {
        var key = _pooledChunk.Keys.ToList()[Random.Range(0, _pooledChunk.Keys.Count)];
        var chunks = _pooledChunk[key];
        
        Chunk chunk = null; 
        if(chunks.Count == 0)
        {
            var chunkPrefab = _chunkPrefabs[key];
            chunk = Instantiate(chunkPrefab);
            chunk.transform.SetParent(_transform, false);            
        } else
        {
            chunk = chunks.Pop();
        }

        chunk.gameObject.SetActive(true);
        chunk.Init(key, _bpm / 60);
        return chunk;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_emptyBeatMax <= _emptyBeatMin)
        {
            _emptyBeatMax++;
        }
    }
#endif
}
