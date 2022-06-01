create database QL_CUAHANGDT
go
use QL_CUAHANGDT
go

--use master
--drop database QL_CUAHANGDT

create table Khachhang
(
	Makh int identity(1,1) not null,
	Tenkh nvarchar(50),
	Ngaysinh date,
	Gioitinh nvarchar(10),
	Diachi nvarchar(50),
	Sodienthoai nchar(11),
	CMND nchar(20),
	primary key (Makh)
)

create table Nhanvien
(
	Manv int identity(1,1) not null,
	Hoten nvarchar(50),
	Ngaysinh date,
	Gioitinh nvarchar(10),
	Sodienthoai nchar(11),
	Diachi nvarchar(50),	
	Luong float,
	primary key (Manv)
)

create table Nhasanxuat
(
	Manhasx int identity(1,1) not null,
	Tennhasx nvarchar(50),
	primary key (Manhasx)
)

create table Nhacungcap
(
	Manhacc int identity(1,1) not null,
	Tennhacc nvarchar(50),
	primary key (Manhacc)
)

create table Loaisp
(
	Maloai int identity(1,1) not null,
	Tenloai nvarchar(50),
	primary key (Maloai)
)

create table Baohanh
(
	Mabaohanh int identity(1,1) not null,
	Tenbaohanh nvarchar(50),
	ID_KH INT,
	primary key (Mabaohanh)
)

create table Chitietbaohanh
(
	Machitiet int identity(1,1) not null,
	Mabaohanh int,
	Dieukienbaohanh nvarchar(100),
	ID_SP INT,
	ID_NV INT,
	ID_HHD INT,
	MOTA NVARCHAR(200), --SẢN PHẨM NÀY BỊ LỖI GÌ ...
	SOLANBAOHANH INT,
	THOIGIANBATDAU DATETIME,
	THOIGIANBAOHANHDUKIEN INT, --NGÀY
	THOIGIANKETTHUC DATETIME,
	TRANGTHAI BIT,
	primary key (Machitiet)
)
--KHÓA NGOẠI BẢNG BẢO HÀNH
GO
ALTER TABLE BAOHANH
ADD CONSTRAINT FK_BH_KH FOREIGN KEY (ID_KH) REFERENCES KHACHHANG (Makh)
GO

GO
create table ChitietCT
(
	Machitiet int identity(1,1) not null,
	Tylegiam float,
	primary key (Machitiet)
)

create table Sanpham
(
	Masp int identity(1,1) not null,
	Maloai int,
	Manhasx int,
	Manhacc int,
	Machitiet int,
	Mabaohanh int,
	Tensp nvarchar(50),
	Gia float,
	Soluong int,
	primary key (Masp)
)
alter table Sanpham
add constraint fk_Sanpham_Nhasanxuat foreign key (Manhasx) references Nhasanxuat(Manhasx)
alter table Sanpham
add constraint fk_Sanpham_Nhacungcap foreign key (Manhacc) references Nhacungcap(Manhacc)
alter table Sanpham
add constraint fk_Sanpham_ChitietCT foreign key (Machitiet) references ChitietCT(Machitiet)
alter table Sanpham
add constraint fk_Sanpham_Baohanh foreign key (Mabaohanh) references Baohanh(Mabaohanh)
alter table Sanpham
add constraint fk_Sanpham_Loaisp foreign key (Maloai) references Loaisp(Maloai)
--KHÓA NGOẠI CHI TIẾT BẢO HÀNH
alter table Chitietbaohanh
add constraint fk_Chitietbaohanh_Baohanh foreign key (Mabaohanh) references Baohanh(Mabaohanh)

GO
ALTER TABLE CHITIETBAOHANH
ADD CONSTRAINT FK_SP_KH FOREIGN KEY (ID_SP) REFERENCES SANPHAM (MASP)

create table CTgiamgia
(
	MaID int identity(1,1) not null,
	Machitiet int,
	TenCT nvarchar(50),
	Trangthai nvarchar(50),
	TGbatdau date,
	TGketthuc date,
	primary key (MaID)
)
alter table CTgiamgia
add constraint fk_CTgiamgia_ChitietCT foreign key (Machitiet) references ChitietCT(Machitiet)

create table Phieunhap
(
	Maphieunhap int identity(1,1) not null,
	Manhacc int,
	Masp int,
	Ngaynhap date,
	Tongtien float,
	primary key (Maphieunhap)
)
alter table Phieunhap
add constraint fk_Phieunhap_Nhacungcap foreign key (Manhacc) references Nhacungcap(Manhacc)
alter table Phieunhap
add constraint fk_Phieunhap_Sanpham foreign key (Masp) references Sanpham(Masp)

create table Chitietphieunhap
(
	Machitiet int identity(1,1) not null,
	Maphieunhap int,
	Soluongnhap int,
	Dongianhap float,
	primary key (Machitiet)
)
alter table Chitietphieunhap
add constraint fk_Chitietphieunhap_Phieunhap foreign key (Maphieunhap) references Phieunhap(Maphieunhap)

create table PhieudatNCC
(
	Maphieudat int identity(1,1) not null,
	Maphieunhap int,
	Masp int,
	Ngaydat date,
	Tongtien float,
	primary key (Maphieudat)
)
alter table PhieudatNCC
add constraint fk_PhieudatNCC_Phieunhap foreign key (Maphieunhap) references Phieunhap(Maphieunhap)
alter table PhieudatNCC
add constraint fk_PhieudatNCC_Sanpham foreign key (Masp) references Sanpham(Masp)

create table Chitietphieudat
(
	Machitiet int identity(1,1) not null,
	Maphieudat int,
	Soluongdat int,
	Dongiadat float,
	primary key (Machitiet)
)
alter table Chitietphieudat
add constraint fk_Chitietphieudat_PhieudatNCC foreign key (Maphieudat) references PhieudatNCC(Maphieudat)

create table Phuongthucthanhtoan
(
	Maphuongthuc int identity(1,1) not null,
	Tenphuongthuc nvarchar(50),
	primary key (Maphuongthuc)
)

create table Hoadon
(
	Mahd int identity(1,1) not null,
	Makh int,
	Manv int,
	Masp int,
	Maphuongthuc int,
	Ngaylap date,
	Tongtien float,
	primary key (Mahd)
)
alter table Hoadon
add constraint fk_Hoadon_Khachhang foreign key (Makh) references Khachhang(Makh)
alter table Hoadon
add constraint fk_Hoadon_Nhanvien foreign key (Manv) references Nhanvien(Manv)
alter table Hoadon
add constraint fk_Hoadon_Sanpham foreign key (Masp) references Sanpham(Masp)
alter table Hoadon
add constraint fk_Hoadon_Phuongthucthanhtoan foreign key (Maphuongthuc) references Phuongthucthanhtoan(Maphuongthuc)

create table Chitiethd
(
	Machitiet int identity(1,1) not null,
	Mahd int,
	Soluongmua int,
	primary key (Machitiet)
)
alter table Chitiethd
add constraint fk_ChitietHD_Hoadon foreign key (Mahd) references Hoadon(Mahd)