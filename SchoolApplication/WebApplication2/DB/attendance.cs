//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication2.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class attendance
    {
        public int atid { get; set; }
        public string studentname { get; set; }
        public int classid { get; set; }
        public System.DateTime dateofattandance { get; set; }
        public string attandences { get; set; }
    
        public virtual @class @class { get; set; }
    }
}