using Microsoft.Extensions.DependencyInjection;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Mssql.Repos;
using Xim.Domain.Repos;
using System;

namespace Xim.Domain.Mssql
{
    public static class MssqlFactory
    {
        /// <summary>
        /// Keep connection
        /// </summary>
        internal static string AppConnection;
        public static void ConfigureAppRepository(IServiceCollection services, string connectionString)
        {
            //init db
            AppConnection = connectionString;

            //inject repo
            services.AddScoped(typeof(IRepo<,>), typeof(MssqlAppRepo<,>));
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IDonviRepo, DonviRepo>();
            services.AddScoped<ICapbacRepo, CapbacRepo>();
            services.AddScoped<IChucvuRepo, ChucvuRepo>();
            services.AddScoped<IUserLogRepo, UserLogRepo>();
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IDMTailieuRepo, DMTailieuRepo>();
            services.AddScoped<IDMLoaiVBRepo, DMLoaiVBRepo>();
            services.AddScoped<IDMLinhvucVBRepo, DMLinhvucVBRepo>();
            services.AddScoped<IDMCoQuanBHVBRepo, DMCoQuanBHVBRepo>();
            services.AddScoped<ILienKetRepo, LienKetRepo>();
            services.AddScoped<IMenuRepo, MenuRepo>();
            services.AddScoped<IMenuSubRepo, MenuSubRepo>();
            services.AddScoped<IModuleRepo, ModuleRepo>();
            services.AddScoped<IQuyenHanRepo, QuyenHanRepo>();
            services.AddScoped<ITinTucRepo, TinTucRepo>();
            services.AddScoped<ICaiDatRepo, CaiDatRepo>();
            services.AddScoped<IThuTruongRepo, ThuTruongRepo>();
            services.AddScoped<IHoatDongRepo, HoatDongRepo>();
            services.AddScoped<INhatKyRepo, NhatKyRepo>();
            services.AddScoped<IThuTruongRepo, ThuTruongRepo>();
            services.AddScoped<IDMThuTruongRepo, DMThuTruongRepo>();
            services.AddScoped<IThuTruongBQPRepo, ThuTruongBQPRepo>();
            services.AddScoped<IBienNienSuKienRepo, BienNienSuKienRepo>();
            services.AddScoped<IAlbumRepo, AlbumRepo>();
            services.AddScoped<IAnh_AlbumRepo, Anh_AlbumRepo>();
            services.AddScoped<IDashBoardDKRepo, DashBoardDKRepo>();
        }
    }
}
