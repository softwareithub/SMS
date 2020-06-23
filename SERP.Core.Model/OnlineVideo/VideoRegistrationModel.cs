using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.OnlineVideo
{
    public class VideoRegistrationModel
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string VideoPath { get; set; }
    }
}
