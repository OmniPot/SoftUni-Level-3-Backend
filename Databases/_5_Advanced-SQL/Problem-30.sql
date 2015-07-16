INSERT INTO Tasks([Date], [Content], [Hours]) VALUES 
	('01-02-03', 'Take a shit.', 2),
	('06-01-11', 'Move some boxes.', 7),
	('04-08-05', 'Do the paper work.', 40),
	('12-26-08', 'Necessary task to be done.', 11),
	('03-17-15', 'Move the parked cars by 1 slot each to the right.', 2),
	('02-27-09', 'Eat a Corny to get Horny.', 23),
	('03-08-10', 'Sit on the chair for a day.', 13),
	('05-09-12', 'Go back to sleep after a long day at work.', 4),
	('05-10-13', 'Do nothing for 20 hours.', 20)

INSERT INTO Comments(Content, TaskId) VALUES
	('Some really hard work to do.', 1),
	('Dont forget the jars. You need to move them back.', 2),
	('Cant do that within the given time!', 5),
	('Piece of cake! :)', 8),
	('Dont get silly by giving me so much time for Christ sake!', 7),
	('I feel bored doint so ordinary stuff. Can I get a more exciting work?', 9)

UPDATE Tasks
SET Hours = 20
WHERE Hours = 2

UPDATE Tasks
SET [Date] = '01-01-01'
WHERE [Date] = '01-02-03'

DELETE FROM Tasks
WHERE Id BETWEEN 3 AND 4