﻿----Seed data for Orders

----UNCOMMENT SCRIPT WHEN YOU NEED TO SEED USERS AND THEN PUBLISH. IF YOU TRY TO RUN THE SCRIPT 
----AND THE TABLES ALREADY EXIST WITH EXISTING VALUES THAT MATCH THE SEED DATA YOU WILL GET AN ERROR
----///---------------------------------------------------------------------------------------------///

--USE [Boxer.DB]
--GO

--IF (EXISTS (SELECT *
--	FROM INFORMATION_SCHEMA.TABLES
--	WHERE TABLE_SCHEMA = 'dbo'
--	AND TABLE_NAME = 'Orders'))
--	BEGIN
--		INSERT INTO [dbo].[Orders]
--			([OrderNumber],[Items],[Quantity],[Date],[Price],[Supplier],[DeliveryDate])
--		VALUES
--			(1001,'Apples',10,'2023-01-01',2.99,'FreshFarm','2023-01-10'),
--			(1002,'Bananas',6,'2023-02-01',1.29,'FruitWorld','2023-02-10'),
--			(1003,'Carrots',20,'2023-03-01',0.99,'VeggieMart','2023-03-10'),
--			(1004,'Chicken Breasts',15,'2023-04-01',5.49,'MeatSupplier','2023-04-10'),
--			(1005,'Bread',8,'2023-05-01',1.99,'BakeryCo','2023-05-10'),
--			(1006,'Milk',12,'2023-06-01',1.49,'DairyFarm','2023-06-10'),
--			(1007,'Tomatoes',25,'2023-07-01',3.99,'GardenFresh','2023-07-10'),
--			(1008,'Potatoes',18,'2023-08-01',2.49,'FarmStand','2023-08-10'),
--			(1009,'Cheese',30,'2023-09-01',4.99,'DairyDelights','2023-09-10'),
--			(1010,'Eggs',22,'2023-10-01',2.79,'FarmEggs','2023-10-10')
--	END;
--ELSE
--	BEGIN
--		PRINT 'No Table in database - Orders'
--	END;
