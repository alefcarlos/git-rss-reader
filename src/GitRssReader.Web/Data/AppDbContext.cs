﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GitRssReader.Web.Data;

public class AppDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<FeedControl> FeedsControl { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}

public class Article
{
    public int Id { get; set; }
    public required string FeedSlug { get; set; }
    public required string Title { get; set; }
    public required string Url { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool IsRead { get; set; }
}

public class FeedControl
{
    [Key]
    public required string FeedSlug { get; set; }
    public required DateTime LastImport { get; set; }
}
