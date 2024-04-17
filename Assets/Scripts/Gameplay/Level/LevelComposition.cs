using UnityEngine;

public class LevelComposition : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private LevelSection[] sections;

    [SerializeField] private float speed = 1.0f;
    #endregion

    #region Attributes
    private float currentDist;

    private float sectionDist;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        InstantiateRandomSection();
    }

    private void Update()
    {
        currentDist += Time.deltaTime * speed;

        if (currentDist >= sectionDist) 
        {
            InstantiateRandomSection();
        }
    }
    #endregion

    #region Private
    private void InstantiateRandomSection()
    {
        LevelSection newSection = Instantiate(sections[Random.Range(0, sections.Length)], this.transform);
        newSection.speed = speed;
        sectionDist += newSection.Size;
    }
    #endregion
}
