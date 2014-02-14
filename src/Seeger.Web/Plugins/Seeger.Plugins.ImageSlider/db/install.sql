create table cms_Slider
(
	Id int not null,
	Name nvarchar(300) not null,
	Width int null,
	Height int null,
	ShowNavigation bit not null,
	ShowPagination bit not null,
	AutoPlay bit not null,
	AutoPlayInterval int not null,
	UtcCreatedAt datetime not null,

	constraint PK_cms_Slider primary key (Id)
);

create table cms_SliderItem
(
	Id int not null,
	Caption nvarchar(300) null,
	Description nvarchar(1000) null,
	ImageUrl nvarchar(300) not null,
	NavigateUrl nvarchar(300) null,
	DisplayOrder int not null,
	SliderId int not null,

	constraint PK_cms_SliderItem primary key (Id),
	constraint FK_cms_SliderItem_SliderId foreign key (SliderId) references cms_Slider(Id) on delete cascade
);

insert into cms_HiValue values ('cms_Slider', 0);
insert into cms_HiValue values ('cms_SliderItem', 0);
