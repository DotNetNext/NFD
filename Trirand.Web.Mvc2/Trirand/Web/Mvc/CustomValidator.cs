namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomValidator : JQGridEditClientSideValidator
    {
        public string ValidationFunction { get; set; }
    }
}

