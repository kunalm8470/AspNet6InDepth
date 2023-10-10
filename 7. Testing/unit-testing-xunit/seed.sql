CREATE TABLE persons (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  first_name CHARACTER varying(200) NOT NULL,
  last_name CHARACTER varying(200) NOT NULL,
  age INTEGER NOT NULL,
  created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT (current_timestamp AT TIME ZONE 'UTC'),
  updated_at TIMESTAMP WITHOUT TIME ZONE
);

CREATE TABLE addresses (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    person_id UUID,
    address CHARACTER varying(500) NOT NULL,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT (current_timestamp AT TIME ZONE 'UTC'),
    updated_at TIMESTAMP WITHOUT TIME ZONE,
    CONSTRAINT fk_persons_id FOREIGN KEY(person_id) REFERENCES persons(id) ON DELETE CASCADE,
    CONSTRAINT unique_person_id UNIQUE (person_id)
);

INSERT INTO persons (id, first_name, last_name, age, created_at, updated_at)
VALUES 
('298df3d3-9c26-4b1b-a269-4a4a36a7a4b4', 'John', 'Doe', 35, '2023-04-17 12:00:01', NULL),
('f99645e4-14a7-42d4-a539-86a12a37b1e1', 'Jane', 'Smith', 27, '2023-04-17 12:00:20', NULL),
('1a3d7b3e-8b13-49b1-bc31-7a774a30a8a7', 'Bob', 'Johnson', 42, '2023-04-17 12:00:21', NULL),
('4499b79a-c710-45e4-ba87-083d22c4d6ad', 'Alice', 'Williams', 23, '2023-04-17 12:00:25', NULL);


INSERT INTO addresses (id, person_id, address, created_at, updated_at)
VALUES
('df1a0582-8c84-47c1-8441-57c54e9a8767', '298df3d3-9c26-4b1b-a269-4a4a36a7a4b4', '123 Main St, Anytown, USA', '2023-04-17 12:07:00', NULL),
('a46b6f7f-2dc4-4dfe-9a90-0aa1d2eeabf8', 'f99645e4-14a7-42d4-a539-86a12a37b1e1', '456 Oak St, Anycity, USA', '2023-04-17 12:08:00', NULL),
('6d2f6e31-6bf5-4ca5-ae67-13b0595c5f53', '1a3d7b3e-8b13-49b1-bc31-7a774a30a8a7', '789 Elm St, Anystate, USA', '2023-04-17 12:09:00', NULL),
('8b011064-04b4-4f85-a4ad-f7b45e78b6f7', '4499b79a-c710-45e4-ba87-083d22c4d6ad', '456 Pine St, Anytown, USA', '2023-04-17 12:11:00', NULL);
