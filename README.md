Leitor OPML

> A ideia é usar um repositório git contendo o arquivo opml e também realizar alterações quando for adicionado um novo feed

# Funcionalidades

- Listar categorias
- Listar feeds por categoria
- Lista artigos do feed
- Job para armazenar os artigos
- Marcar como lido

# Iniciando

```
docker run [variaveis] -p 8080:8080 alefcarlos/git-rss-reader-web
```

Acessar `http://localhost:8080`

## Variáveis de ambiente

| Nome | Descrição | Exemplo |
| - | -| - |
| GIT__REPOSITORY\*| - |- |
| GIT__USERNAME\*| - | - |
| GIT__PASSWORD\*| PAT | - |
| GIT__OPMLFILEPATH\* |- |

> \* São orbigatórios 

# Publicar imagem local

Para publicar imagem docker local

```
dotnet publish .\src\GitRssReader.Web\ /p:PublishProfile=local
```

A imagem `alefcarlos/git-rss-reader-web` estará disponível.
