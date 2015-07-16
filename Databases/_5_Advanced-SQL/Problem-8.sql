select count(*) as [Employees without manager]
from Employees e
where e.ManagerID is null