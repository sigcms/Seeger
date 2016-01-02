/**** Setup ****/

create table cms_HiValue
(
	TableName varchar(50) not null,
	NextValue int not null,

	constraint PK_cms_HiValue primary key (TableName)
);

create table cms_PageItem
(
	ID int not null,
	UniqueName nvarchar(50) null,
	DisplayName nvarchar(50) not null,
	UrlSegment nvarchar(50) not null,
	"Order" int not null,
	CreatedTime datetime not null,
	ModifiedTime datetime not null,
	VisibleInMenu bit not null,
	Published bit not null,
	IsDeletable bit not null,
	LayoutFullName nvarchar(50) not null,
	SkinFullName nvarchar(50) not null,
	BindedDomains nvarchar(300) null,

	MenuText nvarchar(50) not null,
	PageTitle nvarchar(1000) not null,
	MetaKeywords nvarchar(1000) not null,
	MetaDescription nvarchar(3000) not null,

	ParentPageId int null,
	Attributes xml null,
	
	constraint PK_cms_PageItem primary key (Id),
	constraint FK_cms_PageItem_ParentPageId foreign key (ParentPageId) references cms_PageItem(Id)
);

create table cms_LocatedWidget
(
	Id int not null,
	PageId int not null,
	PluginName nvarchar(50) not null,
	WidgetName nvarchar(50) not null,
	ZoneName nvarchar(50) not null,
	"Order" int not null,
	Attributes xml null,	-- this column is used by developers only
	
	constraint PK_cms_LocatedWidget primary key (Id),
	constraint FK_cms_LocatedWidget_PageId foreign key (PageId) references cms_PageItem(Id) on delete cascade
);

create table cms_TextContent
(
	Id int not null,
	Name nvarchar(50) not null,
	CreatedTime datetime not null,
	Content nvarchar(max) not null,
	
	constraint PK_cms_TextContent primary key (Id)
);

create table cms_GlobalSetting
(
	"Key" varchar(50) not null,
	Value nvarchar(max) not null,
	
	constraint PK_cms_GlobalSetting primary key ("Key")
);

/* User Access Control */

create table cms_Role
(
	Id int not null,
	Name nvarchar(50) not null,

	constraint PK_cms_Role primary key (Id)
);

create table cms_RoleGrantedPermission
(
	Id int not null,
	RoleId int not null,
	PluginName nvarchar(50) null,
	PermissionGroupName nvarchar(50) null,
	PermissionName nvarchar(50) not null,
	
	constraint PK_cms_RoleGrantedPermission primary key (Id),
	constraint FK_cms_RoleGrantedPermission_RoleId foreign key (RoleId) references cms_Role(Id) on delete cascade
);

create table cms_User
(
	Id int not null,
	UserName nvarchar(50) not null,
	Nick nvarchar(50) null,
	"Password" varchar(40) not null,
	Email varchar(250) not null,
	SkinName nvarchar(50) null,
	"Language" varchar(10) null,
	IsSuperAdmin bit not null,
	FailedPasswordAttemptCount int not null,
	LastFailedPasswordAttemptTime datetime null,
	LastLoginTime datetime null,
	LastLoginIP varchar(50) null,

	constraint PK_cms_User primary key ("Id")
);

create unique index UQ_cms_User_UserName on cms_User(UserName);

create table cms_UserInRole
(
	Id int not null,
	UserId int not null,
	RoleId int not null,

	constraint PK_cms_UserInRole primary key (Id),
	constraint FK_cms_UserInRole_UserId foreign key (UserId) references cms_User(Id) on delete cascade,
	constraint FK_cms_UserInRole_RoleId foreign key (RoleId) references cms_Role(Id)
);

/* End User Access Control */

create table cms_FrontendLanguage
(
	Name varchar(10) not null,
	DisplayName nvarchar(50) not null,
	BindedDomain nvarchar(50) not null,

	constraint PK_cms_FrontendLanguage primary key (Name)
);

