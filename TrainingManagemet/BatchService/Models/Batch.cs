﻿using System;

namespace BatchService.Models
{
    public class Batch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public int CourseId { get; set; }
    }
}
