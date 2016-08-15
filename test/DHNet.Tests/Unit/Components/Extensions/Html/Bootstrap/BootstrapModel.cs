using System;
using System.ComponentModel.DataAnnotations;

namespace DHNet.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }

        public Decimal Number { get; set; }
        public DateTime? Date { get; set; }

        [Editable(true)]
        public Decimal EditableTrue { get; set; }

        [Editable(false)]
        public Decimal EditableFalse { get; set; }

        public BootstrapModel Relation { get; set; }

        public BootstrapModel()
        {
            NotRequired = "NotRequired";
            Required = "Required";
            EditableFalse = 1.4M;
            EditableTrue = 5.5M;
            Date = DateTime.Now;
            Number = 10.7854M;

            Relation = new BootstrapModel(true)
            {
                Date = new DateTime(2011, 3, 5, 8, 6, 4),
                NotRequired = "NotRequiredRelation",
                Required = "RequiredRelation",
                EditableFalse = 14.57M,
                EditableTrue = 8.4M,
                Number = 1.6666M
            };
        }
        private BootstrapModel(Boolean noInit)
        {
        }
    }
}
