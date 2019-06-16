SET QUOTED_IDENTIFIER OFF;
GO
USE [EPE_Validation_Test];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Boletins_Alunos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Boletins] DROP CONSTRAINT [FK_Boletins_Alunos];
GO
IF OBJECT_ID(N'[dbo].[FK_Validados_Alunos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Validados] DROP CONSTRAINT [FK_Validados_Alunos];
GO
IF OBJECT_ID(N'[dbo].[FK_Validados_Movimentos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Validados] DROP CONSTRAINT [FK_Validados_Movimentos];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Alunos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Alunos];
GO
IF OBJECT_ID(N'[dbo].[Boletins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Boletins];
GO
IF OBJECT_ID(N'[dbo].[Movimentos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Movimentos];
GO
IF OBJECT_ID(N'[dbo].[Validados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Validados];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Alunos'
CREATE TABLE [dbo].[Alunos] (
    [IdAluno] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(255)  NULL,
    [Nome] nvarchar(255)  NULL,
    [DtNasc] datetime  NULL,
    [Escola] nvarchar(255)  NULL,
    [Professor] nvarchar(255)  NULL,
    [EncEduc] nvarchar(255)  NULL,
    [Morada] nvarchar(255)  NULL,
    [CPostal] float  NULL,
    [Localidade] nvarchar(255)  NULL,
    [Cantao] nvarchar(255)  NULL,
    [Telefone] nvarchar(255)  NULL,
    [Telemovel] nvarchar(255)  NULL,
    [Email] nvarchar(255)  NULL
);
GO

-- Creating table 'Boletins'
CREATE TABLE [dbo].[Boletins] (
    [IdAluno] int  NOT NULL,
    [NumBoletim] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Movimentos'
CREATE TABLE [dbo].[Movimentos] (
    [IdMov] int IDENTITY(1,1) NOT NULL,
    [DtEval] datetime  NULL,
    [RelBancaria] float  NULL,
    [Portofolio] nvarchar(max)  NULL,
    [Produto] nvarchar(max)  NULL,
    [IBAN] nvarchar(max)  NULL,
    [Moeda] nvarchar(255)  NULL,
    [DtInicio] datetime  NULL,
    [DtFim] datetime  NULL,
    [Descricao] nvarchar(255)  NULL,
    [DtTransac] datetime  NULL,
    [DtContab] datetime  NULL,
    [DtValor] datetime  NULL,
    [Descricao1] nvarchar(max)  NULL,
    [Descricao2] nvarchar(max)  NULL,
    [Descricao3] nvarchar(max)  NULL,
    [NumTrans] nvarchar(255)  NULL,
    [CursDevis] nvarchar(255)  NULL,
    [SubTotal] float  NULL,
    [Debito] float  NULL,
    [Credito] float  NULL,
    [Saldo] float  NULL
);
GO

-- Creating table 'Validados'
CREATE TABLE [dbo].[Validados] (
    [IdMov] int  NOT NULL,
    [IdAluno] int  NOT NULL,
    [DtValid] datetime  NOT NULL,
    [Valor] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdAluno] in table 'Alunos'
ALTER TABLE [dbo].[Alunos]
ADD CONSTRAINT [PK_Alunos]
    PRIMARY KEY CLUSTERED ([IdAluno] ASC);
GO

-- Creating primary key on [IdAluno], [NumBoletim] in table 'Boletins'
ALTER TABLE [dbo].[Boletins]
ADD CONSTRAINT [PK_Boletins]
    PRIMARY KEY CLUSTERED ([IdAluno], [NumBoletim] ASC);
GO

-- Creating primary key on [IdMov] in table 'Movimentos'
ALTER TABLE [dbo].[Movimentos]
ADD CONSTRAINT [PK_Movimentos]
    PRIMARY KEY CLUSTERED ([IdMov] ASC);
GO

-- Creating primary key on [IdMov], [IdAluno], [DtValid] in table 'Validados'
ALTER TABLE [dbo].[Validados]
ADD CONSTRAINT [PK_Validados]
    PRIMARY KEY CLUSTERED ([IdMov], [IdAluno], [DtValid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdAluno] in table 'Boletins'
ALTER TABLE [dbo].[Boletins]
ADD CONSTRAINT [FK_Boletins_Alunos]
    FOREIGN KEY ([IdAluno])
    REFERENCES [dbo].[Alunos]
        ([IdAluno])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdAluno] in table 'Validados'
ALTER TABLE [dbo].[Validados]
ADD CONSTRAINT [FK_Validados_Alunos]
    FOREIGN KEY ([IdAluno])
    REFERENCES [dbo].[Alunos]
        ([IdAluno])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Validados_Alunos'
CREATE INDEX [IX_FK_Validados_Alunos]
ON [dbo].[Validados]
    ([IdAluno]);
GO

-- Creating foreign key on [IdMov] in table 'Validados'
ALTER TABLE [dbo].[Validados]
ADD CONSTRAINT [FK_Validados_Movimentos]
    FOREIGN KEY ([IdMov])
    REFERENCES [dbo].[Movimentos]
        ([IdMov])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Creating all STORED PROCEDURES
-- --------------------------------------------------
if exists (select * from sysobjects where id = object_id('dbo.USP_STORE_ALUNO') and sysstat & 0xf = 4)
  drop procedure dbo.USP_STORE_ALUNO
GO

CREATE PROCEDURE dbo.USP_STORE_ALUNO
	@IdAluno int,
	@Username nvarchar(255),
	@Nome nvarchar(255),
	@DtNasc datetime,
	@Escola nvarchar(255),
	@Professor nvarchar(255),
	@EncEduc nvarchar(255),
	@Morada nvarchar(255),
	@CPostal float,
	@Localidade nvarchar(255),
	@Cantao nvarchar(255),
	@Telefone nvarchar(255),
	@Telemovel nvarchar(255),
	@Email nvarchar(255)
AS
BEGIN
	IF @IdAluno > 0
	BEGIN
		UPDATE Alunos SET
			Nome = @Nome,
			DtNasc = @DtNasc,
			Escola = @Escola,
			Professor = @Professor,
			EncEduc = @EncEduc,
			Morada = @Morada,
			CPostal = @CPostal,
			Cantao = @Cantao,
			Localidade = @Localidade,
			Telefone = @Telefone,
			Telemovel = @Telemovel,
			Email = @Email
		WHERE IdAluno = @IdAluno
	END
	ELSE
	BEGIN
		INSERT Alunos(Username, Nome, DtNasc, Escola, Professor, EncEduc, Morada, CPostal, Localidade, Cantao, Telefone, Telemovel, Email)
		VALUES(@Username, @Nome, @DtNasc, @Escola, @Professor, @EncEduc, @Morada, @CPostal, @Localidade, @Cantao, @Telefone, @Telemovel, @Email)
		
		SET @IdAluno = SCOPE_IDENTITY();
	END
	
	SELECT IdAluno, Username, Nome, DtNasc, Escola, Professor, EncEduc, Morada, CPostal, Localidade, Cantao, Telefone, Telemovel, Email
	FROM Alunos
	WHERE IdAluno = @IdAluno
END
GO

if exists (select * from sysobjects where id = object_id('dbo.USP_STORE_BOLETIM') and sysstat & 0xf = 4)
  drop procedure dbo.USP_STORE_BOLETIM
GO

CREATE PROCEDURE dbo.USP_STORE_BOLETIM
	@IdAluno int,
	@NumBoletim nvarchar(50)
AS
BEGIN
	INSERT Boletins(IdAluno, NumBoletim)
	VALUES(@IdAluno, @NumBoletim)
END
GO

if exists (select * from sysobjects where id = object_id('dbo.USP_GET_ALUNOS') and sysstat & 0xf = 4)
  drop procedure dbo.USP_GET_ALUNOS
GO

CREATE PROCEDURE dbo.USP_GET_ALUNOS
AS
BEGIN
	SELECT IdAluno, Username, Nome, DtNasc, Escola, Professor, EncEduc, Morada, CPostal, Localidade, Cantao, Telefone, Telemovel, Email
	FROM Alunos
END
GO

if exists (select * from sysobjects where id = object_id('dbo.USP_GET_BOLETINS') and sysstat & 0xf = 4)
  drop procedure dbo.USP_GET_BOLETINS
GO

CREATE PROCEDURE dbo.USP_GET_BOLETINS
AS
BEGIN
	SELECT IdAluno, NumBoletim
	FROM Boletins
END
GO
-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------