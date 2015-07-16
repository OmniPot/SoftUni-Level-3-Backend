select
	u.Username,
	g.Name as [Game],
	ch.Name,
	min(st1.Strength)+max(st2.Strength)+sum(st3.Strength) as Strength,
	min(st1.Defence)+max(st2.Defence)+sum(st3.Defence) as Defence,
	min(st1.Speed)+min(st2.Speed)+sum(st3.Speed) as Speed,
	min(st1.Mind)+min(st2.Mind)+sum(st3.Mind) as Mind,
	min(st1.Luck)+min(st2.Luck)+sum(st3.Luck) as Luck
from Users u
left join UsersGames ug
	on ug.UserId = u.Id
join Games g
	on g.Id = ug.GameId
join GameTypes gt
	on gt.Id = g.GameTypeId

join UserGameItems ugi
	on ugi.UserGameId = ug.Id
join Items i
	on i.Id = ugi.ItemId

join Characters ch
	on ch.Id = ug.CharacterId
join [Statistics] st1
	on gt.BonusStatsId = st1.Id
join [Statistics] st2
	on ch.StatisticId = st2.Id
join [Statistics] st3
	on i.StatisticId = st3.Id

group by u.Username, g.Name, ch.Name
order by Strength desc, Defence desc, Speed desc, Mind desc, Luck desc