using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moduit.Interview.Model
{
    public class ScreeningThirdRawResponseModel
    {
        public int id { get; set; }
        public int? category { get; set; }
        public ScreeningItemResponseModel[] items { get; set; }
        public DateTime? createdAt { get; set; }
        public string[] tags { get; set; }
    }
}
