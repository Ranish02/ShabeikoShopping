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
    
    public partial class order
    {
        public int order_id { get; set; }
        public Nullable<int> order_fkpro { get; set; }
        public Nullable<int> order_fk_invoice { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public Nullable<int> order_qty { get; set; }
        public Nullable<double> order_bill { get; set; }
        public Nullable<double> order_unitprice { get; set; }
        public string order_status { get; set; }
        public string address { get; set; }
        public string phone_no { get; set; }
        public Nullable<int> assigned_emp_id { get; set; }
    
        public virtual bill bill { get; set; }
        public virtual product product { get; set; }
        public virtual deliveryStaff deliveryStaff { get; set; }
    }
}
