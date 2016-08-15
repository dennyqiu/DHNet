using DHNet.Components.Mvc;
using System;

namespace DHNet.Tests.Objects
{
    public class BindersModel
    {
        [NotTrimmed]
        public String NotTrimmed { get; set; }

        public String Trimmed { get ;set; }

        public BindersModel Model { get; set; }
    }
}
