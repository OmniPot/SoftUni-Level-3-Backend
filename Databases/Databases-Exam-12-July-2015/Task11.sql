select
	i.Name as [Item],
	i.Price as [Price],
	i.MinLevel as [MinLevel],
	gt.Name as [Forbidden Game Type]
from Items i
left join GameTypeForbiddenItems gtfi
	on gtfi.ItemId = i.Id
left join GameTypes gt
	on gt.Id = gtfi.GameTypeId
order by gt.Name desc, i.Name