CREATE VIEW SystemErrors 
AS SELECT
    o.Id as 'Id',
    o.CreatedAt as 'OccuredAt',
    o.CorrelationId as 'CorrelationId',
    o.Payload as 'Payload',
    o.Type as 'EventType',
    CONCAT(u.FirstName, ' ', u.LastName) as 'User'
FROM dbo.OutboxMessages o
INNER JOIN dbo.AspNetUsers u on u.Id = o.UserId