alter procedure usp_MountainsPeaksJSON
as
begin
	declare @result nvarchar(max) = '',
			@prevMountain nvarchar(max) = '',
			@mountain nvarchar(max) = '',

			@prevPeak nvarchar(max) = '',
			@peak nvarchar(max) = '',

			@prevElevation int,
			@elevation int

	declare mountainsPeaks cursor for
		select
			m.MountainRange,
			p.PeakName,
			p.Elevation
		from Mountains m
		left join Peaks p
			on p.MountainId = m.Id

		open mountainsPeaks
		fetch next from mountainsPeaks into @prevMountain, @prevPeak, @prevElevation
		print '{"mountains":[{"name":"' + @prevMountain + '","peaks":[{"name":"' + @prevPeak + '","elevation":' + cast(@prevElevation as nvarchar(max)) + '}'

		while (@@FETCH_STATUS = 0)
		begin
			fetch next from mountainsPeaks into @mountain, @peak, @elevation
			
			if (@prevMountain = @mountain)
				begin
					print ',{"name":"' + @peak + '","elevation":' + cast(@elevation as nvarchar(max)) + '}'
				end
			else
				begin
					if (@peak is null)
						begin
							print ']},{"name":"' + @mountain + '","peaks":['
						end
					else
						begin
							print ']},{"name":"' + @mountain + '","peaks":[{"name":"' + @peak + '","elevation":' + cast(@elevation as nvarchar(max)) + '}'
						end
				
					set @prevMountain = @mountain
				end
		end
		
		print ']}]}'

		close mountainsPeaks
		deallocate mountainsPeaks
end

exec usp_MountainsPeaksJSON
