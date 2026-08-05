-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Signed inventory batch movements, dated by Activity.ActivityDate.
--
--              This is the single definition of "what happened to a batch and
--              when", used by every point-in-time stock report. Summing
--              SignedQty for a batch over all dates reproduces
--              InventoryBatch.QuantityRemaining exactly (validated against all
--              835 live batches).
--
--              Sign rules:
--                Action 1 (Add new stock)          -> always an addition
--                Action 2 (Move stockroom -> loc)  -> the row on the stockroom
--                                                     batch is the deduction,
--                                                     the row on the destination
--                                                     batch is the addition
--                Action 3 (Return loc -> stockroom)-> mirror of action 2
--                Actions 4,5,6,7,8                 -> always a deduction
--                (delete / sale / promo / damaged / personal use)
--
--              NOTE: deliberately does NOT filter on Activity.Deleted.
--              Activity_Delete soft-deletes the Activity row but does not
--              reverse the batch movement, so the InventoryBatchActivity rows
--              remain the authoritative record of stock. Filtering on
--              Activity.Deleted here breaks the reconciliation on 16 batches.
-- =============================================================================
CREATE VIEW [finance].[vw_InventoryBatchMovement]
AS
SELECT
    iba.InventoryBatchId,
    ib.ProductId,
    ib.ProductTypeId,
    ib.LocationId,
    ib.UnitCost,
    ib.InventoryBatchStatusId,
    a.ActivityDate,
    ac.Id AS ActionId,
    ac.ActionName,
    CASE
        WHEN ac.Id = 1 THEN  1
        WHEN ac.Id = 2 THEN CASE WHEN ib.LocationId = 1 THEN -1 ELSE 1 END
        WHEN ac.Id = 3 THEN CASE WHEN ib.LocationId = 1 THEN  1 ELSE -1 END
        ELSE -1
    END * iba.Quantity AS SignedQty
FROM finance.InventoryBatchActivity iba
INNER JOIN finance.InventoryBatch ib ON iba.InventoryBatchId = ib.Id
INNER JOIN dbo.Activity a            ON iba.ActivityId = a.Id
INNER JOIN dbo.[Action] ac           ON a.ActionId = ac.Id
WHERE iba.Deleted = 0
  AND ib.Deleted = 0;
