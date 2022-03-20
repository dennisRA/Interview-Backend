using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moduit.Interview.Model
{

    public class ScreeningItemResponseModel
    {
        public int id { get; set; }
        public int? category { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string footer { get; set; }
        public string[] tags { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
