﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Dtos.Events
{
    public class UserEvent
    {
        public Guid UserId { get; set; } 
        public string ServiceName { get; set; }
    }
}
