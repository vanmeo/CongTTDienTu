namespace API.Controllers
{
    public class CaiDatForm
    {
        public string ten { get; set; }
        public IFormFile logofile { get; set; } // Ảnh đại diện
        public IFormFile icongioithieufile { get; set; }
        public IFormFile icon_lienhefile { get; set; }
        public string? diachitv { get; set; }
        public string? vanphong1 { get; set; }
        public string? dienthoai { get; set; }
        public string? fax { get; set; }
        public string? email { get; set; }
        public string? hotline { get; set; }
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

        public string? tieudegioithieu { get; set; }
        public string? noidunggioithieu { get; set; }

        public string? ghichu { get; set; }
        public bool? is_locked { get; set; }
        public bool? is_deleted { get; set; }
    }
    public class Dashboard
    {
        public DateTime Created { get; set; }
        public string? MoTaAnh { get; set; }
        public List<NhatKySuKien>? NhatKySuKien { get; set; }
        public Dictionary<string, List<TrangThaiCacVung>>? TrangThaiAntoanCacHT { get; set; }
        public List<KetQuaChung>? KetQuaDienTap { get; set; }
        public KetQuaVung12? KetQuaVung1 { get; set; }
        public KetQuaVung12? KetQuaVung2 { get; set; }
        public KetQuaVung34? KetQuaVung3 { get; set; }
        public KetQuaVung34? KetQuaVung4 { get; set; }
    }
    public class TrangThaiCacVung
    {
        public string? Vung { get; set; }
        public string? KhuVuc { get; set; }
        public string? TrangThai { get; set; }
    }
    public class NhatKySuKien
    {
        public string? ThoiGian { get; set; }
        public string? NoiDung { get; set; }
    }
    public class KetQuaCuThe
    {
        public float? BenA {  get; set; }
        public float? BenB { get; set; }
    }
    public class BieuDoChiTiet
    {
        public float? ketquachung { get; set; }
        public float? antoan { get; set; }
        public float? khongantoan { get; set; }
    }
    public class KetQuaChung
    {
        public string? NoiDung { get; set; }
        public KetQuaCuThe? KetQuaChiTiet { get; set; }
    }
    public class KetQuaVung12
    {
        public string TrangThaiDienTap {  get; set; }
       public List<KetQuaChung> KetQuachungs { get;set; }
    }
    public class KetQuaVung34
    {
        public string TrangThaiDienTap { get; set; }
        public BieuDoChiTiet? BenA { get; set; }
        public BieuDoChiTiet? BenB { get; set; }
    }


}
