using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Models
{
    public class Model
    {
        public Dictionary<string, object> Only(params string[] props)
        {
            Dictionary<string, object> newObject = new Dictionary<string, object>();

            foreach (var prop in this.GetType().GetProperties())
            {
                if (props.Contains(prop.Name))
                {
                    newObject[prop.Name] = prop.GetValue(this);
                }
            }

            return newObject;
        }

        public Dictionary<string, object> ExceptNull()
        {
            Dictionary<string, object> newObj = new Dictionary<string, object>();

            foreach (var property in this.GetType().GetProperties())
            {
                var value = property.GetValue(this);

                if (value != null)
                {
                    newObj[property.Name] = value;
                }
            }
            return newObj;
        }
    }
}
