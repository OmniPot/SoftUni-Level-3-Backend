create table #Sums(
	SumCash money
)

create function fn_CashInUsersGames(@gameName nvarchar(50)) returns money
as
begin
	declare cur cursor for
		select ug.Cash
		from UsersGames ug
		join Games g
			on ug.GameId = g.Id
		where g.Name = @gameName
		order by ug.Cash desc
	
	declare @sumCash money = 0,
			@cash money = 0,
			@rowNumber int = 1

	open cur
	fetch next from cur into @cash

	while @@FETCH_STATUS = 0
	begin

		if @rowNumber % 2 = 1
		begin
			set @sumCash = @sumCash + @cash 
		end

		set @rowNumber = @rowNumber + 1

		fetch next from cur into @cash
	end

	close cur
	deallocate cur
	
	return @sumCash
end
go

declare @result money
exec @result = fn_CashInUsersGames @gameName = 'Bali'
insert into #Sums(SumCash) values (@result)

declare @result2 money
exec @result2 = fn_CashInUsersGames @gameName = 'Lily Stargazer'
insert into #Sums(SumCash) values (@result2)

declare @result3 money
exec @result3 = fn_CashInUsersGames @gameName = 'Love in a mist'
insert into #Sums(SumCash) values (@result3)

declare @result4 money
exec @result4 = fn_CashInUsersGames @gameName = 'Mimosa'
insert into #Sums(SumCash) values (@result4)

declare @result5 money
exec @result5 = fn_CashInUsersGames @gameName = 'Ming fern'
insert into #Sums(SumCash) values (@result5)

select SumCash from #Sums
order by SumCash

truncate table #Sums