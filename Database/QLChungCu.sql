USE master;
ALTER DATABASE QLChungCu SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE QLChungCu;
GO

CREATE DATABASE QLChungCu
GO

USE QLChungCu
GO


CREATE TABLE NguoiQuanLy(
	IDNguoiQuanLy VARCHAR(30) PRIMARY KEY NOT NULL,
	TenNguoiQuanLy NVARCHAR(100) NOT NULL,
	NgaySinh DATE,
	SDTNguoiQuanLy VARCHAR(12))
GO

CREATE TABLE Admin(
	IDAdmin VARCHAR(30) PRIMARY KEY NOT NULL,
	TenAdmin NVARCHAR(200) NOT NULL
)
GO

CREATE TABLE CuDan (
	IDCuDan VARCHAR(30) PRIMARY KEY NOT NULL, 
	TenCuDan NVARCHAR(200) NOT NULL,
	NgaySinh DATE,
	GioiTinh NVARCHAR(50),
	GiayToTuyThan VARCHAR(50) NOT NULL,
	SoCanHo VARCHAR(20) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
);
GO

CREATE TABLE TaiKhoan (	 
    IDTaiKhoan VARCHAR(30) PRIMARY KEY NOT NULL,
    MatKhau VARCHAR(50) NOT NULL,
    QuyenHan NVARCHAR(50) NOT NULL,
	Avatar NVARCHAR(MAX),
    IDNguoiQuanLy VARCHAR(30),
    IDAdmin VARCHAR(30),
    IDCuDan VARCHAR(30)
	FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
	FOREIGN KEY (IDAdmin) REFERENCES Admin(IDAdmin),
	FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
);
GO

CREATE TRIGGER trg_AfterInsertNguoiQuanLy
ON NguoiQuanLy
AFTER INSERT
AS
BEGIN
    INSERT INTO TaiKhoan (IDTaiKhoan, MatKhau, QuyenHan, Avatar, IDNguoiQuanLy)
    SELECT 
        IDNguoiQuanLy,
        '123456',
        N'Quản lý',
        'https://uploads.commoninja.com/searchengine/wordpress/user-avatar-reloaded.png',
        IDNguoiQuanLy
    FROM INSERTED
    WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE IDTaiKhoan = INSERTED.IDNguoiQuanLy);
END;
GO
GO

INSERT INTO NguoiQuanLy (IDNguoiQuanLy, TenNguoiQuanLy, NgaySinh, SDTNguoiQuanLy)
VALUES 
('quanly01', N'Lê Thị B', '1990-11-25', '0912345678'),
('quanly02', N'Nguyễn Tiến D', '1989-5-10', '0987654321'),
('quanly03', N'Phạm Tiến A', '1985-10-15', '0134879543');
GO

CREATE TRIGGER trg_AfterInsertAdmin
ON Admin
AFTER INSERT
AS
BEGIN
    INSERT INTO TaiKhoan (IDTaiKhoan, MatKhau, QuyenHan, Avatar, IDAdmin)
    SELECT 
        IDAdmin,
        '123456',
        N'Admin',
		'https://uploads.commoninja.com/searchengine/wordpress/user-avatar-reloaded.png',
        IDAdmin
    FROM INSERTED
    WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE IDTaiKhoan = INSERTED.IDAdmin);
END;
GO

INSERT INTO Admin (IDAdmin, TenAdmin)
VALUES 
('admin01', N'Nguyễn Văn A')
GO

CREATE TRIGGER trg_AfterInsertCuDan
ON CuDan
AFTER INSERT
AS
BEGIN
    INSERT INTO TaiKhoan (IDTaiKhoan, MatKhau, QuyenHan, Avatar, IDCuDan)
    SELECT 
        IDCuDan,
        '123456',
        N'Cư dân',
		'https://uploads.commoninja.com/searchengine/wordpress/user-avatar-reloaded.png',
        IDCuDan
    FROM INSERTED
    WHERE NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE IDTaiKhoan = INSERTED.IDCuDan);
END;
GO

INSERT INTO CuDan (IDCuDan, TenCuDan, NgaySinh, GioiTinh, GiayToTuyThan, SoCanHo, IDNguoiQuanLy)
VALUES 
('cda101-1', N'Nguyễn Văn A', '1985-06-15', N'Nam', 'CMND123456', 'A101', 'quanly01'),
('cda101-2', N'Nguyễn Thị B', '1990-09-10', N'Nữ', 'CMND123457', 'A101', 'quanly01'),

('cda102-1', N'Phạm Văn C', '1978-03-20', N'Nam', 'CMND123458', 'A102', 'quanly01'),
('cda102-2', N'Nguyễn Thị D', '1982-01-25', N'Nữ', 'CMND123459', 'A102', 'quanly01'),

