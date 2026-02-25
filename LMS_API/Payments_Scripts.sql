-- Create Payments Table
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    ClaimId NVARCHAR(50),
    CountryRegion NVARCHAR(100),
    ClaimStatus NVARCHAR(50),
    Program NVARCHAR(100),
    Activity NVARCHAR(100),
    Amount DECIMAL(18, 2),
    FiscalYear NVARCHAR(20),
    Quarter NVARCHAR(10),
    ClaimIDOverride NVARCHAR(50),
    Remarks NVARCHAR(MAX),
    AmountOverride DECIMAL(18, 2),
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- Stored Procedure: Payment_GetList
CREATE OR ALTER PROCEDURE Payment_GetList
    @FiscalYear NVARCHAR(20) = NULL,
    @Quarter NVARCHAR(10) = NULL
AS
BEGIN
    SELECT 
        PaymentId,
        ClaimId,
        CountryRegion,
        ClaimStatus,
        Program,
        Activity,
        Amount,
        FiscalYear,
        Quarter,
        ClaimIDOverride,
        Remarks,
        AmountOverride,
        CreatedDate
    FROM Payments
    WHERE (@FiscalYear IS NULL OR FiscalYear = @FiscalYear)
      AND (@Quarter IS NULL OR Quarter = @Quarter)
      AND IsActive = 1
    ORDER BY CreatedDate DESC;
END;
GO

-- Stored Procedure: Payment_InsertUpdate
CREATE OR ALTER PROCEDURE Payment_InsertUpdate
    @PaymentId INT,
    @ClaimId NVARCHAR(50),
    @CountryRegion NVARCHAR(100),
    @ClaimStatus NVARCHAR(50),
    @Program NVARCHAR(100),
    @Activity NVARCHAR(100),
    @Amount DECIMAL(18, 2),
    @FiscalYear NVARCHAR(20),
    @Quarter NVARCHAR(10),
    @ClaimIDOverride NVARCHAR(50) = NULL,
    @Remarks NVARCHAR(MAX) = NULL,
    @AmountOverride DECIMAL(18, 2) = NULL,
    @Result NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Payments WHERE ClaimId = @ClaimId AND PaymentId <> @PaymentId AND IsActive = 1)
        BEGIN
            SET @Result = 'Error: ClaimId ' + @ClaimId + ' already exists.';
            RETURN;
        END

        IF (@PaymentId = 0)
        BEGIN
            INSERT INTO Payments (ClaimId, CountryRegion, ClaimStatus, Program, Activity, Amount, FiscalYear, Quarter, ClaimIDOverride, Remarks, AmountOverride)
            VALUES (@ClaimId, @CountryRegion, @ClaimStatus, @Program, @Activity, @Amount, @FiscalYear, @Quarter, @ClaimIDOverride, @Remarks, @AmountOverride);
            SET @Result = 'Payment record inserted successfully.';
        END
        ELSE
        BEGIN
            UPDATE Payments
            SET ClaimId = @ClaimId,
                CountryRegion = @CountryRegion,
                ClaimStatus = @ClaimStatus,
                Program = @Program,
                Activity = @Activity,
                Amount = @Amount,
                FiscalYear = @FiscalYear,
                Quarter = @Quarter,
                ClaimIDOverride = @ClaimIDOverride,
                Remarks = @Remarks,
                AmountOverride = @AmountOverride
            WHERE PaymentId = @PaymentId;
            SET @Result = 'Payment record updated successfully.';
        END
    END TRY
    BEGIN CATCH
        SET @Result = ERROR_MESSAGE();
    END CATCH
END;
GO

-- Create User-Defined Table Type for Bulk Insert
IF EXISTS (SELECT * FROM sys.types WHERE name = 'PaymentType')
    DROP TYPE PaymentType;
GO

CREATE TYPE PaymentType AS TABLE (
    ClaimId NVARCHAR(50),
    CountryRegion NVARCHAR(100),
    ClaimStatus NVARCHAR(50),
    Program NVARCHAR(100),
    Activity NVARCHAR(100),
    Amount DECIMAL(18, 2),
    FiscalYear NVARCHAR(20),
    Quarter NVARCHAR(10),
    ClaimIDOverride NVARCHAR(50),
    Remarks NVARCHAR(MAX),
    AmountOverride DECIMAL(18, 2)
);
GO

-- Stored Procedure: Payment_BulkInsert
CREATE OR ALTER PROCEDURE Payment_BulkInsert
    @Payments PaymentType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (
            SELECT 1 FROM @Payments p 
            INNER JOIN Payments t ON p.ClaimId = t.ClaimId 
            WHERE t.IsActive = 1
        )
        BEGIN
            SELECT 'Error: One or more ClaimIds already exist in the database.' AS Result, 0 AS Success;
            RETURN;
        END

        INSERT INTO Payments (ClaimId, CountryRegion, ClaimStatus, Program, Activity, Amount, FiscalYear, Quarter, ClaimIDOverride, Remarks, AmountOverride)
        SELECT ClaimId, CountryRegion, ClaimStatus, Program, Activity, Amount, FiscalYear, Quarter, ClaimIDOverride, Remarks, AmountOverride
        FROM @Payments;
        
        SELECT 'Bulk payment records inserted successfully.' AS Result, 1 AS Success;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS Result, 0 AS Success;
    END CATCH
END;
GO

-- Stored Procedure: Payment_BulkUpdateOverrides
CREATE OR ALTER PROCEDURE Payment_BulkUpdateOverrides
    @Payments PaymentType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE t
        SET t.ClaimIDOverride = p.ClaimIDOverride,
            t.Remarks = p.Remarks,
            t.AmountOverride = p.AmountOverride
        FROM Payments t
        INNER JOIN @Payments p ON t.ClaimId = p.ClaimId 
                               AND t.FiscalYear = p.FiscalYear 
                               AND t.Quarter = p.Quarter
        WHERE t.IsActive = 1;

        SELECT 'Override records saved successfully.' AS Result, 1 AS Success;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS Result, 0 AS Success;
    END CATCH
END;
GO

-- Insert Sample Data
INSERT INTO Payments (ClaimId, CountryRegion, ClaimStatus, Program, Activity, Amount, FiscalYear, Quarter)
VALUES 
('C1001', 'India', 'Pending', 'Event', 'New brand launch', 1500.00, 'FY26', 'Q1'),
('C1002', 'Japan', 'Pending', 'Event', 'Collaboration', 2500.00, 'FY26', 'Q1'),
('C1003', 'China', 'Done', 'Event', 'Appraisal fest', 3000.00, 'FY25', 'Q4'),
('C1004', 'USA', 'Processing', 'Marketing', 'Global Campaign', 5000.00, 'FY26', 'Q2');
GO
