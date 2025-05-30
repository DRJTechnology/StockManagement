CREATE PROCEDURE [dbo].[Activity_LoadFiltered]
    @ActivityDate datetime = NULL,
    @ActionId int = NULL,
    @ProductId int = NULL,
    @ProductTypeId int = NULL,
    @VenueId int = NULL,
    @Quantity int = NULL,
    @CurrentPage int = 1,
    @PageSize int = 20,
    @TotalPages int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Err int;

    -- Use a table variable to store filtered results
    DECLARE @FilteredActivity TABLE (
        [Id] int,
        [ActivityDate] datetime,
        [ActionId] int,
        [ActionName] nvarchar(100),
        [ProductId] int,
        [ProductName] nvarchar(100),
        [ProductTypeId] int,
        [ProductTypeName] nvarchar(100),
        [VenueId] int,
        [VenueName] nvarchar(100),
        [Quantity] int,
        [Deleted] bit,
        [AmendUserID] int,
        [AmendDate] datetime
    );

    INSERT INTO @FilteredActivity
    SELECT
        a.[Id],
        a.[ActivityDate],
        a.[ActionId],
        act.[ActionName],
        a.[ProductId],
        p.[ProductName],
        a.[ProductTypeId],
        pt.[ProductTypeName],
        a.[VenueId],
        v.[VenueName],
        a.[Quantity],
        a.[Deleted],
        a.[AmendUserID],
        a.[AmendDate]
    FROM [Activity] a
    INNER JOIN [Product] p ON a.[ProductId] = p.Id
    INNER JOIN [ProductType] pt ON a.[ProductTypeId] = pt.Id
    INNER JOIN [Venue] v ON a.[VenueId] = v.Id
    INNER JOIN [Action] act ON a.[ActionId] = act.Id
    WHERE
        a.[Deleted] <> 1
        AND (@ActivityDate IS NULL OR CAST(a.[ActivityDate] AS DATE) = CAST(@ActivityDate AS DATE))
        AND (@ActionId IS NULL OR a.[ActionId] = @ActionId)
        AND (@ProductId IS NULL OR a.[ProductId] = @ProductId)
        AND (@ProductTypeId IS NULL OR a.[ProductTypeId] = @ProductTypeId)
        AND (@VenueId IS NULL OR a.[VenueId] = @VenueId)
        AND (@Quantity IS NULL OR a.[Quantity] = @Quantity);

    -- Get total count and calculate total pages
    DECLARE @TotalCount int;
    SELECT @TotalCount = COUNT(*) FROM @FilteredActivity;

    SELECT @TotalPages = 
        CASE 
            WHEN @TotalCount = 0 THEN 1
            ELSE CEILING(CAST(@TotalCount AS float) / @PageSize)
        END;

    -- Return paged result set
    SELECT *
    FROM @FilteredActivity
    ORDER BY
        [ActivityDate] DESC,
        [ProductName] ASC,
        [ProductTypeName] ASC,
        [VenueName] ASC
    OFFSET (@CurrentPage - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SET @Err = @@Error;
    RETURN @Err;
END