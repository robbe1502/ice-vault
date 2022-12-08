CREATE VIEW SystemErrors 
AS SELECT
    o.Id as 'Id',
    o.CreatedAt as 'OccuredAt',
    o.CorrelationId as 'CorrelationId',
    o.Payload as 'Payload',
    o.Type as 'EventType',
    o.UserName as 'User'
FROM dbo.OutboxMessages o