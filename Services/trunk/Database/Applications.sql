CREATE TABLE [Applications] (
  [ApplicationID] GUID NOT NULL, 
  [Name] VARCHAR NOT NULL, 
  [Version] VARCHAR NOT NULL DEFAULT ('0.0.0.0'), 
  [URL] VARCHAR, 
  CONSTRAINT [] PRIMARY KEY ([ApplicationID]));

