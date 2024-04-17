using System.Collections.Generic;
using UnityEngine;

public class LevelSectionParallax : Parallax
{
    #region Serialized fields
    [SerializeField] private float offset = -2f;

    [SerializeField] private float initialWidth = 4f;

    [SerializeField] private ParallaxObject[] props = null;
    #endregion

    #region Attributes
    private float currentWidth;

    private Dictionary<ParallaxObject, Queue<ParallaxObject>> pools = new Dictionary<ParallaxObject, Queue<ParallaxObject>>();

    private Dictionary<ParallaxObject, ParallaxObject> instanceToPrefab = new Dictionary<ParallaxObject, ParallaxObject>();

    private Queue<ParallaxObject> instances = new Queue<ParallaxObject>();

    private int previousIndex = -1;
    #endregion

    #region API
    public float CurrentEnd { get; private set; }

    public float CurrentPosition;

    public void SetWidth(float newWidth)
    {
        currentWidth = newWidth;
    }

    public void EnqueueObject(ParallaxObject prefab, float offset)
    {
        ParallaxObject instance = InstantiateProps(prefab);
        instances.Enqueue(instance);

        instance.transform.localPosition = Vector3.right * offset;

        CurrentEnd = offset + instance.Width;
    }

    public void FillUpTo(float offset, bool canOverflow = true)
    {
        // TODO overflow
        Fill(offset);
    }
    #endregion

    #region Unity methods
    protected override void Awake()
    {
        base.Awake();

        Scrolling = true;

        currentWidth = initialWidth;
        CurrentEnd = -offset;
    }

    private void Update()
    {
        if (Scrolling)
        {
            float f = speed * SpeedFactor;

            CurrentEnd -= f * Time.deltaTime;

            foreach (ParallaxObject parallaxObject in instances)
            {
                parallaxObject.Move(f);
            }
        }

        if (Spawning)
            Fill();

        if (instances.TryPeek(out ParallaxObject props))
        {
            if (props.transform.localPosition.x + props.Width < 0)
            {
                instances.Dequeue();
                RemoveProps(props);
            }
        }

        
    }
    #endregion

    #region Private
    private void Fill()
    {
        while (CurrentEnd < currentWidth)
            AddObjectToCurrentOffset();
    }

    private void Fill(float offset)
    {
        while (CurrentEnd < offset)
            AddObjectToCurrentOffset();
    }

    private void AddObjectToCurrentOffset()
    {
        int randomIndex = Random.Range(0, props.Length);

        if (randomIndex == previousIndex)
            randomIndex = (randomIndex + 1) % props.Length;

        previousIndex = randomIndex;

        ParallaxObject randomProps = props[randomIndex];
        ParallaxObject instanciatedProps = InstantiateProps(randomProps);

        instances.Enqueue(instanciatedProps);
        instanciatedProps.transform.localPosition = Vector3.right * CurrentEnd;

        CurrentEnd += randomProps.Width;
    }

    private ParallaxObject InstantiateProps(ParallaxObject prefab)
    {
        if (pools.TryGetValue(prefab, out Queue<ParallaxObject> queue) && queue.Count > 0)
        {
            ParallaxObject instance = queue.Dequeue();
            instance.gameObject.SetActive(true);

            return instance;
        }

        ParallaxObject newInstance = Instantiate(prefab, transform);
        instanceToPrefab[newInstance] = prefab;

        return newInstance;
    }

    private void RemoveProps(ParallaxObject instance)
    {
        ParallaxObject prefab = instanceToPrefab[instance];
        instance.gameObject.SetActive(false);

        if (!pools.TryGetValue(prefab, out Queue<ParallaxObject> queue))
        {
            queue = new Queue<ParallaxObject>();
            pools[prefab] = queue;
        }

        queue.Enqueue(instance);
    }
    #endregion

    #region Debug
    private void OnDrawGizmosSelected()
    {
        Vector3 start = transform.position - Vector3.right * offset;
        Vector3 end = start + Vector3.right * initialWidth;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
    }
    #endregion
}
