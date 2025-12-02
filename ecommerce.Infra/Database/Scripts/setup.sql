-- Criar usuário se não existir
DO
$$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'ecommerce') THEN
        CREATE ROLE ecommerce LOGIN PASSWORD '123456';
    END IF;
END
$$;

-- Criar banco se não existir (fora do bloco DO)
SELECT 'CREATE DATABASE ecommerce OWNER ecommerce'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'ecommerce')
\gexec
