//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory_Management_system.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tblSale
    {
        public int id { get; set; }
        public string Sale_prod { get; set; }
        public string Sale_qnty { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime Sale_date { get; set; }
    }
}
