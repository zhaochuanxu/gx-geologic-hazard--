//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GU_DATA.Entitys
{
    using System;
    using System.Collections.Generic;
    
    public partial class gd_loss
    {
        public string code { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public Nullable<long> direct_dead { get; set; }
        public Nullable<int> injured { get; set; }
        public Nullable<int> indirect_dead { get; set; }
        public Nullable<int> missing { get; set; }
        public Nullable<int> building_living_failing { get; set; }
        public Nullable<int> building_working_failing { get; set; }
        public Nullable<int> building_industry_failing { get; set; }
        public Nullable<int> building_living_damaged { get; set; }
        public Nullable<int> building_woring_demaged { get; set; }
        public Nullable<int> building_industry_dema { get; set; }
        public Nullable<decimal> other_loss { get; set; }
        public string other_loss_dec { get; set; }
        public string other { get; set; }
    }
}