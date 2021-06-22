using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.DataTransferObjects
{
    public class StepDto
    {
        public int Id { get; set; }
        public string StepName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImagePath { get; set; }
        public int StepsCount { get; set; }
        public string CreatedBy { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}