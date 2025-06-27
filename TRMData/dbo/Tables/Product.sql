CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ProductName] NVARCHAR(1000) NOT NULL, 
	[Description] NVARCHAR(MAX) NOT NULL, 
	[RetailPrice] money not null,
	[QuantityInStock] INT NOT NULL DEFAULT 1,
	[CreateDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[LastModified] DATETIME2 NOT NULL DEFAULT getutcdate()
)
