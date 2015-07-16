CREATE TABLE beers (
    `drunkOn` DATE,
    `description` VARCHAR(50)
)
PARTITION BY RANGE (YEAR(drunkOn)) (
    PARTITION part0 VALUES LESS THAN (2025),
    PARTITION part1 VALUES LESS THAN (2035),
    PARTITION part2 VALUES LESS THAN (9999)
);

#Create stored procedure that fills the table with beers.

DELIMITER $$
CREATE PROCEDURE fill_beers()
BEGIN
	DECLARE daysToAdd INT DEFAULT 1;   
	DECLARE description VARCHAR (50) DEFAULT 'Beers need no description!...';   
    
    WHILE daysToAdd < 1000000 DO
		INSERT INTO beers(drunkOn, description)
		VALUES(ADDDATE(NOW(), INTERVAL daysToAdd DAY), description);
		SET daysToAdd = daysToAdd + 1;
	END WHILE;
END$$

#Run the procedure to fill the table..... IMPORTANT !!! VERY SLOW

CALL fill_beers();

#No tests because Im using a slow hard drive and its impossible to insert 1 000 000 rows. Its just crashes.