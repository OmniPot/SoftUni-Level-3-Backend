select
	CountryName,
	CountryCode,
	(case CurrencyCode when 'EUR' then 'Inside' else 'Outside' end)as [Eurozone]
from Countries
order by CountryName