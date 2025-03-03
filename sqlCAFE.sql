create database quanly_quancafe;
use quanly_quancafe;
go
 drop database quanly_quancafe;

CREATE TABLE NguoiDung ( 
    TenTK VARCHAR(100) PRIMARY KEY,       -- Tên tài khoản (khóa chính)
    MatKhau NVARCHAR(300) NOT NULL,       -- Mật khẩu đã được mã hóa
    VaiTro NVARCHAR(20) NOT NULL CHECK (VaiTro IN ('Admin', 'User')) -- Vai trò: Admin hoặc User
);
select * from nguoidung

-- Bảng nhân viên
CREATE TABLE NhanVien (
    NhanVienID VARCHAR(100) PRIMARY KEY,  -- Sửa lại NhanVienID thành NVARCHAR(100)
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    SoDienThoai NVARCHAR(15),
    DiaChi NVARCHAR(255),
    TenTK VARCHAR(100),
    FOREIGN KEY (TenTK) REFERENCES NguoiDung(TenTK)
);

-- Bảng danh mục đồ ăn uống
CREATE TABLE DanhMuc (
    DanhMucID INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL
);

-- Bảng khu vực
CREATE TABLE KhuVuc (
    KhuVucID INT PRIMARY KEY IDENTITY(1,1),
    TenKhuVuc NVARCHAR(50) NOT NULL
);
-- Bảng đồ ăn uống
CREATE TABLE DoAnUong (
    DoAnUongID INT PRIMARY KEY IDENTITY(1,1),
    TenDoAnUong NVARCHAR(100) NOT NULL,
    Gia DECIMAL(10, 2) NOT NULL,
    DanhMucID INT NOT NULL,
    FOREIGN KEY (DanhMucID) REFERENCES DanhMuc(DanhMucID)
);



-- Bảng bàn
CREATE TABLE Ban (
    BanID INT PRIMARY KEY IDENTITY(1,1),
    SoBan NVARCHAR(10) NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Trống' CHECK (TrangThai IN (N'Trống', N'Đang Sử Dụng')),
    KhuVucID INT NOT NULL,
    FOREIGN KEY (KhuVucID) REFERENCES KhuVuc(KhuVucID)
);

CREATE TABLE HoaDon (
    HoaDonID INT PRIMARY KEY IDENTITY(1,1),
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    TongTien DECIMAL(10, 2) NOT NULL DEFAULT 0,
    NhanVienID VARCHAR(100) NOT NULL,   -- Thay TenTK bằng NhanVienID
    BanID INT NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa Thanh Toán' CHECK (TrangThai IN (N'Chưa Thanh Toán', N'Đã Thanh Toán')),
    NgayThanhToan DATETIME NULL,  -- Cột Ngày Thanh Toán
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID),  -- Tham chiếu đến bảng NhanVien
    FOREIGN KEY (BanID) REFERENCES Ban(BanID)   -- Tham chiếu đến bảng Ban
);

-- Bảng chi tiết hóa đơn
CREATE TABLE ChiTietHoaDon (
    ChiTietID INT PRIMARY KEY IDENTITY(1,1),
    HoaDonID INT NOT NULL,
    DoAnUongID INT NOT NULL,
    SoLuong INT NOT NULL,
    Gia DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (HoaDonID) REFERENCES HoaDon(HoaDonID),
    FOREIGN KEY (DoAnUongID) REFERENCES DoAnUong(DoAnUongID)
);



-- Trigger xóa chi tiết hóa đơn và cập nhật trạng thái bàn
ALTER TRIGGER trg_XoaMonAn
ON ChiTietHoaDon
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Xóa hóa đơn nếu không còn món ăn nào trong chi tiết hóa đơn
    DELETE FROM HoaDon
    WHERE HoaDonID IN (
        SELECT d.HoaDonID
        FROM HoaDon d
        LEFT JOIN ChiTietHoaDon c ON d.HoaDonID = c.HoaDonID
        WHERE c.HoaDonID IS NULL
    );
END;




select * from ban
select * from HoaDon
select * from ChiTietHoaDon
delete from ChiTietHoaDon


ALTER TABLE HoaDon
ALTER COLUMN TongTien DECIMAL(10, 2) NULL;


CREATE TRIGGER trg_UpdateTongTien
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật tổng tiền cho hóa đơn có thay đổi trong chi tiết hóa đơn (INSERTED)
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        UPDATE HoaDon
        SET TongTien = (
            SELECT COALESCE(SUM(SoLuong * Gia), 0)
            FROM ChiTietHoaDon
            WHERE HoaDonID = i.HoaDonID
        )
        FROM inserted i
        WHERE HoaDon.HoaDonID = i.HoaDonID;
    END

    -- Cập nhật tổng tiền cho hóa đơn có thay đổi trong chi tiết hóa đơn (DELETED)
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        UPDATE HoaDon
        SET TongTien = (
            SELECT COALESCE(SUM(SoLuong * Gia), 0)
            FROM ChiTietHoaDon
            WHERE HoaDonID = d.HoaDonID
        )
        FROM deleted d
        WHERE HoaDon.HoaDonID = d.HoaDonID;
    END
END


CREATE TABLE [dbo].[tbStore](
	[id] [bigint] NOT NULL,
	[nameStore] [nvarchar](500) NULL,
	[addressStore] [nvarchar](500) NULL,
	[phoneStore] [nvarchar](500) NULL,
	[taxCode] [nvarchar](250) NULL,
 CONSTRAINT [PK_tbStore] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbStore] ([id], [nameStore], [addressStore], [phoneStore], [taxCode]) VALUES (202109221152128393, N'Quán cafe Tuấn Anh', N'Không rõ địa chỉ', N'0944369698', N'000000000000')
