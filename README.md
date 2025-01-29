Leitor OPML

> A ideia é usar um repositório git contendo o arquivo opml e também realizar alterações quando for adicionado um novo feed

# Funcionalidades

- Listar categorias
- Listar feeds por categoria
- Lista artigos do feed
- Job para armazenar os artigos
- Marcar ocmo lido

# Publicar imagem local

Para publicar imagem docker local

```
dotnet publish .\src\GitRssReader.Web\ /p:PublishProfile=local
```

A imagem `local/git-rss-reader-web` estará disponível.

# Rodando

```
docker run -p 8080:8080 local/git-rss-reader-web
```

Acessar `http://localhost:8080`
