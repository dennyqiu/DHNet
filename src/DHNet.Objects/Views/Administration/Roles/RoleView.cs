using Datalist;
using DHNet.Components.Extensions.Html;
using System;
using System.ComponentModel.DataAnnotations;

namespace DHNet.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [DatalistColumn]
        [StringLength(128)]
        public String Title { get; set; }

        public JsTree Permissions { get; set; }

        public RoleView()
        {
            Permissions = new JsTree();
        }
    }
}
