IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Pessoas] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [Idade] int NOT NULL,
    [Sexo] varchar(1) NULL,
    [DataCadastro] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NULL,
    [Codigo] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Pessoas] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240805013549_InitialCreate', N'8.0.7');
GO

COMMIT;
GO

