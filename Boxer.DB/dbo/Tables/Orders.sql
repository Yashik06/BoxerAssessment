CREATE TABLE [dbo].[Orders]
(
	[OrderID] INT IDENTITY (1, 1) NOT NULL, 
    [OrderNumber] INT NOT NULL, 
    [Items] NVARCHAR (50) NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Date] DATE NOT NULL, 
    [Price] FLOAT NOT NULL, 
    [Supplier] NVARCHAR (50) NOT NULL, 
    [DeliveryDate] DATE NOT NULL, 
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID]),
    CONSTRAINT [UC_Orders] UNIQUE ([OrderNumber])
)
