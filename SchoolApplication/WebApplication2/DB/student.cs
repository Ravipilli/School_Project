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
    
    public partial class student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public student()
        {
            this.Marks = new HashSet<Mark>();
        }
    
        public int admissionnum { get; set; }
        public string studentname { get; set; }
        public string fathername { get; set; }
        public string phonenumber { get; set; }
        public System.DateTime dateofbirth { get; set; }
        public bool gender { get; set; }
        public string adress { get; set; }
        public byte[] photo { get; set; }
        public System.DateTime dateofjoining { get; set; }
        public int classid { get; set; }
        public string acadamicyear { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<int> rid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    
        public virtual @class @class { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual Role Role { get; set; }
    }
}
