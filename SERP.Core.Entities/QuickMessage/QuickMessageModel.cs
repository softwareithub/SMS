using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.QuickMessage
{
    public class QuickMessageModel
    {
        public List<CandidateDetail> CandidateDetails { get; set; }
        public string Message { get; set; }
        public bool IsEmail { get; set; }
        public bool IsMobile { get; set; }
    }
    public class CandidateDetail
    {
        public string CandidateName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
