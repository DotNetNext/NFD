namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class PivotDimension
    {
        private Hashtable _functionsHash;

        public PivotDimension()
        {
            this.DataName = "";
            this.Converter = "";
        }

        public PivotDimension(JQGrid grid) : this()
        {
            this._functionsHash = grid.FunctionsHash;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hash = new Hashtable();
            hash.AddIfNotEmpty("dataName", this.DataName);
            hash.AddIfNotEmpty("converter", this.Converter, this._functionsHash);
            return hash;
        }

        public string Converter { get; set; }

        public string DataName { get; set; }
    }
}

