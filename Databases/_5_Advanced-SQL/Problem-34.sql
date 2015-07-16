USE SoftUni
GO

CREATE TABLE #EmployeesProjectsTemp(
	EmployeeID int NOT NULL,
	ProjectID int NOT NULL,
	CONSTRAINT PK_EmployeesProjects PRIMARY KEY CLUSTERED (
		EmployeeID ASC,
		ProjectID ASC),
	CONSTRAINT FK_EmployeesProjects_Employees FOREIGN KEY(EmployeeID)
		REFERENCES Employees (EmployeeID),
	CONSTRAINT FK_EmployeesProjects_Projects FOREIGN KEY(ProjectID)
		REFERENCES Projects (ProjectID)
)
GO

INSERT INTO #EmployeesProjectsTemp(EmployeeID, ProjectID) SELECT * FROM EmployeesProjects
GO

TRUNCATE TABLE EmployeesProjects
GO

INSERT INTO EmployeesProjects(EmployeeID, ProjectID) SELECT * FROM #EmployeesProjectsTemp
GO