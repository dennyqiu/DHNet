﻿using DHNet.Components.Mvc;
using System;
using Xunit;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class AreaAttributeTests
    {
        #region AreaAttribute(String name)

        [Fact]
        public void AreaAttribute_SetsName()
        {
            String actual = new AreaAttribute("Name").Name;
            String expected = "Name";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
