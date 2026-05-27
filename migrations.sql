-- TrackCare - Script de criacao das tabelas

-- Tabela: recolhimentos
CREATE TABLE IF NOT EXISTS recolhimentos (
    "Id" serial NOT NULL,
    "Hgid" character varying(50) NOT NULL,
    "NumeroSerie" character varying(100) NOT NULL,
    "Modelo" text,
    "ClienteNome" character varying(200) NOT NULL,
    "ClienteContato" character varying(200),
    "ClienteEmail" character varying(200),
    "ClienteTelefone" character varying(50),
    "ClientePlano" integer NOT NULL,
    "TicketHub" character varying(50),
    "TicketBlip" character varying(50),
    "DescricaoProblema" character varying(2000),
    "RelatorioN3" character varying(4000),
    "JaRecolhido" boolean NOT NULL DEFAULT false,
    "Status" integer NOT NULL,
    "DataSolicitacao" timestamptz NOT NULL,
    "DataPrevistaColeta" timestamptz,
    "DataColetaReal" timestamptz,
    "DataPrevistaDevolucao" timestamptz,
    "DataDevolucaoReal" timestamptz,
    "Observacoes" character varying(4000),
    "CreatedBy" character varying(100),
    "Created" timestamptz NOT NULL,
    "LastModified" timestamptz NOT NULL,
    CONSTRAINT "PK_recolhimentos" PRIMARY KEY ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_recolhimentos_ClienteNome" ON recolhimentos ("ClienteNome");
CREATE INDEX IF NOT EXISTS "IX_recolhimentos_DataSolicitacao" ON recolhimentos ("DataSolicitacao");
CREATE INDEX IF NOT EXISTS "IX_recolhimentos_Hgid" ON recolhimentos ("Hgid");
CREATE INDEX IF NOT EXISTS "IX_recolhimentos_NumeroSerie" ON recolhimentos ("NumeroSerie");
CREATE INDEX IF NOT EXISTS "IX_recolhimentos_Status" ON recolhimentos ("Status");

-- Tabela: anexos
CREATE TABLE IF NOT EXISTS anexos (
    "Id" serial NOT NULL,
    "RecolhimentoId" integer NOT NULL,
    "Tipo" integer NOT NULL,
    "NomeOriginal" character varying(255) NOT NULL,
    "NomeArquivo" character varying(255) NOT NULL,
    "TamanhoBytes" bigint NOT NULL,
    "UsuarioUpload" character varying(100),
    "DataUpload" timestamptz NOT NULL,
    "CaminhoCompleto" character varying(500) NOT NULL,
    CONSTRAINT "PK_anexos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_anexos_recolhimentos_RecolhimentoId"
        FOREIGN KEY ("RecolhimentoId")
        REFERENCES recolhimentos("Id")
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_anexos_RecolhimentoId" ON anexos ("RecolhimentoId");

-- Tabela: comentarios
CREATE TABLE IF NOT EXISTS comentarios (
    "Id" serial NOT NULL,
    "RecolhimentoId" integer NOT NULL,
    "Texto" character varying(2000) NOT NULL,
    "Usuario" character varying(100),
    "Setor" character varying(50),
    "DataCriacao" timestamptz NOT NULL,
    CONSTRAINT "PK_comentarios" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_comentarios_recolhimentos_RecolhimentoId"
        FOREIGN KEY ("RecolhimentoId")
        REFERENCES recolhimentos("Id")
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_comentarios_RecolhimentoId" ON comentarios ("RecolhimentoId");

-- Tabela: historico_status
CREATE TABLE IF NOT EXISTS historico_status (
    "Id" serial NOT NULL,
    "RecolhimentoId" integer NOT NULL,
    "StatusAnterior" integer NOT NULL,
    "StatusNovo" integer NOT NULL,
    "Observacao" character varying(1000),
    "Usuario" character varying(100),
    "DataAlteracao" timestamptz NOT NULL,
    CONSTRAINT "PK_historico_status" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_historico_status_recolhimentos_RecolhimentoId"
        FOREIGN KEY ("RecolhimentoId")
        REFERENCES recolhimentos("Id")
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_historico_status_RecolhimentoId" ON historico_status ("RecolhimentoId");
