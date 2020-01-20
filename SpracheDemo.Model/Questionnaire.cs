using System;
using System.Collections.Generic;
using System.Linq;

namespace SpracheDemo.Model
{
    public class Questionnaire
    {
        public Questionnaire(IEnumerable<Section> sections)
        {
            if (sections == null) throw new ArgumentNullException("sections");

            Sections = sections.ToArray();
        }

        public IEnumerable<Section> Sections { get; }
    }
}