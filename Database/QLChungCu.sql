USE master;
ALTER DATABASE QLChungCu SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE QLChungCu;
GO

CREATE DATABASE QLChungCu
GO

USE QLChungCu
GO

-- Tạo bảng NguoiQuanLy
CREATE TABLE NguoiQuanLy(
	IDNguoiQuanLy VARCHAR(30) PRIMARY KEY NOT NULL,
	TenNguoiQuanLy NVARCHAR(100) NOT NULL,
	NgaySinh DATE,
	SoGiayTo VARCHAR(20),
	SDTNguoiQuanLy VARCHAR(12)
)
GO

-- Tạo bảng Admin
CREATE TABLE Admin(
	IDAdmin VARCHAR(30) PRIMARY KEY NOT NULL,
	TenAdmin NVARCHAR(200) NOT NULL
)
GO

-- Tạo bảng TaiKhoan
CREATE TABLE TaiKhoan (
    IDTaiKhoan VARCHAR(30) PRIMARY KEY NOT NULL,
    MatKhau VARCHAR(50) NOT NULL,
    QuyenHan NVARCHAR(50) NOT NULL,
    IDNguoiQuanLy VARCHAR(30),
    IDAdmin VARCHAR(30),
    CONSTRAINT FK_TaiKhoan_NguoiQuanLy FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_TaiKhoan_Admin FOREIGN KEY (IDAdmin) REFERENCES Admin(IDAdmin)
);
GO

CREATE TRIGGER trg_AfterInsertNguoiQuanLy
ON NguoiQuanLy
AFTER INSERT
AS
BEGIN
    -- Thêm tài khoản vào bảng TaiKhoan nếu chưa có
    INSERT INTO TaiKhoan (IDTaiKhoan, MatKhau, QuyenHan, IDNguoiQuanLy)
    SELECT 
        IDNguoiQuanLy,
        '123456',
        N'Quản lý',
        IDNguoiQuanLy
    FROM INSERTED
    WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE IDTaiKhoan = INSERTED.IDNguoiQuanLy);
END;
GO
INSERT INTO NguoiQuanLy (IDNguoiQuanLy, TenNguoiQuanLy, NgaySinh, SDTNguoiQuanLy)
VALUES 
('quanly01', N'Lê Thị B', '1990-11-25', '0912345678'),
('quanly02', N'Nguyễn Tiến D', '1989-5-10', '0987654321');
GO

CREATE TRIGGER trg_AfterInsertAdmin
ON Admin
AFTER INSERT
AS
BEGIN
    INSERT INTO TaiKhoan (IDTaiKhoan, MatKhau, QuyenHan, IDAdmin)
    SELECT 
        IDAdmin,
        '123456',
        N'Admin',
        IDAdmin
    FROM INSERTED
    WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE IDTaiKhoan = INSERTED.IDAdmin);
END;
GO

INSERT INTO Admin (IDAdmin, TenAdmin)
VALUES 
('admin01', N'Nguyễn Văn A')
GO

CREATE TABLE MatBangThuongMai(
	IDMBTM INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	DienTich FLOAT,
	TenDonViThue NVARCHAR(50),
	GiaThue FLOAT,
	TinhTrang NVARCHAR(50) NOT NULL
)
GO

INSERT INTO MatBangThuongMai (DienTich, TenDonViThue, GiaThue, TinhTrang)
VALUES 
(120.5, N'Công ty ABC', 20000000, N'Đang thuê'),
(80.0, N'Lê Văn C', 15000000, N'Chưa cho thuê'),
(100.0, N'Cửa hàng XYZ', 18000000, N'Đang thuê');
GO

CREATE TABLE HoaDonTM (
    IDHoaDon INT IDENTITY(1,1) PRIMARY KEY NOT NULL,    
    SoTien FLOAT NOT NULL,
    HanThanhToan DATE NOT NULL, 
    TrangThai NVARCHAR(50)  NOT NULL,
	IDMBTM INT NOT NULL,
    FOREIGN KEY (IDMBTM) REFERENCES MatBangThuongMai(IDMBTM)
)
GO

INSERT INTO HoaDonTM (SoTien, HanThanhToan, TrangThai, IDMBTM)
VALUES 
(20000000, '2024-12-15', N'Chưa thanh toán', 1),
(15000000, '2024-11-30', N'Quá hạn', 2),
(18000000, '2024-12-10', N'Đã thanh toán', 3);
GO

