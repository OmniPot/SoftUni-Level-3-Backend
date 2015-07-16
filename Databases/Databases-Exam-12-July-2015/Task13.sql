--begin tran
--insert into UserGameItems(UserGameId, ItemId)
--values (110, 16),
--	(110, 45),
--	(110, 108),
--	(110, 111),
--	(110, 176),
--	(110, 184),
--	(110, 191),
--	(110, 194),
--	(110, 195),
--	(110, 247),
--	(110, 280),
--	(110, 475),
--	(110, 500),
--	(110, 552)

--commit tran

--begin tran
--insert into UserGameItems(UserGameId, ItemId)
--values
--	(110, 4), 
--	(110, 28),
--	(110, 34),
--	(110, 55),
--	(110, 100),
--	(110, 135),
--	(110, 140),
--	(110, 157),
--	(110, 163),
--	(110, 209),
--	(110, 222),
--	(110, 282),
--	(110, 330),
--	(110, 367),
--	(110, 479),
--	(110, 511),
--	(110, 521),
--	(110, 532),
--	(110, 560)

--commit tran

--select i.Id from Items i where i.MinLevel in(19, 20, 21)

select 
	i.Name as [Item Name]
from Items i
join UserGameItems ugi
	on ugi.ItemId = i.Id
join UsersGames ug
	on ug.Id = ugi.UserGameId
join Games g
	on g.Id = ug.GameId
join Users u
	on u.Id = ug.UserId
where g.Name = 'Safflower'
order by i.Name

--begin tran
--update UsersGames
--set Cash = Cash - (select i.Price from Items i where i.Name = 'Blackguard')
--where Id = (select ug.Id from UsersGames ug
--				join Games g on g.Id = ug.GameId 
--				join Users u on u.Id = ug.UserId
--			where g.Name = 'Edinburgh' and u.Username = 'Alex')

--16    --11-12
--45
--108
--111
--176
--184
--191
--194
--195
--247
--280
--475
--500
--552

--4   --19-21
--28
--34
--55
--100
--135
--140
--157
--163
--209
--222
--282
--330
--367
--479
--511
--521
--532
--560