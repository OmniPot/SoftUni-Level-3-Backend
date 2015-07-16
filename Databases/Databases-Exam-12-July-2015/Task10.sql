select
	i.Name,
	i.Price,
	i.MinLevel,
	st.Strength,
	st.Defence,
	st.Speed,
	st.Luck,
	st.Mind
from Items i
join [Statistics] st
	on st.Id = i.StatisticId
where (st.Mind > (select avg(Strength) from [Statistics]) and
		st.Luck > (select avg(Luck) from [Statistics]) and
		st.Speed > (select avg(Speed) from [Statistics]))
order by i.Name