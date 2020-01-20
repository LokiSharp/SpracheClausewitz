using System;

namespace SpracheDemo.Model
{
    public class Question
    {
        public Question(string id, string prompt, AnswerType answerType)
        {
            Prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
            Id = id ?? throw new ArgumentNullException(nameof(id));
            AnswerType = answerType;
        }

        public string Id { get; }

        public AnswerType AnswerType { get; }

        public string Prompt { get; }
    }
}