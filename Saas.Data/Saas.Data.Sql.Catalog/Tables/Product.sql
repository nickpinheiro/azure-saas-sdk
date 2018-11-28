CREATE TABLE [dbo].[Product]
(
    [Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(500) NOT NULL,  
	[Description] NTEXT NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id])  
)
