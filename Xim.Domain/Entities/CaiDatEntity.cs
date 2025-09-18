using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Xim.Domain.Entities
{
    public class CaiDatEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string ten { get; set; }
		public string? ten_tv { get; set; }
        public string? ten_ta { get; set; }
        public string? logo { get; set; }
        public string? diachita { get; set; }
        public string? diachitv { get; set; }
        public string? vanphong1 { get; set; }
        public string? vanphong2 { get; set; }
        public string? vanphong3 { get; set; }
        public string? vanphong4 { get; set; }
        public string? vanphong5 { get; set; }
        public string? dienthoai { get; set; }
        public string? dienthoai_hienthi { get; set; }
        public string? fax { get; set; }
        public string? fax_hienthi { get; set; }
        public string? email { get; set; }
        public string? email_hienthi { get; set; }
        public string? hotline { get; set; }
        public string? hotline_hienthi { get; set; }
        public string? taikhoannganhang { get; set; }
        public string? tennganhang { get; set; }
        public string? ngaythanhlap { get; set; }
        public string? nguoidaidien { get; set; }
        public string? chucvu { get; set; }
        public string? website { get; set; }
        public string? link_facebook { get; set; }
        public string? link_twitter { get; set; }
        public string? link_google { get; set; }
        public string? link_youtube { get; set; }
        public string? link_instagram { get; set; }
        public string? icongioithieu { get; set; }
        public string? tieudegioithieu { get; set; }
        public string? noidunggioithieu { get; set; }
        public string? icon_lienhe { get; set; }
        public string? ghichu { get; set; }
       
        public bool? is_locked { get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }


    }
}
