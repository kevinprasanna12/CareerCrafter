use CareerCrafterDB

select * from users

select * from Employees

select * from JobListings

select * from JobSeekers

select * from Applications

select * from Resumes

ALTER TABLE JobSeekers
ADD CONSTRAINT UQ_JobSeeker_UserId UNIQUE (UserId);
select j.* from JobListings j join Employees e on j.EmployerId=e.EmployerId where j.EmployerId = 9

Insert into Users(Username,Password,Role) values ('admin55','admin12','Employer')

Delete Users where role='something'

DROP INDEX IX_Employees_UserId ON Employees;


CREATE UNIQUE INDEX IX_Employees_UserId ON Employees(UserId);

sp_help JobListings;
sp_help Employees;
