CREATE TABLE task_type
(
  task_type_id int IDENTITY(1,1) not null,
  task_name nvarchar(256),
  assembly_location nvarchar(1024),
  class_name nvarchar(1024),  
  CONSTRAINT pk_task_type PRIMARY KEY CLUSTERED (task_type_id ASC)
)
GO

CREATE TABLE batch
(
  batch_id UNIQUEIDENTIFIER not null,
  task_type_id int not null,
  chunk_init_parameters nvarchar(max) null,
  batch_complete_task_type_id int null,
  batch_complete_task_parameters nvarchar(max) null,
  CONSTRAINT pk_batch PRIMARY KEY CLUSTERED (batch_id ASC),
  CONSTRAINT fk_batch1 FOREIGN KEY (task_type_id) REFERENCES task_type (task_type_id),
  CONSTRAINT fk_batch2 FOREIGN KEY (batch_complete_task_type_id) REFERENCES task_type (task_type_id),
)
GO

CREATE SEQUENCE task_item_seq as int START WITH 1 MINVALUE 1;
CREATE TABLE task_item
(
  task_item_id int not null default(NEXT VALUE FOR task_item_seq),  
  batch_id UNIQUEIDENTIFIER not null,
  group_key nvarchar(256) null,  
  exclusion_key nvarchar(256) null,
  task_parameters nvarchar(max) not null,
  CONSTRAINT pk_task_item PRIMARY KEY CLUSTERED (task_item_id ASC),
  CONSTRAINT fk_task_item1 FOREIGN KEY (batch_id) REFERENCES batch (batch_id),
)
GO