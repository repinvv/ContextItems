//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Citest
{
    using System;
    using System.Collections.Generic;
    
    public partial class task_item
    {
        public task_item()
        {
            this.task_item_status = new HashSet<task_item_status>();
            this.task_item_chunk = new HashSet<task_item_chunk>();
        }
    
        public int task_item_id { get; set; }
        public System.Guid batch_id { get; set; }
        public string group_key { get; set; }
        public string exclusion_key { get; set; }
        public string task_parameters { get; set; }
    
        public virtual batch batch { get; set; }
        public virtual ICollection<task_item_status> task_item_status { get; set; }
        public virtual ICollection<task_item_chunk> task_item_chunk { get; set; }
    }
}