('cda103-1', N'Nguyễn Minh E', '1995-11-05', N'Nam', 'CMND123460', 'A103', 'quanly01'),
('cda103-2', N'Phan Thị F', '1993-04-17', N'Nữ', 'CMND123461', 'A103', 'quanly01'),

('cda201-1', N'Nguyễn Hoàng G', '1980-07-12', N'Nam', 'CMND123462', 'A201', 'quanly02'),
('cda201-2', N'Nguyễn Thị H', '1988-11-28', N'Nữ', 'CMND123463', 'A201', 'quanly02'),

('cda202-1', N'Phạm Minh I', '1992-08-03', N'Nam', 'CMND123464', 'A202', 'quanly02'),
('cda202-2', N'Nguyễn Kim J', '1987-05-22', N'Nữ', 'CMND123465', 'A202', 'quanly02'),

('cda301-1', N'Trần Quang K', '1983-02-11', N'Nam', 'CMND123466', 'A301', 'quanly03'),
('cda301-2', N'Phan Thị L', '1990-07-17', N'Nữ', 'CMND123467', 'A301', 'quanly03'),

('cda302-1', N'Nguyễn Hoàng M', '1989-04-25', N'Nam', 'CMND123468', 'A302', 'quanly03'),
('cda302-2', N'Phạm Minh N', '1992-01-03', N'Nữ', 'CMND123469', 'A302', 'quanly03'),

('cda303-1', N'Nguyễn Thu O', '1994-03-13', N'Nữ', 'CMND123470', 'A303', 'quanly03'),
('cda303-2', N'Phạm Văn P', '1986-12-22', N'Nam', 'CMND123471', 'A303', 'quanly03');
GO

CREATE TABLE MatBangThuongMai(
	IDMBTM INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	DienTich FLOAT,
	TenDonViThue NVARCHAR(50),
	GiaThue FLOAT,
	TinhTrang NVARCHAR(50) NOT NULL,
	IDNguoiQuanLy VARCHAR(30)
	FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
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
    CONSTRAINT FK_HDTM_MBTM FOREIGN KEY (IDMBTM) REFERENCES MatBangThuongMai(IDMBTM)
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
    CONSTRAINT FK_VT_NQL FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy)
);
GO

INSERT INTO VatTu (TenVatTu, SoLuong, IDNguoiQuanLy)
VALUES 
(N'Thang máy', 3, 'quanly01'),
(N'Bàn ghế', 10, 'quanly02');
GO

CREATE TABLE Tang(
	SoTang VARCHAR(6) PRIMARY KEY NOT NULL,
	TongCanHo INT DEFAULT 0
);
GO

INSERT INTO Tang (SoTang)
VALUES 
('1'),
('2'),
('3'),
('4'),
('5'),
('6'),
('7'),
('8'),
('9'),
('10');
GO

CREATE TABLE CanHo (
	SoCanHo NVARCHAR(20) PRIMARY KEY NOT NULL, 
	SoTang VARCHAR(6) NOT NULL,
	SoCuDan INT DEFAULT 0,
	SoOTo INT DEFAULT 0,
	SoXeMay INT DEFAULT 0,
	SoXeDap INT DEFAULT 0,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    CONSTRAINT FK_CH_NQL FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_CH_Tang FOREIGN KEY (SoTang) REFERENCES Tang(SoTang)
)
GO

CREATE TRIGGER trg_UpdateTongCanHo
ON CanHo
AFTER INSERT, DELETE
AS
BEGIN
    -- Cập nhật tổng căn hộ khi thêm mới
    UPDATE Tang
    SET TongCanHo = (
        SELECT COUNT(*)
        FROM CanHo c
        WHERE c.SoTang = Tang.SoTang
    )
    WHERE Tang.SoTang IN (
        SELECT SoTang FROM inserted -- Thay đổi do thêm
        UNION
        SELECT SoTang FROM deleted -- Thay đổi do xóa
    );
END;
GO

-- Thêm dữ liệu vào bảng CanHo
INSERT INTO CanHo (SoCanHo, SoTang, IDNguoiQuanLy)
VALUES 
('A101', '1', 'quanly01'),
('A102', '1', 'quanly01'),
('A103', '1', 'quanly01'),
('A104', '1', 'quanly01'),
('A105', '1', 'quanly01'),

('A201', '2', 'quanly01'),
('A202', '2', 'quanly01'),
('A203', '2', 'quanly01'),
('A204', '2', 'quanly01'),
('A205', '2', 'quanly01'),

('A301', '3', 'quanly02'),
('A302', '3', 'quanly02'),
('A303', '3', 'quanly02'),
('A304', '3', 'quanly02'),
('A305', '3', 'quanly02')
GO

