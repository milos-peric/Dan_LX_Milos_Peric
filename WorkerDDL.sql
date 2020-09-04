--we create database  
CREATE DATABASE WorkerDatabase;
GO

use WorkerDatabase;

GO

 --we delete tables in case they exist
 
DROP TABLE IF EXISTS tblLocation;
DROP TABLE IF EXISTS tblWorker;
DROP TABLE IF EXISTS tblGender;
DROP TABLE IF EXISTS tblSector;


--we create table tblLocation
 CREATE TABLE tblLocation (
    LocationID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Address varchar(50),
	City varchar(50),
	Country varchar(50)   
);

--we create table tblGender
 CREATE TABLE tblGender (
    GenderID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    GenderName nvarchar(50) 
);

--we create table tblSector
 CREATE TABLE tblSector (
    SectorID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    SectorName varchar(50)   
);


--we create table tblWorker
CREATE TABLE tblWorker (
    WorkerID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    FirstName varchar(50),
	LastName varchar(50),
	DateOfBirth date,
	JMBG varchar(50),
	PersonalIDNumber varchar(50),
    PhoneNumber varchar(50),   
	LocationID int FOREIGN KEY REFERENCES tblLocation(LocationID),
	SectorID int FOREIGN KEY REFERENCES tblSector(SectorID),
	ManagerID int FOREIGN KEY REFERENCES tblWorker(WorkerID),
	GenderID int FOREIGN KEY REFERENCES tblGender(GenderID) 
);



GO


CREATE VIEW vwLocation
AS

SELECT LocationID, Address + ', '+ Country + ',' + Country AS FullLocation
FROM dbo.tblLocation

GO

GO


CREATE VIEW vwManager
AS

SELECT        WorkerID, ManagerID, FirstName, LastName, FirstName + ' ' + LastName AS Manager
FROM            dbo.tblWorker

GO

GO
CREATE VIEW vwWorker
AS

SELECT        dbo.tblWorker.WorkerID, dbo.tblWorker.FirstName, dbo.tblWorker.LastName, dbo.tblWorker.DateOfBirth, dbo.tblWorker.JMBG, dbo.tblWorker.PersonalIDNumber, dbo.tblWorker.PhoneNumber, dbo.tblWorker.LocationID, 
                         dbo.tblWorker.SectorID, dbo.tblWorker.ManagerID, dbo.tblWorker.GenderID, dbo.tblGender.GenderName, dbo.tblLocation.Country, dbo.tblLocation.City, dbo.tblLocation.Address, dbo.tblSector.SectorName, 
                         dbo.vwManager.Manager
FROM            dbo.tblGender INNER JOIN
                         dbo.tblWorker ON dbo.tblGender.GenderID = dbo.tblWorker.GenderID INNER JOIN
                         dbo.tblLocation ON dbo.tblWorker.LocationID = dbo.tblLocation.LocationID INNER JOIN
                         dbo.tblSector ON dbo.tblWorker.SectorID = dbo.tblSector.SectorID INNER JOIN
                         dbo.vwManager ON dbo.tblWorker.ManagerID = dbo.vwManager.WorkerID
GO



 INSERT INTO  tblGender VALUES ('Male');
 INSERT INTO  tblGender VALUES ('Female');
 INSERT INTO  tblGender VALUES ('Other');

 INSERT INTO  tblSector VALUES ('HR');
 INSERT INTO  tblSector VALUES ('QA');
 INSERT INTO  tblSector VALUES ('RnD');
 
 INSERT INTO tblWorker VALUES ('None', '', null, null, null, null, null, null, 1, null);
 
  --****************