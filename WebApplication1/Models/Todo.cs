﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool? Iscomplete { get; set; }
    }
}
