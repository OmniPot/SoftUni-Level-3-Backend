select
	u.Username,
	g.Name as [Game],
	count(i.Id) as [Items Count],
	sum(i.Price) as [Items Price]
from Users u
join UsersGames ug
	on ug.UserId = u.Id
join Games g
	on g.Id = ug.GameId
join UserGameItems ugi
	on ugi.UserGameId = ug.Id
join Items i
	on i.Id = ugi.ItemId
group by u.Username, g.Name
having count(i.id) >= 10
order by [Items Count] desc, sum(i.Price) desc, u.Username