CREATE TABLE VatTu (
	IDVatTu INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	TenVatTu NVARCHAR(200) NOT NULL,
	SoLuong INT,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
)
GO

INSERT INTO VatTu (TenVatTu, SoLuong, IDNguoiQuanLy)
VALUES 
(N'Thang máy', 3, 'quanly01'),
(N'Bàn ghế', 10, 'quanly02');
GO

CREATE TABLE CanHo (
	SoCanHo NVARCHAR(20) PRIMARY KEY NOT NULL, 
	SoTang VARCHAR(6) NOT NULL,
	SoCuDan INT,
	SoOTo INT,
	SoXeMay INT,
	SoXeDap INT,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
)
GO

INSERT INTO CanHo (SoCanHo, SoTang, IDNguoiQuanLy)
VALUES 
('A101', '1', 'quanly01'),
('B202', '2', 'quanly01'),
('C303', '3','quanly02');
GO

CREATE TABLE CuDan (
	IDCuDan VARCHAR(30) PRIMARY KEY NOT NULL, 
	TenCuDan NVARCHAR(200) NOT NULL,
	NgaySinh DATE,
	GioiTinh NVARCHAR(10),
	GiayToTuyThan VARCHAR(50) NOT NULL,
	SoCanHo VARCHAR(40) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
)
GO

INSERT INTO CuDan (IDCuDan, TenCuDan, NgaySinh, GioiTinh, GiayToTuyThan, SoCanHo, IDNguoiQuanLy)
VALUES 
('cudan01', N'Phạm Văn C', '1990-05-12', N'Nam', '123456789', 'A101', 'quanly01'),
('cudan02', N'Nguyễn Thị E', '1985-07-22', N'Nữ', '987654321', 'B202', 'quanly01'),
('cudan03', N'Lê Thị F', '1995-09-15', N'Nữ', '456789123', 'C303', 'quanly02');
GO

CREATE TABLE CuDan_CanHo (
	IDCuDan VARCHAR(30) NOT NULL,
	SoCanHo NVARCHAR(20) NOT NULL, 
	FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan),
    FOREIGN KEY (SoCanHo) REFERENCES CanHo(SoCanHo)
)
GO

INSERT INTO CuDan_CanHo (IDCuDan, SoCanHo)
VALUES 
('cudan01', 'A101'),
('cudan02', 'B202'),
('cudan03', 'C303');
GO


CREATE TABLE HoaDonCuDan (
	IDHoaDon INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	SoCanHo NVARCHAR(20) NOT NULL,
	SoTien FLOAT NOT NULL,
	HanDong DATE NOT NULL,
	TrangThai NVARCHAR(50) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    FOREIGN KEY (SoCanHo) REFERENCES CanHo(SoCanHo)
)
GO

INSERT INTO HoaDonCuDan (SoCanHo, SoTien, HanDong, TrangThai, IDNguoiQuanLy)
VALUES 
('A101', 500000, '2024-11-30', N'Chưa thanh toán', 'quanly01'),
('B202', 700000, '2024-11-25', N'Quá hạn', 'quanly01'),
('C303', 600000, '2024-12-05', N'Đã thanh toán', 'quanly02');
GO

CREATE TABLE XeOTo (
	IDXeOTo INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	BienSoXe NVARCHAR(50) NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

INSERT INTO XeOTo (BienSoXe, LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
(N'29A-12345', N'Toyota Camry', N'Đen', 'cudan03', 'quanly01'),
(N'30B-67890', N'Honda CR-V', N'Trắng', 'cudan01', 'quanly01'),
(N'31C-54321', N'Ford Ranger', N'Xanh', 'cudan02', 'quanly02');
GO

CREATE TABLE XeMay (
	IDXeMay INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	BienSoXe NVARCHAR(50) NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

INSERT INTO XeMay (BienSoXe, LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
(N'29AB-12345', N'Yamaha Sirius', N'Xanh', 'cudan01', 'quanly01'),
(N'30BC-67890', N'Honda Air Blade', N'Đỏ', 'cudan02', 'quanly01'),
(N'31CD-54321', N'Honda Vision', N'Đen', 'cudan02', 'quanly02');
GO


CREATE TABLE XeDap (
	IDXeDap INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

INSERT INTO XeDap (LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
(N'Xe đạp thể thao', N'Đen', 'cudan01', 'quanly01'),
(N'Xe đạp thường', N'Xanh', 'cudan01', 'quanly01'),
(N'Xe đạp gấp', N'Bạc', 'cudan02', 'quanly02');
GO
