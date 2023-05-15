CREATE TABLE [dbo].[Location]
(
	[LID] INT NOT NULL IDENTITY (1, 1),
	[locationName] Nvarchar(100) NOT NULL,
	[weatherElement] Nvarchar(max) NOT NULL,
	CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([LID] ASC)
)
