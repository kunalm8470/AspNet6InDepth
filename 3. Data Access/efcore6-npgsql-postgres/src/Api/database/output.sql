CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'fakecompany') THEN
            CREATE SCHEMA fakecompany;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    CREATE TABLE fakecompany.departments (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name character varying(200) NOT NULL,
        created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT (current_timestamp AT TIME ZONE 'UTC'),
        updated_at TIMESTAMP WITHOUT TIME ZONE NULL,
        CONSTRAINT pk_departments_id PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    CREATE TABLE fakecompany.employees (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        first_name character varying(200) NOT NULL,
        last_name character varying(200) NOT NULL,
        "DisplayName" text GENERATED ALWAYS AS ("last_name" || ', ' || "first_name") STORED,
        email character varying(256) NOT NULL,
        phone character varying(100) NOT NULL,
        salary numeric(18,2) NOT NULL,
        hire_date DATE NOT NULL,
        department_id uuid NOT NULL,
        created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT (current_timestamp AT TIME ZONE 'UTC'),
        updated_at TIMESTAMP WITHOUT TIME ZONE NULL,
        CONSTRAINT pk_employees_id PRIMARY KEY (id),
        CONSTRAINT fk_employees_departmentid FOREIGN KEY (department_id) REFERENCES fakecompany.departments (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('288d6e20-44dd-42a4-871d-61afba5e6979', 'Finance', NULL);
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('53bbb954-3484-476c-a623-44d264b9cfb6', 'Board', NULL);
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('7763a7a3-fd7a-4d66-99cf-b4e03a6dbbb6', 'Legal', NULL);
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('7c198eef-df03-4f4a-a91f-4502f7dda492', 'Engineering', NULL);
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('badf4ee9-32d2-4e75-8ddf-4a6161dff48a', 'Human Resources', NULL);
    INSERT INTO fakecompany.departments (id, name, updated_at)
    VALUES ('baf283ce-3d62-4e74-aee3-f2e412d6150f', 'Admin', NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    CREATE INDEX "IX_employees_department_id" ON fakecompany.employees (department_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    CREATE UNIQUE INDEX "IX_employees_email" ON fakecompany.employees (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    CREATE UNIQUE INDEX "IX_employees_phone" ON fakecompany.employees (phone);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230424120443_Initial Migration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230424120443_Initial Migration', '6.0.16');
    END IF;
END $EF$;
COMMIT;

