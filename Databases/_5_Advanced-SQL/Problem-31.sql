USE SoftUni
GO

CREATE TABLE TasksLogs(
	Id int IDENTITY,
	OldDate datetime,
	NewDate datetime,
	OldContent nvarchar(MAX),
	NewContent nvarchar(MAX),
	OldEmployeeId int,
	NewEmployeeId int,
	Command nvarchar(10) NOT NULL,
	CONSTRAINT PK_TasksLogId PRIMARY KEY(Id),
	CONSTRAINT FK_TasksLogsNew_Employees
		FOREIGN KEY(OldEmployeeId)
		REFERENCES Employees(EmployeeId),
	CONSTRAINT FK_TasksLogsOld_Employees
		FOREIGN KEY(NewEmployeeId)
		REFERENCES Employees(EmployeeId)
)
GO

CREATE TRIGGER tr_TasksLogsInsertUpdateDelete ON Tasks FOR INSERT, UPDATE, DELETE
AS
	DECLARE @oldDate datetime = (SELECT [Date] FROM DELETED),
			@newDate datetime = (SELECT [Date] FROM INSERTED),
			@oldContent nvarchar(MAX) = (SELECT Content FROM DELETED),
			@newContent nvarchar(MAX) = (SELECT Content FROM INSERTED),
			@oldEmpId int = (SELECT Id FROM DELETED),
			@newEmpId int = (SELECT Id FROM INSERTED),
			@command nvarchar(10),
			@delCount INT,
			@insertCount INT

	SELECT @delCount = COUNT(*) FROM deleted
	SELECT @insertCount = COUNT(*) FROM inserted
	IF(@delCount & @insertCount > 0 )
		SET @command = 'UPDATE'
	ELSE IF (@delCount > 0 )
		SET @command = 'DELETE'
	ELSE IF (@insertCount > 0 )
		SET @command = 'INSERT'

	INSERT INTO TasksLogs(
		OldDate, 
		NewDate, 
		OldContent, 
		NewContent, 
		OldEmployeeId, 
		NewEmployeeId, 
		Command) 
	VALUES(
		@oldDate, 
		@newDate, 
		@oldContent, 
		@newContent, 
		@oldEmpId, 
		@newEmpId, 
		@command)
GO

INSERT INTO Tasks([Date], [Content], [Hours]) VALUES 
	('11-11-11', 'LEEEROOOOOYYY JENKIIIIINNNNSSSSS', 20)

UPDATE Tasks
SET [Date] = '12-12-12',
	[Content] = 'LEEEEEEEEEEEEEEEEEEEEEEERROOOOOOOOOOOOOOYYYY',
	[Hours] = 256
WHERE [Date] = '11-11-11' AND [Hours] = 20