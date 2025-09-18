using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Mssql.Tables
{
    /// <summary>
    /// Lấy thông tin table ứng với entity
    /// </summary>
    public static class TableMap
    {
        static readonly Dictionary<string, TableConfig> _map = new Dictionary<string, TableConfig>
        {
            { typeof(UserEntity).Name, new TableConfig("users") },
            { typeof(DonviEntity).Name, new TableConfig("Donvis") },
            { typeof(CapbacEntity).Name, new TableConfig("Capbacs") },
            { typeof(ChucvuEntity).Name, new TableConfig("Chucvus") },
            { typeof(UserLogEntity).Name, new TableConfig("user_log") },
            { typeof(RoleEntity).Name, new TableConfig("role") },
            { typeof(RolePermissionEntity).Name, new TableConfig("role_permission") },
            { typeof(QuyenHanEntity).Name, new TableConfig("QuyenHan") },
            { typeof(ModuleEntity).Name, new TableConfig("Module") },
            { typeof(UserRoleEntity).Name, new TableConfig("user_role") },
            { typeof(CaiDatEntity).Name, new TableConfig("CaiDat") },
            { typeof(DMCoQuanBHVBEntity).Name, new TableConfig("DMCoQuanBHVB") },
            { typeof(DMLinhvucVBEntity).Name, new TableConfig("DMLinhvucVB") },
            { typeof(DMLoaiVBEntity).Name, new TableConfig("DMLoaiVB") },
            { typeof(DMTailieuEntity).Name, new TableConfig("DMTailieu") },
            { typeof(LienKetEntity).Name, new TableConfig("LienKet") },
            { typeof(MenuEntity).Name, new TableConfig("Menu") },
            { typeof(MenuSubEntity).Name, new TableConfig("MenuSub") },
            { typeof(TinTucEntity).Name, new TableConfig("TinTuc") },
            { typeof(ThuTruongEntity).Name, new TableConfig("ThuTruong") },
            { typeof(HoatDongEntity).Name, new TableConfig("HoatDong") },
            { typeof(NhatKyEntity).Name, new TableConfig("NhatKy") },
            { typeof(DMThuTruongEntity).Name, new TableConfig("DMThuTruong") },
            { typeof(ThuTruongBQPEntity).Name, new TableConfig("ThuTruongBQP") },
            { typeof(BienNienSuKienEntity).Name, new TableConfig("BienNienSuKien") },
               { typeof(AlbumEntity).Name, new TableConfig("album") },
                  { typeof(Anh_AlbumEntity).Name, new TableConfig("Anh_Album") },
                    { typeof(DashBoardDKEntity).Name, new TableConfig("DashboardDK") }
        };

        public static TableConfig Get<T>()
        {
            return Get(typeof(T));
        }
        public static TableConfig Get(Type type)
        {
            //TODO xảy ra ngoài lệ ở đây là thiếu cấu hình bảng cho entity
            return _map[type.Name];
        }
    }
}