CREATE TABLE CuDan_CanHo (
	IDCuDan VARCHAR(30) NOT NULL,
	SoCanHo NVARCHAR(20) NOT NULL, 
	CONSTRAINT FK_CDCH_CD FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan),
    CONSTRAINT FK_CDCH_CH FOREIGN KEY (SoCanHo) REFERENCES CanHo(SoCanHo)
)
GO

CREATE TRIGGER trg_UpdateTongCuDan
ON CuDan_CanHo
AFTER INSERT, DELETE
AS
BEGIN
	PRINT N'Trigger'
    -- Cập nhật tổng số cư dân trong bảng CanHo sau khi chèn hoặc xóa
    UPDATE CanHo
    SET SoCuDan = (
        SELECT COUNT(*)
        FROM CuDan_CanHo
        WHERE CuDan_CanHo.SoCanHo = CanHo.SoCanHo
    )
    FROM CanHo
    INNER JOIN CuDan_CanHo ON CanHo.SoCanHo = CuDan_CanHo.SoCanHo
    WHERE CuDan_CanHo.IDCuDan IN (
        SELECT IDCuDan FROM inserted -- Thay đổi do thêm
        UNION
        SELECT IDCuDan FROM deleted -- Thay đổi do xóa
    );
END;
GO

INSERT INTO CuDan_CanHo (IDCuDan, SoCanHo)
VALUES
('cda101-1', 'A101'),
('cda101-2', 'A101'),
('cda102-1', 'A102'),
('cda102-2', 'A102'),
('cda103-1', 'A103'),
('cda103-2', 'A103'),
('cda201-1', 'A201'),
('cda201-2', 'A201'),
('cda202-1', 'A202'),
('cda202-2', 'A202'),
('cda301-1', 'A301'),
('cda301-2', 'A301'),
('cda302-1', 'A302'),
('cda302-2', 'A302'),
('cda303-1', 'A303'),
('cda303-2', 'A303');
GO

CREATE TABLE HoaDonCuDan (
	IDHoaDon INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	SoCanHo NVARCHAR(20) NOT NULL,
	SoTien FLOAT NOT NULL,
	HanDong DATE NOT NULL,
	TrangThai NVARCHAR(50) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    CONSTRAINT FK_HoaDonCuDan_NguoiQuanLy FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_HoaDonCuDan_CanHo FOREIGN KEY (SoCanHo) REFERENCES CanHo(SoCanHo)
)
GO

INSERT INTO HoaDonCuDan (SoCanHo, SoTien, HanDong, TrangThai, IDNguoiQuanLy)
VALUES 
('A101', 500000, '2024-12-01', N'Chưa thanh toán', 'quanly01'),
('A102', 750000, '2024-12-05', N'Chưa thanh toán', 'quanly01'),
('A103', 600000, '2024-12-10', N'Chưa thanh toán', 'quanly01'),
('A201', 700000, '2024-12-01', N'Chưa thanh toán', 'quanly01'),
('A202', 850000, '2024-12-05', N'Chưa thanh toán', 'quanly01'),
('A203', 650000, '2024-12-10', N'Chưa thanh toán', 'quanly01'),
('A301', 600000, '2024-12-01', N'Chưa thanh toán', 'quanly02'),
('A302', 750000, '2024-12-05', N'Chưa thanh toán', 'quanly02'),
('A303', 650000, '2024-12-10', N'Chưa thanh toán', 'quanly02')
GO

