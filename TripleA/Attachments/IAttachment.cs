using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Attachments
{
    public interface IAttachment
    {
        void Apply(string attachTo, Dictionary<string, string> options);
    }
}