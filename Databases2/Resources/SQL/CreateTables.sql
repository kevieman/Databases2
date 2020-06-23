CREATE TABLE user_info.Accounts (
  ID               int IDENTITY NOT NULL, 
  [E-mail]         varchar(255) NOT NULL UNIQUE, 
  Password         varchar(255) NOT NULL, 
  Activated        bit DEFAULT 0 NULL, 
  Had_Discount     bit DEFAULT 0 NULL, 
  Trials_Days_Left tinyint NULL, 
  LanguageID       int DEFAULT 1 NOT NULL, 
  SubscriptionsID  int NULL, 
  AccountsID       int NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Blocked_Accounts (
  AccountsID        int NOT NULL, 
  [Date]            datetime NULL, 
  Amount            int NOT NULL, 
  Currently_Blocked bit NOT NULL, 
  PRIMARY KEY (AccountsID));
CREATE TABLE Episodes (
  ID            int IDENTITY NOT NULL, 
  Episode_Title varchar(255) NULL, 
  Videos_ID     int NOT NULL, 
  Seasons_ID    int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Film_has_Video (
  Videos_ID         int NOT NULL, 
  [Films/Series_ID] int NOT NULL, 
  PRIMARY KEY (Videos_ID, 
  [Films/Series_ID]));
CREATE TABLE [Films/Series] (
  ID          int IDENTITY NOT NULL, 
  Title       varchar(255) NOT NULL, 
  Type        varchar(255) NOT NULL CHECK(Type = 'Film' OR Type = 'Serie'), 
  Description varchar(255) NULL, 
  PRIMARY KEY (ID));
CREATE TABLE [Films/Series_Genres] (
  [Films/Series_ID] int NOT NULL, 
  Genres_ID         int NOT NULL, 
  PRIMARY KEY ([Films/Series_ID], 
  Genres_ID));
CREATE TABLE [Films/Series_Viewer_Guides] (
  [Films/Series_ID] int NOT NULL, 
  Viewer_Guides_ID  int NOT NULL, 
  PRIMARY KEY ([Films/Series_ID], 
  Viewer_Guides_ID));
CREATE TABLE Genres (
  ID   int IDENTITY NOT NULL, 
  Name varchar(255) NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Languages (
  ID           int IDENTITY NOT NULL, 
  Name         varchar(255) NOT NULL, 
  abbreviation varchar(255) NULL, 
  PRIMARY KEY (ID));
CREATE TABLE [Profile_Films/Series_Queue] (
  ID                int IDENTITY NOT NULL, 
  [Films/Series_ID] int NOT NULL, 
  Profiles_ID       int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Profile_Preferences (
  ID                   int IDENTITY NOT NULL, 
  Interested_in_films  bit NULL, 
  Interested_in_series bit NULL, 
  LanguageID           int NOT NULL, 
  Profiles_ID          int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Profile_Preferences_Genres (
  Profile_Preferences_ID int NOT NULL, 
  Genres_ID              int NOT NULL, 
  PRIMARY KEY (Profile_Preferences_ID, 
  Genres_ID));
CREATE TABLE Profile_Preferences_Viewer_Guides (
  Profile_Preferences_ID int NOT NULL, 
  Viewer_Guides_ID       int NOT NULL, 
  PRIMARY KEY (Profile_Preferences_ID, 
  Viewer_Guides_ID));
CREATE TABLE Profile_SeenVideos (
  ID                 int IDENTITY NOT NULL, 
  Currently_Watching bit NOT NULL, 
  [Date]             datetime NULL, 
  Paused_Timestamp   int NULL, 
  Profiles_ID        int NOT NULL, 
  Videos_ID          int NOT NULL, 
  Subtitles_ID       int NULL, 
  PRIMARY KEY (ID));
CREATE TABLE user_info.Profiles (
  ID              int IDENTITY NOT NULL, 
  Name            varchar(255) NOT NULL, 
  Age             tinyint NULL, 
  Profile_Picture varchar(255) NULL, 
  LanguageID      int NOT NULL, 
  AccountsID      int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Qualities (
  ID         int IDENTITY NOT NULL, 
  Name       varchar(255) NOT NULL, 
  Resolution varchar(255) NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Seasons (
  ID                int IDENTITY NOT NULL, 
  Season_Title      varchar(255) NULL, 
  [Films/Series_ID] int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Subscriptions (
  ID           int IDENTITY NOT NULL, 
  Name         varchar(255) NOT NULL, 
  Price        decimal(10, 2) NOT NULL, 
  Qualities_ID int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Subtitles (
  ID            int IDENTITY NOT NULL, 
  File_Location varchar(255) NOT NULL, 
  Languages_ID  int NOT NULL, 
  VideosID      int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Videos (
  ID            int IDENTITY NOT NULL, 
  [File]        varchar(255) NOT NULL, 
  Length        int NOT NULL, 
  Credits_Start int NULL, 
  Qualities_ID  int NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE Viewer_Guides (
  ID   int IDENTITY NOT NULL, 
  Name varchar(255) NOT NULL, 
  PRIMARY KEY (ID));

ALTER TABLE user_info.Accounts ADD CONSTRAINT FK_AccountsLanguage FOREIGN KEY (LanguageID) REFERENCES Languages (ID) ON UPDATE Cascade ON DELETE Set default;
ALTER TABLE user_info.Accounts ADD CONSTRAINT FK_AccountsSubscriptions FOREIGN KEY (SubscriptionsID) REFERENCES Subscriptions (ID) ON UPDATE Cascade ON DELETE Set null;
ALTER TABLE Blocked_Accounts ADD CONSTRAINT FK_BlockedAccounts FOREIGN KEY (AccountsID) REFERENCES user_info.Accounts (ID);
ALTER TABLE Episodes ADD CONSTRAINT FK_EpisodesSeasons FOREIGN KEY (Seasons_ID) REFERENCES Seasons (ID);
ALTER TABLE Episodes ADD CONSTRAINT FK_EpisodesVideos FOREIGN KEY (Videos_ID) REFERENCES Videos (ID);
ALTER TABLE [Films/Series_Genres] ADD CONSTRAINT [FK_Films/SeriesGenres] FOREIGN KEY ([Films/Series_ID]) REFERENCES [Films/Series] (ID);
ALTER TABLE [Films/Series_Viewer_Guides] ADD CONSTRAINT [FK_Films/SeriesGuides] FOREIGN KEY ([Films/Series_ID]) REFERENCES [Films/Series] (ID);
ALTER TABLE Film_has_Video ADD CONSTRAINT FK_FilmsVideos FOREIGN KEY ([Films/Series_ID]) REFERENCES [Films/Series] (ID);
ALTER TABLE [Films/Series_Genres] ADD CONSTRAINT [FK_GenresFilms/Series] FOREIGN KEY (Genres_ID) REFERENCES Genres (ID);
ALTER TABLE Profile_Preferences_Genres ADD CONSTRAINT FK_GenresPREFERENCES FOREIGN KEY (Genres_ID) REFERENCES Genres (ID);
ALTER TABLE [Films/Series_Viewer_Guides] ADD CONSTRAINT [FK_GuidesFilms/Series] FOREIGN KEY (Viewer_Guides_ID) REFERENCES Viewer_Guides (ID);
ALTER TABLE Profile_Preferences_Viewer_Guides ADD CONSTRAINT FK_GuidesPREFERENCES FOREIGN KEY (Viewer_Guides_ID) REFERENCES Viewer_Guides (ID);
ALTER TABLE Profile_Preferences ADD CONSTRAINT FK_PreferencedLanguages FOREIGN KEY (LanguageID) REFERENCES Languages (ID);
ALTER TABLE Profile_Preferences_Genres ADD CONSTRAINT FK_PreferencesGenres FOREIGN KEY (Profile_Preferences_ID) REFERENCES Profile_PREFERENCES (ID);
ALTER TABLE Profile_Preferences_Viewer_Guides ADD CONSTRAINT FK_PreferencesGuides FOREIGN KEY (Profile_Preferences_ID) REFERENCES Profile_PREFERENCES (ID);
ALTER TABLE user_info.Profiles ADD CONSTRAINT FK_ProfileLanguage FOREIGN KEY (LanguageID) REFERENCES Languages (ID);
ALTER TABLE user_info.Profiles ADD CONSTRAINT FK_ProfilesAccounts FOREIGN KEY (AccountsID) REFERENCES user_info.Accounts (ID) ON UPDATE Cascade ON DELETE No action;
ALTER TABLE Profile_SeenVideos ADD CONSTRAINT FK_ProfileSeenVideos FOREIGN KEY (Profiles_ID) REFERENCES user_info.Profiles (ID);
ALTER TABLE Profile_Preferences ADD CONSTRAINT FK_ProfilesPREFERENCES FOREIGN KEY (Profiles_ID) REFERENCES user_info.Profiles (ID);
ALTER TABLE [Profile_Films/Series_Queue] ADD CONSTRAINT FK_ProfilesQueue FOREIGN KEY (Profiles_ID) REFERENCES user_info.Profiles (ID);
ALTER TABLE [Profile_Films/Series_Queue] ADD CONSTRAINT [FK_QueueFilms/Series] FOREIGN KEY ([Films/Series_ID]) REFERENCES [Films/Series] (ID);
ALTER TABLE user_info.Accounts ADD CONSTRAINT FK_RelatedAccounts FOREIGN KEY (AccountsID) REFERENCES user_info.Accounts (ID) ON UPDATE NO ACTION ON DELETE NO ACTION;
ALTER TABLE Seasons ADD CONSTRAINT FK_SeasonsSeries FOREIGN KEY ([Films/Series_ID]) REFERENCES [Films/Series] (ID);
ALTER TABLE Profile_SeenVideos ADD CONSTRAINT FK_SeenVideosSubtitles FOREIGN KEY (Subtitles_ID) REFERENCES Subtitles (ID);
ALTER TABLE Profile_SeenVideos ADD CONSTRAINT FK_SeenVideosVid FOREIGN KEY (Videos_ID) REFERENCES Videos (ID);
ALTER TABLE Subscriptions ADD CONSTRAINT FK_SubscriptionQuality FOREIGN KEY (Qualities_ID) REFERENCES Qualities (ID);
ALTER TABLE Subtitles ADD CONSTRAINT FK_SubtitleLanguage FOREIGN KEY (Languages_ID) REFERENCES Languages (ID);
ALTER TABLE Subtitles ADD CONSTRAINT FK_SubtitlesVideos FOREIGN KEY (VideosID) REFERENCES Videos (ID);
ALTER TABLE Film_has_Video ADD CONSTRAINT FK_VideosFilms FOREIGN KEY (Videos_ID) REFERENCES Videos (ID);
ALTER TABLE Videos ADD CONSTRAINT FK_VideosQualities FOREIGN KEY (Qualities_ID) REFERENCES Qualities (ID);