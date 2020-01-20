using System;
using System.Collections.Generic;
using System.Linq;

namespace SpracheDemo.Model
{
    public class Section
    {
        public Section(string id, string title, IEnumerable<Question> questions)
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Questions = questions.ToArray();
        }

        public string Id { get; }

        public string Title { get; }

        public IEnumerable<Question> Questions { get; }
    }
}