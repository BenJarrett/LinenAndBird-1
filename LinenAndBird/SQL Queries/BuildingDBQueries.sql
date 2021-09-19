CREATE DATABASE [LinenAndBird]

CREATE TABLE dbo.Birds
	(
	Id uniqueidentifier NOT NULL Primary Key,
	Type int NOT NULL,
	Color varchar(25) NOT NULL,
	Size varchar(10) NOT NULL,
	Name varchar(50) NULL
	)

CREATE TABLE dbo.Hats
	(
	Id uniqueidentifier NOT NULL Primary Key default(newId()), -- from scratch, how to set default value of new GUID
	Designer varchar(200) NULL, -- by default, 'null' option will be default value if not listed
	Color varchar(25) NOT NULL, 
	Style int NOT NULL default(0)
	)

DROP TABLE DBO.HATS -- how to delete a table if you made a mistake creating a table, and are in early stages where you can easily start over

select * from Birds;


create table dbo.Orders
	(
	Id uniqueidentifier NOT NULL Primary Key default(newid()),
	BirdId uniqueidentifier NOT NULL,
	HatId uniqueidentifier NOT NULL,
	Price decimal(18,2) NOT NULL, --(precision, scale) inside decimal()
	CONSTRAINT FK_BirdId_BirdsID FOREIGN KEY (BirdId) -- creates constraint to 
		REFERENCES dbo.Birds (Id),
	CONSTRAINT FK_HatId_HatsID FOREIGN KEY (HatId) 
		REFERENCES dbo.Hats (Id)
	)

select * 
from orders o
	join Hats h
		on h.Id = o.HatId
	join Birds b
		on b.Id = o.BirdId;