create table cms_CustomRedirect
(
	Id int not null,
	"From" nvarchar(300) not null,
	"To" nvarchar(300) not null,
	"Description" nvarchar(500) null,
	MatchByRegex bit not null,
	RedirectMode int not null,
	Priority int not null,
	IsEnabled bit not null,
	UtcCreatedTime datetime not null,

	constraint PK_cms_CustomRedirect primary key (Id)
);

create table cms_SiteInfo
(
	Culture varchar(10) not null,
	SiteTitle nvarchar(100) null,
	SiteSubtitle nvarchar(300) null,
	LogoFilePath nvarchar(255) null,
	Copyright nvarchar(100) null,
	MiiBeiAnNumber nvarchar(50) null,
	SEO_PageTitle nvarchar(300) null,
	SEO_MetaKeywords nvarchar(1000) null,
	SEO_MetaDescription nvarchar(1000) null,

	constraint PK_cms_SiteInfo primary key (Culture)
);

create table cms_TaskEntry
(
	Id int not null,
	[Description] nvarchar(100) not null,
	QueueName nvarchar(50) not null,
	[State] nvarchar(max) null,
	TaskType nvarchar(300) not null,
	[Status] int not null,
	TotalRetries int not null,
	ErrorMessage nvarchar(300) null,
	ErrorDetail nvarchar(max) null,
	CreatedAtUtc datetime not null,
	LastStartedAtUtc datetime null,
	LastStoppedAtUtc datetime null,

	constraint PK_cms_TaskEntry primary key (Id)
);

create table cms_EntityPropertyLocalization
(
	Id int not null,
	EntityType nvarchar(100) not null,
	EntityKey nvarchar(100) not null,
	PropertyPath nvarchar(100) not null,
	Culture varchar(10) not null,
	PropertyValue nvarchar(max) null,

	constraint PK_cms_EntityPropertyLocalization primary key (Id)
);

create index IX_cms_EntityPropertyLocalization on cms_EntityPropertyLocalization(EntityType asc, EntityKey asc);

create table cms_LogEntry
(
	Id int not null,
	LoggerName nvarchar(100) not null,
	[Level] nvarchar(50) null,
	[Message] nvarchar(max) null,
	[Exception] nvarchar(max) null,
	UtcTimestamp datetime not null,
	Operator_Id int not null,
	Operator_UserName nvarchar(50) not null,
	Operator_Nick nvarchar(50) null,

	constraint PK_cms_LogEntry primary key (Id)
);

/* Required Data */

insert into cms_HiValue values ('cms_PageItem', 0);
insert into cms_HiValue values ('cms_TextContent', 0);
insert into cms_HiValue values ('cms_Role', 1);
insert into cms_HiValue values ('cms_RoleGrantedPermission', 1);
insert into cms_HiValue values ('cms_User', 1);
insert into cms_HiValue values ('cms_UserInRole', 1);
insert into cms_HiValue values ('cms_TaskEntry', 0);
insert into cms_HiValue values ('cms_CustomRedirect', 0);
insert into cms_HiValue values ('cms_LocatedWidget', 0);
insert into cms_HiValue values ('cms_EntityPropertyLocalization', 0);
insert into cms_HiValue values ('cms_LogEntry', 0);

/* Password: abc123 */
insert into cms_User values (1, 'sa', 'Admin', '6367C48DD193D56EA7B0BAAD25B19455E529F5EE', 'support@sigcms.com', null, null, 1, 0, null, null, null);

insert into cms_FrontendLanguage values ('en-US', 'English', '');
insert into cms_FrontendLanguage values ('zh-CN', '简体中文', '');

insert into cms_GlobalSetting values ('Seeger.SiteInfo.SiteTitle', '西格CMS');
insert into cms_GlobalSetting values ('Seeger.SiteInfo.PageTitle', '西格CMS');
insert into cms_GlobalSetting values ('Seeger.SiteInfo.Copyright', '&copy; 2011 西格CMS');

/* Version 1.0.1 */
alter table cms_LocatedWidget add ViewName varchar(50) null;