--create table Countries(
--	Id int identity,
--	Name nvarchar(50) not null
--	constraint PK_CountryId primary key(Id)
--)

--alter table Towns
--	add CountryId int
--	constraint FK_Towns_Countries foreign key(CountryId) references Countries(Id)

--INSERT INTO Countries(Name) VALUES ('Bulgaria'), ('Germany'), ('France')
--UPDATE Towns SET CountryId = (SELECT Id FROM Countries WHERE Name='Bulgaria')
--INSERT INTO Towns VALUES
--('Munich', (SELECT Id FROM Countries WHERE Name='Germany')),
--('Frankfurt', (SELECT Id FROM Countries WHERE Name='Germany')),
--('Berlin', (SELECT Id FROM Countries WHERE Name='Germany')),
--('Hamburg', (SELECT Id FROM Countries WHERE Name='Germany')),
--('Paris', (SELECT Id FROM Countries WHERE Name='France')),
--('Lyon', (SELECT Id FROM Countries WHERE Name='France')),
--('Nantes', (SELECT Id FROM Countries WHERE Name='France'))

--update Ads
--set TownId = (select Id from Towns where Name = 'Paris')
--where dateName(dw, Date) = 'Friday'

--update Ads
--set TownId = (select Id from Towns where Name = 'Hamburg')
--where dateName(dw, Date) = 'Thursday'

--delete from Ads
--where OwnerId in (
--	select u.Id
--	from AspNetUsers u
--	join AspNetUserRoles ur
--		on ur.UserId = u.Id
--	join AspNetRoles r
--		on r.Id = ur.RoleId
--	where r.Name = 'Partner'
--	)

--insert into Ads(Title, Text, Date, OwnerId, StatusId)
--values(
--	'Free Book', 
--	'Free C# Book', 
--	getDate(),
--	(select u.Id from AspNetUsers u where UserName = 'nakov'), 
--	(select s.Id from AdStatuses s where s.Status = 'Waiting Approval')
--)

--select
--	t.Name as [Town],
--	c.Name as [Country],
--	count(a.Id) as [AdsCount]
--from Towns t
--full join Countries c
--	on c.Id = t.CountryId
--full join Ads a
--	on a.TownId = t.Id
--group by t.Name, c.Name
--order by t.Name, c.Name