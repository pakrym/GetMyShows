
CREATE TABLE [dbo].[Episodes] (
    [EpisodeId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [Number] int  NOT NULL,
    [Season] int  NOT NULL,
    [SeriesId] int  NOT NULL,
    [Watched] bit  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [AirDate] datetime  NOT NULL
);
GO

-- Creating table 'Profiles'
CREATE TABLE [dbo].[Profiles] (
    [ProfileId] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(50)  NOT NULL,
    [UserId] int  NOT NULL,
    [Password] nchar(32)  NOT NULL
);
GO

-- Creating table 'Series'
CREATE TABLE [dbo].[Series] (
    [SeriesId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [ProfileId] int  NOT NULL,
    [Image] blob  NULL
);
GO

-- Creating table 'Subtitles'
CREATE TABLE [dbo].[Subtitles] (
    [SubtitlesId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [EpisodeId] int  NOT NULL,
    [File] blob  NOT NULL
);
GO

-- Creating table 'Torrents'
CREATE TABLE [dbo].[Torrents] (
    [TorrentId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [File] blob  NULL,
    [Magnet] nvarchar(1024)  NULL,
    [EpisodeId] int  NOT NULL,
    [Seed] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [EpisodeId] in table 'Episodes'
ALTER TABLE [dbo].[Episodes]
ADD CONSTRAINT [PK_Episodes]
    PRIMARY KEY CLUSTERED ([EpisodeId] ASC);
GO

-- Creating primary key on [ProfileId] in table 'Profiles'
ALTER TABLE [dbo].[Profiles]
ADD CONSTRAINT [PK_Profiles]
    PRIMARY KEY CLUSTERED ([ProfileId] ASC);
GO

-- Creating primary key on [SeriesId] in table 'Series'
ALTER TABLE [dbo].[Series]
ADD CONSTRAINT [PK_Series]
    PRIMARY KEY CLUSTERED ([SeriesId] ASC);
GO

-- Creating primary key on [SubtitlesId] in table 'Subtitles'
ALTER TABLE [dbo].[Subtitles]
ADD CONSTRAINT [PK_Subtitles]
    PRIMARY KEY CLUSTERED ([SubtitlesId] ASC);
GO

-- Creating primary key on [TorrentId] in table 'Torrents'
ALTER TABLE [dbo].[Torrents]
ADD CONSTRAINT [PK_Torrents]
    PRIMARY KEY CLUSTERED ([TorrentId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SeriesId] in table 'Episodes'
ALTER TABLE [dbo].[Episodes]
ADD CONSTRAINT [FK_Episode_Series]
    FOREIGN KEY ([SeriesId])
    REFERENCES [dbo].[Series]
        ([SeriesId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Episode_Series'
CREATE INDEX [IX_FK_Episode_Series]
ON [dbo].[Episodes]
    ([SeriesId]);
GO

-- Creating foreign key on [EpisodeId] in table 'Subtitles'
ALTER TABLE [dbo].[Subtitles]
ADD CONSTRAINT [FK_Subtitles_Episode]
    FOREIGN KEY ([EpisodeId])
    REFERENCES [dbo].[Episodes]
        ([EpisodeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subtitles_Episode'
CREATE INDEX [IX_FK_Subtitles_Episode]
ON [dbo].[Subtitles]
    ([EpisodeId]);
GO

-- Creating foreign key on [EpisodeId] in table 'Torrents'
ALTER TABLE [dbo].[Torrents]
ADD CONSTRAINT [FK_Torrent_Episode]
    FOREIGN KEY ([EpisodeId])
    REFERENCES [dbo].[Episodes]
        ([EpisodeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Torrent_Episode'
CREATE INDEX [IX_FK_Torrent_Episode]
ON [dbo].[Torrents]
    ([EpisodeId]);
GO

-- Creating foreign key on [ProfileId] in table 'Series'
ALTER TABLE [dbo].[Series]
ADD CONSTRAINT [FK_Series_Profile]
    FOREIGN KEY ([ProfileId])
    REFERENCES [dbo].[Profiles]
        ([ProfileId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Series_Profile'
CREATE INDEX [IX_FK_Series_Profile]
ON [dbo].[Series]
    ([ProfileId]);
GO