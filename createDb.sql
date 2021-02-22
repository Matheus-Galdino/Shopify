create database shopify;

create table category(
id int primary key identity(1, 1),
name varchar(50) NOT NULL
);

create table item(
id int primary key identity(1, 1),
name varchar(100) not null,
note varchar(400),
imageurl varchar(400),
categoryId int foreign key references category(id)
);

create table list(
id int primary key IDENTITY(1,1),
listName varchar(50) NOT NULL,
listDate date NOT NULL,
isActive bit NOT NULL default 0,
status varchar(9) NOT NULL default 'progress' check (status='cancelled' OR status='completed' OR status ='progress'),
);

CREATE TABLE list_item(
idList int NOT NULL,
idItem int NOT NULL,
quantity int NOT NULL default 1,
iscompleted bit NULL default 0,
primary key(idList, idItem));