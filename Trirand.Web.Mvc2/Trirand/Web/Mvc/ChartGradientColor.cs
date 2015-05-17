namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartGradientColor
    {
        public ChartGradientColor()
        {
            this.LinearGradient = new ChartLinearGradient();
            this.Stops = new List<string>();
        }

        internal bool IsSet()
        {
            return (this.Stops.Count > 0);
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            List<object> list = new List<object>();
            string[] strArray = new string[] { this.LinearGradient.FromX.ToString(), this.LinearGradient.FromY.ToString(), this.LinearGradient.ToX.ToString(), this.LinearGradient.ToY.ToString() };
            int num = 0;
            foreach (string str in this.Stops)
            {
                object[] item = new object[] { num++, str };
                list.Add(item);
            }
            hashtable.Add("linearGradient", strArray);
            hashtable.Add("stops", list);
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public ChartLinearGradient LinearGradient { get; set; }

        public List<string> Stops { get; set; }
    }
}

