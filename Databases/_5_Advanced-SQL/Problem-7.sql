select count(*) as [Employees with manager]
from Employees e
where e.ManagerID is not null