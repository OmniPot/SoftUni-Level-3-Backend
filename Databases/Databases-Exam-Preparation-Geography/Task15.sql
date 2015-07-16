CREATE TABLE Monasteries(
	Id int IDENTITY,
	Name nvarchar(100) NOT NULL,
	CountryCode char(2) NOT NULL,
	CONSTRAINT PK_MonasteryId PRIMARY KEY(Id),
	CONSTRAINT FK_Monastaries_Countries FOREIGN KEY(CountryCode)
		REFERENCES Countries(CountryCode)
)

INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

ALTER TABLE Countries
ADD IsDeleted bit DEFAULT(0) NOT NULL

UPDATE Countries
SET IsDeleted = 1
WHERE CountryCode IN (
	select c.CountryCode from Countries c
	join CountriesRivers cr
		on cr.CountryCode = c.CountryCode
	join Rivers r
		on cr.RiverId = r.Id
	group by c.CountryCode
	having COUNT(r.Id) > 3
)

select
	m.Name as Monastery,
	c.CountryName as Country
from Monasteries m
join Countries c
	on c.CountryCode = m.CountryCode
where IsDeleted = 0
order by m.Name

update Countries
set CountryName = 'Burma'
where CountryName = 'Myanmar'

insert into Monasteries (Name, CountryCode)
values 
('Hanga Abbey', (select CountryCode from Countries where CountryName = 'Tanzania')),
('Myin-Tin-Daik', (select CountryCode from Countries where CountryName = 'Myanmar'))

select con.ContinentName, cou.CountryName, count(m.Id) as MonasteriesCount
from Continents con
left join Countries cou
	on cou.ContinentCode = con.ContinentCode
left join Monasteries m
	on m.CountryCode = cou.CountryCode
where cou.IsDeleted = 0
group by ContinentName, CountryName
order by MonasteriesCount desc, CountryName

