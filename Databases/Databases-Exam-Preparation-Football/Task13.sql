--create table FriendlyMatches(
--	Id int Identity,
--	HomeTeamId int,
--	AwayTeamId int,
--	MatchDate datetime,
--	constraint PK_FriendlyMatchesId primary key(Id),
--	constraint FK_FriendlyMatches_HomeTeams foreign key(HomeTeamId) references Teams(Id),
--	constraint FK_FriendlyMatches_AwayTeams foreign key(AwayTeamId) references Teams(Id)
--)

--INSERT INTO Teams(TeamName) VALUES
-- ('US All Stars'),
-- ('Formula 1 Drivers'),
-- ('Actors'),
-- ('FIFA Legends'),
-- ('UEFA Legends'),
-- ('Svetlio & The Legends')
--GO

--INSERT INTO FriendlyMatches(
--  HomeTeamId, AwayTeamId, MatchDate) VALUES
  
--((SELECT Id FROM Teams WHERE TeamName='US All Stars'), 
-- (SELECT Id FROM Teams WHERE TeamName='Liverpool'),
-- '30-Jun-2015 17:00'),
 
--((SELECT Id FROM Teams WHERE TeamName='Formula 1 Drivers'), 
-- (SELECT Id FROM Teams WHERE TeamName='Porto'),
-- '12-May-2015 10:00'),
 
--((SELECT Id FROM Teams WHERE TeamName='Actors'), 
-- (SELECT Id FROM Teams WHERE TeamName='Manchester United'),
-- '30-Jan-2015 17:00'),

--((SELECT Id FROM Teams WHERE TeamName='FIFA Legends'), 
-- (SELECT Id FROM Teams WHERE TeamName='UEFA Legends'),
-- '23-Dec-2015 18:00'),

--((SELECT Id FROM Teams WHERE TeamName='Svetlio & The Legends'), 
-- (SELECT Id FROM Teams WHERE TeamName='Ludogorets'),
-- '22-Jun-2015 21:00')

--GO

select
	ht.TeamName as [Home Team], 
	at.TeamName as [Away Team],
	fm.MatchDate as [Match Date]
from FriendlyMatches fm
join Teams ht
	on ht.Id = fm.HomeTeamId
join Teams at
	on at.Id = fm.AwayTeamId

UNION

select
	ht.TeamName, 
	at.TeamName,
	tm.MatchDate
from TeamMatches tm
join Teams ht
	on ht.Id = tm.HomeTeamId
join Teams at
	on at.Id = tm.AwayTeamId
order by MatchDate desc
