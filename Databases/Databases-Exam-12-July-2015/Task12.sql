----begin tran

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Blackguard')
----)

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Bottomless Potion of Amplification')
----)

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Eye of Etlich (Diablo III)')
----)

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Gem of Efficacious Toxin')
----)

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Golden Gorget of Leoric')
----)

----insert into UserGameItems(UserGameId, ItemId)
----values(
----	(select ug.Id from UsersGames ug
----		join Games g on g.Id = ug.GameId 
----		join Users u on u.Id = ug.UserId
----	where g.Name = 'Edinburgh' and u.Username = 'Alex'),
----	(select i.id from Items i where i.Name = 'Hellfire Amulet')
----)

----commit

----begin tran

--select ug.Cash from UsersGames ug
--				join Games g on g.Id = ug.GameId 
--				join Users u on u.Id = ug.UserId
--			where g.Name = 'Edinburgh' and u.Username = 'Alex'

----	Blackguard,
----	Bottomless Potion of Amplification,
----  Eye of Etlich (Diablo III), 
----  Gem of Efficacious Toxin,
----  Golden Gorget of Leoric
----	Hellfire Amulet


--update UsersGames
--set Cash = 4702.00
--where Id = (select ug.Id from UsersGames ug
--				join Games g on g.Id = ug.GameId 
--				join Users u on u.Id = ug.UserId
--			where g.Name = 'Edinburgh' and u.Username = 'Alex')

--select
--	u.Username,
--	g.Name,
--	ug.Cash,
--	i.Name as [Item Name]
--from Users u
--join UsersGames ug
--	on ug.UserId = u.Id
--join Games g
--	on g.Id = ug.GameId
--join UserGameItems ugi
--	on ug.Id = ugi.UserGameId
--join Items i
--	on i.Id = ugi.ItemId
--where g.Name = 'Edinburgh'
--order by i.Name