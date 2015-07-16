create function fn_MountainsPeaksJSON() RETURNS nvarchar(MAX)
AS
BEGIN
	DECLARE @json nvarchar(MAX) = '{"mountains":['

	DECLARE mountainsCursor CURSOR
	FOR	SELECT Id, MountainRange FROM Mountains

	OPEN mountainsCursor
	DECLARE @mountainName NVARCHAR(MAX),
			@mountainId INT 

	FETCH NEXT FROM mountainCursor INTO @mountainId, @mountainName

	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @json = @json + '{"name":"' + @mountainName + '","peaks":['

			DECLARE peaksCursor CURSOR FOR
			SELECT PeakName, Elevation FROM Peaks
			WHERE MountainId = @mountainId

			OPEN peaksCursor
			DECLARE @peakName NVARCHAR(MAX),
					@elevation INT

			FETCH NEXT FROM peaksCursor INTO @peakName, @elevation
			WHILE @@FETCH_STATUS = 0
				BEGIN
					set @json = @json + '{"name":"' + @peakName + '",' + '"elevation"' + CONVERT(NVARCHAR(MAX), @elevation) + '}'
					FETCH NEXT FROM peaksCursor INTO @peakName, @elevation
					IF @@FETCH_STATUS = 0
						SET @json = @json + ','
				END
			CLOSE peaksCursor
			DEALLOCATE peaksCursor
			SET @json = @json + ']}'
		END
	CLOSE mountainsCursor
	DEALLOCATE mountainsCursor
	SET @json = @json + ']}'

	RETURN @json
END