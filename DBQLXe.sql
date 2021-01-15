Use DBQLXe
create table Xe(
 ID	int IDENTITY(1,1) PRIMARY KEY,
 tenXe nvarchar(255),
 hinh varbinary(MAX),
 gia int,
 nguoiBan nvarchar(50),
 IDhang int,
 IDLoai int,
 SLDaBan int,
 SLTonKho int,
);

create table Hang(
 ID int IDENTITY(1,1) PRIMARY KEY,
 logo varbinary(MAX),
 tenHang nvarchar(50),
);
create table NguoiDung(
 taiKhoan nvarchar(50) primary Key,
 ten nvarchar(50),
 anhDaiDien varbinary(MAX),
 ngaySinh DateTime,
 ChucVu char(20)
);

create table BinhLuan(
 ID int IDENTITY(1,1) PRIMARY KEY,
 taiKhoan nvarchar(50),
 IDXe int,
 ngayBinhLuan DateTime,
 noiDung nvarchar(MAX),
);
create table DanhGia(
 taiKhoan nvarchar(50),
 IDxe int,
 SoSao int,
 constraint DanhGia_pk PRIMARY KEY(taiKhoan,IDxe)
);

create table GioHang(
 taiKhoan nvarchar(50),
 IDXe int,
);
create table Loai(
 ID int IDENTITY(1,1) PRIMARY KEY,
 ten nvarchar(50)
)
create table DaGiaoDich(
 TaiKhoan nvarchar(50),
 IDxe int,
 NgayGiaoDich DateTime
)

ALTER TABLE Xe
ADD CONSTRAINT FK_Xe_Hang FOREIGN KEY (IDHang) REFERENCES Hang(ID);
ALTER TABLE Xe
Add CONSTRAINT FK_Xe_NguoiDung FOREIGN KEY (nguoiBan) REFERENCES NguoiDung(taikhoan);
ALTER TABLE Xe
Add CONSTRAINT FK_Xe_Loai FOREIGN KEY (IDLoai) REFERENCES Loai(ID);


ALTER TABLE BinhLuan
ADD CONSTRAINT FK_BinhLuan_Xe FOREIGN KEY (IDxe) REFERENCES Xe(ID);
ALTER TABLE BinhLuan
ADD CONSTRAINT FK_BinhLuan_NguoiDung FOREIGN KEY(taikhoan) REFERENCES NguoiDung(taiKhoan);

ALTER TABLE DanhGia
ADD CONSTRAINT FK_DanhGia_Xe FOREIGN KEY (IDxe) REFERENCES Xe(ID);
ALTER TABLE DanhGia
ADD CONSTRAINT FK_DanhGia_NguoiDung FOREIGN KEY (taiKhoan) REFERENCES NguoiDung(taiKhoan);

ALTER TABLE GioHang
ADD CONSTRAINT FK_GioHang_Xe FOREIGN KEY (IDxe) REFERENCES xe(ID);
ALTER TABLE GioHang
ADD CONSTRAINT FK_GioHang_NguoiDung FOREIGN KEY (taikhoan) REFERENCES NguoiDung(TaiKhoan);

ALTER TABLE DaGiaoDich
ADD CONSTRAINT FK_DaGiaDich_NguoiDung FOREIGN KEY (TaiKhoan) REFERENCES NguoiDung(TaiKhoan);
ALTER TABLE DaGiaoDich
ADD CONSTRAINT FK_DaGiaoDich_Xe FOREIGN KEY (IDxe) REFERENCES Xe(ID); 


create or alter view v_XeMuaNhieu
as
select*
from Xe

create or alter view v_XeMac
as
select *
from Xe

create or alter view v_XeRe
as
select*
from Xe

create or alter function f_XeTrongKhoan(@tu int,@den int)
returns @tableXe table
(
 ID	int,
 tenXe nvarchar(255),
 hinh varbinary(MAX),
 gia int,
 nguoiBan nvarchar(50),
 IDhang int,
 IDLoai int,
 SLDaBan int,
 SLTonKho int
)
AS
Begin
insert into @tableXe
SELECT *
FROM Xe
Where gia>=@tu and gia<=@den
return
End

select *
from f_XeTrongKhoan(10,30000)