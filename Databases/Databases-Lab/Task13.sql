--create table Towns(
--	Id int identity,
--	Name nvarchar(50) not null,
--	constraint PK_TownsId primary key(Id)
--)

--alter table Users
--add TownId int
--	constraint FK_Users_Towns foreign key(TownId) references Towns(Id)

--INSERT INTO Towns(Name) 
--VALUES ('Sofia'), ('Berlin'), ('Lyon')

--UPDATE Users 
--SET TownId = (SELECT Id FROM Towns WHERE Name='Sofia')

--INSERT INTO Towns 
--VALUES ('Munich'), ('Frankfurt'), ('Varna'), ('Hamburg'), ('Paris'), ('Lom'), ('Nantes')

--update Answers
--set QuestionId = (
--	select q.Id
--	from Questions q
--	where q.Title = 'Java += operator'
--)
--where 
--	datepart(mm, CreatedOn) = 2 
--	and (datename(dw, CreatedOn) = 'Monday'
--	or datename(dw, CreatedOn) = 'Sunday')

--create table #AnswerIds(
--	AnswerId int
--)

--insert into #AnswerIds
--	select v.AnswerId
--	from Votes v
--	group by v.AnswerId
--	having sum(v.Value) < 0

--delete from Votes
--where AnswerId in (
--	select v.AnswerId
--	from Votes v
--	group by v.AnswerId
--	having sum(v.Value) < 0
--)

--drop table #AnswerIds

--insert into Questions
--values(
--	'Fetch NULL values in PDO query',
--	'When I run the snippet, NULL values are converted to empty strings. How can fetch NULL values?',
--	(select Id from Categories where Name = 'Databases'),
--	(select Id from Users where Username = 'darkcat'),
--	getdate()
--)

select 
	t.Name as [Town],
	u.Username as [Username],
	count(a.Id) as [AnswersCount]
from Towns t
left join Users u
	on t.Id = u.TownId
left join Answers a
	on u.Id = a.UserId
group by t.Name, u.Username
order by AnswersCount desc, u.Username