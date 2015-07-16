alter procedure usp_ListUsersAds
as
begin
	create table #UsersAds(
		UserName nvarchar(30),
		AdDates nvarchar(max)
	)

	declare ads cursor
	for
		select
			u.UserName as [UserName],
			convert(nvarchar(30), a.Date, 112) as [AdDates]
		from AspNetUsers u
		left join ads a
			on u.Id = a.OwnerId
		order by u.UserName desc, a.Date

		declare @prevUser nvarchar(30) = ''
		declare @prevDate nvarchar(max) = ''
		declare @user nvarchar(30) = ''
		declare @date nvarchar(max) = ''


		open ads
		fetch next from ads into @prevUser, @prevDate
		while @@FETCH_STATUS = 0 
		begin  
			fetch next from ads into @user, @date;
			if(@user = @prevUser)
				begin
					set @prevDate = @prevDate + '; ' + @date
				end;
			else
				begin
					insert into #UsersAds(UserName, AdDates)
					values(@prevUser, @prevDate)
					set @prevUser = @user
					set @prevDate = @date
				end;
		end;
		insert into #UsersAds(UserName, AdDates)
		values(@prevUser, @prevDate)

		close ads
		deallocate ads

		select * from #UsersAds

		drop table #UsersAds
end
go

exec usp_ListUsersAds