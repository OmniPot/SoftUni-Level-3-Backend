select 
	Content,
	CreatedOn
from Answers
where CreatedOn between '2012-06-15 00:00:00' and '2013-03-21 23:59:59'
order by CreatedOn, Id