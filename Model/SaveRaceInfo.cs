using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SaveRaceInfo<T> where T : ITemplateParticipant<T>
    {
        private List<T> _list = new List<T>();

        public void AddToList(T t)
        {
            t.Add(_list);
        }

        public List<T> GetList()
        {
            return _list;
        }
    }
}
