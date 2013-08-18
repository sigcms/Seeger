create table sample_BlogPost
(
	Id int not null,
	Title nvarchar(100) not null,
	Content nvarchar(max) null,
	CreatedAt datetime not null,

	constraint PK_sample_BlogPost primary key (Id)
);

insert into cms_HiValue values ('sample_BlogPost', 0);
