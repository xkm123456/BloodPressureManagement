create database BloodPressureManagement on(
name='BloodPressureManagement',
filename='E:\����ѧϰ\������\����׿Խ��Ŀ\���ݿ�\BloodPressureManagement.mdf'
)
log on(
name='BloodPressureManagement_log',
filename='E:\����ѧϰ\������\����׿Խ��Ŀ\���ݿ�\BloodPressureManagement.ldf'
)
use BloodPressureManagement


--1.�ܵ��Head_office
create table Head_office(
Hid int primary key identity(1,1),	-- �ܵ�id
Hname nvarchar(50) not null,		-- �ܵ���
Contact nvarchar(50) not null,		-- ��ϵ��
Phone nvarchar(11) not null			-- �绰
)

insert into Head_office values('��ũ�ϴ�ҩ�������µ�','С��','18290384901')
insert into Head_office values('�Ľ���ҩ�����ǽ�������','С��','14908765211')
insert into Head_office values('�����ǳ�����ҩ��������·��','С��','17678421341')
select * from Head_office
--2.�ֵ��Branch
create table Branch(
Bid int primary key identity(1,1),					-- �ֵ�id
Hid int foreign key references Head_office(Hid),	-- �ܵ�id
Bnane nvarchar(50) not null,						-- �ֵ���
Province nvarchar(50) not null,						-- ����ʡ
City nvarchar(50) not null,							-- ������
County nvarchar(50) not null,						-- ������/��
DetailAddress nvarchar(50) not null,				-- �����ַ
)

delete  from Projects
delete  from AdminInfo
delete from Branch
delete from UserInfo where Uid = 1011
select * from Head_office
select * from Branch
select * from UserInfo
select * from Projects
select * from AdminInfo

select c.Uname,
                          c.Usex,
                          c.Uage,
                          p.Decipher,
                          p.Pro_Time,
                          c.Phone,
                          c.LastTime from UserInfo c,Projects p
where c.Uid = p.Uid 
insert into Branch values(1,'���ھ��µ�','����ʡ','��ɳ��','���������','����ʡ��ɳ�и�����������·һ������')
insert into Branch values(1,'�ȷ��µ�','����ʡ','��ɳ��','������','�������ȷ������ƺ������һ·1190��')

insert into Branch values(2,'�ɻ���·��','����ʡ','��ɳ��','ܽ����','�ɻ���·�����³�K1-102��')
insert into Branch values(2,'����·�ֵ�','����ʡ','��ɳ��','�껨��','����·88-102��1��105')
insert into Branch values(3,'��ɽ·��','����ʡ','��ɳ��','�껨��','��ɽ��·421�����Ͷ�ʹ�˾��¥')
insert into Branch values(3,'��¡��','����ʡ','��ɳ��','ܽ����','��¡��32��2����һ��')
insert into Branch values(1,'̩���µ�','����ʡ','��ɳ��','������','ܽ����·3��558���ɿ����ռ�105������')
select * from Branch
--3.�����(�˿�)�� Customer
--create table Customer(
--Cid int primary key identity(1,1),				-- �����id
--Cname nvarchar(50) not null,					-- ����	
--Csex char(2) not null,							-- �Ա�
--Cage int not null,								-- ����
--Phone nvarchar(11) not null,					-- ��ϵ�绰
--IdentificationCard  nvarchar(18) not null,		-- ���֤����
--MemberNumber nvarchar(50) not null,				-- ��Ա��
--Bid int foreign key references Branch(Bid),		-- �ֵ�id
--Number int,										-- ������
--LastTime nvarchar(50)							-- �����ʱ��
--)

--3.�����(�˿�)�� UserInfo
create table UserInfo(
Uid int primary key identity(1,1),				-- �����id
Uacount nvarchar(50) not null,					-- �˺�
Upwd nvarchar(50) not null,						-- ����
Uname nvarchar(50) not null,					-- ����	
Usex char(2) not null,							-- �Ա�
Uage int not null,								-- ����
Phone nvarchar(11) not null,					-- ��ϵ�绰
IdentificationCard  nvarchar(18) not null,		-- ���֤����
Bid int foreign key references Branch(Bid),		-- �ֵ�id
Number int,										-- ������
LastTime Date									-- �����ʱ��
)
select * from UserInfo



