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
    
    public partial class Cart
    {
        public int cart_id { get; set; }
        public int user_id_fk { get; set; }
        public int pro_id_fk { get; set; }
        public int qty { get; set; }
        public double unitprice { get; set; }
        public double total { get; set; }
    
        public virtual product product { get; set; }
        public virtual User User { get; set; }
    }
}
