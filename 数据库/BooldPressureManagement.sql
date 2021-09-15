create database BloodPressureManagement on(
name='BloodPressureManagement',
filename='E:\不爱学习\非语言\大三卓越项目\数据库\BloodPressureManagement.mdf'
)
log on(
name='BloodPressureManagement_log',
filename='E:\不爱学习\非语言\大三卓越项目\数据库\BloodPressureManagement.ldf'
)
use BloodPressureManagement


--1.总店表Head_office
create table Head_office(
Hid int primary key identity(1,1),	-- 总店id
Hname nvarchar(50) not null,		-- 总店名
Contact nvarchar(50) not null,		-- 联系人
Phone nvarchar(11) not null			-- 电话
)

insert into Head_office values('新农合大药房红莲新店','小徐','18290384901')
insert into Head_office values('心健大药房红星健康福店','小龙','14908765211')
insert into Head_office values('海王星辰健康药房黄兴南路店','小李','17678421341')
select * from Head_office
--2.分店表Branch
create table Branch(
Bid int primary key identity(1,1),					-- 分店id
Hid int foreign key references Head_office(Hid),	-- 总店id
Bnane nvarchar(50) not null,						-- 分店名
Province nvarchar(50) not null,						-- 所在省
City nvarchar(50) not null,							-- 所在市
County nvarchar(50) not null,						-- 所在县/区
DetailAddress nvarchar(50) not null,				-- 详情地址
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
insert into Branch values(1,'好邻居新店','湖南省','长沙市','高桥友谊村','湖南省长沙市高桥友谊村桂友路一号门面')
insert into Branch values(1,'先锋新店','湖南省','长沙市','天心区','大托镇先锋村刘家坪组中意一路1190号')

insert into Branch values(2,'荷花东路店','湖南省','长沙市','芙蓉区','荷花东路东方新城K1-102号')
insert into Branch values(2,'城南路分店','湖南省','长沙市','雨花区','城南路88-102号1栋105')
insert into Branch values(3,'韶山路店','湖南省','长沙市','雨花区','韶山中路421号深国投资公司二楼')
insert into Branch values(3,'登隆店','湖南省','长沙市','芙蓉区','登隆街32号2栋第一层')
insert into Branch values(1,'泰合新店','湖南省','长沙市','天心区','芙蓉中路3段558号巧克力空间105号门面')
select * from Branch
--3.检测者(顾客)表 Customer
--create table Customer(
--Cid int primary key identity(1,1),				-- 检测者id
--Cname nvarchar(50) not null,					-- 姓名	
--Csex char(2) not null,							-- 性别
--Cage int not null,								-- 年龄
--Phone nvarchar(11) not null,					-- 联系电话
--IdentificationCard  nvarchar(18) not null,		-- 身份证号码
--MemberNumber nvarchar(50) not null,				-- 会员号
--Bid int foreign key references Branch(Bid),		-- 分店id
--Number int,										-- 检查次数
--LastTime nvarchar(50)							-- 最后检测时间
--)

--3.检测者(顾客)表 UserInfo
create table UserInfo(
Uid int primary key identity(1,1),				-- 检测者id
Uacount nvarchar(50) not null,					-- 账号
Upwd nvarchar(50) not null,						-- 密码
Uname nvarchar(50) not null,					-- 姓名	
Usex char(2) not null,							-- 性别
Uage int not null,								-- 年龄
Phone nvarchar(11) not null,					-- 联系电话
IdentificationCard  nvarchar(18) not null,		-- 身份证号码
Bid int foreign key references Branch(Bid),		-- 分店id
Number int,										-- 检查次数
LastTime Date									-- 最后检测时间
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

insert into UserInfo values('wuhu','123','张三','男',55,'19568486512','431689196605153546',2002,1,'2020-1-9')
insert into UserInfo values('dsm','123456','李四','女',60,'13564968764','458645196106215864',2003,2,'2021-3-3')
insert into UserInfo values('www','666','王五','男',52,'18946546545','489456413512313212',2005,3,'2021-4-9')
insert into UserInfo values('lolo','888','赵六','女',63,'14523131233','465654685468465465',2005,4,'2019-8-2')


insert into Customer values('李小龙','女',60,'19856457645','465434654135135214','08809',1002,1,'2019-8-2')
insert into Customer values('吴小康','男',57,'13654654651','495786756875875788','08810',1003,3,'2018-2-7')
insert into Customer values('徐凤年','女',70,'18654651213','437675354324321355','08811',1005,4,'2021-5-5')
insert into Customer values('徐龙象','男',38,'17846513212','496872354245324232','08812',1006,3,'2020-4-8')
insert into Customer values('陈龙','女',67,'16874531541','486778687687687687','08813',1005,1,'2021-3-9')
delete from Customer where Cid=2010

delete from UserInfo where Uid = 1015
drop trigger tri_DelUserProRecord
--创建一个触发器，删除检测者之前，先删除所有它所有的检测记录
create trigger tri_DelUserProRecord on UserInfo instead of delete
as
    --判断此检测者是否有检测记录
	--条件为要被删除的检测者id(即将被删除的那一列记录存在于deleted表中)
	if exists(select * from Projects where Uid = (select Uid from deleted))
		begin
			--删除所有检测记录
			delete from Projects where Uid = (select Uid from deleted)
			--并删除检测者
			delete from UserInfo where Uid = (select Uid from deleted)
		end
	else
		begin
			--删除检测者
			delete from UserInfo where Uid= (select Uid from deleted)
		end
go
drop trigger tri_DelCustProRecord
--4.检测项目(血压)表 Projects
create table Projects(
Did int primary key identity(1,1),				-- 检测项目id
Uid int foreign key references UserInfo(Uid),	-- 检测者id
Dname nvarchar(50) not null,					-- 检测项目名  血压
Util nvarchar(50) not null,						-- 单位/mmHg
ShouSuoYa nvarchar(50) not null,				-- 收缩压
ShuZhangYa nvarchar(50) not null,				-- 舒张压
Method nvarchar(50) not null,					-- 检测方法
Decipher nvarchar(50) not null,					-- 结果解读	  使用触发器来检测结果
Pro_Time date
)
select * from Projects
drop table Projects
insert into Projects values(2010,'血压','mmHg','90','180','血压仪','血压正常') 
-- 用触发器，判断舒张压和收缩压的范围，再插入结果
-- 正常收缩压90-140  正常舒张压60-90
--如果收缩压低于90mmHg，舒张压低于60mmHg，属于低血压


--超级管理员账号 SuperAdmin
create table SuperAdmin(
Aid int primary key identity(1,1),			-- 账号id
Aacount nvarchar(50) not null,				-- 账号
Apwd nvarchar(50) not null,				    -- 密码
)

insert into SuperAdmin values('admin','123456')
select * from SuperAdmin
delete from SuperAdmin where Aid = 2
--5.系统账号表 Account_number
create table Account_number(
Aid int primary key identity(1,1),			-- 账号id
Number nvarchar(50) not null,				-- 账号
Account_Name nvarchar(50) not null,			-- 姓名
Phone nvarchar(11) not null,				-- 联系电话
Bid int foreign key references Branch(Bid),	-- 分店id
Account_State nvarchar(50) not null			-- 状态
)
--5.管理员账号表 AdminInfo
create table AdminInfo(
Aid int primary key identity(1,1),			-- 账号id
Aacount nvarchar(50) not null,				-- 账号
Apwd nvarchar(50) not null,					-- 密码
Aname nvarchar(10) not null,				-- 姓名
AOr bit not null,							-- 判断（是否为总店管理员）
Phone nvarchar(11) not null,				-- 联系电话
Bid int foreign key references Branch(Bid),	-- 分店id
Account_State bit not null			-- 状态
)
insert into AdminInfo values('hlx','123456','黄礼贤',1,'17674674233',1,1)
insert into AdminInfo values('xkm','123456','徐昆贸',0,'17674674232',1,1)
insert into AdminInfo values('wzx','123456','吴治宪',0,'17674674231',1,1)
select * from AdminInfo
--6.角色表 SysRole
create table SysRole(
Rid int primary key identity(1,1),	-- 角色id
Rname nvarchar(50) not null,		-- 角色名
Describle nvarchar(50) not null,	-- 角色描述
Remarks nvarchar(50) not null		-- 备注
)

--7.菜单表
create table Menu(
Mid int primary key identity(1,1),	-- 菜单id
MenuUrl nvarchar(50) not null,		-- 菜单路径
MenuType nvarchar(50) not null,		-- 菜单类型
SuperIor nvarchar(50) not null,		-- 上级菜单
Remarks nvarchar(50) not null		-- 备注
)

--8.账号-角色表 Anrole
create table Anrole(
Aid int foreign key references Account_number(Aid),	-- 账号id
Rid int foreign key references SysRole(Rid),		-- 角色id
)

--9.菜单-角色表(菜单角色中间表)Menurole
create table Menurole(
Mid int foreign key references Menu(Mid),			-- 菜单id
Rid int foreign key references SysRole(Rid),		-- 角色id
)

--10.设备信息表
--create table Equipments(
--Eid int primary key identity(1,1),
--EName nvarchar(30) not null,
--EProductDate datetime not null,
--IsActivite bit not null,
--Cid int foreign key references Customer(Cid),
--)

select c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bnane,c.Number,c.LastTime from Customer c,Branch b
where c.Bid = b.Bid and c.Cname = '张三' and c.Phone = '19568486512'

--创建存储过程，根据姓名，联系电话、分店查询对应用户(因为要频繁查询)
create proc proc_namePhoBranch
-- 输入参数
@Cname varchar(30),		-- 检测者姓名
@Cphone varchar(11),	-- 检测者电话
@BranchId int			-- 分店id 
as
	select c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bid,c.Number,c.LastTime from Customer c,Branch b
	where c.Bid = b.Bid and c.Cname = @Cname and c.Phone = @Cphone and b.Bnane = (select Bnane from Branch where Bid = @BranchId)
go
drop proc proc_namePhoBranch
exec proc_namePhoBranch '张三','19568486512',2

-- 存储过程实现检测者列表分页
-- N页数  每页记录数pagesize
select top 2 c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bnane,c.Number,c.LastTime from Customer c,Branch b
where c.Bid = b.Bid and c.Cid not in(select top ((2-1)*2) Cid from Customer order by Cid)

create proc proc_ExaminerPaging
@PageIndex int,		--页数
@PageSize  int		--每页记录数
as
	select top (@PageSize) c.Cid,c.Cname,c.Csex,c.Cage,c.Phone,c.MemberNumber,c.IdentificationCard,b.Bid,c.Number,c.LastTime from Customer c,Branch b
	where c.Bid = b.Bid and c.Cid not in(select top ((@PageIndex-1) * @PageSize) Cid from Customer order by Cid)
go

exec proc_ExaminerPaging 2,2