select * from UserInfo where LastTime between '2019-03-04' and '2021-03-04' and phone = '14523131233' and Bid = 1002
select u.Uname,u.Usex,u.Uage,u.Phone,u.LastTime,b.Bnane
from UserInfo u,Projects p,Branch b
where u.Uid = p.Uid and u.Bid = b.Bid
select * from Branch
select * from Projects
select * from UserInfo
select DATEADD(day,-2,Pro_Time) from Projects
delete from UserInfo where Uid = 5
select * from Branch
select * from Branch

insert into UserInfo values('wuhu','123','����','��',55,'19568486512','431689196605153546',2002,1,'2020-1-9')
insert into UserInfo values('dsm','123456','����','Ů',60,'13564968764','458645196106215864',2003,2,'2021-3-3')
insert into UserInfo values('www','666','����','��',52,'18946546545','489456413512313212',2005,3,'2021-4-9')
insert into UserInfo values('lolo','888','����','Ů',63,'14523131233','465654685468465465',2005,4,'2019-8-2')


insert into Customer values('��С��','Ů',60,'19856457645','465434654135135214','08809',1002,1,'2019-8-2')
insert into Customer values('��С��','��',57,'13654654651','495786756875875788','08810',1003,3,'2018-2-7')
insert into Customer values('�����','Ů',70,'18654651213','437675354324321355','08811',1005,4,'2021-5-5')
insert into Customer values('������','��',38,'17846513212','496872354245324232','08812',1006,3,'2020-4-8')
insert into Customer values('����','Ů',67,'16874531541','486778687687687687','08813',1005,1,'2021-3-9')
delete from Customer where Cid=2010

delete from UserInfo where Uid = 1015
drop trigger tri_DelUserProRecord
--����һ����������ɾ�������֮ǰ����ɾ�����������еļ���¼
create trigger tri_DelUserProRecord on UserInfo instead of delete
as
    --�жϴ˼�����Ƿ��м���¼
	--����ΪҪ��ɾ���ļ����id(������ɾ������һ�м�¼������deleted����)
	if exists(select * from Projects where Uid = (select Uid from deleted))
		begin
			--ɾ�����м���¼
			delete from Projects where Uid = (select Uid from deleted)
			--��ɾ�������
			delete from UserInfo where Uid = (select Uid from deleted)
		end
	else
		begin
			--ɾ�������
			delete from UserInfo where Uid= (select Uid from deleted)
		end
go
drop trigger tri_DelCustProRecord
--4.�����Ŀ(Ѫѹ)�� Projects
create table Projects(
Did int primary key identity(1,1),				-- �����Ŀid
Uid int foreign key references UserInfo(Uid),	-- �����id
Dname nvarchar(50) not null,					-- �����Ŀ��  Ѫѹ
Util nvarchar(50) not null,						-- ��λ/mmHg
ShouSuoYa nvarchar(50) not null,				-- ����ѹ
ShuZhangYa nvarchar(50) not null,				-- ����ѹ
Method nvarchar(50) not null,					-- ��ⷽ��
Decipher nvarchar(50) not null,					-- ������	  ʹ�ô������������
Pro_Time date
)
select * from Projects
drop table Projects
insert into Projects values(2010,'Ѫѹ','mmHg','90','180','Ѫѹ��','Ѫѹ����') 
-- �ô��������ж�����ѹ������ѹ�ķ�Χ���ٲ�����
-- ��������ѹ90-140  ��������ѹ60-90
--�������ѹ����90mmHg������ѹ����60mmHg�����ڵ�Ѫѹ


--��������Ա�˺� SuperAdmin
create table SuperAdmin(
Aid int primary key identity(1,1),			-- �˺�id
Aacount nvarchar(50) not null,				-- �˺�
Apwd nvarchar(50) not null,				    -- ����
)

