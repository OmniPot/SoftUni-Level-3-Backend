--create view AllQuestions as
--select 
--	u.Id as [UId],
--	u.Username,
--	u.FirstName,
--	u.LastName,
--	u.Email,
--	u.PhoneNumber,
--	u.RegistrationDate,
--	q.Id as [QId],
--	q.Title,
--	q.Content,
--	q.CategoryId,
--	q.UserId,
--	q.CreatedOn
--from Users u
--left join Questions q
--	on q.UserId = u.Id
--GO

create procedure usp_ListUsersQuestions
as 
begin
	create table #UserQuestions(
		UserName nvarchar(50),
		Questions nvarchar(max)
	);

	declare users_questions_cursor cursor read_only for 
		select Username, Title 
		from AllQuestions
		order by Username, Title desc;
	
	declare @username nvarchar(50);
	declare @prevUsername nvarchar(50);
	declare @title nvarchar(max);
	declare @prevTitle nvarchar(max);

	open users_questions_cursor;
	fetch next from users_questions_cursor into @prevUsername, @prevTitle;
	while @@FETCH_STATUS = 0 
		begin  
			fetch next from users_questions_cursor into @username, @title;
			if(@username = @prevUsername)
				begin
					set @prevTitle = @prevTitle + ', ' + @title
				end;
			else
				begin
					insert into #UserQuestions(UserName, Questions)
					values(@prevUsername, @prevTitle)
					set @prevUsername = @username
					set @prevTitle = @title
				end;
		end;
	insert into #UserQuestions(UserName, Questions)
	values(@prevUsername, @prevTitle)

	close users_questions_cursor;
	deallocate users_questions_cursor;

	select UserName, Questions from #UserQuestions;
end
GO

EXEC usp_ListUsersQuestions