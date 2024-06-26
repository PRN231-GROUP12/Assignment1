USE master
GO

CREATE DATABASE FStoreDB;
GO

USE FStoreDB;
GO

CREATE TABLE Member (
	Id int NOT NULL PRIMARY KEY,
	Email varchar(100) NOT NULL,
	CompanyName varchar(40) NOT NULL,
	City varchar(15) NOT NULL,
	Country varchar(15) NOT NULL,
	Password varchar(30) NOT NULL
);
GO

CREATE TABLE [Order] (
	Id int NOT NULL PRIMARY KEY,
	MemberId int FOREIGN KEY REFERENCES Member(Id) ON DELETE CASCADE,
	OrderDate datetime NOT NULL,
	RequiredDate datetime,
	ShippedDate datetime,
	Freight money
);
GO
CREATE TABLE Category(
	Id int NOT NULL PRIMARY KEY,
	CategoryName varchar(40) NOT NULL,
);
GO

CREATE TABLE Product (
	Id int NOT NULL PRIMARY KEY,
	CategoryId int FOREIGN KEY REFERENCES Category(CategoryId) ON DELETE CASCADE,
	ProductName varchar(40) NOT NULL,
	Weight varchar(20) NOT NULL,
	UnitPrice money NOT NULL,
	UnitsInStock int NOT NULL
);
GO


CREATE TABLE OrderDetail (
	Id int NOT NULL PRIMARY KEY,
	OrderId int REFERENCES [Order](Id) ON DELETE CASCADE,
	ProductId int,
	UnitPrice money NOT NULL,
	Quantity int NOT NULL,
	Discount float NOT NULL,
);
GO

INSERT [dbo].[Member] ([Id], [Email], [CompanyName], [City], [Country], [Password]) VALUES (1, N'member1@fstore.com', N'KMS', N'HCM', N'Viet nam', N'1')
GO
INSERT [dbo].[Member] ([Id], [Email], [CompanyName], [City], [Country], [Password]) VALUES (2, N'member2@fstore.com', N'CyberSoft', N'HCM', N'Viet nam', N'1')

INSERT [dbo].[Category]([Id], [CategoryName]) VALUES (1, N'Food')
GO

INSERT [dbo].[Category]([Id], [CategoryName]) VALUES (2, N'Drink')

GO
INSERT [dbo].[Order] ([Id], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (4665, 1, CAST(N'2021-11-05 12:05:07.677' AS DateTime), CAST(N'2021-11-04 00:00:00.000' AS DateTime), CAST(N'2021-11-05 00:00:00.000' AS DateTime), 10000.0000)
GO
INSERT [dbo].[Order] ([Id], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (6113, 2, CAST(N'2021-11-05 14:04:07.950' AS DateTime), CAST(N'2021-11-04 00:00:00.000' AS DateTime), CAST(N'2021-11-05 00:00:00.000' AS DateTime), 20000.0000)
GO
INSERT [dbo].[Order] ([Id], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (6259, 1, CAST(N'2021-11-05 14:02:50.557' AS DateTime), CAST(N'2021-11-06 00:00:00.000' AS DateTime), CAST(N'2021-11-07 00:00:00.000' AS DateTime), 15000.0000)
GO

INSERT [dbo].[Product] ([Id], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (1, 1, N'Candy', N'500g', 20000.0000, 19)
GO
INSERT [dbo].[Product] ([Id], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (2, 1, N'Mixed Candy', N'300g', 300000.0000, 18)
GO
INSERT [dbo].[Product] ([Id], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (3, 1, N'Cake', N'200g', 15000.0000, 40)
GO
INSERT [dbo].[Product] ([Id], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (4, 2, N'Pepsi', N'250ml', 10000.0000, 45)
GO
INSERT [dbo].[Product] ([Id], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (5, 1, N'Snack Oshi''s', N'100g', 15000.0000, 31)
GO

INSERT [dbo].[OrderDetail] ([Id], [OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (1, 4665, 1, 20000.0000, 1, 5)
GO
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (2, 6113, 4, 10000.0000, 3, 10)
GO
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (3, 6113, 5, 15000.0000, 4, 15)
GO
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (4, 6259, 2, 300000.0000, 2, 5)
GO
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (5, 6259, 4, 10000.0000, 2, 5)
GO
