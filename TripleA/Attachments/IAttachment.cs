using System.Collections.Generic;

namespace TripleA.Attachments
{
    public interface IAttachment
    {
        void Apply(string attachTo, Dictionary<string, string> options);
    }
}