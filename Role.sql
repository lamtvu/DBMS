create login DKhang with password = '123123';
create user DKhang for login DKhang;
create login PDuc with password = '123123';
create user PDuc for login PDUC;

create role Developer_role;
grant ALL to Developer_role;
grant select,update,delete,insert to Developer_role
alter role Developer_role add member[DKhang];
alter role Developer_role add member[PDuc];






grant select on Xe to nguoidung_role
grant select on Hang to nguoidung_role
grant insert,select on DanhGia to nguoidung_role
grant insert on DaGiaoDich to nguoidung_role
GRANT EXECUTE to nguoidung_role
create role nguoidung_role;


grant select to quanly_role
grant update,Delete,insert on Xe to quanly_role
grant update on Hang to quanly_role
GRANT EXECUTE to quanly_role
create role quanly_role;


create role admin_role;
grant ALL to admin_role;
grant select,update,delete,insert to admin_role;

DROP user TLam1
DROP Login TLam1



CREATE TRIGGER Mua
ON DaGiaoDich
AFTER INSERT
AS
declare @soslcon int
declare @idxe int
select @soslcon = Xe.SLTonKho,@idxe = Xe.ID
from inserted,Xe
where inserted.IDxe = Xe.ID
if(@soslcon = 0)
	rollback;
else
begin
	update Xe set Xe.SLDaBan =Xe.SLDaBan+1 where ID = @idxe
	update Xe set Xe.SLTonKho = Xe.SLTonKho - 1 where ID = @idxe 
end
from 