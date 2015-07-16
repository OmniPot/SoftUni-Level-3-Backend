--Find the number of countries for each currency. 
--Display three columns: currency code, currency description and number of countries. Sort the results by number 
--of countries (from highest to lowest), then by currency description alphabetically.
--Name the columns exactly like in the table below. Submit for evaluation the result grid with headers.

select
	cu.CurrencyCode,
	Description as [Currency],
	count(co.CurrencyCode) as [NumberOfCountries]
from Currencies cu
left join Countries co
	on cu.CurrencyCode = co.CurrencyCode
group by cu.CurrencyCode, Description
order by [NumberOfCountries] desc, [Description]