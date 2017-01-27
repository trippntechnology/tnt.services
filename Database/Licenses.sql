CREATE TABLE [Licenses] (
  [LicenseKey] VARCHAR NOT NULL, 
  [ApplicationID] GUID NOT NULL, 
  [IssuedTo] VARCHAR NOT NULL, 
  [EmailAddress] VARCHAR, 
  [Address] VARCHAR, 
  [City] VARCHAR, 
  [State] VARCHAR, 
  [Zip] VARCHAR, 
  [PhoneNumber] VARCHAR, 
  [IssuedOn] DATETIME NOT NULL, 
  CONSTRAINT [sqlite_autoindex_Licenses_1] PRIMARY KEY ([LicenseKey]));