insert into SuperAdmin values('admin','123456')
select * from SuperAdmin
delete from SuperAdmin where Aid = 2
--5.ϵͳ�˺ű� Account_number
create table Account_number(
Aid int primary key identity(1,1),			-- �˺�id
Number nvarchar(50) not null,				-- �˺�
Account_Name nvarchar(50) not null,			-- ����
Phone nvarchar(11) not null,				-- ��ϵ�绰
Bid int foreign key references Branch(Bid),	-- �ֵ�id
Account_State nvarchar(50) not null			-- ״̬
)
--5.����Ա�˺ű� AdminInfo
create table AdminInfo(
Aid int primary key identity(1,1),			-- �˺�id
Aacount nvarchar(50) not null,				-- �˺�
Apwd nvarchar(50) not null,					-- ����
Aname nvarchar(10) not null,				-- ����
AOr bit not null,							-- �жϣ��Ƿ�Ϊ�ܵ����Ա��
Phone nvarchar(11) not null,				-- ��ϵ�绰
Bid int foreign key references Branch(Bid),	-- �ֵ�id
Account_State bit not null			-- ״̬
)
insert into AdminInfo values('hlx','123456','������',1,'17674674233',1,1)
insert into AdminInfo values('xkm','123456','����ó',0,'17674674232',1,1)
insert into AdminInfo values('wzx','123456','������',0,'17674674231',1,1)
select * from AdminInfo
--6.��ɫ�� SysRole
create table SysRole(
Rid int primary key identity(1,1),	-- ��ɫid
Rname nvarchar(50) not null,		-- ��ɫ��
Describle nvarchar(50) not null,	-- ��ɫ����
Remarks nvarchar(50) not null		-- ��ע
)

--7.�˵���
create table Menu(
Mid int primary key identity(1,1),	-- �˵�id
MenuUrl nvarchar(50) not null,		-- �˵�·��
MenuType nvarchar(50) not null,		-- �˵�����
SuperIor nvarchar(50) not null,		-- �ϼ��˵�
Remarks nvarchar(50) not null		-- ��ע
)

--8.�˺�-��ɫ�� Anrole
create table Anrole(
Aid int foreign key references Account_number(Aid),	-- �˺�id
Rid int foreign key references SysRole(Rid),		-- ��ɫid
)

--9.�˵�-��ɫ��(�˵���ɫ�м��)Menurole
create table Menurole(
Mid int foreign key references Menu(Mid),			-- �˵�id
Rid int foreign key references SysRole(Rid),		-- ��ɫid
)

--10.�豸��Ϣ��
--create table Equipments(
--Eid int primary key identity(1,1),
--EName nvarchar(30) not null,
--EProductDate datetime not null,
--IsActivite bit not null,
--Cid int foreign key references Customer(Cid),
--)

select c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bnane,c.Number,c.LastTime from Customer c,Branch b
where c.Bid = b.Bid and c.Cname = '����' and c.Phone = '19568486512'

--�����洢���̣�������������ϵ�绰���ֵ��ѯ��Ӧ�û�(��ΪҪƵ����ѯ)
create proc proc_namePhoBranch
-- �������
@Cname varchar(30),		-- ���������
@Cphone varchar(11),	-- ����ߵ绰
@BranchId int			-- �ֵ�id 
as
	select c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bid,c.Number,c.LastTime from Customer c,Branch b
	where c.Bid = b.Bid and c.Cname = @Cname and c.Phone = @Cphone and b.Bnane = (select Bnane from Branch where Bid = @BranchId)
go
drop proc proc_namePhoBranch
exec proc_namePhoBranch '����','19568486512',2

-- �洢����ʵ�ּ�����б��ҳ
-- Nҳ��  ÿҳ��¼��pagesize
select top 2 c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bnane,c.Number,c.LastTime from Customer c,Branch b
where c.Bid = b.Bid and c.Cid not in(select top ((2-1)*2) Cid from Customer order by Cid)

create proc proc_ExaminerPaging
@PageIndex int,		--ҳ��
@PageSize  int		--ÿҳ��¼��
as
	select top (@PageSize) c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bid,c.Number,c.LastTime from Customer c,Branch b
	where c.Bid = b.Bid and c.Cid not in(select top ((@PageIndex-1) * @PageSize) Cid from Customer order by Cid)
go

exec proc_ExaminerPaging 2,2