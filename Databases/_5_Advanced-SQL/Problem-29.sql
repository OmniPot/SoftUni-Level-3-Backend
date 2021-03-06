CREATE TABLE Tasks(
	[Id] INT IDENTITY,
	[Date] DATETIME,
	[Content] NVARCHAR(MAX) NOT NULL,
	[Hours] INT NOT NULL,
	CONSTRAINT PK_TaskId PRIMARY KEY(Id)
)

CREATE TABLE Comments(
	[Id] INT IDENTITY,
	[Content] NVARCHAR(MAX) NOT NULL,
	[TaskId] int NOT NULL,
	CONSTRAINT PK_CommentId PRIMARY KEY(Id),
	CONSTRAINT FK_Comments_Tasks FOREIGN KEY(TaskId) REFERENCES Tasks(Id)
)