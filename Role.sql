create login DKhang with password = '123123';
create user DKhang for login DKhang;
create login PDuc with password = '123123';
create user PDuc for login PDUC;

create role Developer_role;
grant ALL to Developer_role;
grant select,update,delete,insert to Developer_role
alter role Developer_role add member[DKhang];
alter role Developer_role add member[PDuc];
