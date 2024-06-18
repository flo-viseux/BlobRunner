using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SectionRepetitionConstraints
{
    [SerializeField] private Section section = null;
    [SerializeField] private int maxRepetitions = 1;
    [SerializeField] private int constraintHistoryDepth = 1;

    public bool IsConstrainedActivityAllowed(List<Section> sections)
    {
        int skip = Mathf.Max(0, sections.Count - constraintHistoryDepth);
        int sameActivityCount = sections.Skip(skip).Count(x => x == section);

        return sameActivityCount >= maxRepetitions;
    }

    public Section Section => section;
}
