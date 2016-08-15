using System;

namespace DHNet.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AreaAttribute : Attribute
    {
        public String Name { get; private set; }

        public AreaAttribute(String name)
        {
            Name = name;
        }
    }
}
