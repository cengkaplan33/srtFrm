﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public  class RoleAccessiblePageView
    {
        public int Id { get; set; }
        public int SystemId { get; set; }     
        public string Name { get; set; }
        public string ObjectTypePrefix { get; set; }
        public string ObjectTypeName { get; set; }
        public string BigImagePath { get; set; }
        public string SmallImagePath { get; set; }
        public int IsAccess { get; set; }
       // public int? AccessibleItemId { get; set; }
    }
}