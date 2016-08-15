using DHNet.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace DHNet.Objects
{
    public class ProfileDeleteView : BaseView
    {
        [Required]
        [NotTrimmed]
        [StringLength(32)]
        public String Password { get; set; }
    }
}
