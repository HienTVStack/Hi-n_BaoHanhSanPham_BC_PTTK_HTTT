CREATE DATABASE DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH_1
GO
USE DA_QLBANTHIETBIDIENTU_CHUNANG_BAOHANH_1
GO
--Tạo các bảng
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
GO
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
GO
create table Baohanh
(
	Mabaohanh int identity(1,1) not null,
	Tenbaohanh nvarchar(50),
	ID_KH INT,
	primary key (Mabaohanh)
)
GO
create table Chitietbaohanh
(
	Machitiet int identity(1,1) not null,
	Mabaohanh int,
	Dieukienbaohanh nvarchar(100),
	ID_SP INT,
	ID_NV INT,
	ID_HD INT,
	MOTA NVARCHAR(200), --SẢN PHẨM NÀY BỊ LỖI GÌ ...
	SOLANBAOHANH INT DEFAULT 0,
	THOIGIANBATDAU DATETIME,
	THOIGIANBAOHANHDUKIEN INT, --NGÀY
	THOIGIANKETTHUC DATETIME,
	TRANGTHAI BIT,
	primary key (Machitiet)
)
GO
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
GO
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

--KHÓA NGOẠI CÁC BẢNG
ALTER TABLE BAOHANH
ADD CONSTRAINT FK_BH_KH FOREIGN KEY (ID_KH) REFERENCES KHACHHANG(MAKH)

ALTER TABLE CHITIETBAOHANH
ADD CONSTRAINT FK_CTBH_SP FOREIGN KEY (ID_SP) REFERENCES SANPHAM(MASP)

ALTER TABLE CHITIETBAOHANH
ADD CONSTRAINT FK_CTBH_BH FOREIGN KEY (Mabaohanh) REFERENCES Baohanh(Mabaohanh)

SELECT * FROM BAOHANH, CHITIETBAOHANH WHERE CHITIETBAOHANH.MABAOHANH = BAOHANH.MABAOHANH
AND BAOHANH.ID_KH = '1' AND CHITIETBAOHANH.TRANGTHAI = 0;

--NHẬP DỮ LIỆU BAN ĐẦU
INSERT INTO KHACHHANG(TENKH, SODIENTHOAI) VALUES (N'HIỀN - KH', '0337122712')

INSERT INTO NHANVIEN(HOTEN) VALUES (N'HIỀN - NV')

INSERT INTO SANPHAM(TENSP, GIA) VALUES (N'IPHONE 6S PLUS', 5400000),
										(N'IPHONE 7S PLUS', 7000000)
										
INSERT INTO BAOHANH(ID_KH) VALUES (1)

INSERT INTO CHITIETBAOHANH(MABAOHANH, ID_SP, TRANGTHAI)
VALUES	(1, 1, 0),
		(1, 2, 0)


SELECT CHITIETBAOHANH.MACHITIET, ID_SP, TENSP, GIA, ID_HD, SOLANBAOHANH FROM BAOHANH, CHITIETBAOHANH, SANPHAM
WHERE BAOHANH.MABAOHANH = CHITIETBAOHANH.MABAOHANH
AND SANPHAM.MASP = CHITIETBAOHANH.ID_SP
AND CHITIETBAOHANH.TRANGTHAI = 0
AND BAOHANH.ID_KH = 1

SELECT ID_SP, TENSP, GIA, ID_HD, SOLANBAOHANH FROM BAOHANH, CHITIETBAOHANH, SANPHAM WHERE BAOHANH.MABAOHANH = CHITIETBAOHANH.MABAOHANH AND SANPHAM.MASP = CHITIETBAOHANH.ID_SP AND CHITIETBAOHANH.TRANGTHAI = 0 AND BAOHANH.ID_KH = 1
