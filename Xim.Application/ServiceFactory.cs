using Microsoft.Extensions.DependencyInjection;
using Xim.Application.Contracts.Users;
using Xim.Application.Contracts.Donvis;
using Xim.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xim.Library.Constants;
using Xim.Library.Exceptions;
using Xim.Application.Contracts.Capbacs;
using Xim.Application.Contracts.Chucvus;
using Xim.Application.Contracts.Role;
using Xim.Application.Contracts.CaiDat;
using Xim.Application.Contracts.DMCoQuanBHVB;
using Xim.Application.Contracts.DMLinhvucVB;
using Xim.Application.Contracts.DMLoaiVB;
using Xim.Application.Contracts.DMTailieu;
using Xim.Application.Contracts.LienKet;
using Xim.Application.Contracts.Menu;
using Xim.Application.Contracts.MenuSub;
using Xim.Application.Contracts.TinTuc;
using Xim.Application.Contracts.ThuTruong;
using Xim.Application.Contracts.QuyenHan;
using Xim.Application.Contracts.Module;
using Xim.Application.Contracts.HoatDong;
using Xim.Application.Contracts.NhatKy;
using Xim.Application.Contracts.DMThuTruong;
using Xim.Application.Contracts.ThuTruongBQP;
using Xim.Application.Contracts.BienNienSuKien;
using Xim.Application.Contracts.Album;
using Xim.Application.Contracts.Anh_Album;
using Xim.Application.Contracts.DashBoardDK;
namespace Xim.Application
{
    public static class ServiceFactory
    {
        /// <summary>
        /// Cấu hình service
        /// </summary>
        public static void ConfigureService(IServiceCollection services)
        {
            //utility service
            // Đọc FtpSettings từ appsettings.json
   
            //inject business service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDonviService, DonviService>();
            services.AddScoped<ICapbacService, CapbacService>();
            services.AddScoped<IChucvuService, ChucvuService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICaiDatService, CaiDatService>();
            services.AddScoped<IDMCoQuanBHVBService, DMCoQuanBHVBService>();
            services.AddScoped<IDMLinhvucVBService, DMLinhvucVBService>();
            services.AddScoped<IDMLoaiVBService, DMLoaiVBService>();
            services.AddScoped<IDMTailieuService, DMTailieuService>();
            services.AddScoped<ILienKetService, LienKetService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuSubService, MenuSubService>();
            services.AddScoped<ITinTucService, TinTucService>();
            services.AddScoped<IThuTruongService, ThuTruongService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IQuyenHanService, QuyenHanService>();
            services.AddScoped<IHoatDongService, HoatDongService>();
            services.AddScoped<INhatKyService,NhatKyService>();
            services.AddScoped<IDMThuTruongService, DMThuTruongService>();
            services.AddScoped<IThuTruongBQPService, ThuTruongBQPService>();
            services.AddScoped<IBienNienSuKienService, BienNienSuKienService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAnh_AlbumService,Anh_AlbumService>();
            services.AddScoped<IDashBoardDKService, DashBoardDKService>();
        }
    }
}
