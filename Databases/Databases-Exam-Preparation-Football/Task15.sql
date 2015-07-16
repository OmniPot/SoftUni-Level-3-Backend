create procedure fn_TeamsJSON as
	
	declare match cursor 
	for
		select 
			t.TeamName,
			(select TeamName from Teams where Id = tm.HomeTeamId) as [HomeTeam],
			HomeGoals,
			(select TeamName from Teams where Id = tm.AwayTeamId) as [AwayTeam],
			AwayGoals,
			tm.MatchDate,
			convert(nvarchar(max), MatchDate, 103) [MatchDate]
		from Teams t
		left join TeamMatches tm
			on t.Id = tm.HomeTeamId or t.Id = tm.AwayTeamId
		where t.CountryCode = 'BG'
		order by t.TeamName, tm.MatchDate desc

	declare @previousTeam nvarchar(max) = '',
			@currentTeam nvarchar(max) = '',
			@homeTeam nvarchar(max) = '',
			@awayTeam nvarchar(max) = '',
			@homeGoals int,
			@awayTeams int,
			@rawDate datetime,
			@date nvarchar(max) = ''

	open match

	fetch next from match into 
		@currentTeam,
		@homeTeam,
		@homeGoals,
		@awayTeam,
		@awayGoals,
		@rawDate,
		@date

	print '{"teams":['
	while (@@FETCH_STATUS = 0)
	begin
		if (@previousTeam != @currentTeam)
		begin
			 if(@previousTeam != '')
			 begin
				print ']'
			 end
			print '{"name":"' + @currentTeam + '","matches":['
			
		end

		if (@previousTeam = @currentTeam)
		begin
			print '{"' + @homeTeam + '":' + cast(@homeGoals as nvarchar(2)) + '}'
		end
	end
	


--drop procedure fn_TeamsJSON