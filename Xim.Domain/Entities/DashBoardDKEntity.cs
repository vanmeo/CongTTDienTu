using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DashBoardDKEntity : BaseEntity<Guid>, IEntityCreated
    {

        public string Url_Anh { get; set; }
        public string? MoTaAnh { get; set; }
        public string? NhatKySuKien { get; set; }
        public string? TrangThaiAntoanCacHT { get; set; }
        public string? KetQuaDienTap { get; set; }
        public string? KetQuaVung1 { get; set; }
        public string? KetQuaVung2 { get; set; }
        public string? KetQuaVung3 { get; set; }
        public string? KetQuaVung4 { get; set; }
        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }

    }
}
