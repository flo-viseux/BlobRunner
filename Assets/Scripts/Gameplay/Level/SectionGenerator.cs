using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectionGenerator : Parallax
{
    #region Serialized fields
    [SerializeField] private WeightedSection[] initialSections = null;

    [SerializeField] private WeightedSection[] potentialSections = null;

    [SerializeField] private SectionRepetitionConstraints[] sectionRepetitionConstraints = null;
    #endregion

    #region Attributes
    private float offset;

    /*private Dictionary<WeightedSection, Queue<SectionGeometry>> sectionGeoPool = new Dictionary<WeightedSection, Queue<SectionGeometry>>();

    private Dictionary<SectionGeometry, WeightedSection> instanceToPrefab = new Dictionary<SectionGeometry, WeightedSection>();

    private Queue<Section> sectionSequence = new Queue<Section>();*/

    private List<Section> sectionSequence = new List<Section>();

    private List<SectionGeometry> sectionGeoSequence = new List<SectionGeometry>();
    #endregion

    #region API
    public float CurrentEnd { get; private set; }
    #endregion

    #region Unity methods
    protected override void Awake()
    {
        base.Awake();

        Scrolling = true;

        CurrentEnd = 0;

        InitialFill();
    }

    private void Update()
    {
        if (Scrolling)
        {
            float f = speed * SpeedFactor;

            CurrentEnd -= f * Time.deltaTime;

            foreach (SectionGeometry geo in sectionGeoSequence)
            {
                geo.Move(f);
            }
        }

        if (Spawning)
            Fill();

        /*if (sectionSequence.TryPeek(out Section props))
        {
            if (props.Geometry.transform.localPosition.x + props.Geometry.GetComponent<SectionGeometry>().Width < 0 + offset)
            {
                sectionSequence.Dequeue();
                RemoveProps(props.Geometry.GetComponent<SectionGeometry>());
            }
        }*/
    }
    #endregion

    #region Private
    private void InitialFill()
    {
        for (int i = 0; i < initialSections.Length; ++i)
        {
            SectionGeometry initialProps = Instantiate(initialSections[i].Section.Geometry, transform).GetComponent<SectionGeometry>();
            initialProps.gameObject.transform.localPosition = Vector3.right * CurrentEnd;

            Section instanciatedProps = initialSections[i].Section;
            //sectionSequence.Enqueue(instanciatedProps);
            sectionSequence.Add(instanciatedProps);
            sectionGeoSequence.Add(initialProps);

            offset += initialProps.Width;
            CurrentEnd += initialProps.Width;
        }
    }

    private void Fill()
    {
        while (CurrentEnd < offset)
            AddSectionToCurrentOffset();
    }

    private void Fill(float offset)
    {
        while (CurrentEnd < offset)
            AddSectionToCurrentOffset();
    }

    private void AddSectionToCurrentOffset()
    {
        WeightedSection newSection = GetRdWeightedSection();
        newSection.SetCooldown();

        SectionGeometry newGeo = InstantiateProps(newSection);
        newGeo.transform.localPosition = Vector3.right * CurrentEnd;

        //sectionSequence.Enqueue(newSection.Section);
        sectionSequence.Add(newSection.Section);
        sectionGeoSequence.Add(newGeo);


        foreach (WeightedSection potentialSection in potentialSections)
            potentialSection.DecreaseCooldown();

        CurrentEnd += newGeo.Width;
    }

    private WeightedSection GetRdWeightedSection()
    {
        foreach (SectionRepetitionConstraints sectionRepetitionConstraint in sectionRepetitionConstraints)
        {
            if (sectionRepetitionConstraint.IsConstrainedActivityAllowed(sectionSequence))
            {
                potentialSections.First(x => x.Section == sectionRepetitionConstraint.Section).ForceCooldown();
            }
        }

        float totalWeights = potentialSections.Sum(x => x.GetWeightAt());


        float rd = Random.value * totalWeights;

        for (int i = 0; i < potentialSections.Length; ++i)
        {
            float weightAt = potentialSections[i].GetWeightAt();

            if (rd < weightAt)
                return potentialSections[i];

            rd -= weightAt;
        }

        return potentialSections.Last();
    }

    private SectionGeometry InstantiateProps(WeightedSection weightedSection)
    {
        /*if (sectionGeoPool.TryGetValue(weightedSection, out Queue<SectionGeometry> queue) && queue.Count > 0)
        {
            SectionGeometry instance = queue.Dequeue();
            instance.gameObject.SetActive(true);

            return instance;
        }*/

        SectionGeometry newGeo = Instantiate(weightedSection.Section.Geometry, transform).GetComponent<SectionGeometry>();
        //instanceToPrefab[newGeo] = weightedSection;

        return newGeo;
    }

    private void RemoveProps(SectionGeometry instance)
    {
        /*WeightedSection prefab = instanceToPrefab[instance];
        instance.gameObject.SetActive(false);

        if (!sectionGeoPool.TryGetValue(prefab, out Queue<SectionGeometry> queue))
        {
            queue = new Queue<SectionGeometry>();
            sectionGeoPool[prefab] = queue;
        }

        queue.Enqueue(instance);*/
    }
    #endregion
}
