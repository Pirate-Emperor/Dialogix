USE [master]
GO
/****** Object:  Database [Dialogix]    Script Date: 27-10-2024 10:01:35 ******/
CREATE DATABASE [Dialogix]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Dialogix', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.PIRATE-EMPERORSQL\MSSQL\DATA\Dialogix.mdf' , SIZE = 51200KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Dialogix_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.PIRATE-EMPERORSQL\MSSQL\DATA\Dialogix_log.ldf' , SIZE = 51200KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Dialogix] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Dialogix].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Dialogix] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Dialogix] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Dialogix] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Dialogix] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Dialogix] SET ARITHABORT OFF 
GO
ALTER DATABASE [Dialogix] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Dialogix] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Dialogix] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Dialogix] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Dialogix] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Dialogix] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Dialogix] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Dialogix] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Dialogix] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Dialogix] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Dialogix] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Dialogix] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Dialogix] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Dialogix] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Dialogix] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Dialogix] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Dialogix] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Dialogix] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Dialogix] SET  MULTI_USER 
GO
ALTER DATABASE [Dialogix] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Dialogix] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Dialogix] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Dialogix] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Dialogix] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Dialogix] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Dialogix] SET QUERY_STORE = OFF
GO
USE [Dialogix]
GO
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 27-10-2024 10:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessage](
	[ChatMessageID] [int] IDENTITY(1,1) NOT NULL,
	[SenderID] [int] NOT NULL,
	[ReceiverID] [int] NOT NULL,
	[ChatMessage] [varchar](200) NOT NULL,
	[TimeStamp] [smalldatetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatUser]    Script Date: 27-10-2024 10:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatUser](
	[ChatUserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[ProfilePictureURL] [varchar](100) NULL,
	[Status] [bit] NULL,
	[AuthenticationTime] [smalldatetime] NULL,
	[Gender] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserConnectionList]    Script Date: 27-10-2024 10:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserConnectionList](
	[ConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[RequestID] [int] NOT NULL,
	[AcceptID] [int] NOT NULL,
	[TIMESTAMP] [datetime] NOT NULL,
	[IsRequestAccepted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatMessage]    Script Date: 27-10-2024 10:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_ChatMessage]    
( @Mode               INT,    
  @UserName           VARCHAR,  
  @ChatUserID    INT  
)    
 AS    
 BEGIN    
 SET NOCOUNT ON;    
  IF(@Mode=1)    
  BEGIN    
         Select * from ChatUser where UserName LIKE '%' + @UserName + '%'    
  END   
  IF(@Mode=2)  
  BEGIN  
         Select UserName,ProfilePictureURL,Email,ChatUserID from ChatUser Where ChatUserID = @ChatUserID  
  END  
END 
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatUser]    Script Date: 27-10-2024 10:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ChatUser]          
(          
    @Mode                INT,          
    @UserName            VARCHAR(30) ,          
    @Password            VARCHAR(20) ,          
    @Email               VARCHAR(50) ,        
    @Gender              VARCHAR(10) ,        
    @ProfilePictureURL   VARCHAR(100) NULL = '',   
	@FullName			 VARCHAR(50) ,
    @ChatUser            INT OUTPUT  -- Changed to OUTPUT parameter    
)          
AS           
BEGIN           
    SET NOCOUNT ON;          
    
    DECLARE @EmailMessage VARCHAR(150);      
    DECLARE @UserMessage VARCHAR(150);      
    
    -- Insert          
    IF (@Mode = 1)          
    BEGIN        
        -- Check the Email availability      
        IF EXISTS (SELECT 1 FROM ChatUser WHERE Email = @Email)      
        BEGIN      
            SET @EmailMessage = 'Email ID already exists. Please use a new email to register or login with the same email.';      
            RAISERROR(@EmailMessage, 16, 1);      
            RETURN;      
        END      
      
        -- Check the UserName availability      
        IF EXISTS (SELECT 1 FROM ChatUser WHERE UserName = @UserName)      
        BEGIN      
            SET @UserMessage = 'Username already exists. Please use a new username to register or login with the same username.';      
            RAISERROR(@UserMessage, 16, 1);      
            RETURN;      
        END      
      
        -- Insert the Data for Creating New User      
        INSERT INTO ChatUser (UserName, Email, Password, AuthenticationTime, Gender, ProfilePictureURL, FullName)          
        VALUES (@UserName, @Email, @Password, GETDATE(), @Gender, @ProfilePictureURL, @FullName);         
      
        -- Return the Last Id Inserted Here      
        SET @ChatUser = SCOPE_IDENTITY();      
    END  
 --For Login  
 IF (@Mode = 2)   
 BEGIN  
      SELECT * FROM ChatUser WHERE UserName=@UserName AND Password = @Password  
 END  
END 
GO
/****** Object:  StoredProcedure [dbo].[usp_MessageRecord]    Script Date: 27-10-2024 10:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_MessageRecord]        
(        
    @Mode             INT = 0,        
    @SenderID         INT = 0,        
    @ReceiverID       INT = 0,        
    @Message          VARCHAR(100) = '',        
    @ChatMessageID    INT = 0    
)        
AS        
BEGIN        
    SET NOCOUNT ON;        
    DECLARE @LoginID INT;    
    
    -- Insert a new chat message        
    IF (@Mode = 1)        
    BEGIN        
        INSERT INTO ChatMessage (SenderID, ReceiverID, ChatMessage, TimeStamp)        
        VALUES (@SenderID, @ReceiverID, @Message, GETDATE());        
    END        
        
    -- Chat History List        
    IF (@Mode = 2)        
    BEGIN    
           
           SELECT DISTINCT ChatUserID
           INTO #TempChatUsers
           FROM ChatUser
           WHERE ChatUserId IN (
               -- Get distinct ReceiverIDs where the logged-in user is the Sender
               SELECT DISTINCT ReceiverID FROM ChatMessage WHERE SenderID = @ChatMessageID
               
               UNION
               
               -- Get distinct SenderIDs where the logged-in user is the Receiver
               SELECT DISTINCT SenderID FROM ChatMessage WHERE ReceiverID = @ChatMessageID
                );
           
             -- Now select user details for those distinct user IDs
                SELECT UserName, FullName, ProfilePictureURL, ChatUserID FROM ChatUser
                WHERE ChatUserID IN (SELECT ChatUserID FROM #TempChatUsers);
           
              -- Clean up the temporary table
                 DROP TABLE #TempChatUsers;    
    END        
        
    -- Chat Record between the Users        
    IF (@Mode = 3)        
    BEGIN        
        IF (@SenderID = @ReceiverID)        
        BEGIN        
            SELECT *         
            FROM ChatMessage         
            WHERE         
                (SenderID = @SenderID AND ReceiverID = @ReceiverID);        
        END        
        
        IF (@SenderID <> @ReceiverID)        
        BEGIN        
            SELECT *         
            FROM ChatMessage         
            WHERE         
                (SenderID = @SenderID OR ReceiverID = @SenderID) AND         
                (ReceiverID = @ReceiverID OR SenderID = @ReceiverID);        
        END        
    END        
END 
GO
USE [master]
GO
ALTER DATABASE [Dialogix] SET  READ_WRITE 
GO
