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

        public string GetBestInfo()
        {
            if (_list.Count != 0)
                return _list[0].GetBest(_list);
            return "";
        }

        public List<T> GetList()
        {
            return _list;
        }
    }
}
