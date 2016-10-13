create proc danhsach_khdsThich
as
select  kh.TenKhachHang, kh.TaiKhoan, kh.MatKhau, kh.Email, kh.DiaChi, kh.GioiTinh, kh.SoDienThoai, kh.SoTaiKhoan, kh.NgaySinh, COUNT(ds.MaMatHang) AS SL 
FROM DSHangYeuThich ds inner JOIN KHACHHANG kh on ds.MaKhachHang = kh.MaKhachHang 
GROUP BY kh.TenKhachHang, kh.TaiKhoan, kh.MatKhau, kh.Email, kh.DiaChi, kh.GioiTinh, kh.SoDienThoai, kh.SoTaiKhoan, kh.NgaySinh

create proc danhsach_chiTietKHThich
as
select  kh.MaKhachHang, kh.TenKhachHang, ds.MaMatHang, hh.TenMatHang
from KHACHHANG kh inner join DSHangYeuThich ds on kh.MaKhachHang = ds.MaKhachHang inner join HANGHOA hh on ds.MaMatHang = hh.MaMatHang
group by kh.MaKhachHang, kh.TenKhachHang, ds.MaMatHang, hh.TenMatHang
 