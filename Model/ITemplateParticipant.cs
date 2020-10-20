using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Model
{
    public interface ITemplateParticipant<T>
    {
        string Name { get; set; }

        public void Add(List<T> list);
        public void GetBest(List<T> list);
    }
}
