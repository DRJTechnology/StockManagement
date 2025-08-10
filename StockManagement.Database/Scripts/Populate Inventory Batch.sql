
select act.Id, act.ActivityDate, atn.ActionName, l.[Name] AS 'Location', p.ProductName, pt.ProductTypeName, act.Quantity, act.AmendDate
from Activity act
INNER JOIN [Action] atn ON act.ActionId = atn.Id
INNER JOIN [Location] l ON act.LocationId = l.Id
INNER JOIN [Product] p ON act.ProductId = p.Id
INNER JOIN [ProductType] pt ON act.ProductTypeId = pt.Id
Where act.Deleted = 0
ORDER BY act.ActivityDate, act.ActionId

Select * from finance.InventoryBatch