CREATE TABLE XeOTo (
	IDXeOTo INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	BienSoXe NVARCHAR(50) NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    CONSTRAINT FK_XeOTo_NguoiQuanLy FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_XeOTo_CuDan FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

CREATE TRIGGER trg_UpdateTongOTo
ON XeOTo
AFTER INSERT, DELETE
AS
BEGIN
    -- Cập nhật tổng số ô tô trong bảng CanHo sau khi chèn hoặc xóa xe
    UPDATE CanHo
    SET SoOTo = (
        SELECT COUNT(*)
        FROM XeOTo
        WHERE XeOTo.IDCuDan = CuDan_CanHo.IDCuDan
    )
    FROM CanHo
    INNER JOIN CuDan_CanHo ON CanHo.SoCanHo = CuDan_CanHo.SoCanHo
    WHERE CuDan_CanHo.IDCuDan IN (
        SELECT IDCuDan FROM inserted -- Thay đổi do thêm
        UNION
        SELECT IDCuDan FROM deleted -- Thay đổi do xóa
    );
END;
GO

INSERT INTO XeOTo (BienSoXe, LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
('29A-12345', N'Sedan', N'Đỏ', 'cda101-1', 'quanly01'),
('29A-12346', N'Hatchback', N'Vàng', 'cda101-2', 'quanly01'),

('29B-23456', N'SUV', N'Xanh', 'cda102-1', 'quanly01'),
('29B-23457', N'Coupe', N'Đen', 'cda102-2', 'quanly01'),

('29C-34568', N'Vehicle', N'Màu bạc', 'cda103-2', 'quanly01'),

('29D-45678', N'SUV', N'Xanh lá', 'cda201-1', 'quanly02'),
('29D-45679', N'Pickup', N'Xám', 'cda201-2', 'quanly02'),

('29E-56789', N'Hatchback', N'Màu hồng', 'cda202-1', 'quanly02'),

('29F-67890', N'Sedan', N'Màu bạc', 'cda301-1', 'quanly02'),

('29G-78902', N'SUV', N'Màu vàng', 'cda302-2', 'quanly03'),

('29H-89012', N'Sedan', N'Màu đen', 'cda303-1', 'quanly03');
GO

CREATE TABLE XeMay (
	IDXeMay INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	BienSoXe NVARCHAR(50) NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    CONSTRAINT FK_XeMay_NguoiQuanLy FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_XeMay_CuDan FOREIGN KEY (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

CREATE TRIGGER trg_UpdateTongXeMay
ON XeMay
AFTER INSERT, DELETE
AS
BEGIN
    -- Cập nhật tổng số ô tô trong bảng CanHo sau khi chèn hoặc xóa xe
    UPDATE CanHo
    SET SoXeMay = (
        SELECT COUNT(*)
        FROM XeMay
        WHERE XeMay.IDCuDan = CuDan_CanHo.IDCuDan
    )
    FROM CanHo
    INNER JOIN CuDan_CanHo ON CanHo.SoCanHo = CuDan_CanHo.SoCanHo
    WHERE CuDan_CanHo.IDCuDan IN (
        SELECT IDCuDan FROM inserted -- Thay đổi do thêm
        UNION
        SELECT IDCuDan FROM deleted -- Thay đổi do xóa
    );
END;
GO

INSERT INTO XeMay (BienSoXe, LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
('29M-12345', N'Xe tay ga', N'Đỏ', 'cda101-1', 'quanly01'),
('29M-12346', N'Xe phân khối lớn', N'Xanh', 'cda102-2', 'quanly01'),
('29M-23456', N'Xe số', N'Vàng', 'cda201-1', 'quanly02'),
('29M-23457', N'Xe tay ga', N'Trắng', 'cda301-2', 'quanly02'),
('29M-34567', N'Xe phân khối lớn', N'Đen', 'cda303-2', 'quanly03');
GO

CREATE TABLE XeDap (
	IDXeDap INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	LoaiXe NVARCHAR(100),
	MauXe NVARCHAR(50),
	IDCuDan VARCHAR(30) NOT NULL,
	IDNguoiQuanLy VARCHAR(30) NOT NULL,
    CONSTRAINT FK_XeDap_NguoiQuanLy FOREIGN KEY (IDNguoiQuanLy) REFERENCES NguoiQuanLy(IDNguoiQuanLy),
    CONSTRAINT FK_XeDap_CuDan FOREIGN KEY  (IDCuDan) REFERENCES CuDan(IDCuDan)
)
GO

CREATE TRIGGER trg_UpdateTongXeDap
ON XeDap
AFTER INSERT, DELETE
AS
BEGIN
    -- Cập nhật tổng số ô tô trong bảng CanHo sau khi chèn hoặc xóa xe
    UPDATE CanHo
    SET SoXeDap = (
        SELECT COUNT(*)
        FROM XeDap
        WHERE XeDap.IDCuDan = CuDan_CanHo.IDCuDan
    )
    FROM CanHo
    INNER JOIN CuDan_CanHo ON CanHo.SoCanHo = CuDan_CanHo.SoCanHo
    WHERE CuDan_CanHo.IDCuDan IN (
        SELECT IDCuDan FROM inserted -- Thay đổi do thêm
        UNION
        SELECT IDCuDan FROM deleted -- Thay đổi do xóa
    );
END;
GO

INSERT INTO XeDap (LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy)
VALUES 
(N'Xe đạp thể thao', N'Đỏ', 'cda101-1', 'quanly01'),
(N'Xe đạp điện', N'Xanh', 'cda102-2', 'quanly01'),
(N'Xe đạp gấp', N'Vàng', 'cda201-1', 'quanly02'),
	(N'Xe đạp thể thao', N'Trắng', 'cda301-2', 'quanly02'),
	(N'Xe đạp điện', N'Đen', 'cda303-2', 'quanly03');
GO
