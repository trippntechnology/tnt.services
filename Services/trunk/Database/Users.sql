CREATE TABLE [Users] (
  [LicenseKey] VARCHAR NOT NULL, 
  [ApplicationID] GUID NOT NULL, 
  [Name] VARCHAR NOT NULL, 
  [EmailAddress] VARCHAR, 
  [Address] VARCHAR, 
  [City] VARCHAR, 
  [State] VARCHAR, 
  [Zip] VARCHAR, 
  [PhoneNumber] VARCHAR, 
  [CreatedOn] DATETIME NOT NULL, 
  CONSTRAINT [sqlite_autoindex_Users_1] PRIMARY KEY ([LicenseKey]));

