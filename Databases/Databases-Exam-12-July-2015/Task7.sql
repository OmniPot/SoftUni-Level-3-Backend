select
	g.Name as [Game],
	gt.Name as [Game Type],
	u.Username,
	ug.Level,
	ug.Cash,
	c.Name as [Character]
from Users u
join UsersGames ug
	on ug.UserId = u.Id
join Games g
	on g.Id = ug.GameId
join GameTypes gt
	on gt.Id = g.GameTypeId
join Characters c
	on c.Id = ug.CharacterId
order by ug.Level desc, u.Username, g.Name