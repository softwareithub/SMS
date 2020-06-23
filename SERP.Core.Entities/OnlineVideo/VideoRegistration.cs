using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.OnlineVideo
{
    public class VideoRegistration:Base<int>
    {
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public int IsSubscribed { get; set; }
        public int SubscribeRequest { get; set; }
        public DateTime SubscribedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
