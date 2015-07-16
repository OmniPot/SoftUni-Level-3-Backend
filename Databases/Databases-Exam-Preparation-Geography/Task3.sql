--Find all countries along with information about their currency.
--Display the country code, country name and information about its 
--currency: either "Euro" or "Not Euro". Sort the results by country name alphabetically. 
--Submit for evaluation the result grid with headers.

select 
	CountryName, 
	CountryCode, 
	(case CurrencyCode when 'EUR' then 'Euro' else 'Not Euro' end) as Currency
from Countries
order by CountryName
