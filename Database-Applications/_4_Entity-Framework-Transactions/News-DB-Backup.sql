USE [master]
GO
/****** Object:  Database [News]    Script Date: 23-Jul-15 14:57:07 ******/
CREATE DATABASE [News]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'News', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\News.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'News_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\News_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [News] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [News].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [News] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [News] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [News] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [News] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [News] SET ARITHABORT OFF 
GO
ALTER DATABASE [News] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [News] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [News] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [News] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [News] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [News] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [News] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [News] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [News] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [News] SET  ENABLE_BROKER 
GO
ALTER DATABASE [News] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [News] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [News] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [News] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [News] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [News] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [News] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [News] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [News] SET  MULTI_USER 
GO
ALTER DATABASE [News] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [News] SET DB_CHAINING OFF 
GO
ALTER DATABASE [News] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [News] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [News] SET DELAYED_DURABILITY = DISABLED 
GO
USE [News]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 23-Jul-15 14:57:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[News]    Script Date: 23-Jul-15 14:57:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NewsContent] [nvarchar](max) NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_dbo.News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201507231030245_InitialCreate', N'News.Data.Migrations.Configuration', 0x1F8B0800000000000400CD57C96E1B3910BD07C83F103C2580237AB9648C560247B6036322DB483BB953DD259908970EC9F648DF96433E29BF30C5DE17C94B3C1804BA749355AF5E3D56155BBF7EFC8CDEAF952477609D307A4A0F26FB94804E4C2AF46A4A73BF7CF396BE7FF7F2457496AA35F95ADB1D053BF4D46E4A6FBDCF8E1973C92D28EE264A24D638B3F493C428C653C30EF7F7FF6207070C1082221621D1E75C7BA1A078C1D799D109643EE7726E5290AE5AC79DB84025975C81CB7802537A09FFB8C929F79C921329381288412E29E15A1BCF3DD23BFEE220F6D6E8559CE10297379B0CD06EC9A5838AF6716BFED80CF60F4306AC75ACA192DC79A39E0878705449C286EEBF252C6D2443D1CE505CBF095917C2959A51320C743C933618559A96D24FC2F31EE9ACEC35C78ED5117E7B64964B9F5B986AC8BDE5728F5CE70B2992BF617363BE819EEA5CCA2E23E4847BBD055CBAB62603EB379F6159F1BC4829617D3F36746CDC3A3E651617DA1F1D527289C1F9424273E09D8C636F2C7C040D967B48AFB9F76075C08042B251F441ACA00AD6AA47EB3A28161AB60A2573BEFE047AE56FA7141F2939176B48EB958AC8172DB0B3D0C9DB1CFAB122D61EDAF828434C2E90758747B12360DBB962FD5747EBAA58FDAC4AD0187CAF3C5A02654B4DCAE56D2C1B3E6DA3B2B253EB8E663B5A3A9AF32C43C13A2D5EAD90B8ECEFD99BF8E91DA04A0C96B82D8DD0B06D226115F0150C763134323D17D6F9305E163C1CD92C5523B39EFA3B94AD4375041E967FAB776D1C9E5B8762C84DB6B9B6A29D631E0AABB148099AE8DB23168E71C225B75B7A686664AEF4AE3EBCCFBBD7155D98DEC6182F62832C860AB1914483D93094FBBE521D9A34D19B921D94665495C9C357D2A86E4A134A50A83B91869A8937CE832A8F34FE2E675214A2D40673AEC5129C2FC727C5EBE17070BDFD39570D732E958FB86FFEF7F92F82A20F4EF8D1F5F0E491AFEFB84D6EB97DA5F8FA7517F0B9637D3C7F9E31B7CB1A9CD27461907849F059137DDC0E11EB7EC745A7E0C4AA85085F751A9250672D686D73A197A6561A13EA32AA4D06073107CF53D4E5C47AB1E489C7ED049C2B2EDFAF5CE66872A616905EE8ABDC67B93F710ED442F6EEF488DD1FBFB8B6FA9CA3AB2CBCB9FF2205A4293005B8D21F7221D386F7F9B81077418412A9AA1B59E1C707C2AD360DD2A5D18F04AAE43B850C74E88D1B5099443077A5637E07BFC30D3F3B3EC18A279B7AAAED0679F820FAB247A782AF2C57AEC268FDC37F1316FE9CBCFB17F62398F1CE0C0000, N'6.1.3-40302')
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201507231129580_AutomaticMigration', N'News.Data.Migrations.Configuration', 0x1F8B0800000000000400CD57DB6EDB38107D5FA0FF40F0A905523397976E20B7489D64116C9D0451DA775A1A3B4479D19294637FDB3EEC27ED2FEC5077C9769CA48BA2F08B44CD9C993973A3FFFDFB9FE8D34A49B204EB84D1637A343AA404746252A117639AFBF9FB0FF4D3C737BF4517A95A916FB5DC4990434DEDC6F4C1FBEC9431973C80E26EA444628D33733F4A8C623C35ECF8F0F0777674C4002128621112DDE5DA0B05C50BBE4E8C4E20F3399753938274D5397E890B5472CD15B88C2730A6D7F0E846E7DC734ACEA4E0E8400C724E09D7DA78EED1BDD3AF0E626F8D5EC4191E7079BFCE00E5E65C3AA8DC3E6DC59F1BC1E1718880B58A3554923B6FD40B018F4E2A4AD850FD55C4D2863224ED02C9F5EB1075415CC919254343A713698350C56949FD283C1F90CEC9419376AC8EF03B20935CFADCC25843EE2D9707E4369F4991FC09EB7BF31DF458E752763D429FF05BEF008F6EADC9C0FAF51DCC2B3FAF524A585F8F0D151BB58E4E19C595F627C7945CA3713E93D024BC1371EC8D853F4083E51ED25BEE3D581D30A0A06CC3FAC05660056BD5A3746D140B0D5B8592295F7D01BDF00F638A8F945C8A15A4F549E5C8572DB0B350C9DB1CF6D9BA338F15EFB5A9CF42738B3E866EC9ADC5365D4F0BB8C256CF850FDB1D78053513A3B2DCC32031116B6B6CB3F202455C205287B6E28B806D6588ED5A55A2ABA8E9135382C6E07BD5DC3A504E805179BCCDCBC69F76AEB072B0D40388ED9840D1946719E6B73391AA131297E368F23E7E79C3AA1283256E4BDF36DE369630337C0183AF2135295C0AEB7C9886331EF23949D586588FFD1DCCD6A63A040FBBB5E5BB160ECFAD42319347DB545BD22E310E85CD5384048DF5ED160BC538E192DB2D2D3F3132577AD7D8784ABBD7C45D98DE87E7E3751BB50BD73DDF448BD8809321DF6C83F0C1601C26EFA9C21F8A34D69B0618147A5415DDFE7DBC5185A5082548D352A4A102E3B5F3A0CA0289FF9213290A8A6B8129D7620ECE97BB83E26E3C1EECF65F67CF32E752F98C65FBD3979F088CEE5D6F1B0BE0C5FB4E2FB94D1EB87DABF8EA5D17F0753BCD9AC7657DB277033DE9FDCB56D2E6ECFC819D5356FC98A6338371951EFED036DA6CBE8875AFCCD13938B16821C2055A4312AABA05AD65AEF4DCD479C080BA1ED52283344DC1F3147939B35ECC79E2F17302CE15F79C6F5CE62872A166905EE99BDC636ECE9C033593BDEB53C49EB65FACDCBECFD14D16DEDCFF1102BA293004B8D19F7321D3C6EFCB2D85B303229448558DE815DEF3106EB16E90AE8D7E265045DF3964A04327DE83CA2482B91B1DF325BCC637BC327D81054FD6F50CDD0DB23F117DDAA373C117962B5761B4FAE16F200BFF033FFE07C4B0437D390E0000, N'6.1.3-40302')
SET IDENTITY_INSERT [dbo].[News] ON 

INSERT [dbo].[News] ([Id], [NewsContent]) VALUES (1, N'I am the king!!!')
INSERT [dbo].[News] ([Id], [NewsContent]) VALUES (2, N'The New York major left the building...')
INSERT [dbo].[News] ([Id], [NewsContent]) VALUES (3, N'Where ur parents are atm? No idea? Batman knows where his are!')
SET IDENTITY_INSERT [dbo].[News] OFF
USE [master]
GO
ALTER DATABASE [News] SET  READ_WRITE 
GO
