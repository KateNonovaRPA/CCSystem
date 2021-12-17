using System.Collections.Generic;

namespace Common.Helpers
{
    public class ListWithDuplicates : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            this.Add(element);
        }
        public void Add(Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                //var element = new KeyValuePair<string, string>(key, value);
                this.Add(element);
            }
        }
    }
}