CREATE SCHEMA auth;
GO

CREATE TABLE [auth].[Users]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Gender] NVARCHAR(10) NOT NULL,
    [DateOfBirth] DATETIME NOT NULL,
    [Username] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_User_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE UNIQUE INDEX [IDX_Users_Email] ON [auth].[Users] ([Email] ASC);
CREATE UNIQUE INDEX [IDX_Users_Username] ON [auth].[Users] ([Username] ASC);

CREATE TABLE [auth].[RefreshTokens]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_RefreshTokens_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC),
	CONSTRAINT [FK_RefreshTokens_UserId] FOREIGN KEY (UserId) REFERENCES [auth].[Users] (id),
);

CREATE UNIQUE INDEX [IDX_RefreshToken_UserId] ON [auth].[RefreshTokens] ([UserId] ASC);

CREATE TABLE [auth].[RevokedRefreshTokens]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_RevokedRefreshTokens_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC),
	CONSTRAINT [FK_RevokedRefreshedTokens_UserId] FOREIGN KEY (UserId) REFERENCES [auth].[Users] (id),
);