/****** Object:  Table [dbo].[tbProduct]    Script Date: 01/19/2022 00:01:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

-- Thêm dữ liệu vào bảng NguoiDung (Người dùng)
INSERT INTO NguoiDung (HoTen, SoDienThoai, Email, TenTK, MatKhau, VaiTro)
VALUES 
(N'Nguyễn Văn A', '0123456789', 'nguyenvana@email.com', 'nguyenvana', 'password123', 'User'),
(N'Phạm Thị B', '0987654321', 'phamthib@email.com', 'phamthib', 'password456', 'Admin');

-- Thêm dữ liệu vào bảng NhanVien (Nhân viên)
INSERT INTO NhanVien (NhanVienID, HoTen, NgaySinh, SoDienThoai, DiaChi, TenTK)
VALUES 
('NV001', 'Nguyễn Văn A', '1990-05-12', '0123456789', 'Hà Nội', 'nguyenvana'),
('NV002', 'Phạm Thị B', '1985-08-25', '0987654321', 'Hồ Chí Minh', 'phamthib');

-- Thêm dữ liệu vào bảng DanhMuc (Danh mục đồ ăn uống)
INSERT INTO DanhMuc (TenDanhMuc)
VALUES 
(N'Đồ uống'),
(N'Đồ ăn vặt');

-- Thêm dữ liệu vào bảng DoAnUong (Đồ ăn uống)
INSERT INTO DoAnUong (TenDoAnUong, Gia, DanhMucID)
VALUES 
(N'Cà phê', 20000, 1),
(N'Tea', 15000, 1),
(N'Bánh mì', 25000, 2),
(N'Bánh ngọt', 18000, 2),
(N'Sinrami', 22000, 2);

-- Thêm dữ liệu vào bảng KhuVuc (Khu vực trong quán)
INSERT INTO KhuVuc (TenKhuVuc)
VALUES 
(N'Khu A'),
(N'Khu B');

-- Thêm dữ liệu vào bảng Ban (Bàn trong quán)
INSERT INTO Ban (SoBan, TrangThai, KhuVucID)
VALUES 
(N'Bàn 1', N'Trống', 1),
(N'Bàn 2', N'Đang Sử Dụng', 2),
(N'Bàn 3', N'Trống', 1),
(N'Bàn 4', N'Trống', 2);

-- Thêm dữ liệu vào bảng HoaDon (Hóa đơn)
INSERT INTO HoaDon (NhanVienID, BanID, TrangThai)
VALUES 
('NV001', 1, N'Chưa Thanh Toán'),
('NV002', 2, N'Chưa Thanh Toán');

-- Thêm dữ liệu vào bảng ChiTietHoaDon (Chi tiết hóa đơn)
INSERT INTO ChiTietHoaDon (HoaDonID, DoAnUongID, SoLuong, Gia)
VALUES 
(1, 1, 2, 20000),  -- 2 phần Cà phê
(1, 3, 1, 25000),  -- 1 phần Bánh mì
(2, 2, 3, 15000),  -- 3 phần Tea
(2, 5, 1, 22000);  -- 1 phần Sinrami



-- Tắt ràng buộc khóa ngoại
ALTER TABLE ChiTietHoaDon NOCHECK CONSTRAINT ALL;
ALTER TABLE HoaDon NOCHECK CONSTRAINT ALL;
ALTER TABLE Ban NOCHECK CONSTRAINT ALL;
ALTER TABLE DoAnUong NOCHECK CONSTRAINT ALL;
ALTER TABLE KhuVuc NOCHECK CONSTRAINT ALL;

-- Xóa dữ liệu từ các bảng
DELETE FROM ChiTietHoaDon;
DELETE FROM HoaDon;
DELETE FROM Ban;
DELETE FROM DoAnUong;
DELETE FROM KhuVuc;

-- Bật lại ràng buộc khóa ngoại
ALTER TABLE ChiTietHoaDon CHECK CONSTRAINT ALL;
ALTER TABLE HoaDon CHECK CONSTRAINT ALL;
ALTER TABLE Ban CHECK CONSTRAINT ALL;
ALTER TABLE DoAnUong CHECK CONSTRAINT ALL;
ALTER TABLE KhuVuc CHECK CONSTRAINT ALL;


SELECT * FROM NguoiDung;
SELECT * FROM DanhMuc;
SELECT * FROM KhuVuc;
SELECT * FROM Ban;
SELECT * FROM DoAnUong;
SELECT * FROM NhanVien;
SELECT * FROM HoaDon;
SELECT * FROM ChiTietHoaDon;

drop trigger trgUpdateTongTien

-- Xóa bảng chi tiết hóa đơn
DROP TABLE IF EXISTS ChiTietHoaDon;

-- Xóa bảng hóa đơn
DROP TABLE IF EXISTS HoaDon;

-- Xóa bảng đồ ăn uống
DROP TABLE IF EXISTS DoAnUong;

-- Xóa bảng bàn
DROP TABLE IF EXISTS Ban;

-- Xóa bảng khu vực
DROP TABLE IF EXISTS KhuVuc;

-- Xóa bảng danh mục
DROP TABLE IF EXISTS DanhMuc;
-- Xóa bảng nhân viên
DROP TABLE IF EXISTS NhanVien;
-- Xóa bảng người dùng
DROP TABLE IF EXISTS NguoiDung;


