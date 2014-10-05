create table cmt_Comment
(
	Id int not null,
	SubjectId nvarchar(50) not null,
	SubjectTitle nvarchar(100) null,
	Content nvarchar(max) null,
	PostedTimeUtc datetime not null,
	TotalReplies int not null,
	ParentCommentId int null,
	CommenterId nvarchar(50) null,
	CommenterNick nvarchar(50) null,
	CommenterAvatar nvarchar(255) null,
	CommenterIP varchar(39) null,

	constraint PK_cmt_Comment primary key (Id)
);

create index IX_cmt_Comment_SubjectId on cmt_Comment(SubjectId);
create index IX_cmt_Comment_ParentCommentId on cmt_Comment(ParentCommentId);

insert into cms_HiValue values ('cmt_Comment', 0);
