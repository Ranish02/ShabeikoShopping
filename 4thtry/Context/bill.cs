//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _4thtry.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bill()
        {
            this.orders = new HashSet<order>();
        }
    
        public int invoice_id { get; set; }
        public Nullable<int> invoice_fk_user { get; set; }
        public Nullable<System.DateTime> invoice_date { get; set; }
        public Nullable<double> invoice_totalbill { get; set; }
        public string Ispaid { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}